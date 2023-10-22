using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using NPOI.SS.Formula.Functions;
using System.Linq;
using LILI_FPMS.Models;
using System.Drawing;
using Newtonsoft.Json;
using System.Web.Helpers;
using System.Xml;
using TMS.Models;
using TMS.Models.ReportsViewModels;
using NPOI.POIFS.Crypt.Dsig;

namespace TMS.Controllers
{
    [Authorize]
    public class ReportController : Controller
    {
        private readonly dbToolsManagementContext _context;
        private readonly IConfiguration _configuration;

        public ReportController(dbToolsManagementContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Home()
        {
          
           var tsaList= ( from c in _context.TblTsasetup
                          from a in _context.TblToolAssign
                          where c.Tsacode == a.Tsacode
                          select new TblTsasetup 
                          { 
                           Tsacode=c.Tsacode,
                           Tsaname=c.Tsaname
                          }
                          ).ToList();
            tsaList.Insert(0, new TblTsasetup { Tsacode="0",Tsaname="<-- Select a TSA -->"});
            ViewBag.ListOfTsa = tsaList;
            return View();
        }
        public ActionResult ToolStockReport()
        {
            var data = new List<ToolStockVM>();
            string connString = _configuration.GetConnectionString("DefaultConnection");
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand(@"sp_ToolStockReport", conn))
                    {
                        //var parameters = new SqlParameter[]
                        //{
                        //    new SqlParameter("@year", year),
                        //    new SqlParameter("@month", month),
                        //    new SqlParameter("@MaterialCategory", materialCategory),
                        //    new SqlParameter("@subBusiness", subBusiness),
                        //    new SqlParameter("@dateFrom", dateFrom),
                        //    new SqlParameter("@dateTo", dateTo)
                        //};
                        //cmd.Parameters.AddRange(parameters);
                        conn.Open();

                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var dc = new ToolStockVM();
                                dc.SlNo = dr.GetInt32(0); 
                                dc.ToolCode = dr.GetString(1);
                                dc.ToolName = dr.GetString(2);
                                dc.Brand = dr.GetString(3);
                                dc.BalanceQty = dr.GetInt32(4);
                                dc.ActionType = dr.GetString(5);
                                dc.Comments = dr.GetString(6);

                                data.Add(dc);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }

            return PartialView("_ToolStockReport", data);

        }
        public ActionResult TsaReport()
        {

            var data = new List<TsaReportVM>();
            string connString = _configuration.GetConnectionString("DefaultConnection");
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand(@"sp_TsaReport", conn))
                    {
                        //var parameters = new SqlParameter[]
                        //{
                        //    new SqlParameter("@year", year),
                        //    new SqlParameter("@month", month),
                        //    new SqlParameter("@MaterialCategory", materialCategory),
                        //    new SqlParameter("@subBusiness", subBusiness),
                        //    new SqlParameter("@dateFrom", dateFrom),
                        //    new SqlParameter("@dateTo", dateTo)
                        //};
                        //cmd.Parameters.AddRange(parameters);
                        conn.Open();

                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var dc = new TsaReportVM();
                                dc.SlNo = dr.GetInt32(0);
                                dc.TSAName = dr.GetString(1);
                                dc.TSACode = dr.GetString(2);
                                dc.Designation = dr.GetString(3);
                                dc.AreaName = dr.GetString(4);
                                dc.RegionName = dr.GetString(5);
                                dc.ToolCode = dr.GetString(6);
                                dc.ToolName = dr.GetString(7);
                                dc.Brand = dr.GetString(8);
                                dc.Quantity = dr.GetInt32(9);
                                dc.ActionTypeName = dr.GetString(10);
                                dc.ActionDate = dr.GetDateTime(11);
                                dc.FileName = dr.GetString(12);
                                dc.OriginalFileName = dr.GetString(13);
                                dc.Location= dr.GetString(14);
                                dc.Description= dr.IsDBNull(15) ? null : dr.GetString(15); 

                                data.Add(dc);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }

            return PartialView("_TsaReportPartialView", data);
        }

        public ActionResult TsaWiseAssignReport(string TsaCode= "TSA-20231000002")
        {
            var data = new List<TsaWiseAssignReportVM>();
            string connString = _configuration.GetConnectionString("DefaultConnection");
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    using (SqlCommand cmd = new SqlCommand(@"sp_TsaWiseAssignReport", conn))
                    {
                        var parameters = new SqlParameter[]
                        {
                            //new SqlParameter("@year", year),
                            //new SqlParameter("@month", month),
                            //new SqlParameter("@MaterialCategory", materialCategory),
                            new SqlParameter("@Tsacode", TsaCode),
                            //new SqlParameter("@dateFrom", dateFrom),
                            //new SqlParameter("@dateTo", dateTo)
                        };
                        cmd.Parameters.AddRange(parameters);
                        conn.Open();

                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var dc = new TsaWiseAssignReportVM();
                                dc.SlNo = dr.GetInt32(0);
                                dc.TSACode = dr.GetString(1);
                                dc.TSAName = dr.GetString(2);
                                dc.Designation = dr.GetString(3);
                                dc.AreaName = dr.GetString(4);
                                dc.RegionName = dr.GetString(5);
                                dc.ToolCode = dr.GetString(6);
                                dc.ToolName = dr.GetString(7);
                                dc.Brand = dr.GetString(8);
                                dc.Qty = dr.GetDecimal(9);
                                dc.ActionTypeName = dr.GetString(10);
                                dc.ActionDate = dr.GetDateTime(11);
                                dc.Description = dr.IsDBNull(12) ? null : dr.GetString(12);

                                data.Add(dc);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }

            return PartialView("_TsaWiseAssignPartialView", data);
        }
    }

}