namespace SignatureRecognition.Core
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