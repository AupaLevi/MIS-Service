using System.Web.Mvc;

namespace MIS_Service.Models
{
    public class PostDataLog
    {
        private string tig01;
        private string tig02;
        private string tig03;
        private string tig04;
        private string tig05;
        private string tig06;
        private string tig07;
        private string tig08;
        private string tig09;

        public string Tig01 { get => tig01; set => tig01 = value; }
        public string Tig02 { get => tig02; set => tig02 = value; }
        public string Tig03 { get => tig03; set => tig03 = value; }
        public string Tig04 { get => tig04; set => tig04 = value; }
        public string Tig05 { get => tig05; set => tig05 = value; }
        public string Tig06 { get => tig06; set => tig06 = value; }
        [AllowHtml]
        public string Tig07 { get => tig07; set => tig07 = value; }
        public string Tig08 { get => tig08; set => tig08 = value; }
        public string Tig09 { get => tig09; set => tig09 = value; }
    }
}