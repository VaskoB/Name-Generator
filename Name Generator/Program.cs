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
            Random random = new Random(DateTime.Now.Second); //Variable to select names randomly

            RandomName nameGenerator = new RandomName(random); //New instance of the RandomName class

            string name = nameGenerator.Generate(Gender.Male, 1); //Generate a male name, with one middle name
            string secondName = nameGenerator.Generate(Gender.Female, 2, true); //Generate a female name with two middle initials
            string thirdName = nameGenerator.Generate(Gender.Female); //Generate a female name with no middle names

            Console.WriteLine(name);
            Console.WriteLine(secondName);
            Console.WriteLine(thirdName);
        }
    }

    public class RandomName //Class used to generate a random name
    {
        class NameList //Class for holding the lists of names from names.json
        {
            public string[] boys { get; set; }
            public string[] girls { get; set; }
            public string[] last { get; set; }

            public NameList() 
            {
                boys = new string[] { };
                girls = new string[] { };
                last = new string[] { };
            }
        }

        Random random;
        List<string> Male;
        List<string> Female;
        List<string> Last;

        public RandomName(Random random)
        {
            this.random = random;
            NameList nl = new NameList();

            JsonSerializer serializer = new JsonSerializer();

            using (StreamReader reader = new StreamReader("names.json"))
            using (JsonReader jreader = new JsonTextReader(reader))
            {
                nl = serializer.Deserialize<NameList>(jreader);
            }

            Male = new List<string>(nl.boys);
            Female = new List<string>(nl.girls);
            Last = new List<string>(nl.last);
        }

        public string Generate(Gender gender, int middle = 0, bool isInitial = false)
        {
            string first = gender == Gender.Male ? Male[random.Next(Male.Count)] : Female[random.Next(Female.Count)]; //Checks the gender and picks a random name from the selected gender
            string last = Last[random.Next(Last.Count)]; //Gets a random last name

            List<string> middles = new List<string>();

            for(int i = 0; i < middle; i++)
            {
                if(isInitial)
                {
                    middles.Add("ABCDEFGHIJKLMNOPQRSTUVWXYZ"[random.Next(0, 25)].ToString() + "."); //Picks an uppercase letter to use as an initial and appends a dot
                }
                else
                {
                    middles.Add(gender == Gender.Male ? Male[random.Next(Male.Count)] : Female[random.Next(Female.Count)]); //Randomly selects a middle name that fits the gender of the person
                }
            }

            StringBuilder name = new StringBuilder();
            name.Append(first + " "); //puts a space after the first name

            foreach(string item in middles) //Appends the middle names or initials if there are any
            {
                name.Append(item + " ");
            }
            name.Append(last); //Appends the last name

            return name.ToString();
        }

        public List<string> RandomNames(int number, int maxMiddleNames, Gender? gender = null, bool? initials = null)
        {
            List<string> names = new List<string>();

            for(int i = 0; i < number; i++)
            {
                if(gender != null && initials != null)
                {
                    names.Add(Generate((Gender)gender, random.Next(0, maxMiddleNames + 1), (bool)initials));
                }
                else if(gender != null)
                {
                    bool initial = random.Next(0, 2) != 0;
                    names.Add(Generate((Gender)gender, random.Next(0, maxMiddleNames + 1), initial));
                }
                else if(initials != null)
                {
                    Gender gndr = (Gender)random.Next(0, 2);
                    bool initial = random.Next(0, 2) != 0;
                    names.Add(Generate(gndr, random.Next(0, maxMiddleNames + 1), initial));
                }
            }

            return names;
        }
    }

    public enum Gender
    {
        Male, 
        Female
    }
}
