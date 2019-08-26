using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using belt.Models;

namespace belt.Controllers {
    public class HomeController : Controller {
        private BeltContext dbContext;
        public HomeController(BeltContext context){
            dbContext = context;
        }
        public IActionResult Index() {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(LoginRegViewModel modelData) {
            User creatingUser = modelData.newUser;
            if(ModelState.IsValid){
                if(dbContext.users.Any(u => u.Email == creatingUser.Email)){
                    ModelState.AddModelError("Email", "Email is already in use!");
                    return View("Index");
                } else {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    creatingUser.Password = Hasher.HashPassword(creatingUser, creatingUser.Password);
                    dbContext.Add(creatingUser);
                    dbContext.SaveChanges();
                    if(HttpContext.Session.GetInt32("UserId") == null){
                        HttpContext.Session.SetInt32("UserId", creatingUser.UserId);
                    }
                    System.Console.WriteLine("******************");
                    System.Console.WriteLine("reg working");
                    return RedirectToAction("Dashboard");
                }
            } else {
                System.Console.WriteLine("*******************");
                System.Console.WriteLine("REGISTRATION NOT WORKING!!!!");
                System.Console.WriteLine(creatingUser.FirstName);
                System.Console.WriteLine(creatingUser.LastName);
                System.Console.WriteLine(creatingUser.Email);
                System.Console.WriteLine("*******************");
                return View("Index");
            }
        }

        public IActionResult Login(LoginRegViewModel modelData){
            LoginReg userLogin = modelData.existingUser;
            if(ModelState.IsValid){
                User userInDB = dbContext.users.FirstOrDefault(u => u.Email == userLogin.Email);
                if(userInDB == null){
                    ModelState.AddModelError("Email", "Invalid email or password");
                    return View("Index");
                } else {
                    var hasher = new PasswordHasher<LoginReg>();
                    var result = hasher.VerifyHashedPassword(userLogin, userInDB.Password, userLogin.Password);
                    if(result == 0){
                        ModelState.AddModelError("Password", "Invalid email or password");
                        return View("Index");
                    }
                    if(HttpContext.Session.GetInt32("UserId") == null){
                        HttpContext.Session.SetInt32("UserId", userInDB.UserId);
                    }
                    return RedirectToAction("Dashboard");
                }
            } else {
                return View("Index");
            }
        }

        [HttpGet("dashboard")]
        public IActionResult Dashboard() {
            if(HttpContext.Session.GetInt32("UserId") == null){
                return RedirectToAction("Index");
            }
            int UserID = HttpContext.Session.GetInt32("UserId").Value;
            WrapperModel newModel = new WrapperModel();
            newModel.LoggedInUser = dbContext.users.SingleOrDefault(u => u.UserId == UserID);
            newModel.AllOccasions = dbContext.occasions.OrderBy(o => o.Date).ThenBy(t => t.Time).Include(o => o.Attendants).ThenInclude(a => a.Person).ToList();
            foreach(Occasion each in newModel.AllOccasions){
                each.CoordinatorUser = dbContext.users.SingleOrDefault(u => u.UserId == each.CoordinatorId);
                var dateString = DateTime.Now.ToShortTimeString();
                var eventString = each.Time.ToShortTimeString();
                DateTime d1 = DateTime.Parse(dateString);
                DateTime d2 = DateTime.Parse(eventString);
                DateTime.Compare(d1, d2);
            }
            System.Console.WriteLine("********************");
            System.Console.WriteLine(DateTime.Today);
            return View(newModel);
        }

        [HttpGet("add")]
        public IActionResult AddOccasionPage(){
            if(HttpContext.Session.GetInt32("UserId") == null){
                return RedirectToAction("Index");
            }
            int UserID = HttpContext.Session.GetInt32("UserId").Value;
            WrapperModel newModel = new WrapperModel();
            newModel.LoggedInUser = dbContext.users.SingleOrDefault(u => u.UserId == UserID);
            return View(newModel);
        }

        public IActionResult CreateActivity(WrapperModel modelData){
            int UserID = HttpContext.Session.GetInt32("UserId").Value;
            // User CurrentUser = dbContext.users.SingleOrDefault(u => u.UserId == UserID);
            Occasion OccasionToAdd = modelData.NewOccasion;
            if (ModelState.IsValid){
                OccasionToAdd.CoordinatorId = UserID;
                OccasionToAdd.CoordinatorUser = dbContext.users.SingleOrDefault(u => u.UserId == UserID);
                System.Console.WriteLine(OccasionToAdd.CoordinatorUser.FirstName);
                dbContext.Add(OccasionToAdd);
                dbContext.SaveChanges();
                System.Console.WriteLine("***********************");
                System.Console.WriteLine(OccasionToAdd.CoordinatorUser.FirstName);
                return RedirectToAction("Dashboard");
            }
            WrapperModel newModel = new WrapperModel();
            newModel.LoggedInUser = dbContext.users.SingleOrDefault(u => u.UserId == UserID);
            // OccasionToAdd.Coordinator = CurrentUser;
            System.Console.WriteLine("**********************");
            System.Console.WriteLine("Create not working");
            // System.Console.WriteLine(CurrentUser.FirstName);
            // System.Console.WriteLine(OccasionToAdd.Coordinator.FirstName);
            return View("AddOccasionPage", newModel);
        }

        [HttpGet("activity/{OccasionId}")]
        public IActionResult Info(int OccasionId){
            if(HttpContext.Session.GetInt32("UserId") == null){
                return RedirectToAction("Index");
            }
            int UserID = HttpContext.Session.GetInt32("UserId").Value;
            Occasion thisOcc = dbContext.occasions.Include(o => o.Attendants).ThenInclude(a => a.Person).SingleOrDefault(to => to.OccasionId == OccasionId);
            thisOcc.CoordinatorUser = dbContext.users.SingleOrDefault(u => u.UserId == thisOcc.CoordinatorId);
            WrapperModel newModel = new WrapperModel();
            newModel.LoggedInUser = dbContext.users.SingleOrDefault(u => u.UserId == UserID);
            newModel.ThisOccasion = thisOcc;
            System.Console.WriteLine("***********************");
            System.Console.WriteLine(newModel.ThisOccasion.CoordinatorUser.UserId);
            return View(newModel);
        }

        [HttpPost("{UserId}/joinActivity{ThingId}")]
        public IActionResult JoinActivity(int UserId, int ThingId){
            Association newAssociation = new Association();
            newAssociation.PersonId = UserId;
            newAssociation.ThingId = ThingId;
            dbContext.Add(newAssociation);
            dbContext.SaveChanges();
            WrapperModel newModel = new WrapperModel();
            newModel.LoggedInUser = dbContext.users.SingleOrDefault(u => u.UserId == UserId);
            newModel.AllOccasions = dbContext.occasions.OrderBy(o => o.Date).Include(o => o.Attendants).ThenInclude(a => a.Person).ToList();
            return RedirectToAction("Dashboard", newModel);
        }

        [HttpPost("{UserId}/leaveActivity{ThingId}")]
        public IActionResult LeaveActivity(int UserId, int ThingId){
            Association FindAssociation = dbContext.associations.FirstOrDefault(a => a.ThingId == ThingId && a.PersonId == UserId);
            dbContext.Remove(FindAssociation);
            dbContext.SaveChanges();
            WrapperModel newModel = new WrapperModel();
            newModel.LoggedInUser = dbContext.users.SingleOrDefault(u => u.UserId == UserId);
            newModel.AllOccasions = dbContext.occasions.OrderBy(o => o.Date).Include(o => o.Attendants).ThenInclude(a => a.Person).ToList();
            return RedirectToAction("Dashboard", newModel);
        }

        [HttpPost("cancelActivity/{activityId}")]
        public IActionResult CancelActivity(int activityId){
            int UserId = HttpContext.Session.GetInt32("UserId").Value;
            Occasion OccasionToRemove = dbContext.occasions.SingleOrDefault(o => o.OccasionId == activityId);
            dbContext.Remove(OccasionToRemove);
            dbContext.SaveChanges();
            WrapperModel newModel = new WrapperModel();
            newModel.LoggedInUser = dbContext.users.SingleOrDefault(u => u.UserId == UserId);
            newModel.AllOccasions = dbContext.occasions.OrderBy(o => o.Date).Include(o => o.Attendants).ThenInclude(a => a.Person).ToList();
            return RedirectToAction("Dashboard", newModel);
        }

        public IActionResult Logout(){
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(){
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
