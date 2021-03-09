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
            Console.WriteLine();

            List<string> Names = nameGenerator.RandomNames(3, 2); // generate 100 random names with up to two middle names
            List<string> Boys = nameGenerator.RandomNames(3, 0, Gender.Male); // generate 100 random boys names
            List<string> Girls = nameGenerator.RandomNames(3, 2, Gender.Female, true); // 100 girls names with intials

            Console.WriteLine(string.Join(", ", Names));
            Console.WriteLine(string.Join(", ", Boys));
            Console.WriteLine(string.Join(", ", Girls));

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

        Random random; //Instance of the Random class
        List<string> Male; //List that will hold all the Male names
        List<string> Female; //List that will hold all the Female names
        List<string> Last; //List that will hold all the Last names

        public RandomName(Random random) //Class that distributes the names from the names.json file to different Lists
        {
            this.random = random;
            NameList nl = new NameList(); //Instance of the NameList class that holds fields for the boys, girls and last names

            JsonSerializer serializer = new JsonSerializer(); //A Serializer that comes with the Deserialize function

            using (StreamReader reader = new StreamReader("names.json")) //A StreamReader to read the names.json
            using (JsonReader jreader = new JsonTextReader(reader)) //A JsonReader that will read the text from the StreamReader
            {
                nl = serializer.Deserialize<NameList>(jreader); //All the names in the json file will get distributed to the fields in the nl instance using a Desirializer
            }

            Male = new List<string>(nl.boys); //Adds boys names from the NameList instance in the Male List
            Female = new List<string>(nl.girls); //Adds boys names from the NameList instance in the Female List
            Last = new List<string>(nl.last); //Adds boys names from the NameList instance in the Last List
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

        //Create a bulk of random names
        public List<string> RandomNames(int number, int maxMiddleNames, Gender? gender = null, bool? initials = null) //? means that the type might have a null value so 
        {
            List<string> names = new List<string>();

            for(int i = 0; i < number; i++)
            {
                if(gender != null && initials != null) //If there is a specific gender and initials 
                {
                    //Generate gender specific name, roll up to n middle names and a true initials bool
                    names.Add(Generate((Gender)gender, random.Next(0, maxMiddleNames + 1), (bool)initials));
                }
                else if(gender != null) //if the gender is specific
                {
                    bool initial = random.Next(0, 2) != 0; //Rolls a random true or false to decide if there's going to be initials
                    names.Add(Generate((Gender)gender, random.Next(0, maxMiddleNames + 1), initial)); //same thing in as in the first if
                }
                else if(initials != null)//if there's going to be initials
                {
                    Gender gndr = (Gender)random.Next(0, 2);
                    bool initial = random.Next(0, 2) != 0;
                    names.Add(Generate(gndr, random.Next(0, maxMiddleNames + 1), initial));
                }
                else
                {
                    Gender gndr = (Gender)random.Next(0, 2);
                    bool initial = random.Next(0, 2) != 0;
                    names.Add(Generate(gndr, random.Next(0, maxMiddleNames + 1), initial));
                }
            }
            return names;
        }
    }

    public enum Gender //A Gender enumator with two values - Male and Female
    {
        Male, 
        Female
    }
}
