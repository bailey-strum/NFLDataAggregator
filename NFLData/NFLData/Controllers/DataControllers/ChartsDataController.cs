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
    public class ChartsDataController : Controller
    {
        public static SqlDatabase db;

        //connection string code to make SQL connection through the web config
        public ChartsDataController(string connectionstring)
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

        //Grabs the selected player and gets their yards for the 2016 season
        public YardageModel GetYards(string quarterback)
        {
            // Readies stored proc from server.
            DbCommand getYards = db.GetStoredProcCommand("sp_getYardage");//returns the quarterback and total yardage for the season

            //adds a parameter
            db.AddInParameter(getYards, "@quarterback", DbType.String, quarterback);

            // Executes stored proc to return values into a DataSet.
            DataSet ds = db.ExecuteDataSet(getYards);

            DataRow dr = ds.Tables[0].Rows[0];

            YardageModel yards = new YardageModel()
            {
                Quarterback = dr.Field<string>("Quarterback"),
                Yards = dr.Field<int>("Yards")
            };

            return yards;
        }

        //Grabs the two selected quarterbacks and then compares their yardage in a chart
        public StatComparison CompareQuarterbacks(string quarterback1,string quarterback2)
        {
            // Readies stored proc from server.
            DbCommand getYards = db.GetStoredProcCommand("sp_GetChartStats");

            //adds two parameters
            db.AddInParameter(getYards, "@Quarterback", DbType.String, quarterback1);
            db.AddInParameter(getYards, "@Quarterback2", DbType.String, quarterback2);

            // Executes stored proc to return values into a DataSet.
            DataSet ds = db.ExecuteDataSet(getYards);

            DataRow dr = ds.Tables[0].Rows[0];

            StatComparison stat = new StatComparison()
                             {
                                //qb number one
                                 QBName = dr.Field<string>("Quarterback"),
                                 QBYards = dr.Field<int>("Yards"),
                                 QBTouchdown = dr.Field<int>("Touchdown"),
                                 QBRate = dr.Field<int>("Rate"),
                                 QBTotal = dr.Field<int>("TotalPoints"),

                                 //qb number two
                                 QB2Name = dr.Field<string>("Quarterback2"),
                                 QB2Yards = dr.Field<int>("Yards2"),
                                 QB2Touchdown = dr.Field<int>("Touchdown2"),
                                 QB2Rate = dr.Field<int>("Rate2"),
                                 QB2Total = dr.Field<int>("TotalPoints2"),

            };

            return stat;
        }

        //gets all qb names from the database
        public List<AllQuarterback> GetAllQBNames()
        {
            DbCommand getSB = db.GetStoredProcCommand("sp_GetAllQBNames");//returns the quarterback and total yardage for the season

            // Executes stored proc to return values into a DataSet.
            DataSet ds = db.ExecuteDataSet(getSB);

            var names = (from drRow in ds.Tables[0].AsEnumerable()
                         select new AllQuarterback()
                         {
                             Name = drRow.Field<string>("Quarterback"),
                             

                         }).ToList();

            return names;
        }
    }


}