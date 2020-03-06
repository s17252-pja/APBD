using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tutorial1
{
    public class Program
    {
        private string firstName; // field

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName { get; set; } //auto props


        public static async Task Main(string[] args)
        {

            //int? g = null;
            //bool b = true;
            //string str = "str";

            //int age = 25;
            //String str1 = "str";

            //Console.WriteLine($"I am {age} years old");
            //Console.WriteLine("hello world!");

            var url = @"https://www.kiepownica.pl/";

            var httpClient = new HttpClient();


            
            
            using (var response = await httpClient.GetAsync(url))
            {
                var content = await response.Content.ReadAsStringAsync();


                var regex = new Regex("[a-z]+[a-z0-9]*@[a-z]+\\.[a-z]+", RegexOptions.IgnoreCase);

                var matches = regex.Matches(content);

                foreach (var match in matches)
                {
                    Console.WriteLine(match.ToString());
                }
            }


        }
    }
}
