using Cw2.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Cw2
{
    [Serializable]
    [XmlRoot("university")]
    [XmlInclude(typeof(Student))]
    public class University
    {
        [XmlAttribute(attributeName: "createdAt")]
        public DateTime CreatedAt { get; set; }
        [XmlAttribute(attributeName: "author")]
        public string Author { get; set; }
        public List<Student> students { get; internal set; }

        public University( DateTime date, string author)
        {
            Author = author;
            CreatedAt = date;
        }
        public University()
        {

        }
    }
}
