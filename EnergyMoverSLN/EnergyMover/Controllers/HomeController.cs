using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Firebase.Database;
using Firebase.Database.Query;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using EnergyMover.Models;
using System.Globalization;
using Newtonsoft.Json;

namespace EnergyMover.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<DataPoint> p = new List<DataPoint>();
            p.Add(new DataPoint(1, 120, DateTime.Now.AddMonths(-5).ToString("MMMM")));
            p.Add(new DataPoint(2, 354, DateTime.Now.AddMonths(-4).ToString("MMMM")));
            p.Add(new DataPoint(3, 334, DateTime.Now.AddMonths(-3).ToString("MMMM")));
            p.Add(new DataPoint(4, 294, DateTime.Now.AddMonths(-2).ToString("MMMM")));
            p.Add(new DataPoint(5, 588, DateTime.Now.AddMonths(-1).ToString("MMMM")));
            p.Add(new DataPoint(6, 551, DateTime.Now.ToString("MMMM")));
            ViewBag.DataPoints = JsonConvert.SerializeObject(p);

            List<DataPoint> q = new List<DataPoint>();
            q.Add(new DataPoint(1, 120, DateTime.Now.AddMonths(-5).ToString("MMMM")));
            q.Add(new DataPoint(2, 354, DateTime.Now.AddMonths(-4).ToString("MMMM")));
            q.Add(new DataPoint(3, 334, DateTime.Now.AddMonths(-3).ToString("MMMM")));
            q.Add(new DataPoint(4, 294, DateTime.Now.AddMonths(-2).ToString("MMMM")));
            q.Add(new DataPoint(5, 588, DateTime.Now.AddMonths(-1).ToString("MMMM")));
            q.Add(new DataPoint(6, 551, DateTime.Now.ToString("MMMM")));
            ViewBag.DataPoints1 = JsonConvert.SerializeObject(q);

            ViewBag.MENProduced = 160;
            ViewBag.MENConsumed = 399;
            ViewBag.MCoins = 22;
            ViewBag.MShops = 13;
            
            ViewBag.YENProduced = 190;
            ViewBag.YENConsumed = 398;
            ViewBag.YCoins = 480;
            ViewBag.YShops = 148;

            /*
             Calcular o ranking
             Ir buscar tudo à base de dados e inserir em cima
             meter links para as barra lateral
             */

            return View();
        }

        public async Task<ActionResult> About()
        {
            //Simulate test user data and login timestamp
            var userId = "Aitor";
            var currentLoginTime = DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss");

            //Save non identifying data to Firebase
            var currentUserInfo = new PowerRatings() { powerInKW = 100, TimestampUtc = currentLoginTime };
            var firebaseClient = new FirebaseClient("https://energymover-8517f.firebaseio.com/");
            var result = await firebaseClient
              .Child("Users/" + userId + "/PowerData")
              .PostAsync(currentUserInfo);

            //Retrieve data from Firebase
            var dbOcurrences = await firebaseClient
              .Child("Users")
              .Child(userId)
              .Child("PowerData")
              .OnceAsync<PowerRatings>();

            var timestampList = new List<DateTime>();
            var power = new List<int>();

            //Convert JSON data to original datatype
            foreach (var ocurrence in dbOcurrences)
            {
                power.Add(ocurrence.Object.powerInKW);
                timestampList.Add(DateTime.ParseExact(ocurrence.Object.TimestampUtc, "MM/dd/yyyy HH:mm:ss", CultureInfo.CurrentCulture));
            }

            //Pass data to the view
            ViewBag.CurrentUser = userId;
            ViewBag.UserPower = power;
            ViewBag.UserInfo = timestampList;
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}