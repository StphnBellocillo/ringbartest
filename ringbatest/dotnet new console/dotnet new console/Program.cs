using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dotnet_new_console
{
    class Program
    {
   
        static void Main(string[] args)
        {
            string path = $"C:\\Users\\Hybrain\\Downloads\\file\\myfile.txt";
            string url = "https://ringba-test-html.s3-us-west-1.amazonaws.com/TestQuestions/output.txt";
            string text;
            var commonLetter1 = string.Empty;
            string commonLetter2;
            int commonNumber1 = 0;
            int commonNumber2 = 0;
            var commonword = string.Empty;
            int commonwordCount = 0;
            Console.WriteLine("Downloading.....");
            Console.WriteLine(url);
            using (var client = new WebClient())
            {
                client.DownloadFile(url, path);
            }
            Console.WriteLine("Done.....");
            DataTable dt = new DataTable();
            DataTable dtS = new DataTable();
            DataTable dtS2 = new DataTable();
            dt.Columns.Add("Element");
            dt.Columns.Add("Counter", typeof(int));

            text = System.IO.File.ReadAllText(path);

            string textspace = (string.Concat(text.Select(x => Char.IsUpper(x) ? "," + x : x.ToString())).TrimStart(' '));

            var tokens = textspace.Split(' ');
            string[] oc = { textspace };
            var removefirstchar = textspace.Substring(1);

            removefirstchar = removefirstchar.Substring(1);
            var json = JsonConvert.SerializeObject(removefirstchar, Formatting.Indented);
            List<string> names = json.Split(',').ToList();

            var query = names.GroupBy(x => x)
               .Where(g => g.Count() > 1)
               .Select(y => new { Element = y.Key.ToString(), Counter = y.Count() })
               .ToList();

            var queryjsonconvert = JsonConvert.SerializeObject(query, Formatting.Indented);
            dt = (DataTable)JsonConvert.DeserializeObject(queryjsonconvert, (typeof(DataTable))); 

            var cw = query.Max(q => q.Counter);
  
            for (int x = 0; x < dt.Rows.Count; x++)
            {
                if (Convert.ToInt32(dt.Rows[x]["Counter"]) == cw)
                {
                    commonword = dt.Rows[x]["Element"].ToString();
                    commonwordCount = cw;
                }
            }
            //string cwww = commonword;
          

            var q1 = text.GroupBy(x => x)
              .Where(g => g.Count() > 1)
               .Select(y => new { Element = y.Key.ToString(), Counter = y.Count() })
              .ToList();

            var q1jsonconvert = JsonConvert.SerializeObject(q1, Formatting.Indented);

            var res1 = q1.Max(y => y.Counter);

            //Console.WriteLine(q1);
            dtS = (DataTable)JsonConvert.DeserializeObject(q1jsonconvert, (typeof(DataTable)));

            for (int x = 0; x < dtS.Rows.Count; x++)
            {
                if (Convert.ToInt32(dtS.Rows[x]["Counter"]) == res1)
                {
                    commonLetter1 = dtS.Rows[x]["Element"].ToString();
                    commonNumber1 = res1;

                }
            }

            string jslettrcount = string.Empty;
            jslettrcount = JsonConvert.SerializeObject(q1);


            string js = string.Empty;

            js = JsonConvert.SerializeObject(dt);



            int txtCountsEveryWords = tokens.Length;
            int txtCountEveryLetter = text.Length;
            int countLetterUpperCase = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (char.IsUpper(text[i])) countLetterUpperCase++;
            }

            Console.WriteLine("Problem 1:");
            Console.WriteLine(q1jsonconvert);
            Console.WriteLine("Problem 2:");
            Console.WriteLine("Number of UpperCase: "+countLetterUpperCase);
            Console.WriteLine("Problem 3:");
            Console.WriteLine("Common word: ");
            Console.WriteLine(commonword);
            Console.WriteLine(commonwordCount);
            Console.WriteLine("Problem 4:");
            Console.WriteLine(commonLetter1);
            Console.WriteLine(commonNumber1);

            //Console.WriteLine(text);
        }
    }
}
