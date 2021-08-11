using MIS_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIS_Service.Controllers
{
    public class PageOperation
    {
        //PageBean pageBean = new PageBean();
        private int totalPageCount;
        private int startNumber;

        public PageOperation(int totalPageCount)
        {
            this.totalPageCount = totalPageCount;
        }

        public int TotalPageCount { get => totalPageCount; set => totalPageCount = value; }

        public int GetPageCount()
        {
            int pageCount = this.totalPageCount;
            return pageCount;
        }

        public List<PageBean> GetPageNumberList()
        {
            int num = (GetPageCount() + 5 - 1) / 5;

            List<PageBean> numberList = new List<PageBean>();
            for (int i = 1; i <= num; i++)
            {
                PageBean pageBean = new PageBean
                {
                    CurrentPage = i,
                    PageNumber = i.ToString()
                };
                numberList.Add(pageBean);
            }
            return numberList;
        }

        public int GetStartNumber(int currentPageNumber)
        {
            if (currentPageNumber == 1)
            {
                this.startNumber = 1;
            }
            else
            {
                this.startNumber = (currentPageNumber - 1) * 5;
            }
            return this.startNumber;
        }

        public int GetEndNumber()
        {
            int endPageNumber = this.startNumber + 4;
            return endPageNumber;
        }
        public bool IsLastPage(int currentPageNumber)
        {
            if (currentPageNumber == this.GetEndNumber())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}