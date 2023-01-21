using hanmudo.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RadicalTherapy.Data.Repository;
using System.Security.Claims;

namespace hanmudo.Controllers
{
    [Authorize(Roles ="Admin",AuthenticationSchemes = "AdminAuth")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public  IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync(string username,string password)
        {
            using (UnitOfWork db=new UnitOfWork())
            {
                if (db.KeyValueRepository.Get(c=>c.key== "" && c.Value==username).Any()&& db.KeyValueRepository.Get(c => c.key == "" && c.Value == password).Any())
                {
                    var claims = new List<Claim>() {
                    new Claim(ClaimTypes.NameIdentifier, username ),
                    new Claim(ClaimTypes.Role,"Admin")
                };
                    var identity = new ClaimsIdentity(claims, "AdminAuth");

                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync("AdminAuth", principal, new AuthenticationProperties()
                    {
                        IsPersistent = true
                    });
                }
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync("AdminAuth");
            return RedirectToAction("Login");
        }
        public IActionResult Event()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                var model = db.EventRepository.Get().ToList();
                return View(model);
            }
        }

        public IActionResult Events()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                var events = db.EventRepository.Get().ToList();
                return View(events);
            }
        }
        public IActionResult AddOrEditEvent(int id)
        {
            if (id == 0)
            {
                return View();
            }
            else
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    return View(db.EventRepository.GetByID(id));
                }
            }
        }
        [HttpPost]
        public IActionResult AddOrEditEvent(Event model, IFormFile img1, IFormFile img2)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                if (model.Id == 0)
                {
                    db.EventRepository.Insert(model);
                    db.Save();
                    var Event = db.EventRepository.Get().Last();
                    Event.img1 = (img1 == null) ? null : Utiliteis.Utility.UploadImage("galery\\event", img1, Event.Id.ToString() + "_img1.jpg");
                    Event.img2 = (img2 == null) ? null : Utiliteis.Utility.UploadImage("galery\\event", img2, Event.Id.ToString() + "_img2.jpg");
                    db.EventRepository.Update(Event);
                    db.Save();
                }
                else
                {
                    var evnt = db.EventRepository.GetByID(model.Id);
                    model.img1 = (img1 == null) ? evnt.img1 : Utiliteis.Utility.UploadImage("galery\\event", img1, model.Id.ToString() + "_img1.jpg");
                    model.img2 = (img2 == null) ? evnt.img2 : Utiliteis.Utility.UploadImage("galery\\event", img2, model.Id.ToString() + "_img2.jpg");
                    using (UnitOfWork db1 = new UnitOfWork())
                    {
                        db1.EventRepository.Update(model);
                        db1.Save();
                    }
                }
            }
            return RedirectToAction("Events");
        }
        public IActionResult deleteEvent(int id)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                db.EventRepository.Delete(id);
                db.Save();
            }
            string PathImg1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\galery\\event\\" + id.ToString() + "_img1.jpg");
            string PathImg2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\galery\\event\\" + id.ToString() + "_img2.jpg");
            FileInfo img1 = new FileInfo(PathImg1);
            FileInfo img2 = new FileInfo(PathImg2);
            if (img1.Exists)
            {
                img1.Delete();
            }
            if (img2.Exists)
            {
                img2.Delete();
            }
            return RedirectToAction("Events");
        }
        public IActionResult Infos()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                var infos = db.InfoRepository.Get();
                return View(infos);
            }
        }
        public IActionResult AddOrEditInfo(int id)
        {
            if (id == 0)
            {
                return View();
            }
            else
            {
                using (UnitOfWork db = new UnitOfWork())
                {
                    return View(db.InfoRepository.GetByID(id));
                }
            }
        }
        [HttpPost]
        public IActionResult AddOrEditInfo(Info model, IFormFile img)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                if (model.Id == 0)
                {
                    db.InfoRepository.Insert(model);
                    db.Save();
                    var Info = db.InfoRepository.Get().Last();
                    Info.img = (img == null) ? null : Utiliteis.Utility.UploadImage("galery\\Info", img, Info.Id.ToString() + "_img.jpg");
                    db.InfoRepository.Update(Info);
                    db.Save();
                }
                else
                {
                    var Info = db.InfoRepository.GetByID(model.Id);
                    model.img = (img == null) ? Info.img : Utiliteis.Utility.UploadImage("galery\\Info", img, model.Id.ToString() + "_img.jpg");
                    using (UnitOfWork db1 = new UnitOfWork())
                    {
                        db1.InfoRepository.Update(model);
                        db1.Save();
                    }
                }
            }
            return RedirectToAction("Infos");
        }
        public IActionResult deleteInfo(int id)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                db.InfoRepository.Delete(id);
                db.Save();
            }
            string PathImg = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\galery\\Info\\" + id.ToString() + "_img.jpg");
            FileInfo img = new FileInfo(PathImg);
            if (img.Exists)
            {
                img.Delete();
            }
            return RedirectToAction("Infos");
        }
        public IActionResult Photos()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                return View(db.PhotoRepository.Get().OrderBy(c => c.Sequence));
            }
        }
        public IActionResult AddOrEditPhoto(int id)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                if (id == 0)
                {
                    return View();
                }
                else
                {
                    return View(db.PhotoRepository.GetByID(id));
                }
            }
        }
        [HttpPost]
        public IActionResult AddOrEditPhoto(Photo model, IFormFile img)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                if (model.Id == 0)
                {
                    db.PhotoRepository.Insert(model);
                    db.Save();
                    var photo = db.PhotoRepository.Get().Last();
                    photo.text = (img == null) ? null : Utiliteis.Utility.UploadImage("galery\\photo", img, photo.Id.ToString() + "_img.jpg");
                    db.PhotoRepository.Update(photo);
                    db.Save();
                }
                else
                {
                    var photo = db.PhotoRepository.GetByID(model.Id);
                    model.text = (img == null) ? photo.text : Utiliteis.Utility.UploadImage("galery\\photo", img, model.Id.ToString() + "_img.jpg");
                    using (UnitOfWork db1 = new UnitOfWork())
                    {
                        db1.PhotoRepository.Update(model);
                        db1.Save();
                    }
                }
                return RedirectToAction("Photos");
            }
        }
        public IActionResult DeletePhoto(int id)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                db.PhotoRepository.Delete(id);
                db.Save();
                string PathImg = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\galery\\photo\\" + id.ToString() + "_img.jpg");
                FileInfo img = new FileInfo(PathImg);
                if (img.Exists)
                {
                    img.Delete();
                }
                return RedirectToAction("Photos");
            }
        }
        public IActionResult Videos()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                return View(db.VideoRepository.Get().OrderBy(c => c.Sequence).ToList());
            }
        }
        public IActionResult AddOrEditVideo(int id)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                if (id == 0)
                {
                    return View();
                }
                else
                {
                    return View(db.VideoRepository.GetByID(id));
                }
            }
        }
        [HttpPost]
        public IActionResult AddOrEditVideo(Video model)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                if (model.Id==0)
                {
                    db.VideoRepository.Insert(model);
                }
                else
                {
                    db.VideoRepository.Update(model);
                }
                db.Save();
                return RedirectToAction("Videos");
            }
        }
        public IActionResult DeleteVideo(int id)
        {
            using (UnitOfWork db=new UnitOfWork())
            {
                db.VideoRepository.Delete(id);
                db.Save();
            }
            return RedirectToAction("Videos");
        }


    }
}
