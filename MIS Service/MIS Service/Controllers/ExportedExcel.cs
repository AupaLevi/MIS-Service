using MIS_Service.Models;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace MIS_Service.Controllers
{
    public class ExportedExcel : Controller
    {
       public ActionResult Exported()
        {
            SQLServerConnector sqlServerConnector = new SQLServerConnector();
            List<PostDataObject> listPosts;
            listPosts = sqlServerConnector.GetPostsList();

            //建立Excel
            ExcelPackage ep = new ExcelPackage();
            //建立第一個Sheet，後方為定義Sheet的名稱
            ExcelWorksheet sheet = ep.Workbook.Worksheets.Add("FirstSheet");

            int col = 1; //欄:直的，因為要從第1欄開始，所以初始為1

            
            
                

           return View();
        }
    }
}