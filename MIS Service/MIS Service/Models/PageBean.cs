using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIS_Service.Models
{
    public class PageBean
    {
        //private int PageCount;       // How Many Pages
        private String pageNumber;   // List of numbers under a Table area
        private int currentPage;     // Marks current page number
        //private int StrPageNum;      // The Start Number to get data
        //private int EndPageNum;      // The End Number to get data

        public string PageNumber { get => pageNumber; set => pageNumber = value; }
        public int CurrentPage { get => currentPage; set => currentPage = value; }

        //public String GetPageNumber()
        //{
        //    return this.PageNumber;
        //}
        //
        //public String SetPageNumber(String num)
        //{
        //    return this.PageNumber = num;
        //}
    }
}