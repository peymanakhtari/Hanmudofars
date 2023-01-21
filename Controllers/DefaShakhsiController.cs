using Microsoft.AspNetCore.Mvc;
using RadicalTherapy.Data.Repository;

namespace hanmudo.Controllers
{
    public class DefaShakhsiController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Amoozesh_Defashakhsi_Ghoflemafsal()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                return View(db.SelfDefenceTeknikRepository.Get().OrderBy(c => c.Sequence));
            }
        }
    }
}
