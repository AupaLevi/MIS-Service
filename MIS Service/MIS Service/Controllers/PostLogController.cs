using MIS_Service.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;


namespace MIS_Service.Controllers
{
    public class PostLogController : Controller
    {
        public ActionResult AddPostedLog()
        {
            SQLServerConnector sqlServerConnector = new SQLServerConnector();
            //PageBean pageBean = new PageBean();
            List<PostDataLog> listLogs;
            List<PageBean> pageNumberList;
            //listPosts = sqlServerConnector.GetPostsList();
            int pageTotalCount = sqlServerConnector.GetTotalLogCount();


            PageOperation pageOperation = new PageOperation(pageTotalCount);
            int currentPage = 1;
            pageNumberList = pageOperation.GetPageNumberList();
            int pageCount = pageOperation.GetPageCount();
            int startPageNum = pageOperation.GetStartNumber(currentPage);
            int endPageNum = pageOperation.GetEndNumber();
            int previousPageNumber = 1;
            int nextPageNumber = previousPageNumber + 1;
            listLogs = sqlServerConnector.GetLimitLogsList("0", "5");

            PostLogViewModel postLogViewModel = new PostLogViewModel();
            postLogViewModel.PostLogList = listLogs;
            postLogViewModel.PageNumberList = pageNumberList;
            postLogViewModel.PageCount = pageCount;
            postLogViewModel.CurrentPage = currentPage;
            postLogViewModel.StrPageNum = startPageNum;
            postLogViewModel.EndPageNum = endPageNum;
            postLogViewModel.PreviousPageNumber = previousPageNumber;
            postLogViewModel.NextPageNumber = nextPageNumber;
            postLogViewModel.FirstPage = true;
            postLogViewModel.LastPage = false;

            //ViewBag.ListOfPosts = listPosts;
            //return View(listPosts);
            return View(postLogViewModel);
        }

        public ActionResult PostLogDetail(String postID)
        {
            String sqlCriteria = "";
            if (postID != null && !postID.IsEmpty())
            {
                if (postID.StartsWith("*"))
                {
                    postID = postID.Remove(1, 1);
                }
                if (postID.EndsWith("*"))
                {
                    postID = postID.Remove(postID.Length - 1, postID.Length);
                }
                sqlCriteria = "tig01 LIKE '%" + postID + "%' ";
            }

            SQLServerConnector sqlServerConnector = new SQLServerConnector();
            List<PostDataLog> listLogs;
            PostDataLog postDataLogForDetail;
            listLogs = sqlServerConnector.GetPostsLogOnDemand(sqlCriteria);
            postDataLogForDetail = listLogs[0];
            //ViewBag.ListOfPosts = listPosts;
            ViewBag.PostLogForDetail = postDataLogForDetail;
            //return View("PostDetail", listPosts);



            return View("PostLogDetail", postDataLogForDetail);
        }

        public ActionResult ToPage(String postID)
        {
            SQLServerConnector sqlServerConnector = new SQLServerConnector();
            //PageBean pageBean = new PageBean();
            List<PostDataLog> listLogs;
            List<PageBean> pageNumberList;
            //listPosts = sqlServerConnector.GetPostsList();
            int pageTotalCount = sqlServerConnector.GetTotalLogCount();

            PageOperation pageOperation = new PageOperation(pageTotalCount);
            int currentPage = int.Parse(postID);
            pageNumberList = pageOperation.GetPageNumberList();
            int pageCount = pageOperation.GetPageCount();
            int startPageNum = pageOperation.GetStartNumber(currentPage);
            int endPageNum = pageOperation.GetEndNumber();
            int previousPageNumber = 1;
            int nextPageNumber = previousPageNumber + int.Parse(postID);
            if (currentPage == 1)
            {
                startPageNum = 0;
            }
            listLogs = sqlServerConnector.GetLimitLogsList(startPageNum.ToString(), "5");

            PostLogViewModel postLogViewModel = new PostLogViewModel();
            postLogViewModel.PostLogList = listLogs;
            postLogViewModel.PageNumberList = pageNumberList;
            postLogViewModel.PageCount = pageCount;
            postLogViewModel.CurrentPage = currentPage;
            postLogViewModel.StrPageNum = startPageNum;
            postLogViewModel.EndPageNum = endPageNum;
            postLogViewModel.PreviousPageNumber = previousPageNumber;
            postLogViewModel.NextPageNumber = nextPageNumber;
            if (startPageNum == 0)
            {
                postLogViewModel.FirstPage = true;
            }
            else
            {
                postLogViewModel.FirstPage = false;
            }
            if (currentPage == pageNumberList.Count)
            {
                postLogViewModel.LastPage = true;
            }
            else
            {
                postLogViewModel.LastPage = false;
            }


            TempData["postLogViewModel"] = postLogViewModel;

            return Redirect("BackToAddPostedLog");
        }

