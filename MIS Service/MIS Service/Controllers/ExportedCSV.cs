using MIS_Service.Models;
using System.IO;
using System.Data;
using System.Web;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using ClosedXML.Excel;
using System.Configuration;
using System.Data.SqlClient;

namespace MIS_Service.Controllers
{
    public class ExportedCSV : Controller
    {
        public ActionResult AddNewPost()
        {
            SQLServerConnector sqlServerConnector = new SQLServerConnector();
            List<PostDataObject> listPosts = new List<PostDataObject>();
            String constring = ConfigurationManager.ConnectionStrings["RConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = " SELECT * FROM tic_file ORDER BY tic01 DESC ";
            DataTable dt = new DataTable();
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            da.Fill(dt);
            con.Close();
            IList<PostDataObject> model = new List<PostDataObject>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                model.Add(new PostDataObject()
                {
                    Tic02 = dt.Rows[i]["Tic02"].ToString(),
                    Tic03 = dt.Rows[i]["Tic03"].ToString(),
                    Tic04 = dt.Rows[i]["Tic04"].ToString(),
                    Tic05 = dt.Rows[i]["Tic05"].ToString(),
                    Tic06 = dt.Rows[i]["Tic06"].ToString(),
                    Tic07 = dt.Rows[i]["Tic07"].ToString(),
                    Tic08 = dt.Rows[i]["Tic08"].ToString(),
                    Tic09 = dt.Rows[i]["Tic09"].ToString(),
                });
            }
                return View(model);
        }

        public ActionResult ExportData()
        {
            String constring = ConfigurationManager.ConnectionStrings["RConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constring);
            string query = " SELECT * FROM tic_file ORDER BY tic01 DESC ";
            DataTable dt = new DataTable();
            dt.TableName = "tic_file";
            con.Open();
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            da.Fill(dt);
            con.Close();

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= TicketsReport.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            return RedirectToAction("AddNewPost", "ExportToExcel");
        }

        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}