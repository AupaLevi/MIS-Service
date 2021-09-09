using MIS_Service.Models;
using System.IO;
using System.Data;
using System.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ClosedXML.Excel;
using System.Configuration;
using System.Data.SqlClient;

namespace MIS_Service.Controllers
{
    public class ExportController : Controller
    {
        public ActionResult ExportExcel()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ExportData()
        {
            SQLServerConnector sqlServerConnector = new SQLServerConnector();
            List<PostDataObject> listPosts = new List<PostDataObject>();
            listPosts = sqlServerConnector.GetPostsList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("MIS Service");
                var currentRow = 1;

                #region Header                                                             
                //worksheet.Cell(currentRow, 1).Value = "Tic01";             //Excel欄位修正   currentRow要修改
                worksheet.Cell(currentRow, 1).Value = "申請日期";       
                worksheet.Cell(currentRow, 2).Value = "收件日期";
                worksheet.Cell(currentRow, 3).Value = "表單序號 ";
                worksheet.Cell(currentRow, 4).Value = "申請部門";
                worksheet.Cell(currentRow, 5).Value = "申請目的";
                worksheet.Cell(currentRow, 6).Value = "申請內容";
                worksheet.Cell(currentRow, 7).Value = "預定完成日";
                worksheet.Cell(currentRow, 8).Value = "工單結案日";
                #endregion

                #region Body
                foreach (var ticket in listPosts)
                {
                    currentRow++;
                    //worksheet.Cell(currentRow, 1).Value = ticket.Tic01;   //Excel欄位修正   currentRow要修改
                    worksheet.Cell(currentRow, 1).Value = ticket.Tic02;
                    worksheet.Cell(currentRow, 2).Value = ticket.Tic03;
                    worksheet.Cell(currentRow, 3).Value = ticket.Tic04;
                    worksheet.Cell(currentRow, 4).Value = ticket.Tic05;
                    worksheet.Cell(currentRow, 5).Value = ticket.Tic06;
                    worksheet.Cell(currentRow, 6).Value = ticket.Tic07;
                    worksheet.Cell(currentRow, 7).Value = ticket.Tic08;
                    worksheet.Cell(currentRow, 8).Value = ticket.Tic09;
                }
                #endregion

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "MIS Service.xlsx"
                        );
                }
            }
        }
    }
}