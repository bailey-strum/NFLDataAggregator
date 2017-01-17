﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Data;
using System.Data.Common;
using System.Web.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using NFLData.Models;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace NFLData.Controllers.DataControllers
{
    public class DataController : Controller
    {
        public static SqlDatabase db;

        public DataController(string connectionstring)
        {
            db = new SqlDatabase(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());

            if (connectionstring.Length > 0)
            {
                if (db == null)
                {
                    db = new SqlDatabase(connectionstring);
                }
            }
            else
            {
                if (db == null)
                {
                    db = new SqlDatabase(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                }
            }
        }
        //write more code down here
        public List<PlayerModel> GetPlayer(string Quarterback)
        {
            // Readies stored proc from server.
            DbCommand getQB = db.GetStoredProcCommand("sp_getQB");//write the stored proc

            db.AddInParameter(getQB, "@Quarterback", DbType.String, Quarterback);

            // Executes stored proc to return values into a DataSet.
            DataSet ds = db.ExecuteDataSet(getQB);

            var player = (from drRow in ds.Tables[0].AsEnumerable()
                          select new PlayerModel()
                          {

                              Quarterback = Quarterback,
                              Attempt = drRow.Field<int>("Attempt"),
                              Completion = drRow.Field<int>("Completion"),
                              Yards = drRow.Field<int>("Yards"),
                              YardsPerAttempt= drRow.Field<double>("YardsPerAttempt"),
                              Touchdown = drRow.Field<int>("Touchdown"),
                              Interception = drRow.Field<int>("Interception"),
                              Long = drRow.Field<int>("Long"),
                              Sack = drRow.Field<int>("Sack"),
                              Loss = drRow.Field<int>("Loss"),
                              Rate = drRow.Field<double>("Rate"),
                              TotalPoints = drRow.Field<int>("TotalPoints"),
                              HomeOrAway = drRow.Field<string>("HomeOrAway"),
                              Year = drRow.Field<string>("Year")
                          }).ToList();

            return player;
        }
    }
}