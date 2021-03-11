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
    public class DonationsController : Controller
    {
        SendMessage msg = new SendMessage();
        private readonly DBCon con;
        public DonationsController(DBCon context)
        {
            con = context;
        }


        public IActionResult Giver()
        {
            PopulateDonationType();
            PopulateDonor();
            return View();
        }

        public async Task<IActionResult> _GiverData()
        {
            try
            {
                var getData = await con.donations.Include(i => i.Donation_Type).Include(i => i.Person)
                    .Where(w => w.dType == "Giver").OrderByDescending(o => o.dId).ToListAsync();
                TempData["Donation"] = getData;
            }
            catch (Exception)
            {

            }
            return PartialView();
        }

        public IActionResult Taker()
        {
            PopulateDonationType();
            PopulateTaker();
            return View();
        }

        public async Task<IActionResult> _TakerData()
        {
            try
            {
                var getData = await con.donations.Include(i => i.Donation_Type).Include(i => i.Person)
                    .Where(w => w.dType == "Taker").OrderByDescending(o => o.dId).ToListAsync();
                TempData["Donation"] = getData;
            }
            catch (Exception)
            {

            }
            return PartialView();
        }

        [HttpPost]
        public ActionResult GetNumber(int? perId)
        {
            
            if (perId != null)
            {
                var getNumber = con.peoples.Where(p => p.perId == perId).Select(p => p.contactOne).FirstOrDefault();
                if(getNumber != null)
                {
                    return Content(""+ getNumber + "");
                }
                else
                {
                    return Content("");
                }
            }
            return Content("");
        }

        [HttpPost]
        public async Task<IActionResult> AddOperation(Donation donation)
        {
            try
            {
                int usrId = Convert.ToInt32(HttpContext.Session.GetInt32("usrId"));
                donation.CreatedBy = usrId;
                donation.CreatedDate = DateTime.Now;
                con.donations.Add(donation);
                await con.SaveChangesAsync();
                if(donation.dType == "Giver")
                {
                    var msgboday = "Ghani Foundation has received " + donation.dAmount + " /-pkr. Thanks for trusting us.";
                    msg.SendSMSTurab(donation.dNumber, msgboday);
                }
                else
                {
                    var msgboday = "You have received " + donation.dAmount + " /-pkr from  Ghani Foundation. Remember us in your prayers.";
                    msg.SendSMSTurab(donation.dNumber, msgboday);
                }
                

                return Content("");
            }
            catch (Exception)
            {
                return Content("Donation no added");
            }
        }

        public void PopulateDonationType()
        {
            try
            {
                SelectList data = new SelectList(con.donation_Types.Where(c => c.dtStatus == "Active").ToList(), "dtId", "dtName");
                ViewData["Types"] = data;
            }
            catch (Exception)
            {

            }
        }

        public void PopulateDonor()
        {
            try
            {
                var GetData = (from p in con.peoples
                               where (p.roleId == 3 || p.roleId == 5) && p.perStatus == "Active"
                               select new
                               {
                                   p.perId,
                                   firstName = p.firstName + " " + p.lastName + " (" + p.cnic + ")" 
                               }).ToList();

                SelectList data = new SelectList(GetData, "perId", "firstName");
                ViewData["Donor"] = data;
            }
            catch (Exception)
            {

            }
        }

        public void PopulateTaker()
        {
            try
            {
                var GetData = (from p in con.peoples
                               where (p.roleId == 4 || p.roleId == 5) && p.perStatus == "Active"
                               select new
                               {
                                   p.perId,
                                   firstName = p.firstName + " " + p.lastName + " (" + p.cnic + ")"
                               }).ToList();

                SelectList data = new SelectList(GetData, "perId", "firstName");
                ViewData["Taker"] = data;
            }
            catch (Exception)
            {

            }
        }

    }
}
