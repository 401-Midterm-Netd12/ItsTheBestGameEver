using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TheBestGameEver.Models
{
  public class CRUD<T>
  {
    static HttpClient client = new HttpClient();
    static string URL = "https://customcharacter1.azurewebsites.net";
    static HttpWebRequest WebReq;
    static HttpWebResponse WebResp;

    public CRUD()
    {
      client.BaseAddress = new Uri(URL);
      client.DefaultRequestHeaders.Accept.Clear();
      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    // https://stackoverflow.com/questions/44775645/how-to-get-data-from-json-api-with-c-sharp-using-httpwebrequest
    // Question answered by: Keyur Patel
    // Answer provided on: Jun 27 '17 at 8:51
    public async Task<List<T>> GetModels(string modelURL, int id = 0)
    {
      string newURL = URL + modelURL;
      string jsonString;

      if (id != 0)
      {
        newURL += $"/{id}";
      }

      WebReq = (HttpWebRequest)WebRequest.Create(string.Format(newURL));
      WebReq.Method = "GET";
      WebResp = (HttpWebResponse)WebReq.GetResponse();

      using (Stream stream = WebResp.GetResponseStream()) //modified from your code since the using statement disposes the stream automatically when done
      {
        StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
        jsonString = reader.ReadToEnd();
      }

      if (id == 0)
      {
        List<T> items = JsonConvert.DeserializeObject<List<T>>(jsonString);
        return items;
      }
      List<T> itemsA = new List<T>();
      itemsA.Add(JsonConvert.DeserializeObject<T>(jsonString));
      return itemsA;
    }

    public async Task<string> DeleteModels(int id, string modelURL)
    {
      string StrResp;
      string newURL = URL + modelURL + $"/{id}";

      WebReq = (HttpWebRequest)WebRequest.Create(newURL);
      WebReq.Method = "DELETE";
      WebResp = (HttpWebResponse)WebReq.GetResponse();

      using (StreamReader stream = new StreamReader(WebResp.GetResponseStream()))
      {
        StrResp = stream.ReadToEnd();
      }

      return StrResp;
    }

    public HttpResponseMessage CreateModel(T newObject, string objRoute)
    {
      HttpResponseMessage response = null;
      string jsonObj = JsonConvert.SerializeObject(newObject);

      try
      {
        response = client.PostAsync(objRoute, new StringContent(jsonObj, Encoding.UTF8, "application/json")).Result;
        response.EnsureSuccessStatusCode();
        // Handle success
      }
      catch (HttpRequestException)
      {
        Console.WriteLine("Code broke");
        // Handle failure
      }

      return response;
    }

    public HttpResponseMessage UpdateModel(T updateObj, string objRoute)
    {
      try
      {
        HttpResponseMessage response = null;
        string jsonObj = JsonConvert.SerializeObject(updateObj);
        Console.WriteLine(jsonObj);

        response = client.PutAsync(objRoute, new StringContent(jsonObj, Encoding.UTF8, "application/json")).Result;
        response.EnsureSuccessStatusCode();
        return response;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        throw e;
      }
    }
  } // end of Class
} // end of NameSpace
