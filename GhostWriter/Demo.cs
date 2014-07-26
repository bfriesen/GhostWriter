using System.Collections.Generic;
using System.Xml.Serialization;

namespace GhostWriter
{
    public class Demo
    {
        public Demo()
        {
            Steps = new List<Step>();
        }

        public string TargetApplicationTitle { get; set; }
        public string InitialCode { get; set; }

        [XmlElement("Step")]
        public List<Step> Steps { get; set; }
    }
}