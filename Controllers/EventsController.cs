using Microsoft.AspNetCore.Mvc;
using RadicalTherapy.Data.Repository;

namespace hanmudo.Controllers
{
    public class EventsController : Controller
    {
        public IActionResult Index()
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                var events = db.EventRepository.Get().OrderByDescending(c => c.Sequence).ToList();
                return View(events);
            }
        }
        public IActionResult Detail(int id)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                var Event = db.EventRepository.GetByID(id);
                if (Event.LongText != null)
                {
                    string text = Event.LongText;
                    var val = text.Replace("*", "<br>");
                    Event.LongText = val;
                }
                return View(Event);
            }
        }
        

    }

}
