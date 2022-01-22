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
        // POST: Tickets/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "tic03,tic04,tic05,tic06,tic07,tic08")] PostDataObject postDataObject)
        {
            SQLServerConnector sqlServerConnector = new SQLServerConnector();
            //List<PostDataObject> listPosts;
            //listPosts = sqlServerConnector.getPostsList();

            postDataObject.Tic01 = DateTime.Now.ToString("yyyyMMddHHmmss");
            postDataObject.Tic02 = DateTime.Now.ToString("yyyy-MM-dd");  //Post Date
            postDataObject.Tic09 = "";

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
                sqlCriteria = "tic01 LIKE '%" + postID + "%' ";
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
            String sqlCriteria = "tic01 = '" + postID + "'";

            listPosts = sqlServerConnector.GetPostsListOnDemand(sqlCriteria);

            postDataObjectForEdit = listPosts[0];
            //ViewBag.ListOfPosts = listPosts;
            ViewBag.PostDataForEdit = postDataObjectForEdit;

            return View("EditPost", postDataObjectForEdit);
        }

        [HttpPost, ActionName("ConfirmedEdit")]
        public ActionResult UpdatePost([Bind(Include = "Tic01,Tic02,Tic03,Tic04,Tic05,Tic06,Tic07,Tic08,Tic09")] PostDataObject postDataObject)
        {
            SQLServerConnector sqlServerConnector = new SQLServerConnector();

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
                sqlCriteria = "tic01 LIKE '%" + postID + "%' ";
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
        public ActionResult InputTicketToLog(String postID , [Bind(Include = "Tic01,Tic02,Tic03,Tic04,Tic05,Tic06,Tic07,Tic08,Tic09")] PostDataObject postDataObject)
        {
            SQLServerConnector sqlServerConnector = new SQLServerConnector();
            string logSQLString = sqlServerConnector.getSelectticDataSQL(postID);
            DataTable dataTable;
            string sqlResult;
            List<PostDataLog> goodpostDataLog;
            List<PostDataLog> insertedpostDataLog;
            PostDataLog postDataLog;
            string actionResult;
            int dataCount;
            insertedpostDataLog = new List<PostDataLog>();

            sqlServerConnector.ConfirmedUpdToCaseClosed(postDataObject);

            dataTable = sqlServerConnector.GetDataTable(logSQLString);
            sqlResult = "";

            goodpostDataLog = new List<PostDataLog>();

            if (dataTable.Rows.Count > 0)
            {
                postDataLog = new PostDataLog();

                

                foreach (DataRow row in dataTable.Rows)
                {

                    sqlResult = "Y";

                    try
                    {
                        postDataLog.Tig01 = row[dataTable.Columns["tic01"]].ToString();
                        postDataLog.Tig02 = (row[dataTable.Columns["tic02"]]) == DBNull.Value ? "" :
                            Convert.ToDateTime(row[dataTable.Columns["tic02"]]).ToString("yyyy-MM-dd");
                        postDataLog.Tig03 = row[dataTable.Columns["tic03"]].ToString();
                        postDataLog.Tig04 = row[dataTable.Columns["tic04"]].ToString();
                        postDataLog.Tig05 = row[dataTable.Columns["tic05"]].ToString();
                        postDataLog.Tig06 = row[dataTable.Columns["tic06"]].ToString();
                        postDataLog.Tig07 = row[dataTable.Columns["tic07"]].ToString();
                        postDataLog.Tig08 = row[dataTable.Columns["tic08"]].ToString();
                        postDataLog.Tig09 = row[dataTable.Columns["tic09"]].ToString();

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
                            dataCount = sqlServerConnector.SelectTigRowCounts(postDataLog.Tig01);
                            if (dataCount == 0)
                            {
                                goodpostDataLog.Add(postDataLog);
                            }

                        }
                    }
                }
                actionResult = "FAILED";
              
                if (goodpostDataLog.Count > 0)
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
            actionResult = "FAILED";

            if (insertedpostDataLog.Count >0)
            {
                //actionResult = sqlServerConnector.ConfirmedUpdToCaseClosed(postDataObject);
                string DelResult = sqlServerConnector.ConfirmedDelToCaseClosed(postDataObject);
            }

            

            return RedirectToAction("AddNewPost", "Post");
        }



    }
}