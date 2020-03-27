using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Cw2.Models
{
    [Serializable]
    
    public class Studies
    {
        [XmlElement(elementName: "name")]
        public string Name { get; set; }
        [XmlElement(elementName: "mode")]
        public string Mode { get; set; }
        public Studies() { }
        public Studies(string name, string mode)
        {
            Name = name;
            Mode = mode;
        }
        //        [XmlAttribute(AttributeName = "studies")]
    }
}
