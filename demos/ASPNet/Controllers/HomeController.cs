using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASPNet.Models;
using Microsoft.Data.Sqlite;

namespace ASPNet.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var data = new List<DemoModel>();

            using (var c = new SqliteConnection(MvcApplication.ConnectionString))
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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}