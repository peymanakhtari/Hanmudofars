using hanmudo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RadicalTherapy.Data.Repository;
using System.Text;

namespace hanmudo.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "AdminAuth")]
    public class Admin4Controller : Controller
    {
        private UnitOfWork db = new UnitOfWork();
        private MemoryStream ms = new MemoryStream();
        public IActionResult InfoUser()
        {
            var infos = new List<InfoUserViewModel>();
            foreach (var item in db.InfoUserRepository.Get().OrderByDescending(c => c.Sequence))
            {
                infos.Add(new InfoUserViewModel { infoUser = item, belt = db.BeltRepository.Get(c => c.Code == item.belt).First().Text });
            }
            return View(infos);
        }
        public IActionResult AddOrEditInfoUser(int id)
        {
            ViewBag.belts = db.BeltRepository.Get().ToList();
            if (id == 0)
            {
                return View();
            }
            else
            {
                return View(db.InfoUserRepository.GetByID(id));
            }
        }
        [HttpPost]
        public IActionResult AddOrEditInfoUser(InfoUser model)
        {
            if (model.Id == 0)
            {
                model.datetime = DateTime.Now;
                db.InfoUserRepository.Insert(model);
            }
            else
            {
                db.InfoUserRepository.Update(model);
            }
            db.Save();
            return RedirectToAction("InfoUser");
        }
        public IActionResult deleteInfoUser(int id)
        {
            db.InfoUserRepository.Delete(id);
            db.Save();
            return RedirectToAction("InfoUser");
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        public IActionResult ResetInfo(int id)
        {
            foreach (var item in db.SeenInfoUserRepository.Get(c => c.InfoUserId == id))
            {
                db.SeenInfoUserRepository.Delete(item.Id);
            }
            db.Save();
            return RedirectToAction("InfoUser");
        }
        public IActionResult StudentSeenInfo(int id)
        {
            List<User> model = db.UserRepository.Get(c => c.belt >= db.InfoUserRepository.GetByID(id).belt).ToList();
            foreach (var item in db.SeenInfoUserRepository.Get(c => c.InfoUserId == id))
            {
                model.RemoveAll(c => c.Id == item.UserId);
            }
            return View(model);
        }
        public IActionResult RoleContents()
        {

            return View(db.RoleContentRepository.Get().OrderBy(c => c.Category).ThenBy(c => c.Sequence));
        }
        public IActionResult AddOrEditContentRole(int id)
        {
            List<KeyValue> categories = new List<KeyValue>();
            categories.Add(new KeyValue() { key = "قوانین کلاسی", Value = "1" });
            categories.Add(new KeyValue() { key = "قوانین مربی‌گری", Value = "2" });
            categories.Add(new KeyValue() { key = "قوانین ‌داوری", Value = "3" });
            ViewBag._categories = categories;

            List<KeyValue> types = new List<KeyValue>();
            types.Add(new KeyValue() { key = "تیتر", Value = "1" });
            types.Add(new KeyValue() { key = "متن", Value = "2" });
            types.Add(new KeyValue() { key = "عکس", Value = "3" });
            types.Add(new KeyValue() { key = "ویدیو", Value = "4" });
            ViewBag._types = types;
            return View(db.RoleContentRepository.GetByID(id));
        }
        [HttpPost]
        public IActionResult AddOrEditContentRole(RoleContent model, IFormFile img, IFormFile vid)
        {

            if (model.Type == 3)
            {
                if (img == null)
                {
                    return RedirectToAction("RoleContents");
                }
                else
                {
                    img.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    string pic = "data:image/jpeg;base64," + Convert.ToBase64String(fileBytes);
                    var bytes = Encoding.ASCII.GetBytes(pic);
                    model.img = bytes;
                }
            }
            else
            {
                if (model.Type == 4)
                {
                    model.img = null;
                    if (vid != null)
                    {
                        if (model.Id == 0)
                        {
                            model.Text = (vid == null) ? null : Utiliteis.Utility.UploadImage("galery\\roleContent", vid, Guid.NewGuid().ToString() + ".mp4");
                        }
                        else
                        {
                            model.Text = (vid == null) ? null : Utiliteis.Utility.UploadImage("galery\\roleContent", vid, model.Text);
                        }
                    }
                }
            }
            if (model.Id == 0)
            {
                db.RoleContentRepository.Insert(model);
            }
            else
            {
                db.RoleContentRepository.Update(model);
            }
            db.Save();
            return RedirectToAction("RoleContents");
        }
        public IActionResult DeleteContentRole(int id)
        {
            db.RoleContentRepository.Delete(id);
            string Pathvideo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\galery\\roleContent\\" + db.RoleContentRepository.GetByID(id).Text);
            FileInfo video = new FileInfo(Pathvideo);
            if (video.Exists)
            {
                video.Delete();
            }
            db.Save();

            return RedirectToAction("RoleContents");
        }
    }
}
