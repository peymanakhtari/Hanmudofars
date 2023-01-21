using MD.PersianDateTime;
using System.Globalization;

namespace hanmudo.Utiliteis
{
    public static class DateConvertor
    {

        public static PersianDateTime ToShamsi(this DateTime dt)
        {
            PersianCalendar pc = new PersianCalendar();
            PersianDateTime datetimePersianDateTime = new PersianDateTime(pc.GetYear(dt), pc.GetMonth(dt), pc.GetDayOfMonth(dt), pc.GetHour(dt), pc.GetMinute(dt), pc.GetSecond(dt));

            return datetimePersianDateTime;
        }
        
       
    }
}
