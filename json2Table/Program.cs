using System;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using System.Data;
using System.Text;
using Serilog;

namespace json2Table
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory.Replace("\\bin\\Debug\\netcoreapp3.1\\", "");
            var json = "";
            if (!string.IsNullOrEmpty(path))
            {
                try
                {
                    json = File.ReadAllText(@path + "\\app\\Sample.json");
                }
                catch(FileNotFoundException ex)
                {
                    Console.WriteLine(ex);
                    Log.Information("File Not Found");
                }
            }
            
            DataRow dataRow;

            DataTable dataTable = new DataTable();

            dataTable.Columns.Add("id");
            dataTable.Columns.Add("type");
            dataTable.Columns.Add("name");            
            dataTable.Columns.Add("batter");
            dataTable.Columns.Add("topping");

            JObject totalItems = JObject.Parse(json);

            try
            {
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
                            Log.Information("Row added to Datatable!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            StreamWriter objWriter=null;
            var resultToText = new StringBuilder();
            try
            {
                foreach (DataRow row in dataTable.Rows)
                {
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        resultToText.Append(row[i].ToString());
                        resultToText.Append(i == dataTable.Columns.Count - 1 ? "\n" : ",");
                    }
                    resultToText.AppendLine();
                    Log.Information("Result Appeneded!");
                }

                objWriter = new StreamWriter(@path + "\\app\\result\\sortbyid.table.txt", false);
                objWriter.WriteLine(resultToText.ToString());                
            }
            catch(IOException ioexception)
            {
                Console.WriteLine(ioexception.Message);
                Log.Information(ioexception.Message);
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception);
            }
            finally
            {       
                if(objWriter!=null)
                {
                    objWriter.Flush();
                    objWriter.Close();                    
                    objWriter.Dispose();
                    Log.Information("Streamwriter disposed!");
                }                
            }            
            Console.WriteLine("File Generted Successfully");            
        }
    }
}
