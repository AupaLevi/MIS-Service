using System.Web.Mvc;

namespace MIS_Service.Models
{
    public class PostDataObject
    {
        private string tic01;
        private string tic02;
        private string tic03;
        private string tic04;
        private string tic05;
        private string tic06;

        private string tic07;
        private string tic08;
        private string tic09;

        public string Tic01 { get => tic01; set => tic01 = value; }
        public string Tic02 { get => tic02; set => tic02 = value; }
        public string Tic03 { get => tic03; set => tic03 = value; }
        public string Tic04 { get => tic04; set => tic04 = value; }
        public string Tic05 { get => tic05; set => tic05 = value; }
        public string Tic06 { get => tic06; set => tic06 = value; }
        [AllowHtml]
        public string Tic07 { get => tic07; set => tic07 = value; }
        public string Tic08 { get => tic08; set => tic08 = value; }
        public string Tic09 { get => tic09; set => tic09 = value; }
    }
}