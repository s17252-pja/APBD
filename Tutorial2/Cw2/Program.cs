using Cw2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;
using static Cw2.Models.Student;

namespace Cw2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World");
            string path;
            FileStream writer;
            if (args.Length > 0) { path = args[0]; } else { path = @"Data\dane.csv"; };

            if (args[1].Length > 0) { writer = new FileStream(args[1], FileMode.Create); } else { writer = new FileStream(@"data.xml", FileMode.Create); }
            if (args[2].Length > 0 && args[2].Contains("xml")) { Console.WriteLine("Running XML mode \n"); } else { Console.WriteLine("Invalid output type provided \n"); }


            //Wczytywanie 
            var fi = new FileInfo(path);
            var list = new List<Student>();
            if (!fi.Exists) { throw new System.ArgumentException("The file path is incorrect"); }
            string line = null;
            try
            {
                using (var stream = new StreamReader(fi.OpenRead()))
              
                while ((line = stream.ReadLine()) != null)
                {
                    string[] student = line.Split(',');
                    //Console.WriteLine(line);
                    var fileStudent = new Student
                    {
                        Imie = student[0],
                        Nazwisko = student[1],
                        Studies = new Studies(student[2], student[3]),
                        IndexNumber = student[4],
                        Birthdate = student[5],
                        Email = student[6],
                        MothersName = student[7],
                        FathersName = student[8]
                    };
                        list.Add(fileStudent);
                    }
                } catch (FileNotFoundException ex)
            {
                Console.WriteLine(ex);
              
            }
            //stream.Dispose();

            //XML
            //var list = new List<Student>();
            //var st = new Student
            //{
            //    IndexNumber = "s40269",
            //    Imie = "Jan",
            //    Nazwisko = "Kowalski",
            //    Birthdate = "02.05.1980",
            //    Email = "kowalski@wp.pl",
            //    MothersName = "Pawlak",
            //    FathersName = "Biedny",
            //    Studies = new Studies("IT", "Daily")
            //};

            //list.Add(st);


            FileStream writerXML = new FileStream(@"data.xml", FileMode.Create);
            University u = new University { CreatedAt = DateTime.Now, Author = "Jan Kowalski", students = list };
            UniversityWrapper uw = new UniversityWrapper { University = u };
            XmlSerializer serializer = new XmlSerializer(typeof(University));
            var jsonString = JsonSerializer.Serialize(uw, typeof(UniversityWrapper));
            File.WriteAllText("data.json", jsonString);
                                       //new XmlRootAttribute("university"));
            serializer.Serialize(writer, u);
            serializer.Serialize(writerXML, u);
            //serializer.Serialize(writer, list);

        }
    }
}
