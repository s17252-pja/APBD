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
        private static string mode;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World");
            string path;
            ;
            FileStream writer;
            if (args.Length > 0) { path = args[0]; } else { path = @"Data\dane.csv"; Console.WriteLine("No file provided as the first argument, using built-in dane.csv \n"); };
            if (args[1].Length > 0) { writer = new FileStream(args[1], FileMode.Create); } else { writer = new FileStream(@"data.xml", FileMode.Create); Console.WriteLine("No output file or invalid argument specified. Using default data.xml"); }
            if (args[2].Length > 0 && args[2].Contains("xml")) { Console.WriteLine("Running XML mode \n"); mode = "xml"; } else if (args[2].Contains("json")) { Console.WriteLine("Running JSON mode \n"); mode = "json"; } else { Console.WriteLine("Invalid output type provided \n"); mode = "invalid"; }


            //Wczytywanie 
            var fi = new FileInfo(path);
            var list = new List<Student>();
            if (!fi.Exists) { throw new System.ArgumentException("The file path is incorrect"); }
            string line = null;
            try
            {
                using var stream = new StreamReader(fi.OpenRead());
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
                stream.Dispose();
            }
            catch (FileNotFoundException ex)
            {

                Console.WriteLine(ex);
                Console.WriteLine("File to read from was not found! \n");
            }
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

            University u = new University { CreatedAt = DateTime.Now, Author = "Jan Kowalski", students = list };
            UniversityWrapper uw = new UniversityWrapper { University = u };
            XmlSerializer serializer = new XmlSerializer(typeof(University));

            //new XmlRootAttribute("university"));
            if (mode.Contains("json"))
            {
                var jsonString = JsonSerializer.Serialize(uw, typeof(UniversityWrapper));
                File.WriteAllText("data.json", jsonString);
                serializer.Serialize(writer, u);
            }
            else if (mode.Contains("xml"))
            {
                FileStream writerXML = new FileStream(@"data.xml", FileMode.Create);
                serializer.Serialize(writerXML, u);
            }
            else if(mode.Contains("invalid"))
            {
                Console.WriteLine("You did not provide third parameter! Not writing to any format! \n");
            }
            else
            {
                Console.WriteLine("Unknown state of writing mode, report this to dev! \n");
            }
            //serializer.Serialize(writer, list);
        }
    }
}
