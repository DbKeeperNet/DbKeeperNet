using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASPNETCore.Models;
using Microsoft.Data.Sqlite;

namespace ASPNETCore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var data = new List<DemoModel>();

            using (var c = new SqliteConnection(Startup.ConnectionString))
            {
                c.Open();

                DbCommand cmd = c.CreateCommand();
                cmd.CommandText = "select * from DbKeeperNet_SimpleDemo";
                DbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    data.Add(new DemoModel {Id = reader.GetInt32(0), Name = reader.GetString(1)});
                }
            }

            return View(data);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
