using hanmudo.Models;
using MD.PersianDateTime;
using RadicalTherapy.Data.Repository;
using System.Net;
using System.Net.Mail;

namespace hanmudo.Utiliteis
{
    public class Utility
    {
        public static string UploadImage(string path, IFormFile img, string fileName)
        {
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\"+ path);
            string filePath = Path.Combine(uploadsFolder, fileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                img.CopyTo(fileStream);
            }
            return fileName;
        }
        public static string DateBuilder(PersianDateTime datetime)
        {
            string year = datetime.Year.ToString();
            string mounth = lessThanTen(datetime.Month);
            string day = lessThanTen(datetime.Day);

            return year + "/" + mounth + "/" + day;
        }
        public static string TimeBuilder(PersianDateTime datetime)
        {
            string hour = lessThanTen(datetime.Hour);
            string minute = lessThanTen(datetime.Minute);
            return minute + " : " + hour;
        }
        private static string lessThanTen(int val)
        {
            if (val < 10)
            {
                return "0" + val.ToString();
            }
            else
            {
                return val.ToString();
            }
        }
        public static bool CheckAccess(int belt, int userbelt)
        {
            using (UnitOfWork db = new UnitOfWork())
            {

                if (userbelt>belt)
                {
                    return true;
                }
            }
            return false;
        }
        public static int SeenCount(User User)
        {
            using (UnitOfWork db = new UnitOfWork())
            {
                int resutl = 0;
                foreach (var item in db.InfoUserRepository.Get(c => c.belt <= User.belt && (c.Show || c.datetime >= User.CreationDate)))
                {
                   
                    if (!db.SeenInfoUserRepository.Get().Any(c => c.UserId == User.Id && c.InfoUserId == item.Id))
                    {
                        resutl++;
                    }
                   
                }
            return resutl;
            }
        }
        public static string changePersianNumbersToEnglish(string input)//۱۲۳۴
        {
            string[] persian = new string[10] { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };

            for (int j = 0; j < persian.Length; j++)
                input = input.Replace(persian[j], j.ToString());

            return input;
        }
        public static string PersianToEnglish(string strNum)
        {
         string[] persion = { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };
         string[] english = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            string chash = strNum;
            for (int i = 0; i < 10; i++)
                chash = chash.Replace(persion[i], english[i]);
            return chash;
        }
        public static Task Email(string to,string sub, string body,bool html)
        {
            using (var client = new SmtpClient("mail.hanmudofars.ir", 25))
            {

                var credentials = new NetworkCredential()
                {
                    UserName = "Alavitabar@hanmudofars.ir", // without @gmail.com
                    Password = "2r6B[t5sUyDE9#"
                };

                client.Credentials = credentials;

                using var emailMessage = new MailMessage()
                {
                    To = { new MailAddress(to) },
                    From = new MailAddress("Alavitabar@hanmudofars.ir"), // with @gmail.com
                    Subject = sub,
                    Body = body,
                    IsBodyHtml = html
                };

                client.Send(emailMessage);
            }
            return Task.CompletedTask;
        }
    }
}
