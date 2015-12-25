using System.Collections.Generic;
using System.Xml.Serialization;

namespace SignatureRecognition.Core
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