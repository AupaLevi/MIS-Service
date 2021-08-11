using System.Web.Mvc;

namespace MIS_Service.Models
{
    public class PostDataLog
    {
        private string hy_tic01;
        private string hy_tic02;
        private string hy_tic03;
        private string hy_tic04;
        private string hy_tic05;
        private string hy_tic06;

        private string hy_tic07;
        private string hy_tic08;
        private string hy_tic09;

        public string Hy_tic01 { get => hy_tic01; set => hy_tic01 = value; }
        public string Hy_tic02 { get => hy_tic02; set => hy_tic02 = value; }
        public string Hy_tic03 { get => hy_tic03; set => hy_tic03 = value; }
        public string Hy_tic04 { get => hy_tic04; set => hy_tic04 = value; }
        public string Hy_tic05 { get => hy_tic05; set => hy_tic05 = value; }
        public string Hy_tic06 { get => hy_tic06; set => hy_tic06 = value; }
        [AllowHtml]
        public string Hy_tic07 { get => hy_tic07; set => hy_tic07 = value; }
        public string Hy_tic08 { get => hy_tic08; set => hy_tic08 = value; }
        public string Hy_tic09 { get => hy_tic09; set => hy_tic09 = value; }
    }
}