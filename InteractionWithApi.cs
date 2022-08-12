using EasyBookFront.Model;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasyBookFront
{
    public static class InteractionWithApi
    {
        public static List<Book> GetAll()
        {
            var client = new RestClient("https://localhost:5001");
            var request = new RestRequest("/api/Book");
            try
            {
                var response = client.Execute<List<Book>>(request);
                return response.Data.OrderBy(x => x.BookName).ToList();
            }
            catch
            {
                return new List<Book>();
            }
        }

        public static Book Get(string name)
        {
            var client = new RestClient("https://localhost:5001");
            var request = new RestRequest($"/api/Book/{name}");
            try
            {
                var response = client.Execute<Book>(request);
                return response.Data;
            }
            catch
            {
                return new Book();
            }
        }

        public static bool Post(Book item)
        {
            var client = new RestClient("https://localhost:5001");
            var request = new RestRequest($"/api/Book/", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", JsonSerializer.Serialize(item), ParameterType.RequestBody);
            try
            {
                var response = client.Execute<bool>(request);
                return response.Data;
            }
            catch
            {
                return false;
            }
        }

        public static bool Put(string name, Book item)
        {
            var client = new RestClient("https://localhost:5001");
            var request = new RestRequest($"/api/Book/{name}", Method.Put);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", JsonSerializer.Serialize(item), ParameterType.RequestBody);
            try
            {
                var response = client.Execute<bool>(request);
                return response.Data;
            }
            catch
            {
                return false;
            }
        }

        public static bool Delete(string name)
        {
            var client = new RestClient("https://localhost:5001");
            var request = new RestRequest($"/api/Book/{name}", Method.Delete);
            try
            {
                var response = client.Execute<bool>(request);
                return response.Data;
            }
            catch
            {
                return false;
            }
        }
    }
}
