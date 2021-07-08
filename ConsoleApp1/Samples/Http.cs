using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp1.Core;
using Newtonsoft.Json;

namespace ConsoleApp1.Samples
{
    public class Post
    {
       public int Id { get; set; }
       public int UserId { get; init; }
       public string Body { get; init; }
       public string Title { get; init; } 
    }
    
    [Samples]
    public class Http
    {

        private string State { get; set; } = string.Empty;
        
        [Sequence(1)]
        public async Task<IEnumerable<Post>> GetPosts()
        {
            this.State = "Set from the GET";
            
            using var client = GetHttpClient();

            // HttpResponseMessage is IDisposable
            using var response = await client.GetAsync("http://jsonplaceholder.typicode.com/posts");
            var content = response.Content;
            var stringContent = await content.ReadAsStringAsync();

            var posts = JsonConvert.DeserializeObject<IEnumerable<Post>>(stringContent);

            return posts;
        }

        [Sequence(2)]
        public async Task<Post> CreatePost()
        {
            var post = new Post()
            {
                UserId = 5,
                Title = "Created post",
                Body = "Created post body."
            };

            var state = this.State;
            using var client = GetHttpClient();
            using var payload = new StringContent(JsonConvert.SerializeObject(post), Encoding.UTF8, "application/json");
            
            // HttpResponseMessage is IDisposable
            using var response = await client.PostAsync("http://jsonplaceholder.typicode.com/posts", payload);
            var content = response.Content;
            var stringContent = await content.ReadAsStringAsync();

            var created = JsonConvert.DeserializeObject<Post>(stringContent);
            return created;
        }
        
        public void RegularMethodNoSeq()
        {
            Console.WriteLine("Regular method");
        }

        private HttpClient GetHttpClient()
        {
            // This is suppose to stop validation of SSL.
            // Not sure why SSL doesn't validate to begin with, though.
            var clientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    (sender, clientCertificateOption, chain, sslPolicyErrors) => true
            };

            return new HttpClient(clientHandler);
        }
    }
}