using hanmudo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RadicalTherapy.Data.Repository;
using System.Text;

namespace hanmudo.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "AdminAuth")]
    public class Admin2Controller : Controller
    {
        public IActionResult Teknik()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                var tekniks = db.TeknikRepository.Get().OrderBy(c => c.BeltId).ThenBy(c => c.Sequence).ToList();
                foreach (var item in tekniks)
                {
                    item.Belt = db.BeltRepository.GetByID(item.BeltId);
                }
                return View(tekniks);
            }
        }
        public IActionResult AddOrEditTeknik(int id)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                ViewBag.belts = db.BeltRepository.Get();
                if (id == 0)
                {
                    return View();
                }
                else
                {
                    return View(db.TeknikRepository.GetByID(id));
                }
            }
        }
        [HttpPost]
        public IActionResult AddOrEditTeknik(Teknik model)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                if (model.Id == 0)
                {
                    db.TeknikRepository.Insert(model);
                }
                else
                {
                    db.TeknikRepository.Update(model);
                }
                db.Save();
                return RedirectToAction("Teknik");
            }

        }
        public IActionResult deleteTeknik(int id)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                db.TeknikRepository.Delete(id);
                db.Save();
            }
            return RedirectToAction("Teknik");
        }
        public IActionResult CategoryTeknik(int id)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                var model = db.CategoryTeknikRepository.Get(c => c.TeknikId == id).OrderBy(c => c.Sequence).ToList();
                ViewBag.TeknikId = id;
                ViewBag.name = db.TeknikRepository.GetByID(id).Text;
                return View(model);
            }
        }

        public IActionResult AddOrEditCategory(int id, int TeknikId)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                ViewBag.TeknikId = TeknikId;
                if (id != 0)
                {
                    ViewBag.name = db.CategoryTeknikRepository.GetByID(id).Text;
                }
                return View(db.CategoryTeknikRepository.GetByID(id));
            }
        }
        [HttpPost]
        public IActionResult AddOrEditCategory(CategoryTeknik model)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                if (model.Id == 0)
                {
                    db.CategoryTeknikRepository.Insert(model);
                }
                else
                {
                    db.CategoryTeknikRepository.Update(model);
                }
                db.Save();
                return RedirectToAction("CategoryTeknik", new { id = model.TeknikId });
            }
        }
        public IActionResult deleteCategory(int id, int teknikId)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                db.CategoryTeknikRepository.Delete(id);
                db.Save();
                return RedirectToAction("CategoryTeknik", new { id = teknikId });
            }
        }
        public IActionResult ContentTeknik(int id)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                var contents = db.ContentTeknikRepository.Get(c => c.CategoryTeknikId == id).OrderBy(c => c.Sequence).ToList();
                ViewBag.categoryId = id;
                var category = db.CategoryTeknikRepository.GetByID(id);
                ViewBag.TeknikId = category.TeknikId;
                ViewBag.name = category.Text;
                return View(contents);
            }
        }
        public IActionResult AddOrEditContent(int id, int CategoryId)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                ViewBag.CategoryId = CategoryId;
                return View(db.ContentTeknikRepository.GetByID(id));
            }
        }
        [HttpPost]
        public IActionResult AddOrEditContent(ContentTeknik model, IFormFile pic, IFormFile vid)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                using (var ms = new MemoryStream())
                {
                    if (pic != null && model.Type == 2)
                    {
                        pic.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        string img = "data:image/jpeg;base64," + Convert.ToBase64String(fileBytes);
                        var bytes = Encoding.ASCII.GetBytes(img);
                        model.img = bytes;
                    }
                    else
                    {
                        if (model.Id != 0)
                        {
                            var content = db.ContentTeknikRepository.GetByID(model.Id);
                            model.img = content.img;
                        }
                    }

                }

                if (model.Id == 0)
                {
                    db.ContentTeknikRepository.Insert(model);
                    db.Save();
                    if (model.Type == 3)
                    {
                        var ct = db.ContentTeknikRepository.Get(c => c.Type == model.Type).Last();
                        ct.Text = (vid == null) ? null : Utiliteis.Utility.UploadImage("galery\\contentTeknik", vid, ct.Id.ToString() + "_contentTeknik.mp4");
                        db.ContentTeknikRepository.Update(ct);
                        db.Save();
                    }
                }
                else
                {
                    using (UnitOfWork db1 = new UnitOfWork())
                    {
                        if (model.Type == 3)
                        {
                            model.Text = (vid == null ) ? null : Utiliteis.Utility.UploadImage("galery\\contentTeknik", vid, model.Id.ToString() + "_contentTeknik.mp4");
                        }
                        db1.ContentTeknikRepository.Update(model);
                        db1.Save();
                    }
                }

                return RedirectToAction("ContentTeknik", new { id = model.CategoryTeknikId });
            }
        }
        public IActionResult deleteContent(int id, int CategoryId)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                db.ContentTeknikRepository.Delete(id);
                string Pathvideo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\galery\\contentTeknik\\" + id.ToString() + "_contentTeknik.mp4");
                FileInfo video = new FileInfo(Pathvideo);
                if (video.Exists)
                {
                    video.Delete();
                }
                db.Save();
                return RedirectToAction("ContentTeknik", new { id = CategoryId });
            }
        }
    }
}
