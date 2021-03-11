using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Foundation.Controllers
{
    public class UserController : Controller
    {
        private readonly DBCon con;
        public UserController(DBCon context)
        {
            con = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            try
            {
                var chkData = await con.logins.Where(l => l.usrName == login.usrName).FirstOrDefaultAsync();
                if (chkData == null) { return Content("Invalid username"); }

                if (chkData.usrPassword != login.usrPassword) { return Content("Invalid password"); }

                HttpContext.Session.SetInt32("usrId", chkData.perId);
                HttpContext.Session.SetInt32("roleId", chkData.roleId);

                return Content("");
            }
            catch (Exception)
            {

            }
            return View();
        }


        [CheckSession]
        public IActionResult Registration()
        {
            PopulateCountries();
            PopulateRole();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> _UserData()
        {
            try
            {
                var getData = await con.peoples.Include(i => i.Country).Include(i => i.City).Include(i => i.Role).ToListAsync();
                TempData["Users"] = getData;
                return PartialView();
            }
            catch (Exception ex)
            {
                return Content("An error occured during getting the request. Please try again later");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(Person data)
        {
            try
            {
                var chkCode = await con.peoples.Where(c => c.code == data.code).AnyAsync();
                if (chkCode == true) { return Content("The user code is already exists"); }

                var chkCnin = await con.peoples.Where(c => c.cnic == data.cnic).AnyAsync();
                if (chkCnin == true) { return Content("The CNIC is already exists"); }

                var chkNumber = await con.peoples.Where(c => c.contactOne == data.contactOne).AnyAsync();
                if (chkNumber == true) { return Content("The contact number is already exists"); }

                int usrId = Convert.ToInt32(HttpContext.Session.GetInt32("usrId"));
                data.CreatedBy = usrId;
                data.perStatus = "Active";
                data.CreatedDate = DateTime.Now;
                con.peoples.Add(data);
                await con.SaveChangesAsync();


                Login login = new Login();
                login.perId = data.perId;
                login.roleId = data.roleId;
                login.usrName = data.cnic;
                login.usrPassword = "123";
                login.usrStatus = "Active";

                con.logins.Add(login);
                await con.SaveChangesAsync();


                return Content("");

            }
            catch (Exception)
            {
                return Content("Data Not added");
            }
        }

        [HttpGet]
        public async Task<IActionResult> _UpdateUser(int id)
        {
            try
            {
                PopulateCountries();
                PopulateRole();

                var GetData = await con.peoples.FindAsync(id);

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
        public async Task<IActionResult> UpdateUser(Person data)
        {
            try
            {
                var per = await con.peoples.FindAsync(data.perId);
                if (per != null)
                {
                    var chkCNICE = con.peoples.Where(c => c.cnic == data.cnic && c.perId != data.perId).Any();
                    if (chkCNICE == true) { return Content("The CNIC is already exists"); }


                    var chkNum = con.peoples.Where(c => c.contactOne == data.cnic && c.perId != data.perId).Any();
                    if (chkNum == true) { return Content("The contact number is already exists"); }

                    var chkCode = con.peoples.Where(c => c.code == data.code && c.perId != data.perId).Any();
                    if (chkCode == true) { return Content("The user code is already exists"); }

                    int usrId = Convert.ToInt32(HttpContext.Session.GetInt32("usrId"));
                    
                    per.firstName = data.firstName;
                    per.lastName = data.lastName;
                    per.email = data.cnic;
                    per.email = data.code;
                    per.email = data.email;
                    per.contactOne = data.contactOne;
                    per.contactTwo = data.contactTwo;
                    per.addressOne = data.addressOne;
                    per.cId = data.cId;
                    per.ctId = data.ctId;
                    per.postalCode = data.postalCode;
                    per.roleId = data.roleId;
                    per.perStatus = data.perStatus;
                    per.UpdatedBy = usrId;
                    per.UpdatedDate = DateTime.Now;
                    con.Entry(per).State = EntityState.Modified;
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
                    return BadRequest(new { message = "No Record found." });
                }
            }
            catch (Exception)
            {
                return Content("Data Not added");
            }
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var ChkData = await con.donations.Where(b => b.perId == id).AnyAsync();
                if (ChkData == true)
                {
                    return Content("This user cannot be deleted because it is associated with some Info");
                }
                else
                {
                    var getData = await con.peoples.FindAsync(id);
                    if (getData == null)
                    {
                        return NotFound();
                    }

                    con.peoples.Remove(getData);
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

        public void PopulateRole()
        {
            try
            {
                SelectList data = new SelectList(con.roles.ToList(), "roleId", "roleName");
                ViewData["Roles"] = data;
            }
            catch (Exception)
            {

            }
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }
    }
}
