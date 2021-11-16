using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using System.Data;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;

namespace json2Table
{
    class Program
    {
        public string jsonFile = @"C:\Users\achin\Desktop\Sample.json";
        static void Main(string[] args)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug\\netcoreapp3.1\\", "");
            var json = File.ReadAllText(@path + "\\app\\Sample.json");
            DataRow dataRow;

            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("id");
            dataTable.Columns.Add("type");
            dataTable.Columns.Add("name");
            dataTable.Columns.Add("ppu");
            dataTable.Columns.Add("batter");
            dataTable.Columns.Add("topping");

            JObject totalItems = JObject.Parse(json);
            
            int itemCount = totalItems["items"]["item"].Count();
            for (int i = 0; i < itemCount; i++)
            {
                int battercount = totalItems["items"]["item"][i]["batters"]["batter"].Count();
                int toppingsCount = totalItems["items"]["item"][i]["topping"].Count();

                for (int j = 0; j < battercount; j++)
                {
                    for (int k = 0; k < toppingsCount; k++)
                    {
                        dataRow = dataTable.NewRow();
                        dataRow["id"] = totalItems["items"]["item"][i]["id"].ToString();
                        dataRow["type"] = totalItems["items"]["item"][i]["type"].ToString();
                        dataRow["name"] = totalItems["items"]["item"][i]["name"].ToString();
                        dataRow["batter"] = totalItems["items"]["item"][0]["batters"]["batter"][j]["type"].ToString();
                        dataRow["topping"] = totalItems["items"]["item"][0]["topping"][k]["type"].ToString();
                        dataTable.Rows.Add(dataRow);
                    }
                }
            }
            
            var resultToText = new StringBuilder();
            foreach (DataRow row in dataTable.Rows)
            {
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    resultToText.Append(row[i].ToString());
                    resultToText.Append(i == dataTable.Columns.Count - 1 ? "\n" : ",");
                }
                resultToText.AppendLine();
            }

            StreamWriter objWriter = new StreamWriter(@path+ "\\app\\result\\sortbyid.table.txt", false);
            objWriter.WriteLine(resultToText.ToString());
            objWriter.Close();
            
            Console.WriteLine();
            
        }
    }
}
