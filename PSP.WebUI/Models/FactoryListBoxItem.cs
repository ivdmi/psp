using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PSP.Domain.Service;

namespace PSP.WebUI.Models
{
    public class FactoryListBoxItem
    {
        public string Factory { set; get; }
        public DateTime BeginTime { set; get; }
        public DateTime EndTime { set; get; }
        public int Key { set; get; }
        public string Comment { set; get; }

        public override string ToString()
        {
            return string.Format("[{0:00}:{1:00}-{2:00}:{3:00}] {4} ({5})",
                BeginTime.Hour, BeginTime.Minute, EndTime.Hour, EndTime.Minute, Factory, EventHelper.States[Key].Name);
        }

        public FactoryListBoxItem(string Factory, DateTime BeginTime, DateTime EndTime, string Comment)
        {
            this.Factory = Factory;
            this.BeginTime = BeginTime;
            this.EndTime = EndTime;
            this.Comment = Comment;
        }

        public FactoryListBoxItem(string Factory)
        {
            this.Factory = Factory;
            DefaultTimes();
        }

        private void DefaultTimes()
        {
            BeginTime = new DateTime(1900, 1, 1, 9, 0, 0);
            EndTime = new DateTime(1900, 1, 1, 18, 0, 0);
        }

        public string Pack()
        {
            return string.Format("{0},{1},{2},{3},{4}", Factory.Replace(',', ' '), BeginTime.ToShortTimeString(), EndTime.ToShortTimeString(), Key, Comment);
        }


        public static string GetFactoryName(string S)
        {
            string[] Array = S.Split(',');
            if (Array.Length > 0)
                return Array[0];
            return "";
        }

        //public static bool UnpackFromString(string InStr, out string Factory, out DateTime Begin, out DateTime End, out int Key, out string Comment)
        //{
        //    Factory = "";
        //    Begin = End = DateTime.Now;
        //    Key = 0;
        //    Comment = "";
        //    string[] Array = InStr.Split(new[] { ',' }, 5);
        //    if (Array.Length >= 4)
        //    {
        //        Factory = Array[0];
        //        DateTime begin = new DateTime();
        //        DateTime end = new DateTime();
        //        short key;
        //        if (DateTime.TryParse(Array[1], out begin) && DateTime.TryParse(Array[2], out end) && Int16.TryParse(Array[3], out key))                
        //        {
        //            Begin = begin;
        //            End = end;
        //            Key = key;
        //    }
        //        else
        //        {
        //            return false;
        //        }
            
        //    if (Array.Length > 4)
        //            Comment = Convert.ToString(Array[4]);

        //        return true;
        //    }
        //    return false;
        //}
    }
}