using System;
using System.Collections.Generic;
using System.Drawing;
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
    public class Signature
    {
        public List<SignaturePoint> Points { get; set; }

        public string Name { get; set; }

        public Signature()
        {
            Points = new List<SignaturePoint>();
        }
    }
}