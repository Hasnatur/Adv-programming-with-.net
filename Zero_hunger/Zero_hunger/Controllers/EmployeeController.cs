using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zero_hunger.Authorization;
using Zero_hunger.DB;
using Zero_hunger.DTO;

namespace Zero_hunger.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        [HttpGet]
        [emAccess]
        public ActionResult Index()
        {
            return View();
        }
        [emAccess]
        [HttpGet]
        public ActionResult collectRequest(int id)
        {
            var db = new zhEntities();
            Request rq = db.Requests.Find(id);
            if (rq == null)
            {
                TempData["msg"] = "Three is no request with id " + id.ToString();
            }
            else
            {
                rq.status = "collected";
                db.Requests.AddOrUpdate(rq);
                TempData["msg"] = "Request of id " + id.ToString() + " is set as collected";
                db.SaveChanges();
            }
            return RedirectToAction("requestlist");
        }
        [emAccess]
        [HttpGet]
        public ActionResult requestlist()
        {
            var db = new zhEntities();
            return View(db.Employees.Find((int)Session["id"]).Requests.ToList());
        }
        [emAccess]
        [HttpGet]
        public ActionResult requestdetails(int id)
        {
            var db = new zhEntities();
            return View(db.Requests.Find(id));
        }
        [emAccess]
        [HttpGet]
        public ActionResult restaurantDetails(int id)
        {
            var db = new zhEntities();
            return View(db.Restaurants.Find(id));
        }
        [HttpGet]
        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult login(loginDTO obj)
        {
            if (ModelState.IsValid)
            {
                var db = new zhEntities();
                var user = (from u in db.Employees
                            where
                                u.username.Equals(obj.username) &&
                                u.password.Equals(obj.password)
                            select u).SingleOrDefault();
                if (user != null)
                {
                    Session["user"] = user.username;
                    Session["id"] = user.id;
                    Session["type"] = "employee";
                    TempData["msg"] = "Successfully logged in";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["msg"] = "Invalid credential";
                }
            }
            return View(obj);
        }
        [HttpGet]
        public ActionResult logout()
        {
            Session.Clear();
            TempData["msg"] = "Successfully logged out";
            return RedirectToAction("login");
        }
    }
}