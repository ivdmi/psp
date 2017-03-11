using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSP.Domain.Service
{
    public static class EventHelper
    {
        public class StateElement
        {
            public new string Name { get; set; }

            public int Key { set; get; }
            public Color NetColor { set; get; }
            public override string ToString() { return Name; }
        }

        public static readonly StateElement[] States = new[] {
            new StateElement { Name = "Аудит",                  Key = 0,    NetColor = Color.Blue       },                                              
            new StateElement { Name = "Консультация",           Key = 1,    NetColor = Color.Green      },                                              
            new StateElement { Name = "Больничный",             Key = 2,    NetColor = Color.DarkOrange },                                              
            new StateElement { Name = "Отгул",                  Key = 3,    NetColor = Color.Gray       },                                              
            new StateElement { Name = "Отпуск",                 Key = 4,    NetColor = Color.Yellow     },                                              
            new StateElement { Name = "Офис",                   Key = 5,    NetColor = Color.Cyan       },                                              
            new StateElement { Name = "Бухгалтерский Учет",     Key = 6,    NetColor = Color.Brown      },                                              
        };

        public static bool UnpackEventFromString(string InStr, out string Factory, out DateTime Begin, out DateTime End, out int Key, out string Comment)
        {
            Factory = "";
            Begin = End = DateTime.Now;
            Key = 0;
            Comment = "";
            string[] Array = InStr.Split(new[] { ',' }, 5);
            if (Array.Length >= 4)
            {
                Factory = Array[0];
                Begin = Convert.ToDateTime(Array[1]);
                End = Convert.ToDateTime(Array[2]);
                Key = Convert.ToInt32(Array[3]);
                if (Array.Length > 4)
                    Comment = Convert.ToString(Array[4]);

                return true;
            }
            return false;
        }

        public static int GetMinutes(DateTime Begin, DateTime End)
        {
            int Total = Convert.ToInt32((End - Begin).TotalMinutes);
            //if (Begin.Hour == 9 && Begin.Minute == 0 && End.Hour == 18 && End.Minute == 0) {
            //    Total -= 60;
            //}
            return Total;
        }

        public static string FormatMinutes(int Minutes)
        {
            if (0 == Minutes)
                return "";

            int H = Minutes / 60;
            int M = Minutes % 60;

            string Buf = "";

            if (0 != H)
                Buf += H + " час. ";

            if (0 != M)
                Buf += M + " мин. ";

            return Buf.TrimEnd(' ');
        }
    }
}
