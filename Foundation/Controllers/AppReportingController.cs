using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Foundation.Controllers
{
    public class AppReportingController : Controller
    {
        private readonly DBCon con;
        public AppReportingController(DBCon context)
        {
            con = context;
        }
        public IActionResult AccountBalance()
        {
            return View();
        }

        public async Task<IActionResult> _AccountBalance(DateTime fDate, DateTime tDate)
        {
            try
            {
                if (fDate.Year == 1 && tDate.Year == 1)
                {
                    var getTypes = await con.donation_Types.ToListAsync();
                    List<Donation> dList = new List<Donation>();
                    foreach (var i in getTypes)
                    {
                        Donation donation = new Donation();
                        var getDonationSum = con.donations.Where(d => d.dtId == i.dtId && d.dType == "Giver").Sum(s => s.dAmount);
                        var getTakerSum = con.donations.Where(d => d.dtId == i.dtId && d.dType == "Taker").Sum(s => s.dAmount);

                        donation.dType = i.dtName;
                        donation.dAmount = getDonationSum - getTakerSum;
                        dList.Add(donation);
                    }
                    TempData["Accounts"] = dList;
                }
                else
                {
                    var getTypes = await con.donation_Types.ToListAsync();
                    List<Donation> dList = new List<Donation>();
                    foreach (var i in getTypes)
                    {
                        Donation donation = new Donation();
                        var getDonationSum = con.donations.Where(d => d.dtId == i.dtId && d.dType == "Giver" && d.CreatedDate >= fDate && d.CreatedDate <= tDate).Sum(s => s.dAmount);
                        var getTakerSum = con.donations.Where(d => d.dtId == i.dtId && d.dType == "Taker").Sum(s => s.dAmount);

                        donation.dType = i.dtName;
                        donation.dAmount = getDonationSum - getTakerSum;
                        dList.Add(donation);
                    }
                    TempData["Accounts"] = dList;
                }
            }
            catch (Exception)
            {

            }
            return PartialView();
        }

        public IActionResult AccountLedger()
        {
            PopulateDonationType();
            return View();
        }

        public async Task<IActionResult> _AccountLedger(int? dtId, DateTime fDate, DateTime tDate)
        {
            try
            {
                if (dtId == null && fDate.Year == 1 && tDate.Year == 1)
                {
                    var getDonations = await con.donations.Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["AccountLedger"] = getDonations;
                }
                else if (dtId != null && fDate.Year == 1 && tDate.Year == 1)
                {
                    var getDonations = await con.donations.Where(d => d.dtId == dtId)
                        .Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["AccountLedger"] = getDonations;
                }
                else if (dtId != null && fDate.Year != 1 && tDate.Year != 1)
                {
                    var getDonations = await con.donations.Where(d => d.dtId == dtId && d.CreatedDate >= fDate && d.CreatedDate <= tDate)
                        .Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["AccountLedger"] = getDonations;
                }
                else if (fDate.Year != 1 && tDate.Year != 1)
                {
                    var getDonations = await con.donations.Where(d => d.CreatedDate >= fDate && d.CreatedDate <= tDate)
                        .Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["AccountLedger"] = getDonations;
                }
                else
                {
                    var getDonations = await con.donations.Where(d => d.dtId == dtId)
                       .Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["AccountLedger"] = getDonations;
                }
            }
            catch (Exception)
            {

            }

            return PartialView();
        }

        public async Task<IActionResult> DonorList()
        {
            try
            {
                var getDonors = await con.peoples.Where(p => p.roleId == 3 || p.roleId == 5).ToListAsync();
                TempData["Donors"] = getDonors;
                return View();
            }
            catch (Exception)
            {

            }
            return View();
        }

        public IActionResult DonationList()
        {
            PopulateDonor();
            return View();
        }

        public async Task<IActionResult> _DonationList(int? perId, DateTime fDate, DateTime tDate)
        {
            try
            {
                if (perId == null && fDate.Year == 1 && tDate.Year == 1)
                {
                    var getDonations = await con.donations.Where(d => d.dType == "Giver")
                        .Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["Donations"] = getDonations;
                }
                else if (perId != null && fDate.Year == 1 && tDate.Year == 1)
                {
                    var getDonations = await con.donations.Where(d => d.dType == "Giver" && d.perId == perId)
                        .Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["Donations"] = getDonations;
                }
                else if (perId != null && fDate.Year != 1 && tDate.Year != 1)
                {
                    var getDonations = await con.donations.Where(d => d.dType == "Giver" && d.perId == perId && d.CreatedDate >= fDate && d.CreatedDate <= tDate)
                        .Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["Donations"] = getDonations;
                }
                else if (fDate.Year != 1 && tDate.Year != 1)
                {
                    var getDonations = await con.donations.Where(d => d.dType == "Giver" && d.CreatedDate >= fDate && d.CreatedDate <= tDate)
                        .Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["Donations"] = getDonations;
                }
                else
                {
                    var getDonations = await con.donations.Where(d => d.dType == "Giver" && d.perId == perId)
                       .Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["Donations"] = getDonations;
                }
            }
            catch (Exception)
            {

            }

            return PartialView();
        }

        public async Task<IActionResult> DonationReceiverList()
        {
            try
            {
                var getTakers = await con.peoples.Where(p => p.roleId == 4 || p.roleId == 5).ToListAsync();
                TempData["Takers"] = getTakers;
                return View();
            }
            catch (Exception)
            {

            }
            return View();
        }

        public IActionResult DonationReceiverDetail()
        {
            PopulateTaker();
            return View();
        }

        public async Task<IActionResult> _DonationReceiverDetail(int? perId, DateTime fDate, DateTime tDate)
        {
            try
            {
                if (perId == null && fDate.Year == 1 && tDate.Year == 1)
                {
                    var getDonations = await con.donations.Where(d => d.dType == "Taker")
                        .Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["Taker"] = getDonations;
                }
                else if (perId != null && fDate.Year == 1 && tDate.Year == 1)
                {
                    var getDonations = await con.donations.Where(d => d.dType == "Taker" && d.perId == perId)
                        .Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["Taker"] = getDonations;
                }
                else if (perId != null && fDate.Year != 1 && tDate.Year != 1)
                {
                    var getDonations = await con.donations.Where(d => d.dType == "Taker" && d.perId == perId && d.CreatedDate >= fDate && d.CreatedDate <= tDate)
                        .Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["Taker"] = getDonations;
                }
                else if (fDate.Year != 1 && tDate.Year != 1)
                {
                    var getDonations = await con.donations.Where(d => d.dType == "Taker" && d.CreatedDate >= fDate && d.CreatedDate <= tDate)
                        .Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["Taker"] = getDonations;
                }
                else
                {
                    var getDonations = await con.donations.Where(d => d.dType == "Taker" && d.perId == perId)
                       .Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["Taker"] = getDonations;
                }
            }
            catch (Exception)
            {

            }

            return PartialView();
        }

        public IActionResult DonorandReceiverList()
        {
            PopulateDonorandTaker();
            return View();
        }

        public async Task<IActionResult> _DonorandReceiverList(int? perId, DateTime fDate, DateTime tDate)
        {
            try
            {
                if (perId == null && fDate.Year == 1 && tDate.Year == 1)
                {
                    var getDonations = await con.donations.Where(p => p.Person.roleId == 5).Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["DonorandTaker"] = getDonations;
                }
                else if (perId != null && fDate.Year == 1 && tDate.Year == 1)
                {
                    var getDonations = await con.donations.Where(d => d.perId == perId)
                        .Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["DonorandTaker"] = getDonations;
                }
                else if (perId != null && fDate.Year != 1 && tDate.Year != 1)
                {
                    var getDonations = await con.donations.Where(d => d.perId == perId && d.CreatedDate >= fDate && d.CreatedDate <= tDate)
                        .Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["DonorandTaker"] = getDonations;
                }
                else if (fDate.Year != 1 && tDate.Year != 1)
                {
                    var getDonations = await con.donations.Where(d => d.Person.roleId == 5 && d.CreatedDate >= fDate && d.CreatedDate <= tDate)
                        .Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["DonorandTaker"] = getDonations;
                }
                else
                {
                    var getDonations = await con.donations.Where(d => d.perId == perId)
                       .Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["DonorandTaker"] = getDonations;
                }
            }
            catch (Exception)
            {

            }

            return PartialView();
        }

        public IActionResult SystemUsersReceivedDonation()
        {
            PopulateSystemUsers();
            return View();
        }

        public async Task<IActionResult> _SystemUsersRecievedDonation(int? perId, DateTime fDate, DateTime tDate)
        {
            try
            {
                if (perId == null && fDate.Year == 1 && tDate.Year == 1)
                {
                    var getDonations = await con.donations.Where(p => p.dType == "Giver").Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["ReceivedDonation"] = getDonations;
                }
                else if (perId != null && fDate.Year == 1 && tDate.Year == 1)
                {
                    var getDonations = await con.donations.Where(d => d.CreatedBy == perId && d.dType == "Giver")
                        .Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["ReceivedDonation"] = getDonations;
                }
                else if (perId != null && fDate.Year != 1 && tDate.Year != 1)
                {
                    var getDonations = await con.donations.Where(d => d.CreatedBy == perId && d.dType == "Giver" && d.CreatedDate >= fDate && d.CreatedDate <= tDate)
                        .Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["ReceivedDonation"] = getDonations;
                }
                else if (fDate.Year != 1 && tDate.Year != 1)
                {
                    var getDonations = await con.donations.Where(d => d.dType == "Giver" && d.CreatedDate >= fDate && d.CreatedDate <= tDate)
                        .Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["ReceivedDonation"] = getDonations;
                }
                else
                {
                    var getDonations = await con.donations.Where(d => d.CreatedBy == perId && d.dType == "Giver")
                       .Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["ReceivedDonation"] = getDonations;
                }
            }
            catch (Exception)
            {

            }

            return PartialView();
        }

        public IActionResult SystemUsersDistributeDonation()
        {
            PopulateSystemUsers();
            return View();
        }

        public async Task<IActionResult> _SystemUsersDistributeDonation(int? perId, DateTime fDate, DateTime tDate)
        {
            try
            {
                if (perId == null && fDate.Year == 1 && tDate.Year == 1)
                {
                    var getDonations = await con.donations.Where(p => p.dType == "Taker").Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["ReceivedDonation"] = getDonations;
                }
                else if (perId != null && fDate.Year == 1 && tDate.Year == 1)
                {
                    var getDonations = await con.donations.Where(d => d.CreatedBy == perId && d.dType == "Taker")
                        .Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["ReceivedDonation"] = getDonations;
                }
                else if (perId != null && fDate.Year != 1 && tDate.Year != 1)
                {
                    var getDonations = await con.donations.Where(d => d.CreatedBy == perId && d.dType == "Taker" && d.CreatedDate >= fDate && d.CreatedDate <= tDate)
                        .Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["ReceivedDonation"] = getDonations;
                }
                else if (fDate.Year != 1 && tDate.Year != 1)
                {
                    var getDonations = await con.donations.Where(d => d.dType == "Taker" && d.CreatedDate >= fDate && d.CreatedDate <= tDate)
                        .Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["ReceivedDonation"] = getDonations;
                }
                else
                {
                    var getDonations = await con.donations.Where(d => d.CreatedBy == perId && d.dType == "Taker")
                       .Include(i => i.Donation_Type).Include(i => i.Person).ToListAsync();
                    TempData["ReceivedDonation"] = getDonations;
                }
            }
            catch (Exception)
            {

            }

            return PartialView();
        }


        public IActionResult ExpenseList()
        {
            return View();
        }

        public IActionResult ExpenseDetail()
        {
            return View();
        }

        public IActionResult SystemUsersList()
        {
            return View();
        }

        public IActionResult SystemLoginsList()
        {
            return View();
        }

        public IActionResult SystemUsersBlockedList()
        {
            return View();
        }

        public IActionResult SystemUsersActiveList()
        {
            return View();
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

        public void PopulateDonorandTaker()
        {
            try
            {
                var GetData = (from p in con.peoples
                               where (p.roleId == 5) && p.perStatus == "Active"
                               select new
                               {
                                   p.perId,
                                   firstName = p.firstName + " " + p.lastName + " (" + p.cnic + ")"
                               }).ToList();

                SelectList data = new SelectList(GetData, "perId", "firstName");
                ViewData["DonorandTaker"] = data;
            }
            catch (Exception)
            {

            }
        }

        public void PopulateSystemUsers()
        {
            try
            {
                var GetData = (from p in con.peoples
                               where (p.roleId == 1 || p.roleId == 2) && p.perStatus == "Active"
                               select new
                               {
                                   p.perId,
                                   firstName = p.firstName + " " + p.lastName + " (" + p.cnic + ")"
                               }).ToList();

                SelectList data = new SelectList(GetData, "perId", "firstName");
                ViewData["SystemUsers"] = data;
            }
            catch (Exception)
            {

            }
        }
    }
}
