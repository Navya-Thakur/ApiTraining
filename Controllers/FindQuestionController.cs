using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using backend.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/ques")]
    public class FindQuestionController : ControllerBase
    {
        //List<FindQuestion> source = new List<FindQuestion>();
        string result;
        string source;
        public string filename2;

        string number2;
        string lmsData;
        string msgData;
        string config;
        string count;


        [HttpGet]
        [Route("/getques")]
        public string GetQuestions()
        {

            using (StreamReader r = new StreamReader("DemoFile/Demo1.json"))
            {

                String json = r.ReadToEnd();
                var parsedObject = JsonDocument.Parse(json);
                var rootElement = parsedObject.RootElement;

                var countques = rootElement.GetProperty("count");
                var countq = System.Text.Json.JsonSerializer.Serialize(countques);
                count = JsonConvert.DeserializeObject<string>(countq);
                var testJson = rootElement.GetProperty("question").GetProperty("test");
                var numberJson = rootElement.GetProperty("question").GetProperty("no");
                var number = System.Text.Json.JsonSerializer.Serialize(numberJson);
                number2 = JsonConvert.DeserializeObject<string>(number);
                string filename = System.Text.Json.JsonSerializer.Serialize(testJson);
                filename2 = JsonConvert.DeserializeObject<string>(filename);


            }


            string _path = "Jsonfiles/" + filename2 + ".json";
            using (StreamReader r = new StreamReader(_path))
            {
                String json = r.ReadToEnd();

                var parsedObject = JsonDocument.Parse(json);

                var rootElement = parsedObject.RootElement;
                var assignJson = rootElement.GetProperty("msgData").GetProperty("assignment");
                source = System.Text.Json.JsonSerializer.Serialize(assignJson);

                result = JsonConvert.DeserializeObject<string>(source);
                var lms = System.Text.Json.JsonSerializer.Serialize(rootElement.GetProperty("lmsData"));
                lmsData = JsonConvert.DeserializeObject<string>(lms);
                // var msg=System.Text.Json.JsonSerializer.Serialize(rootElement.GetProperty("msgData"));
                // msgData=JsonConvert.DeserializeObject<string>(msg);
                var conf = System.Text.Json.JsonSerializer.Serialize(rootElement.GetProperty("msgData").GetProperty("config"));
                config = JsonConvert.DeserializeObject<string>(conf);






            }
            var path = "JsonWritefiles/test2.json";
            using (StreamWriter sw = new StreamWriter(path))
            {

                // string result2="";
                var myDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);

                foreach (var key in myDictionary.Keys)
                {
                    if (key.Contains("question." + number2 + ".") == false && key.Contains("question.count") == false)
                    {
                        // result2+=key+":"+myDictionary[key]+","+" ";
                        if(key.Contains("question.count"))
                        {
                            //myDictionary.Replace(myDictionary[key], count);
                            myDictionary[key] = count;

                        }
                        myDictionary.Remove(key);
                    }
                }

                var result2 = System.Text.Json.JsonSerializer.Serialize(myDictionary);
                //result2 += ",\"question.count\":" + count;
                // var result3 = System.Text.Json.JsonSerializer.Serialize(result2);


                JObject jsonObject =
                new JObject(
                             new JProperty("lmsData", lmsData),
                            //  new JProperty("msgData",msgData),
                            new JProperty("msgData", new JObject(

                              new JProperty("config", config),
                             new JProperty("assignment", result2)
                            ))

                        );

                //  Console.WriteLine(jsonObject);
                sw.WriteLine(JsonConvert.SerializeObject(jsonObject, Formatting.None));




            }
            return result;
        }


    }


}


