using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace formatJson
{
  class Program
  {
    static void Main(string[] args)
    {
      ReadWriteJsonFile();
    }

    // classes for the old structure

    public class PrevRootObject
    {
      public List<Test> tests { get; set; }
    }

    public class Test
    {
      public string category { get; set; }
      public string test { get; set; }
      public List<string> testData { get; set; }
    }


    // classes for the structure
    public class NewTest
    {
      public string category { get; set; }
      public string TestName { get; set; }
      public List<Section> sections { get; set; } = new List<Section>();
    }

    public class Section
    {
      public string header { get; set; }
      public List<string> questions { get; set; } = new List<string>();
    }

    public class NewRootObject
    {
      public List<NewTest> tests { get; set; }

    }

    public static void ReadWriteJsonFile()
    {
      var myJsonString = File.ReadAllText(@"C:\Users\14022\SDG\week8\formatJson\testData.json");
      var OldStuff = JsonConvert.DeserializeObject<PrevRootObject>(myJsonString);


      var newJson = new NewRootObject
      {
        tests = new List<NewTest>()
      };
      for (var i = 0; i < OldStuff.tests.Count; i++)
      {
        var _category = OldStuff.tests[i].category;
        var _testName = OldStuff.tests[i].test;
        Section _section = null;
        var newTest = new NewTest
        {
          category = _category,
          TestName = _testName
        };
        for (int j = 0; j < OldStuff.tests[i].testData.Count; j++)
        {
          var test = OldStuff.tests[i].testData[j];
          if (test.Contains("*"))
          {
            // if the old section is a thing add it to the list
            if (_section != null)
            {
              newTest.sections.Add(_section);
            }
            // make a new section
            _section = new Section
            {
              header = test
            };
            Console.WriteLine("header === " + test);
          }
          else
          {
            Console.WriteLine("question === " + test);
            _section.questions.Add(test);
          }
        }


        newJson.tests.Add(newTest);
      }
      string json = JsonConvert.SerializeObject(newJson);
      // Console.WriteLine(json);

      using (var file = new System.IO.StreamWriter(@"test.json"))
      {
        file.WriteLine(json);
      }

      // Console.ReadLine();
      //   create a category object, create a test array with a name object and a sections array with an object.

    }
  }
}
