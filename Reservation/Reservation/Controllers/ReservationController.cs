using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reservation.Data;
using Reservation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Reservation.Controllers
{
    public class ReservationController: Controller
    {
        private readonly ApplicationDbContext _adb;

        private readonly UserManager<IdentityUser> _userManager;

        public ReservationController(ApplicationDbContext adb, UserManager<IdentityUser> userManager)
        {
            _adb = adb;
            _userManager = userManager;


        }

        
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Student"))
                {
                    IndexStd();
                }
                else if (User.IsInRole("Admin"))
                {
                   IndexAdmin();
                }
                else
                {
                    return NotFound();
                }
            }
            return View();
        }


        public IActionResult IndexAdmin()
        {


            var list = (from r in _adb.Reservations
                        join s in _adb.Students
                        on r.Student.Id equals s.Id
                        join rt in _adb.TypeReservations
                        on r.TypeReservation.Id equals rt.Id
                        orderby s.RestCount

                        select new ReservationViewModel
                        {
                            RId = s.Id,
                            Id = r.Id,
                            FirstName = s.FirstName,
                            LastName = s.LastName,
                            Email = s.Email,
                            Date = r.Date,
                            Name = rt.Name,
                            Cause = r.Cause,
                            Status = r.Status
                        }).ToList();

            return View(list.Where(d => d.Date >= DateTime.Today && d.Status == "Pending"));
        }

        public IActionResult IndexStd()
        {
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);// will give the user's userId

            var result = (from r in _adb.Reservations
                          join s in _adb.Students
                          on r.Student.Id equals s.Id
                          join rt in _adb.TypeReservations
                          on r.TypeReservation.Id equals rt.Id
                          where r.Student.Id == userId
                          select new ReservationViewModel
                          {
                              RId = s.Id,
                              Id = r.Id,
                              FirstName = s.FirstName,
                              LastName = s.LastName,
                              Email = s.Email,
                              Date = r.Date,
                              Name = rt.Name,
                              Cause = r.Cause,
                              Status = r.Status
                          }).ToList();

            return View(result);
        }




        [HttpGet]
        public IActionResult Create()
        {
            var list = _adb.TypeReservations;
            ViewBag.types = list.ToList();
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(ReservationModel st)
        {

            

            if (ModelState.IsValid)
            {

                var type = _adb.TypeReservations.FirstOrDefault(rt => rt.Id == st.TypeReservationId);
               
                var student = await _userManager.GetUserAsync(HttpContext.User);
              
                st.StudentId = student.Id;
                st.TypeReservationId = type.Id;
                _adb.Add(st);
                await _adb.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(st);
        }



        public async Task<ActionResult> Edit(int? Id)
        {

            var lists = _adb.TypeReservations;
            ViewBag.gen = lists.ToList();
            if (Id == null)
            {
                return NotFound();
            }

            var res = await _adb.Reservations.FindAsync(Id);
            if (res == null)
            {
                return NotFound();
            }
            return View(res);
        }

        [HttpPost]

        public async Task<ActionResult> Edit(ReservationModel stu)
        {
            if (ModelState.IsValid)
            {

                var type = _adb.TypeReservations.FirstOrDefault(rt => rt.Id == stu.TypeReservationId);

                var student = await _userManager.GetUserAsync(HttpContext.User);

                stu.StudentId = student.Id;
                
                stu.TypeReservationId = type.Id;
                _adb.Update(stu);
                await _adb.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(stu);
        }






        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var ress = await _adb.Reservations
                .FirstOrDefaultAsync(r => r.Id == Id);
            if (ress == null)
            {
                return NotFound();
            }

            return View(ress);
        }



        [HttpPost]

        public async Task<ActionResult> Delete(int Id)
        {

            var stdDetails = await _adb.Reservations.FindAsync(Id);
            _adb.Reservations.Remove(stdDetails);
            await _adb.SaveChangesAsync();

            return RedirectToAction("Index");



        }


        public void Increment(int id)
        {
            var usr = _adb.Reservations.Find(id);
            //var res = new Student();
            //var inc = usr.Student.resCount;
            //var incr = new Student().resCount;

            //var student = await _userManager.GetUserAsync(HttpContext.User);
            var u = _adb.Students.FirstOrDefault(s => s.Id == usr.StudentId);
            var inc = usr.Student.RestCount;
            u.RestCount = inc + 1;
            //int inc = Convert.ToInt32(usr.Student.resCount.ToString());
            //res.resCount += inc;
            //usr.Student.resCount = incr + 1;
            _adb.Update(usr);
            _adb.Update(u);
            _adb.SaveChanges();
        }

        public async Task<IActionResult> Confirm(int id)
        {
            var resr = _adb.Reservations.Find(id);
            if (resr.Status != "Approved")
            {
                Increment(id);
               
                resr.Status = "Approved";
                _adb.Update(resr);
                await _adb.SaveChangesAsync();
              
            }
            else
            {
                
            }

            return RedirectToAction("index");
        }

        public IActionResult Decline(int id)
        {
            var resr = _adb.Reservations.Find(id);

            if (resr.Status != "Declined")
            {
                
                resr.Status = "Declined";
                _adb.Update(resr);
                _adb.SaveChanges();
              

            }
            else
            {
              
            }

            return RedirectToAction("index");

        }

        [Authorize(Roles = "Admin")]
        public IActionResult IndexX()
        {


            var list = (from r in _adb.Reservations
                        join s in _adb.Students
                        on r.Student.Id equals s.Id
                        join rt in _adb.TypeReservations
                        on r.TypeReservation.Id equals rt.Id
                        orderby s.RestCount

                        select new ReservationViewModel
                        {
                            RId = s.Id,
                            Id = r.Id,
                            FirstName = s.FirstName,
                            LastName = s.LastName,
                            Email = s.Email,
                            Date = r.Date,
                            Name = rt.Name,
                            Cause = r.Cause,
                            Status = r.Status
                        }).ToList();

            return View(list);
        }

    }
}