        [HttpPost, ActionName("PostLogDetail")]
        public ActionResult InputLogBackToTicket(String postID)
        {
            SQLServerConnector sqlServerConnector = new SQLServerConnector();
            string btTicketString = sqlServerConnector.getSelecttigDataSQL(postID);
            DataTable dataTable;
            string sqlResult;
            List<PostDataObject> goodpostLogToTicket;
            List<PostDataObject> insertedpostLogToTicket;

            PostDataObject postDataObject;
            string actionResult;
            int dataCount;

            dataTable = sqlServerConnector.GetDataTable(btTicketString);
            sqlResult = "";
            goodpostLogToTicket = new List<PostDataObject>();

            if (dataTable.Rows.Count >0)
            {
                postDataObject = new PostDataObject();
                foreach(DataRow row in dataTable.Rows)
                {
                    sqlResult = "Y";

                    try
                    {
                        postDataObject.Tic01 = row[dataTable.Columns["tig01"]].ToString();
                        postDataObject.Tic02 = row[dataTable.Columns["tig02"]] == DBNull.Value ? "" :
                            Convert.ToDateTime(row[dataTable.Columns["tig02"]]).ToString("yyyy-MM-dd");
                        postDataObject.Tic03 = row[dataTable.Columns["tig03"]].ToString();
                        postDataObject.Tic04 = row[dataTable.Columns["tig04"]].ToString();
                        postDataObject.Tic05 = row[dataTable.Columns["tig05"]].ToString();
                        postDataObject.Tic06 = row[dataTable.Columns["tig06"]].ToString();
                        postDataObject.Tic07 = row[dataTable.Columns["tig07"]].ToString();
                        postDataObject.Tic08 = row[dataTable.Columns["tig08"]].ToString();
                        postDataObject.Tic09 = row[dataTable.Columns["tig09"]].ToString();
                    }
                    catch (Exception ex)
                    {
                        sqlResult = "N";
                        Console.WriteLine("Foreach Exception:" + ex.Message);
                        break;
                    }
                    finally
                    {
                        if (sqlResult == "Y")
                        {
                            dataCount = 0;
                            dataCount = sqlServerConnector.SelectTicRowCounts(postDataObject.Tic01);
                            if (dataCount ==0)
                            {
                                goodpostLogToTicket.Add(postDataObject);
                            }
                        }
                    }
                }
                actionResult = "FAILED";
                insertedpostLogToTicket = new List<PostDataObject>();

                if (goodpostLogToTicket.Count > 0)
                {
                    foreach (PostDataObject postInsTicket in goodpostLogToTicket)
                    {
                        actionResult = sqlServerConnector.InsertPostData(postDataObject);
                        if (actionResult == "SUCCESS")
                        {
                            insertedpostLogToTicket.Add(postInsTicket);
                        }
                    }
                }
            }
            string DelResult = sqlServerConnector.ConfirmedLogDelete(postID);

            return RedirectToAction("AddPostedLog", "PostLog");
        }

        public ActionResult BackToAddPostedLog()
        {
            PostLogViewModel postLogViewModel = (PostLogViewModel)TempData["postLogViewModel"];
            return View("AddPostedLog", postLogViewModel);
        }
    }
}