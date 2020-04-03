using System;
using System.Xml.Serialization;

namespace Cw2.Models
{
    [Serializable]
    [XmlInclude(typeof(Studies))]
    public class Student
    {
        [XmlAttribute(attributeName: "indexNumber")]
        public string IndexNumber { get; set; }

        [XmlElement(elementName: "fname")]
        public string Imie { get; set; }

        //propfull + tabx2

        // private string _nazwisko;
        [XmlElement(elementName: "lname")]
        public string Nazwisko { get; set; }

        /*{
            get { return _nazwisko; }
            set
            {
                if (value == null) throw new ArgumentException();
                _nazwisko = value;
            }
        }*/

        [XmlElement(elementName: "birthdate")]
        public string Birthdate { get; set; }

        [XmlElement(elementName: "email")]
        public string Email { get; set; }

        [XmlElement(elementName: "mothersName")]
        public string MothersName { get; set; }

        [XmlElement(elementName: "fathersName")]
        public string FathersName { get; set; }

        [XmlElement(elementName: "studies")]
        public Studies Studies { get; set; }

        public Student()
        {
        }
    }
}