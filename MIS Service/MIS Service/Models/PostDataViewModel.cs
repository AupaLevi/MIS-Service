using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MIS_Service.Models
{
    public class PostDataViewModel
    {
        private int currentPage;
        private int strPageNum;
        private int endPageNum;
        private int previousPageNumber;
        private int nextPageNumber;
        private bool firstPage;
        private bool lastPage;

        public IEnumerable<PostDataObject> PostDataList { get; set; }
        public IEnumerable<PostDataLog> PostLogList { get; set; }
        public IEnumerable<PageBean> PageNumberList { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage { get => currentPage; set => currentPage = value; }
        public int StrPageNum { get => strPageNum; set => strPageNum = value; }
        public int EndPageNum { get => endPageNum; set => endPageNum = value; }
        public int PreviousPageNumber { get => previousPageNumber; set => previousPageNumber = value; }
        public int NextPageNumber { get => nextPageNumber; set => nextPageNumber = value; }
        public bool LastPage { get => lastPage; set => lastPage = value; }
        public bool FirstPage { get => firstPage; set => firstPage = value; }

        //public IEnumerable<UserBasicDataObject> UserBasicData { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
    }
}