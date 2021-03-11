using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Foundation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace Foundation.Controllers
{
    [CheckSession]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DBCon con;

        public HomeController(ILogger<HomeController> logger, DBCon context)
        {
            _logger = logger;
            con = context;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var getStats = await con.donations.ToListAsync();
                ViewBag.CurrentMonthCollection = getStats.Where(s => s.dType == "Giver" && s.CreatedDate.Month == DateTime.Now.Month && s.CreatedDate.Year == DateTime.Now.Year).Sum(s => s.dAmount);
                ViewBag.CurrentMonthDistribution = getStats.Where(s => s.dType == "Taker" && s.CreatedDate.Month == DateTime.Now.Month && s.CreatedDate.Year == DateTime.Now.Year).Sum(s => s.dAmount);
                ViewBag.TotalDonation = getStats.Where(s => s.dType == "Giver").Sum(s => s.dAmount);
                ViewBag.TotalDistribution = getStats.Where(s => s.dType == "Taker").Sum(s => s.dAmount);
                ViewBag.Donor = await con.peoples.Where(r => r.roleId == 3 || r.roleId == 5).CountAsync();
                ViewBag.Taker = await con.peoples.Where(r => r.roleId == 4 || r.roleId == 5).CountAsync();
                ViewBag.Center = await con.locations.CountAsync();
            }
            catch (Exception)
            {

            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        public IActionResult Countries()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> _CountryData()
        {
            try
            {
                var getData = await con.countries.ToListAsync();
                TempData["Countries"] = getData;
                return PartialView();
            }
            catch (Exception)
            {
                return Content("An error occured during getting the request. Please try again later");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCountry(Country data)
        {
            try
            {
                var chkData = await con.countries.Where(c => c.cCode == data.cCode).AnyAsync();
                if (chkData == true)
                {
                    return Content("The country code is already exist");
                }
                else
                {
                    data.CreatedDate = DateTime.Now;
                    con.countries.Add(data);
                    await con.SaveChangesAsync();
                    return Content("");
                }
            }
            catch (Exception)
            {
                return Content("Data Not added");
            }
        }

        [HttpGet]
        public async Task<IActionResult> _UpdateCountry(int id)
        {
            try
            {
                var GetData = await con.countries.FindAsync(id);

                if (GetData == null)
                {
                    return NotFound();
                }

                return PartialView(GetData);

            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occured while getting the data. Please try again later." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCountry(Country data)
        {
            try
            {
                var getData = await con.countries.FindAsync(data.cId);
                if (getData != null)
                {
                    var chkCode = con.countries.Where(c => c.cCode == data.cCode && c.cId != data.cId).Any();
                    if (chkCode == false)
                    {
                        getData.cFullName = data.cFullName;
                        getData.cShortName = data.cShortName;
                        getData.cCode = data.cCode;
                        getData.cStatus = data.cStatus;
                        getData.UpdatedBy = data.UpdatedBy;
                        getData.UpdatedDate = DateTime.Now;
                        con.Entry(getData).State = EntityState.Modified;
                        try
                        {
                            await con.SaveChangesAsync();
                            return Ok();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            return BadRequest(new { message = "An error occured while updating the Info. Please try again later" });
                        }
                    }
                    else
                    {
                        return BadRequest(new { message = "This Country code is already exists." });
                    }
                }
                else
                {
                    return BadRequest(new { message = "No Record found." });
                }
            }
            catch (Exception)
            {
                return Content("Data Not added");
            }
        }

        public async Task<IActionResult> DelCountry(int id)
        {
            try
            {
                var ChkData = await con.countries.Where(b => b.cId == id).AnyAsync();
                if (ChkData == true)
                {
                    return Content("This country cannot be deleted because it is associated with some Info");
                }
                else
                {
                    var getData = await con.countries.FindAsync(id);
                    if (getData == null)
                    {
                        return NotFound();
                    }

                    con.countries.Remove(getData);
                    await con.SaveChangesAsync();
                    return Content("");
                }
            }
            catch (Exception)
            {
                return Content("Not deleted");
            }
        }

        public IActionResult DonationType()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> _Donation_TypeData()
        {
            try
            {
                var getData = await con.donation_Types.ToListAsync();
                TempData["Donation_Type"] = getData;
                return PartialView();
            }
            catch (Exception)
            {
                return Content("An error occured during getting the request. Please try again later");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddDonation_Type(Donation_Type data)
        {
            try
            {
                var chkData = await con.donation_Types.Where(c => c.dtCode == data.dtCode).AnyAsync();
                if (chkData == true)
                {
                    return Content("The code is already exist");
                }
                else
                {
                    con.donation_Types.Add(data);
                    await con.SaveChangesAsync();
                    return Content("");
                }
            }
            catch (Exception)
            {
                return Content("Data Not added");
            }
        }

        [HttpGet]
        public async Task<IActionResult> _UpdateDonation_Type(int id)
        {
            try
            {
                var GetData = await con.donation_Types.FindAsync(id);

                if (GetData == null)
                {
                    return NotFound();
                }

                return PartialView(GetData);

            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occured while getting the data. Please try again later." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDonation_Type(Donation_Type data)
        {
            try
            {
                var getData = await con.donation_Types.FindAsync(data.dtId);
                if (getData != null)
                {
                    var chkCode = con.donation_Types.Where(c => c.dtCode == data.dtCode && c.dtId != data.dtId).Any();
                    if (chkCode == false)
                    {
                        getData.dtName = data.dtName;
                        getData.dtCode = data.dtCode;
                        getData.dtStatus = data.dtStatus;
                        con.Entry(getData).State = EntityState.Modified;
                        try
                        {
                            await con.SaveChangesAsync();
                            return Ok();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            return BadRequest(new { message = "An error occured while updating the Info. Please try again later" });
                        }
                    }
                    else
                    {
                        return BadRequest(new { message = "This Country code is already exists." });
                    }
                }
                else
                {
                    return BadRequest(new { message = "No Record found." });
                }
            }
            catch (Exception)
            {
                return Content("Data Not added");
            }
        }

        public async Task<IActionResult> DelDonation_Type(int id)
        {
            try
            {
                var ChkData = await con.countries.Where(b => b.cId == id).AnyAsync();
                if (ChkData == true)
                {
                    return Content("This country cannot be deleted because it is associated with some Info");
                }
                else
                {
                    var getData = await con.countries.FindAsync(id);
                    if (getData == null)
                    {
                        return NotFound();
                    }

                    con.countries.Remove(getData);
                    await con.SaveChangesAsync();
                    return Content("");
                }
            }
            catch (Exception)
            {
                return Content("Not deleted");
            }
        }

        public IActionResult Cities()
        {
            PopulateCountries();
            return View();
        }

        [HttpGet]
        public IActionResult _CityData()
        {
            try
            {
                var getData = con.cities.Include(i => i.Country).ToList();
                TempData["Cities"] = getData;
                return PartialView();
            }
            catch (Exception)
            {
                return Content("An error occured during getting the request. Please try again later");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCity(City data)
        {
            try
            {
                var chkData = await con.cities.Where(c => c.ctCode == data.ctCode).AnyAsync();
                if (chkData == true)
                {
                    return BadRequest(new { message = "The city code is already exist" });
                }
                else
                {

                    data.CreatedDate = DateTime.Now;
                    con.cities.Add(data);
                    await con.SaveChangesAsync();
                    return Content("");
                }
            }
            catch (Exception)
            {
                return Content("Data Not added");
            }
        }

        [HttpGet]
        public async Task<IActionResult> _UpdateCity(int id)
        {
            try
            {
                PopulateCountries();

                var GetData = await con.cities.FindAsync(id);

                if (GetData == null)
                {
                    return NotFound();
                }

                return PartialView(GetData);

            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occured while getting the data. Please try again later." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCity(City data)
        {
            try
            {
                var getData = await con.cities.FindAsync(data.cId);
                if (getData != null)
                {
                    var chkCode = con.cities.Where(c => c.ctCode == data.ctCode && c.ctId != data.ctId).Any();
                    if (chkCode == false)
                    {
                        getData.ctFullName = data.ctFullName;
                        getData.ctShortName = data.ctShortName;
                        getData.ctCode = data.ctCode;
                        getData.ctStatus = data.ctStatus;
                        getData.UpdatedBy = data.UpdatedBy;
                        getData.UpdatedDate = DateTime.Now;
                        con.Entry(getData).State = EntityState.Modified;
                        try
                        {
                            await con.SaveChangesAsync();
                            return Ok();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            return BadRequest(new { message = "An error occured while updating the Info. Please try again later" });
                        }
                    }
                    else
                    {
                        return BadRequest(new { message = "This Country code is already exists." });
                    }
                }
                else
                {
                    return BadRequest(new { message = "No Record found." });
                }
            }
            catch (Exception)
            {
                return Content("Data Not added");
            }
        }

        public async Task<IActionResult> DelCity(int id)
        {
            try
            {
                var ChkData = await con.locations.Where(b => b.ctId == id).AnyAsync();
                if (ChkData == true)
                {
                    return Content("This city cannot be deleted because it is associated with some Info");
                }
                else
                {
                    var getData = await con.cities.FindAsync(id);
                    if (getData == null)
                    {
                        return NotFound();
                    }

                    con.cities.Remove(getData);
                    await con.SaveChangesAsync();
                    return Content("");
                }
            }
            catch (Exception)
            {
                return Content("Not deleted");
            }
        }


        public IActionResult Location()
        {
            PopulateCountries();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> _LocationData()
        {
            try
            {
                var getData = await con.locations.Include(i => i.Country).Include(i => i.City).ToListAsync();
                TempData["Location"] = getData;
                return PartialView();
            }
            catch (Exception)
            {
                return Content("An error occured during getting the request. Please try again later");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddLocation(Location data)
        {
            try
            {
                var chkData = await con.locations.Where(c => c.locName == data.locName).AnyAsync();
                if (chkData == true)
                {
                    return BadRequest(new { message = "The location name is already exist" });
                }
                else
                {

                    data.CreatedDate = DateTime.Now;
                    con.locations.Add(data);
                    await con.SaveChangesAsync();
                    return Content("");
                }
            }
            catch (Exception)
            {
                return Content("Data Not added");
            }
        }

        [HttpGet]
        public async Task<IActionResult> _UpdateLocation(int id)
        {
            try
            {
                var GetData = await con.locations.FindAsync(id);

                if (GetData == null)
                {
                    return NotFound();
                }

                return PartialView(GetData);

            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occured while getting the data. Please try again later." });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateLocation(Location data)
        {
            try
            {
                var getData = await con.locations.FindAsync(data.locId);
                if (getData != null)
                {
                    var chkCode = con.locations.Where(c => c.PostalCode == data.PostalCode && c.locId != data.locId).Any();
                    if (chkCode == false)
                    {
                        getData.locName = data.locName;
                        getData.PostalCode = data.PostalCode;
                        getData.locStatus = data.locStatus;
                        getData.cId = data.cId;
                        getData.ctId = data.ctId;
                        getData.UpdatedBy = data.UpdatedBy;
                        getData.UpdatedDate = DateTime.Now;
                        con.Entry(getData).State = EntityState.Modified;
                        try
                        {
                            await con.SaveChangesAsync();
                            return Ok();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            return BadRequest(new { message = "An error occured while updating the Info. Please try again later" });
                        }
                    }
                    else
                    {
                        return BadRequest(new { message = "This Country code is already exists." });
                    }
                }
                else
                {
                    return BadRequest(new { message = "No Record found." });
                }
            }
            catch (Exception)
            {
                return Content("Data Not added");
            }
        }

        public async Task<IActionResult> DelLocation(int id)
        {
            try
            {
                var ChkData = await con.locations.Where(b => b.ctId == id).AnyAsync();
                if (ChkData == true)
                {
                    return Content("This city cannot be deleted because it is associated with some Info");
                }
                else
                {
                    var getData = await con.cities.FindAsync(id);
                    if (getData == null)
                    {
                        return NotFound();
                    }

                    con.cities.Remove(getData);
                    await con.SaveChangesAsync();
                    return Content("");
                }
            }
            catch (Exception)
            {
                return Content("Not deleted");
            }
        }




        public void PopulateCountries()
        {
            try
            {
                SelectList data = new SelectList(con.countries.Where(c => c.cStatus == true).ToList(), "cId", "cFullName");
                ViewData["Countries"] = data;
            }
            catch (Exception)
            {

            }
        }

        //public void PopulateCities()
        //{
        //    try
        //    {
        //        SelectList data = new SelectList(con.cities.Where(c => c.ctStatus == true).ToList(), "ctId", "ctFullName");
        //        ViewData["Cities"] = data;
        //    }
        //    catch (Exception)
        //    {

        //    }
        //}

        public ActionResult PopulateCities()
        {
            SelectList data = new SelectList(con.cities.Where(c => c.ctStatus == true).ToList(), "ctId", "ctFullName");
            return Json(data);
        }

        
    }
}
