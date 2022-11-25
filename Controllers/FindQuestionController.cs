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
        string count;
        [HttpGet]
        [Route("/getques")]
        public FindQuestion GetQuestions()
        {
            var questionList = new List<QuestionData>();
            var testDetailsList = new List<FindQuestion>();
            var test = new FindQuestion();
            using (StreamReader r = new StreamReader("DemoFile/Demo1.json"))
            {

                var json = r.ReadToEnd();
                var createTestData = JsonConvert.DeserializeObject<FindDemoData>(json);

                var parsedObject = JsonDocument.Parse(json);
                var rootElement = parsedObject.RootElement;

                var countques = rootElement.GetProperty("count");
                var countq = System.Text.Json.JsonSerializer.Serialize(countques);
                count = JsonConvert.DeserializeObject<string>(countq);

                questionList.AddRange(createTestData.question);


                /*------------ FOR READING SINGLE OBJECT PROPERTIES---------------- */

                // var testJson = rootElement.GetProperty("question").GetProperty("test");
                // var numberJson = rootElement.GetProperty("question").GetProperty("no");
                // var number = System.Text.Json.JsonSerializer.Serialize(numberJson);
                // number2 = JsonConvert.DeserializeObject<string>(number);
                // string filename = System.Text.Json.JsonSerializer.Serialize(testJson);
                // filename2 = JsonConvert.DeserializeObject<string>(filename);


            }

            foreach (QuestionData questionData in questionList)
            {
                string _path = "Jsonfiles/" + questionData.test + ".json";
                using (StreamReader r = new StreamReader(_path))
                {
                    String json = r.ReadToEnd();
                    var testData = JsonConvert.DeserializeObject<FindQuestion>(json);
                    testDetailsList.Add(testData);


                    /*------------------------ FOR READING SINGLE OBJECT PROPERTIES------------------------ */

                    // var parsedObject = JsonDocument.Parse(json);
                    // var rootElement = parsedObject.RootElement;
                    // var assignJson = rootElement.GetProperty("msgData").GetProperty("assignment");
                    // source = System.Text.Json.JsonSerializer.Serialize(assignJson)
                    // result = JsonConvert.DeserializeObject<string>(source);
                    // var lms = System.Text.Json.JsonSerializer.Serialize(rootElement.GetProperty("lmsData"));
                    // lmsData = JsonConvert.DeserializeObject<string>(lms);
                    // // var msg=System.Text.Json.JsonSerializer.Serialize(rootElement.GetProperty("msgData"));
                    // // msgData=JsonConvert.DeserializeObject<string>(msg);
                    // var conf = System.Text.Json.JsonSerializer.Serialize(rootElement.GetProperty("msgData").GetProperty("config"));
                    // config = JsonConvert.DeserializeObject<string>(conf);

                }
            }
            var path = "JsonWritefiles/test2.json";
            using (StreamWriter sw = new StreamWriter(path))
            {

                var testQuesNum = 1;

                test.lmsData = testDetailsList[0].lmsData;
                test.msgData = new MsgData();
                test.msgData.config = testDetailsList[0].msgData.config;
                JObject testAssignment = new JObject();
                for (var i = 0; i < testDetailsList.Count; i++)
                {
                    var testDetails = testDetailsList[i];
                    JObject testAssign = JObject.Parse(testDetails.msgData.assignment);




                    foreach (KeyValuePair<string, JToken> var in testAssign)
                    {

                        if (var.Key.Contains("question." + questionList[i].no + "."))
                        {
                            var newProp = "question." + testQuesNum + "." + var.Key.Replace("question." + questionList[i].no + ".", "");
                            testAssignment[newProp] = var.Value;
                        }
                    }
                    testQuesNum++;
                }


                testAssignment["count"] = count;
                test.msgData.assignment = JsonConvert.SerializeObject(testAssignment, Formatting.None);
                sw.WriteLine(JsonConvert.SerializeObject(test, Formatting.Indented));




                /*---------------------- FOR WRITING SINGLE OBJECT PROPERTIES-------------------------- */



                // // string result2="";
                // var myDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                // foreach (var key in myDictionary.Keys)
                // {
                //     if (key.Contains("question." + number2 + ".") == false && key.Contains("question.count") == false)
                //     {
                //         myDictionary.Remove(key);
                //     }
                // }
                // myDictionary["question.count"] = count;
                // var result2 = System.Text.Json.JsonSerializer.Serialize(myDictionary);
                // //result2 += ",\"question.count\":" + count;
                // // var result3 = System.Text.Json.JsonSerializer.Serialize(result2);
                // JObject jsonObject =
                // new JObject(
                //              new JProperty("lmsData", lmsData),
                //             //  new JProperty("msgData",msgData),
                //             new JProperty("msgData", new JObject(
                //               new JProperty("config", config),
                //              new JProperty("assignment", result2)
                //             ))
                //         );
                // //  Console.WriteLine(jsonObject);
                // sw.WriteLine(JsonConvert.SerializeObject(jsonObject, Formatting.None));
            }

            return test;
        }


    }


}


