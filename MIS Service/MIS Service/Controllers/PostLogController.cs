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

        public ActionResult BackToAddPostedLog()
        {
            PostLogViewModel postLogViewModel = (PostLogViewModel)TempData["postLogViewModel"];
            return View("AddPostedLog", postLogViewModel);
        }
    }
}