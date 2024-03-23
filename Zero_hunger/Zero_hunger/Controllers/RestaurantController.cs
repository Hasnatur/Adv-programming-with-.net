using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zero_hunger.Authorization;
using Zero_hunger.DB;
using Zero_hunger.DTO;

namespace Zero_hunger.Controllers
{
    public class RestaurantController : Controller
    {
        // GET: Restaurant
        [resAccess]
        [HttpGet]
        public ActionResult Index()
        {
            var db = new zhEntities();
            return View(db.Restaurants.Find((int)Session["id"]).Requests.ToList());
        }
        [resAccess]
        [HttpGet]
        public ActionResult add()
        {
            return View();
        }
        [resAccess]
        [HttpGet]
        public ActionResult clear()
        {
            Session["foodlist"] = null;
            return RedirectToAction("cart");
        }
        [resAccess]
        [HttpGet]
        public ActionResult checkout()
        {
            if (Session["foodlist"] == null)
            {
                TempData["msg"] = "The food list is empty!";
                return RedirectToAction("cart");
            }
            return View();
        }
        [resAccess]
        [HttpPost]
        public ActionResult checkout(processRequestDTO pr)
        {
            if (Session["foodlist"] == null)
            {
                TempData["msg"] = "The food list is empty!";
                return RedirectToAction("cart");
            }
            if (ModelState.IsValid)
            {
                var db = new zhEntities();
                    Request rq = new Request()
                {
                    status = "Processing",
                    order_time = DateTime.Now,
                    expire_time = pr.expire_datetime,
                    restaurant_id = (int)Session["id"]
                };
                db.Requests.Add(rq);
                foreach (var item in (List<addFoodDTO>)Session["foodlist"])
                {
                    Food fd = new Food()
                    {
                        type = item.type,
                        quantity = item.quantity,
                        request_id = rq.id
                    };
                    db.Foods.Add(fd);
                    rq.total_quantity += item.quantity;
                }
                db.SaveChanges();
                Session["foodlist"] = null;
                TempData["msg"] = "Successfully added an request";
                return RedirectToAction("cart");
            }
            return View(pr);
        }
        [resAccess]
        [HttpGet]
        public ActionResult requestdetails(int id)
        {
            var db = new zhEntities();
            return View(db.Requests.Find(id));
        }
        [resAccess]
        [HttpPost]
        public ActionResult add(addFoodDTO afDTO)
        {
            if (ModelState.IsValid)
            {
                List<addFoodDTO> foodList = null;
                if (Session["foodlist"] == null)
                {
                    foodList = new List<addFoodDTO>();
                }
                else
                {
                    foodList = (List<addFoodDTO>)Session["foodlist"];
                }
                foodList.Add(afDTO);
                Session["foodlist"] = foodList;
                TempData["msg"] = "New food is added to request(total food " + foodList.Count + ")";
                return RedirectToAction("cart");
            }
            return View(afDTO);
        }
        [resAccess]
        [HttpGet]
        public ActionResult cart()
        {
            return View((List<addFoodDTO>)Session["foodlist"]);
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
                var user = (from u in db.Restaurants
                            where
                                u.username.Equals(obj.username) &&
                                u.password.Equals(obj.password)
                            select u).SingleOrDefault();
                if (user != null)
                {
                    Session["user"] = user.username;
                    Session["id"] = user.id;
                    Session["type"] = "restaurant";
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
        [HttpGet]
        public ActionResult signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult signup(restSignUpDTO obj)
        {
            if (ModelState.IsValid)
            {
                var db = new zhEntities();
                db.Restaurants.Add(convert(obj));
                db.SaveChanges();
                TempData["msg"] = "Successfully signed up";
                return RedirectToAction("login");
            }
            return View(obj);
        }
        Restaurant convert(restSignUpDTO obj)
        {
            return new Restaurant()
            {
                name = obj.name,
                email = obj.email,
                phone = obj.phone,
                address = obj.address,
                username = obj.username,
                password = obj.password
            };
        }
    }
}