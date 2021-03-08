using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;  
using Newtonsoft.Json.Linq;  

namespace Name_Generator
{
    class Program
    {
        static void Main(string[] args)
        {

        }
    }

    public class RandomName //Class used to generate a random name
    {
        class NameList //Class for holding the lists of names from names.json
        {
            public string[] boys { get; set; }
            public string[] girls { get; set; }
            public string[] last { get; set; }
        }
    }
}
