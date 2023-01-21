using hanmudo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RadicalTherapy.Data.Repository;

namespace hanmudo.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "AdminAuth")]
    public class Admin3Controller : Controller
    {
        public IActionResult Students()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                var users = db.UserRepository.Get().ToList();
                var usermodel = new List<UserViewModel>();
                foreach (var item in users)
                {
                    usermodel.Add(new UserViewModel { User = item, belt = db.BeltRepository.Get(c => c.Code == item.belt).First().Text });
                }
                return View(usermodel);
            }
        }
        public IActionResult AddOrEditStudent(int id)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                ViewBag.belts = db.BeltRepository.Get().ToList();
                if (id == 0)
                {
                    return View();
                }
                else
                {
                    var user = db.UserRepository.GetByID(id);
                    var usermodel = new UserViewModel() { User = user };

                    return View(usermodel);
                }
            }
        }
        [HttpPost]
        public IActionResult AddOrEditStudent(UserViewModel model)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                User user = model.User;
                user.belt = Convert.ToInt32(model.belt);
                if (model.User.Id == 0)
                {
                    user.CreationDate = DateTime.Now;
                    db.UserRepository.Insert(user);
                }
                else
                {
                    db.UserRepository.Update(user);
                }
                db.Save();
            }
            return RedirectToAction("Students");
        }
        public IActionResult deleteUser(int id)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                db.UserRepository.Delete(id);
                db.Save();
                return RedirectToAction("Students");
            }
        }
        public IActionResult Darian()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                var model = db.DarianRepository.Get().OrderBy(c => c.Sequence).ToList();
                return View(model);
            }
        }
        public IActionResult AddOrEditVideoDarian(int id)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                if (id == 0)
                {
                    return View();
                }
                else
                {
                    var model = db.DarianRepository.GetByID(id);
                    return View(model);
                }
            }
        }
        [HttpPost]
        public IActionResult AddOrEditVideoDarian(IFormFile vid, Darian model)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                if (model.Id == 0)
                {
                    db.DarianRepository.Insert(model);
                    db.Save();
                    var darian = db.DarianRepository.Get().Last();
                    darian.video = (vid == null) ? null : Utiliteis.Utility.UploadImage("galery\\darian", vid, darian.Id.ToString() + "_Darian.mp4");
                    db.DarianRepository.Update(darian);
                    db.Save();
                }
                else
                {
                    var darian = db.DarianRepository.GetByID(model.Id);
                    model.video = (vid == null) ? darian.video : Utiliteis.Utility.UploadImage("galery\\darian", vid, darian.Id.ToString() + "_Darian.mp4");
                    using (UnitOfWork db1 = new UnitOfWork())
                    {
                        db1.DarianRepository.Update(model);
                        db1.Save();
                    }
                }
            }
            return RedirectToAction("Darian");
        }
        public IActionResult deleteDarian(int id)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                db.DarianRepository.Delete(id);
                db.Save();
                string Pathvideo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\galery\\darian\\" + id.ToString() + "_Darian.mp4");
                FileInfo video = new FileInfo(Pathvideo);
                if (video.Exists)
                {
                    video.Delete();
                }
                return RedirectToAction("Darian");
            }
        }
        public IActionResult selfDefence()
        {
            using (UnitOfWork db = new UnitOfWork())
            {

                return View(db.SelfDefenceTeknikRepository.Get().OrderBy(c => c.Sequence));
            }
        }
        
        public IActionResult AddOrEditSelfDefence(int id)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                if (id == 0)
                {
                   
                    return View();
                }
                else
                {

                    return View(db.SelfDefenceTeknikRepository.GetByID(id));
                }
            }
        }
        [HttpPost]
        public IActionResult AddOrEditSelfDefence(SelfDefence model)
        {
            using (UnitOfWork db=new UnitOfWork())
            {
                if (model.Id==0)
                {
                    db.SelfDefenceTeknikRepository.Insert(model);
                }
                else
                {
                    db.SelfDefenceTeknikRepository.Update(model);
                }
                db.Save();
            }
            return RedirectToAction("selfDefence");
        }
        public IActionResult deleteSelfDefence(int id)
        {
            using (UnitOfWork db=new UnitOfWork())
            {
                db.SelfDefenceTeknikRepository.Delete(id);
                db.Save();
            }
            return RedirectToAction("selfDefence");
        }
    }
}
