using System.Collections.Generic;

namespace SignatureRecognition.Core
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