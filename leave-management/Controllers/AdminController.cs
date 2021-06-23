using leave_management.Data;
using leave_management.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace leave_management.Controllers
{
    [Authorize(Roles = Helper.Admin + "," + Helper.Manager)]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _db;

        public AdminController(UserManager<ApplicationUser> userManager,ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }


        public async Task<IActionResult> Index()
        {
            var users = await _db.Users.ToListAsync();

            return View(users);
        }


        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var leaveList = _db.LeaveRequests.Where(u => u.ApplicationUserId == id);
            var user = _db.Users.FirstOrDefault(u => u.Id == id);

            ViewBag.userId = id;
            ViewBag.UserName = user.UserName;
            ViewBag.Email = user.Email;
            ViewBag.Firstname = user.Firstname;

            return View(leaveList);
        }

        public IActionResult Create(string id)
        {
            var user = _db.Users.FirstOrDefault(u => u.Id == id);
            ViewBag.userName = user.Email;
            ViewBag.id = user.Id;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLeave(LeaveRequest obj)
        {
            if (ModelState.IsValid)
            {
                obj.DateRequested = DateTime.Now;
                _db.LeaveRequests.Add(obj);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }




        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.LeaveRequests.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(LeaveRequest obj)
        {
            if (ModelState.IsValid)
            {
                _db.LeaveRequests.Update(obj);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(obj);
        }

        public async Task<ActionResult> Leaves()
        {
            var listAllLeaves = await _db.LeaveRequests.OrderByDescending(d => d.DateRequested)
                                .Include("ApplicationUser")
                                .ToListAsync();
            return View(listAllLeaves);
        }

        // GET - DELETE
        [Authorize(Roles = Helper.Admin)]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _db.LeaveRequests.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        // POST - DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Helper.Admin)]
        public async Task<ActionResult> DeleteLeave(int? id)
        {
            var obj = _db.LeaveRequests.Find(id.GetValueOrDefault());

            if (obj == null)
            {
                return NotFound();
            }
            _db.Remove(obj);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


    }
}
