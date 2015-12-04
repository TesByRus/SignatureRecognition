using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SignatureRecognition
{
    public class SignaturePoint
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Time { get; set; }


        public SignaturePoint()
        {
            
        }
        public SignaturePoint(int time, int x, int y)
        {
            X = x;
            Y = y;
            Time = time;
        }
    }
}