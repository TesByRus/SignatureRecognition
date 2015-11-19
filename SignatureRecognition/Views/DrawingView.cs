
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

using Android.Graphics;
using Android.Views;

namespace SignatureRecognition
{
    [Register ("signaturerecognition.DrawingView")]
    public class DrawingView : View
    {
        //drawing path
        private Path drawPath;
        //drawing and canvas paint
        private Paint drawPaint, canvasPaint;
        //canvas
        private Canvas drawCanvas;
        //canvas bitmap
        private Bitmap canvasBitmap;

        public DrawingView(Context context)
            : base(context)
        {
            Initialize();
        }

        public DrawingView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Initialize();
            SetupDrawing();
        }

        public DrawingView(Context context, IAttributeSet attrs, int defStyle)
            : base(context, attrs, defStyle)
        {
            Initialize();
        }

        void Initialize()
        {
        }

        void SetupDrawing()
        {
            drawPath = new Path();
            drawPaint = new Paint();
            drawPaint.Color = new Color(Color.Black);
            drawPaint.AntiAlias = true;
            drawPaint.StrokeWidth = 20;
            drawPaint.SetStyle(Paint.Style.Stroke);
            drawPaint.StrokeJoin = Paint.Join.Round;
            drawPaint.StrokeCap = Paint.Cap.Round;
            canvasPaint = new Paint(PaintFlags.Dither);
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            canvasBitmap = Bitmap.CreateBitmap(w, h, Bitmap.Config.Argb8888);
            drawCanvas = new Canvas(canvasBitmap);
        }

        protected override void OnDraw(Canvas canvas)
        {
            //base.OnDraw(canvas);
            canvas.DrawBitmap(canvasBitmap, 0, 0, canvasPaint);
            canvas.DrawPath(drawPath, drawPaint);
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            float touchX = e.GetX();
            float touchY = e.GetY();

            switch (e.Action) {
                case MotionEventActions.Down:
                    drawPath.MoveTo(touchX, touchY);
                    break;
                case MotionEventActions.Move:
                    drawPath.LineTo(touchX, touchY);
                    break;
                case MotionEventActions.Up:
                    drawCanvas.DrawPath(drawPath, drawPaint);
                    drawPath.Reset();
                    break;
                default:
                    return false;
            }

            Invalidate();
            return true;
        }
    }
}

