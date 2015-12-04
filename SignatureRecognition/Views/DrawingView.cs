using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Views;

using System;
using System.Threading;

using Android.App;
using Android.Runtime;
using Android.Widget;
using Android.OS;

namespace SignatureRecognition.Views
{
    // Original Source: http://csharp-tricks-en.blogspot.com/2014/05/android-draw-on-screen-by-finger.html
    public class DrawingView : View
    {
        public DrawingView(Context context)
            : base(context)
        {
            Start();
        }
        public DrawingView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Start();
        }
        public DrawingView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            Start();
        }

        public DrawingView(Context context, IAttributeSet attrs, int defStyle, int defStyleRes) : base(context, attrs, defStyle, defStyleRes)
        {
            Start();
        }

        public delegate void CoordinateChangeMethodContainer(int x, int y);
        public event CoordinateChangeMethodContainer OnCoordinatesChange;

        public Color CurrentLineColor { get; set; }

        public float PenWidth { get; set; }

        private Path DrawPath;
        private Paint DrawPaint;
        private Paint CanvasPaint;
        private Canvas DrawCanvas;
        private Bitmap CanvasBitmap;

        private bool drawed = false;

        private void Start()
        {
            CurrentLineColor = Color.Black;
            PenWidth = 5.0f;

            DrawPath = new Path();
            DrawPaint = new Paint
            {
                Color = CurrentLineColor,
                AntiAlias = true,
                StrokeWidth = PenWidth
            };

            DrawPaint.SetStyle(Paint.Style.Stroke);
            DrawPaint.StrokeJoin = Paint.Join.Round;
            DrawPaint.StrokeCap = Paint.Cap.Round;

            CanvasPaint = new Paint
            {
                Dither = true
            };
        }

        public void Clear()
        {
            this.Start();
            DrawCanvas.DrawColor(Color.Transparent, PorterDuff.Mode.Clear);
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);

            CanvasBitmap = Bitmap.CreateBitmap(w, h, Bitmap.Config.Argb8888);
            DrawCanvas = new Canvas(CanvasBitmap);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            DrawPaint.Color = CurrentLineColor;
            canvas.DrawBitmap(CanvasBitmap, 0, 0, CanvasPaint);
            canvas.DrawPath(DrawPath, DrawPaint);
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            var touchX = e.GetX();
            var touchY = e.GetY();

            switch (e.Action)
            {
                case MotionEventActions.Down:
                    DrawPath.MoveTo(touchX, touchY);
                    OnCoordinatesChange((int)touchX, (int)touchY);
                    break;
                case MotionEventActions.Move:
                    DrawPath.LineTo(touchX, touchY);
                    OnCoordinatesChange((int)touchX, (int)touchY);
                    break;
                case MotionEventActions.Up:
                    DrawCanvas.DrawPath(DrawPath, DrawPaint);
                    DrawPath.Reset();
                    OnCoordinatesChange((int)touchX, (int)touchY);
                    break;
                default:
                    return false;
            }
            PostInvalidate();
            return true;
        }
    }
}

