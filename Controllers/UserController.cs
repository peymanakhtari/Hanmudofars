using hanmudo.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RadicalTherapy.Data.Repository;
using System.Security.Claims;

namespace hanmudo.Controllers
{
    [Authorize(Roles ="User",AuthenticationSchemes ="UserAuth")]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            using (UnitOfWork db = new UnitOfWork())
            {
                var student = db.UserRepository.Get(c => c.UserName == user).First();
                ViewBag.belts = db.BeltRepository.Get().ToList();
                ViewBag.seenCount = Utiliteis.Utility.SeenCount(student);
                return View(student);
            }
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                var belts = db.BeltRepository.Get().ToList();
                ViewBag.message = TempData["message"];
                return View(belts);
            }
        }
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> LoginAsync(string user, string pass)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                User student = db.UserRepository.Get(c => c.UserName == user && c.Password == pass).FirstOrDefault();
                if (student == null)
                {
                    TempData["message"] = "نام کاربری یا کلمه عبور اشتباه است";
                    return RedirectToAction("Login");
                }
                if (student.Expire < DateTime.Now)
                {
                    TempData["message"] = "جهت تمدید عضویت به هان مودو فارس مراجعه نمایید";
                    return RedirectToAction("Login");
                }

                var claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, user ),
                    new Claim(ClaimTypes.Role,"User")
                };
                var identity = new ClaimsIdentity(claims, "UserAuth");

                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("UserAuth", principal, new AuthenticationProperties()
                {
                    IsPersistent = false
                });
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync("UserAuth");
            return RedirectToAction("Login");
        }
        public IActionResult ConfirmRules()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            using (UnitOfWork db = new UnitOfWork())
            {
                var student = db.UserRepository.Get(c => c.UserName == user).First();
                student.ConfirmRules = true;
                db.UserRepository.Update(student);
                db.Save();
                return RedirectToAction("Index");
            }
        }

        public IActionResult Teknik(int Id)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!Utiliteis.Utility.CheckAccess(db.BeltRepository.GetByID(Id).Code, db.UserRepository.Get(c => c.UserName == user).First().belt))
                {
                    return RedirectToAction("LogOut");
                }
                var model = db.TeknikRepository.Get(c => c.BeltId == Id).OrderBy(c => c.Sequence).ToList();
                ViewBag.beltName = db.BeltRepository.GetByID(Id).Text;
                return View(model);
            }
        }
        public IActionResult Category(int Id)
        {

            using (UnitOfWork db = new UnitOfWork())
            {
                int beltId = db.TeknikRepository.GetByID(Id).BeltId;
                var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!Utiliteis.Utility.CheckAccess(db.BeltRepository.GetByID(beltId).Code, db.UserRepository.Get(c => c.UserName == user).First().belt))
                {
                    return RedirectToAction("LogOut");
                }
                var model = db.CategoryTeknikRepository.Get(c => c.TeknikId == Id).OrderBy(c => c.Sequence).ToList();
                ViewBag.categoryName = db.TeknikRepository.GetByID(Id).Text;
                return View(model);
            }
        }
        public IActionResult Content(int Id)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                int teknikId = db.CategoryTeknikRepository.GetByID(Id).TeknikId;
                int beltId = db.TeknikRepository.GetByID(teknikId).BeltId;
                var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!Utiliteis.Utility.CheckAccess(db.BeltRepository.GetByID(beltId).Code, db.UserRepository.Get(c => c.UserName == user).First().belt))
                {
                    return RedirectToAction("LogOut");
                }

                ViewBag.tittle = db.CategoryTeknikRepository.GetByID(Id).Text;
                var model = db.ContentTeknikRepository.Get(c => c.CategoryTeknikId == Id).OrderBy(c => c.Sequence).ToList();
                return View(model);
            }
        }
        public IActionResult Darian()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                return View(db.DarianRepository.Get().OrderBy(c => c.Sequence).ToList());
            }
        }
        public IActionResult AsamiEstelahat()
        {
            return View();
        }
        public IActionResult Info()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            using (UnitOfWork db = new UnitOfWork())
            {
                var User = db.UserRepository.Get(c => c.UserName == user).First();
                List<InfoUserViewModel> infos = new List<InfoUserViewModel>();
                foreach (var item in db.InfoUserRepository.Get(c => c.belt <= User.belt && (c.Show || c.datetime >= User.CreationDate)))
                {
                    var infouser = new InfoUserViewModel() { infoUser = item };
                    if (db.SeenInfoUserRepository.Get().Any(c => c.UserId == User.Id && c.InfoUserId == item.Id))
                    {
                        infouser.seen = true;
                    }
                    infos.Add(infouser);
                }
                return View(infos.OrderBy(c => c.seen).ThenByDescending(c => c.infoUser.Sequence));
            }
        }
        public IActionResult InfoIsSeen(int id)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);
            using (UnitOfWork db = new UnitOfWork())
            {
                var User = db.UserRepository.Get(c => c.UserName == user).First();
                db.SeenInfoUserRepository.Insert(new SeenInfoUser { UserId = User.Id, InfoUserId = id });
                db.Save();
            }
            return RedirectToAction("Info");
        }
        public IActionResult Roles(int id)
        {
            string title="";
            if (id==1)
            {
                title = "قوانین کلاسی";
            }
            if (id == 2)
            {
                title = "قوانین مربی‌گری‌";
            }
            if (id == 3)
            {
                title = "قوانین داوری";
            }
            ViewBag.titl = title;
            using (UnitOfWork db = new UnitOfWork())
            {
                return View(db.RoleContentRepository.Get(c=>c.Category==id).OrderBy(c=>c.Sequence));
            }
        }
    }
}
