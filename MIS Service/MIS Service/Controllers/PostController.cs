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
    public class PostController : Controller
    {
        // GET: Post
        public ActionResult AddNewPost()
        {
            SQLServerConnector sqlServerConnector = new SQLServerConnector();
            //PageBean pageBean = new PageBean();
            List<PostDataObject> listPosts;
            List<PageBean> pageNumberList;
            //listPosts = sqlServerConnector.GetPostsList();
            int pageTotalCount = sqlServerConnector.GetTotalCount();


            PageOperation pageOperation = new PageOperation(pageTotalCount);
            int currentPage = 1;
            pageNumberList = pageOperation.GetPageNumberList();
            int pageCount = pageOperation.GetPageCount();
            int startPageNum = pageOperation.GetStartNumber(currentPage);
            int endPageNum = pageOperation.GetEndNumber();
            int previousPageNumber = 1;
            int nextPageNumber = previousPageNumber + 1;
            listPosts = sqlServerConnector.GetLimitPostsList("0", "5");

            PostDataViewModel postDataViewModel = new PostDataViewModel();
            postDataViewModel.PostDataList = listPosts;
            postDataViewModel.PageNumberList = pageNumberList;
            postDataViewModel.PageCount = pageCount;
            postDataViewModel.CurrentPage = currentPage;
            postDataViewModel.StrPageNum = startPageNum;
            postDataViewModel.EndPageNum = endPageNum;
            postDataViewModel.PreviousPageNumber = previousPageNumber;
            postDataViewModel.NextPageNumber = nextPageNumber;
            postDataViewModel.FirstPage = true;
            postDataViewModel.LastPage = false;

            //ViewBag.ListOfPosts = listPosts;
            //return View(listPosts);
            return View(postDataViewModel);
        }

        public ActionResult ToPage(String postID)
        {
            SQLServerConnector sqlServerConnector = new SQLServerConnector();
            //PageBean pageBean = new PageBean();
            List<PostDataObject> listPosts;
            List<PageBean> pageNumberList;
            //listPosts = sqlServerConnector.GetPostsList();
            int pageTotalCount = sqlServerConnector.GetTotalCount();

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
            listPosts = sqlServerConnector.GetLimitPostsList(startPageNum.ToString(), "5");

            PostDataViewModel postDataViewModel = new PostDataViewModel();
            postDataViewModel.PostDataList = listPosts;
            postDataViewModel.PageNumberList = pageNumberList;
            postDataViewModel.PageCount = pageCount;
            postDataViewModel.CurrentPage = currentPage;
            postDataViewModel.StrPageNum = startPageNum;
            postDataViewModel.EndPageNum = endPageNum;
            postDataViewModel.PreviousPageNumber = previousPageNumber;
            postDataViewModel.NextPageNumber = nextPageNumber;
            if (startPageNum == 0)
            {
                postDataViewModel.FirstPage = true;
            }
            else
            {
                postDataViewModel.FirstPage = false;
            }
            if (currentPage == pageNumberList.Count)
            {
                postDataViewModel.LastPage = true;
            }
            else
            {
                postDataViewModel.LastPage = false;
            }


            TempData["postDataViewModel"] = postDataViewModel;

            return Redirect("BackToAddNewPost");
        }

        public ActionResult BackToAddNewPost()
        {
            PostDataViewModel postDataViewModel = (PostDataViewModel)TempData["postDataViewModel"];
            return View("AddNewPost", postDataViewModel);
        }


        public ActionResult CreatePost()
        {
            return View();
        }
        // POST: Students/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "aaa03,aaa04,aaa05,aaa06,aaa07,aaa08,aaa09")] PostDataObject postDataObject)
        {
            SQLServerConnector sqlServerConnector = new SQLServerConnector();
            //List<PostDataObject> listPosts;
            //listPosts = sqlServerConnector.getPostsList();

            postDataObject.Aaa01 = DateTime.Now.ToString("yyyyMMddHHmmss");
            postDataObject.Aaa02 = DateTime.Now.ToString("yyyy-MM-dd");  //Post Date

            
            //postDataObject.Aaa07 = "";
            //postDataObject.Aaa08 = "";

            String result = sqlServerConnector.InsertPostData(postDataObject);

            return RedirectToAction("AddNewPost", "Post");

        }//Create

        public ActionResult DeletePost(String postID)
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
                sqlCriteria = "aaa01 LIKE '%" + postID + "%' ";
            }

            SQLServerConnector sqlServerConnector = new SQLServerConnector();
            List<PostDataObject> listPosts;
            listPosts = sqlServerConnector.GetPostsListOnDemand(sqlCriteria);
            ViewBag.ListOfPosts = listPosts;
            return View("ConfirmDelete", listPosts);
        }//End of DeletePost

        [HttpPost, ActionName("ConfirmedDelete")]
        public ActionResult ConfirmedDeletePost(String postID)
        {
            SQLServerConnector sqlServerConnector = new SQLServerConnector();
            List<PostDataObject> listPosts = new List<PostDataObject>();
            String result = sqlServerConnector.ConfirmedDelete(postID);
            if (result == "SUCCESS")
            {
                listPosts = sqlServerConnector.GetPostsList();
            }
            ViewBag.ListOfPosts = listPosts;

            //return View("AddNewPost", listPosts);
            return RedirectToAction("AddNewPost", "Post");
        }// End of ConfirmedDeletePost

        public ActionResult EditPost(String postID)
        {
            SQLServerConnector sqlServerConnector = new SQLServerConnector();
            List<PostDataObject> listPosts;
            PostDataObject postDataObjectForEdit;
            String sqlCriteria = "aaa01 = '" + postID + "'";

            listPosts = sqlServerConnector.GetPostsListOnDemand(sqlCriteria);

            postDataObjectForEdit = listPosts[0];
            //ViewBag.ListOfPosts = listPosts;
            ViewBag.PostDataForEdit = postDataObjectForEdit;

            return View("EditPost", postDataObjectForEdit);
        }

        [HttpPost, ActionName("ConfirmedEdit")]
        public ActionResult UpdatePost([Bind(Include = "Aaa01,Aaa02,Aaa03,Aaa04,Aaa05,Aaa06,Aaa07")] PostDataObject postDataObject)
        {
            SQLServerConnector sqlServerConnector = new SQLServerConnector();

            //postDataObject.Aaa02 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            postDataObject.Aaa08 = "";
            String result = sqlServerConnector.ConfirmedEdit(postDataObject);
            List<PostDataObject> listPosts = new List<PostDataObject>();
            if (result == "SUCCESS")
            {
                listPosts = sqlServerConnector.GetPostsList();
            }

            ViewBag.ListOfPosts = listPosts;

            //return View("AddNewPost", listPosts);
            return RedirectToAction("AddNewPost", "Post");
        }

        public ActionResult PostDetail(String postID)
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
                sqlCriteria = "aaa01 LIKE '%" + postID + "%' ";
            }

            SQLServerConnector sqlServerConnector = new SQLServerConnector();
            List<PostDataObject> listPosts;
            PostDataObject postDataObjectForDetail;
            listPosts = sqlServerConnector.GetPostsListOnDemand(sqlCriteria);
            postDataObjectForDetail = listPosts[0];
            //ViewBag.ListOfPosts = listPosts;
            ViewBag.PostDataForDetail = postDataObjectForDetail;
            //return View("PostDetail", listPosts);



            return View("PostDetail", postDataObjectForDetail);
        }

        [HttpPost, ActionName("PostDetail")]
        public ActionResult InputTicketsLog(String postID)
        {
            SQLServerConnector sqlServerConnector = new SQLServerConnector();
            string logSQLString = sqlServerConnector.getSelectaaaDataSQL(postID);
            DataTable dataTable;
            string sqlResult;
            List<PostDataLog> goodpostDataLog;
            List<PostDataLog> insertedpostDataLog;
            PostDataLog postDataLog;
            string actionResult;
            int dataCount;

            dataTable = sqlServerConnector.GetDataTable(logSQLString);
            sqlResult = "";

            goodpostDataLog = new List<PostDataLog>();

            if (dataTable.Rows.Count >0)
            {
                postDataLog = new PostDataLog();
                foreach (DataRow row in dataTable.Rows)
                {

                    sqlResult = "Y";

                    try
                    {
                        postDataLog.Hy_tic01 = row[dataTable.Columns["aaa01"]].ToString();
                        postDataLog.Hy_tic02 = (row[dataTable.Columns["aaa02"]]) == DBNull.Value ? "" :
                            Convert.ToDateTime(row[dataTable.Columns["aaa02"]]).ToString("yyyy-MM-dd");
                        postDataLog.Hy_tic03 = row[dataTable.Columns["aaa03"]].ToString();
                        postDataLog.Hy_tic04 = row[dataTable.Columns["aaa04"]].ToString();
                        postDataLog.Hy_tic05 = row[dataTable.Columns["aaa05"]].ToString();
                        postDataLog.Hy_tic06 = row[dataTable.Columns["aaa06"]].ToString();
                        postDataLog.Hy_tic07 = row[dataTable.Columns["aaa07"]].ToString();
                        postDataLog.Hy_tic08 = row[dataTable.Columns["aaa08"]].ToString();
                        postDataLog.Hy_tic09 = row[dataTable.Columns["aaa09"]].ToString();

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
                            dataCount = sqlServerConnector.SelectHy_TicRowCounts(postDataLog.Hy_tic01);
                            if (dataCount == 0)
                            {
                                goodpostDataLog.Add(postDataLog);
                            }
                           
                        }
                    }
                }
                actionResult = "FAILED";
                insertedpostDataLog = new List<PostDataLog>();

                if (goodpostDataLog.Count >0)
                {
                    foreach (PostDataLog postInsLog in goodpostDataLog)
                    {
                        actionResult = sqlServerConnector.InsertPostlog(postDataLog);
                        if (actionResult == "SUCCESS")
                        {
                            insertedpostDataLog.Add(postInsLog);
                        }
                    }
                }

            }

            return RedirectToAction("AddNewPost", "Post");
        }


        public ActionResult PostLogTickets()
        {
            SQLServerConnector sqlServerConnector = new SQLServerConnector();
            //PageBean pageBean = new PageBean();
            List<PostDataLog> listLogs;
            List<PageBean> pageNumberList;
            //listPosts = sqlServerConnector.GetPostsList();
            int pageTotalCount = sqlServerConnector.GetTotalCount();


            PageOperation pageOperation = new PageOperation(pageTotalCount);
            int currentPage = 1;
            pageNumberList = pageOperation.GetPageNumberList();
            int pageCount = pageOperation.GetPageCount();
            int startPageNum = pageOperation.GetStartNumber(currentPage);
            int endPageNum = pageOperation.GetEndNumber();
            int previousPageNumber = 1;
            int nextPageNumber = previousPageNumber + 1;
            listLogs = sqlServerConnector.GetLimitLogsList("0", "5");

            PostDataViewModel postDataViewModel = new PostDataViewModel();
            postDataViewModel.PostLogList = listLogs;
            postDataViewModel.PageNumberList = pageNumberList;
            postDataViewModel.PageCount = pageCount;
            postDataViewModel.CurrentPage = currentPage;
            postDataViewModel.StrPageNum = startPageNum;
            postDataViewModel.EndPageNum = endPageNum;
            postDataViewModel.PreviousPageNumber = previousPageNumber;
            postDataViewModel.NextPageNumber = nextPageNumber;
            postDataViewModel.FirstPage = true;
            postDataViewModel.LastPage = false;

            //ViewBag.ListOfPosts = listPosts;
            //return View(listPosts);
            return View(postDataViewModel);
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
                sqlCriteria = "hy_tic01 LIKE '%" + postID + "%' ";
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
    }
}