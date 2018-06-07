using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ctOS_Moderation.Modules
{
    public static class JSONHelper
    {
        public static List<JObject> GetJSONObjects(string filename) {
            int BracketCount = 0;
            string JSONString = File.ReadAllText(filename);
            List<string> JsonItems = new List<string>();
            StringBuilder Json = new StringBuilder();

            foreach (char c in JSONString) {
                if (c == '{')
                    ++BracketCount;
                else if (c == '}')
                    --BracketCount;
                Json.Append(c);

                if (BracketCount == 0 && c != ' ') {
                    JsonItems.Add(Json.ToString());
                    Json = new StringBuilder();
                }
            }
            List<JObject> JObjs = new List<JObject>();

            foreach (string JObj in JsonItems) {
                JObjs.Add(JObject.Parse(JObj));
            }
            return JObjs;
        }
        public static string GetJObjectValue(JObject array, string key) {
            foreach (KeyValuePair<string, JToken> keyValuePair in array) {
                if (key == keyValuePair.Key) {
                    return keyValuePair.Value.ToString();
                }
            }
            Console.WriteLine($"Error, no key found for {key}");
            return String.Empty;
        }
    }
}
