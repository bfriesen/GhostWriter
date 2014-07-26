using System.Xml.Serialization;

namespace GhostWriter
{
    public class Step
    {
        [XmlAttribute]
        public int Number { get; set; }
        public string GhostKeyboardData { get; set; }
        public string FinishedCode { get; set; }
        public string Notes { get; set; }
    }
}