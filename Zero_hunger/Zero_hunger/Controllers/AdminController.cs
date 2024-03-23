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
    public class adminController : Controller
    {
        // GET: employee
        [adminAccess]
        public ActionResult Index()
        {
            return View();
        }
        [adminAccess]
        public ActionResult employeeList()
        {
            var db = new zhEntities();
            return View(db.Employees.ToList());
        }
        [adminAccess]
        [HttpGet]
        public ActionResult restaurantDetails(int id)
        {
            var db = new zhEntities();
            return View(db.Restaurants.Find(id));
        }
        [adminAccess]
        public ActionResult requestlist()
        {
            var db = new zhEntities();
            return View(db.Requests.ToList());
        }
        [adminAccess]
        [HttpGet]
        public ActionResult requestDetails(int id)
        {
            var db = new zhEntities();
            ViewBag.empList = db.Employees.ToList();
            return View(db.Requests.Find(id));
        }
        [adminAccess]
        [HttpPost]
        public ActionResult requestDetails(int id, assignEmpDto obj)
        {
            var db = new zhEntities();
            if (ModelState.IsValid)
            {
                Employee emp = db.Employees.Find(obj.emp_id);
                if (emp == null)
                {
                    TempData["msg"] = "Employee id does not exist";
                }
                else
                {
                    Request rq = db.Requests.Find(id);
                    rq.employee_id = obj.emp_id;
                    rq.admin_id = (int)Session["id"];
                    db.Requests.AddOrUpdate(rq);
                    db.SaveChanges();
                    TempData["msg"] = emp.username + "has been assigned for request id " + id;
                    return RedirectToAction("requestlist");
                }
            }
            return View(db.Requests.Find(id));
        }
        [adminAccess]
        public ActionResult deleteEmployee(int id)
        {
            var db = new zhEntities();
            db.Employees.Remove(db.Employees.Find(id));
            db.SaveChanges();
            TempData["msg"] = "Employee of ID " + id + " has been deleted";
            return RedirectToAction("employeelist");
        }
        [HttpPost]
        public ActionResult login(loginDTO obj)
        {
            if (ModelState.IsValid)
            {
                var db = new zhEntities();
                var user = (from u in db.Admins
                            where
                                u.username.Equals(obj.username) &&
                                u.password.Equals(obj.password)
                            select u).SingleOrDefault();
                if (user != null)
                {
                    Session["user"] = user.username;
                    Session["id"] = user.id;
                    Session["type"] = "admin";
                    TempData["msg"] = "Successfully login";
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
        public ActionResult login()
        {
            return View();
        }
        public ActionResult logout()
        {
            Session.Clear();
            return RedirectToAction("login");
        }
        [HttpGet]
        [adminAccess]
        public ActionResult addEmployee()
        {
            return View();
        }
        [HttpPost]
        [adminAccess]
        public ActionResult addEmployee(addEmployeeDTO empModel)
        {
            if (ModelState.IsValid)
            {
                var db = new zhEntities();
                db.Employees.Add(convert(empModel));
                db.SaveChanges();
                TempData["msg"] = "New employee is added successfully";
                return RedirectToAction("employeelist");
            }
            return View(empModel);
        }
        [HttpGet]
        [adminAccess]
        public ActionResult editEmployee(int id)
        {
            var db = new zhEntities();
            Employee emp = db.Employees.Find(id);
            editEmployeeDTO empDTO = new editEmployeeDTO()
            {
                id = emp.id,
                username = emp.username,
                name = emp.name,
                email = emp.email,
                phone = emp.phone,
                address = emp.address,
                //dob = emp.dob
            };
            return View(empDTO);
        }
        [HttpPost]
        [adminAccess]
        public ActionResult editEmployee(editEmployeeDTO empModel)
        {
            if (ModelState.IsValid)
            {
                var db = new zhEntities();
                Employee emp = db.Employees.Find(empModel.id);

                emp.username = empModel.username;
                emp.name = empModel.name;
                emp.email = empModel.email;
                emp.phone = empModel.phone;
                emp.address = empModel.address;
                //emp.dob = empModel.dob;

                db.Employees.AddOrUpdate(emp);
                db.SaveChanges();
                TempData["msg"] = "Information of employee of ID " + empModel.id.ToString() + " is edited successfully";
                return RedirectToAction("employeelist");
            }
            return View(empModel);
        }
        Employee convert(addEmployeeDTO empDTO)
        {
            Employee emp = new Employee()
            {
                name = empDTO.name,
                email = empDTO.email,
                phone = empDTO.phone,
                address = empDTO.address,
                //dob = empDTO.dob,
                username = empDTO.username,
                password = empDTO.password,
                admin_id = (int)(int?)Session["id"]
            };
            return emp;
        }
    }
}
