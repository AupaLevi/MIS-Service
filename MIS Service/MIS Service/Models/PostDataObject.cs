using System.Web.Mvc;

namespace MIS_Service.Models
{
    public class PostDataObject
    {
        private string aaa01;
        private string aaa02;
        private string aaa03;
        private string aaa04;
        private string aaa05;
        private string aaa06;

        private string aaa07;
        private string aaa08;
        private string aaa09;

        public string Aaa01 { get => aaa01; set => aaa01 = value; }
        public string Aaa02 { get => aaa02; set => aaa02 = value; }
        public string Aaa03 { get => aaa03; set => aaa03 = value; }
        public string Aaa04 { get => aaa04; set => aaa04 = value; }
        public string Aaa05 { get => aaa05; set => aaa05 = value; }
        public string Aaa06 { get => aaa06; set => aaa06 = value; }
        [AllowHtml]
        public string Aaa07 { get => aaa07; set => aaa07 = value; }
        public string Aaa08 { get => aaa08; set => aaa08 = value; }
        public string Aaa09 { get => aaa09; set => aaa09 = value; }
    }
}