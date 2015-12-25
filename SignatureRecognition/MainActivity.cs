using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.OS;
using Android.Support.V7.App;
using SignatureRecognition.Core;
using SignatureRecognition.Views;

namespace SignatureRecognition
{
    [Activity(Label = "SignatureRecognition", MainLauncher = true, Icon = "@drawable/icon",
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class MainActivity : AppCompatActivity
    {

        private int _currentX;
        private int _currentY;

        private bool _flag = false;

        public Signature CurSignature { get; set; }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            var folderPath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            folderPath = Path.Combine(folderPath, "_signatureRecognition");
            CurSignature = new Signature();

            SetContentView(Resource.Layout.Main);


            FloatingActionButton floatingButton = FindViewById<FloatingActionButton>(Resource.Id.flBtn);
            floatingButton.Click += (sender, args) =>
            {
                _flag = false;
                var name = FindViewById<EditText>(Resource.Id.edittext);
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string path;
                if (string.IsNullOrEmpty(name.Text))
                {
                    path = Path.Combine(folderPath, "cur_signature.xml");
                }
                else
                {
                    path = Path.Combine(folderPath, name.Text + ".xml");
                }
                var state = FindViewById<TextView>(Resource.Id.state);
                    state.SetText("stopsave",TextView.BufferType.Normal);

                var curSignatures = new Signatures();

                if (CurSignature.Points.Count != 0)
                {
                    if (File.Exists(path))
                    {
                        using (var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read))
                        {
                            using (var streamReader = new StreamReader(fileStream))
                            {
                                XmlSerializer serializer =
                                    new XmlSerializer(typeof(Signatures));
                                curSignatures = (Signatures)serializer.Deserialize(streamReader);
                            }
                        }
                    }

                    using (var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        CurSignature.Name = FindViewById<EditText>(Resource.Id.edittext).Text;
                        curSignatures.SList.Add(CurSignature);

                        var encoding = Encoding.UTF8;
                        using (var streamWriter = new StreamWriter(fileStream, encoding))
                        {
                            XmlSerializer serializer =
                                new XmlSerializer(typeof(Signatures));
                            serializer.Serialize(streamWriter, curSignatures);
                        }
                    }
                }
                //});
            };


            FloatingActionButton floatingButtonClear = FindViewById<FloatingActionButton>(Resource.Id.flBtnClearAndStop);
            floatingButtonClear.Click += (sender, args) =>
            {
                _flag = false;
                CurSignature = new Signature();
                FindViewById<DrawingView>(Resource.Id.drawing).Clear();

                var state = FindViewById<TextView>(Resource.Id.state);
                state.SetText("stopclear",TextView.BufferType.Normal);
            };
            
            var drawerView = FindViewById<DrawingView>(Resource.Id.drawing);
            drawerView.OnCoordinatesChange += CoordinatesChangedHandler;
        }

        public void SheduleReading()
        {
            var i = 0;
            while (_flag)
            {
                CurSignature.Points.Add(new SignaturePoint(i++, _currentX, _currentY));
                Thread.Sleep(10);
            }
        }


        public void CoordinatesChangedHandler(int x, int y)
        {
            if (!_flag)
            {
                _flag = true;
                var t = new Thread(SheduleReading);
                t.Start();

                var state = FindViewById<TextView>(Resource.Id.state);
                state.SetText("drawing",TextView.BufferType.Normal);
            }
            else
            {

                _currentX = x;
                _currentY = y;
                var xTextView = FindViewById<TextView>(Resource.Id.coordX);
                var yTextView = FindViewById<TextView>(Resource.Id.coordY);

                xTextView.SetText("X: " + _currentX, TextView.BufferType.Normal);
                yTextView.SetText("Y: " + _currentY, TextView.BufferType.Normal);
            }
        }

    }
}


