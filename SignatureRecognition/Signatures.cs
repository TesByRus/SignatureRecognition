using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace SignatureRecognition
{
    [XmlRoot(Namespace = "SignatureRecognition", ElementName = "Signature", IsNullable = true)]
    public class Signatures
    {
        public List<Signature> SList { get; set; }

        public Signatures()
        {
            SList = new List<Signature>();
        }
    }
}