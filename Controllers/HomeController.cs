using hanmudo.Models;
using Microsoft.AspNetCore.Mvc;
using RadicalTherapy.Data.Repository;
using System.Diagnostics;
using System.Linq;

namespace hanmudo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {


            using (UnitOfWork db = new UnitOfWork())
            {
                var events = db.EventRepository.Get(c => c.MainPage).OrderBy(c => c.Sequence).ToList();
                ViewBag.events = events;
                ViewBag.info = db.InfoRepository.Get(c => c.MainPage).ToList();
                ViewBag.photos = db.PhotoRepository.Get(c => c.MainPage).OrderBy(c => c.Sequence).ToList();
            }
            return View();
        }

        public IActionResult Clubs()
        {
            return View();
        }
        public IActionResult Members()
        {
            return View();
        }
        public IActionResult PhotoGalery()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                return View(db.PhotoRepository.Get().OrderBy(c => c.Sequence).ToList());
            }
        }
        public IActionResult VideoArchive()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                return View(db.VideoRepository.Get().OrderBy(c => c.Sequence).ToList());
            }
        }
        public IActionResult English()
        {
            return View();
        }
        public IActionResult Info()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                var infos = db.InfoRepository.Get().OrderByDescending(c => c.Sequence).ToList();
                return View(infos);
            }
        }

        public IActionResult ContactUs()
        {
            if (TempData["message sent"] != null)
            {
                TempData["message sent"] = null;
                ViewBag.MessageSent = "is sent";
            }
            return View();
        }
        [HttpPost]
        public IActionResult ContactUs(string firstLastName, string mobile, string message)
        {
            TempData["message sent"] = "is sent";
            Utiliteis.Utility.Email("hanmudofars@yahoo.com", firstLastName, mobile + "___" + message, false);
            //Utiliteis.Utility.Email("peymanakhtari@yahoo.com", firstLastName, mobile + "___" + message, false);
            return RedirectToAction("ContactUs");
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult ExpandPhoto(string id)
        {
            ViewBag.img = id;
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult test()
        {
            return View();
        }
        //public IActionResult galery()
        //{
        //    using (UnitOfWork db=new UnitOfWork())
        //    {
        //        var list = db.PhotoRepository.Get();
        //        List<Photo> nlist = new List<Photo>();
        //        foreach (var item in list)
        //        {
        //            nlist.Add( new Photo {alt= "استاد علوی تبار دفاع شخصی قفل مفصل مبارزات کلاس دفاع شخصی در شیراز آموزش ام ام ای در شیراز رضا علوی تبار امین علوی تبار پلیس",Sequence=item.Sequence,text=item.text,Id=item.Id });

        //        }
        //        foreach (var item in nlist)
        //        {
        //            using (UnitOfWork db1=new UnitOfWork())
        //            {

        //            db1.PhotoRepository.Update(item);
        //            db1.Save();
        //            }
        //        }
        //        return View();
        //    }
        //}
    }
}