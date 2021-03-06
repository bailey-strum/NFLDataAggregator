﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NFLData.Controllers.DataControllers;
using NFLData.Models;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NFLData.Controllers
{
    public class TeamController : Controller
    {
        //shows all the nfl teams in a nice front end ish display
        public ActionResult Teams()
        {
            return View("Teams");
        }

        //takes the selected team from the teams view and then displays all pertinent information
        public ActionResult TeamChoice(string name, string picture)
        {
            //takes the image from the team selection and passes it through to the display page using viewbag
            ViewBag.Logo = string.Format("/FixedImages/{0}", picture);

            TeamDataController dc = new TeamDataController("DefaultConnection");

            TeamModel tm = dc.GetTeam(name);

            return View(tm);
        }

        //team map controller that feeds data to the teammap view
        public ActionResult TeamMap()
        {
            TeamDataController dc = new TeamDataController("DefaultConnection");

            IEnumerable<TeamLocation> tm = dc.GetTeamLocation();

            return View(tm);
        }
    }
}