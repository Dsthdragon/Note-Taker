using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace start_up
{
    public class Note
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }
    }

    public class PostNote
    {
        public string Content { get; set; }
    }

    public class PostResponse
    {

        public string Status { get; set; }

        public string Message { get; set; }
    }

    public class NoteRepository : IDisposable
    {
        private HttpClient httpClient = new HttpClient();

        private DefaultContractResolver contractResolver = new DefaultContractResolver
        {
            NamingStrategy = new SnakeCaseNamingStrategy()
        };

        public NoteRepository()
        {
            this.httpClient.BaseAddress = new Uri("http://localhost:8080/note_taker/api/");
            this.httpClient.DefaultRequestHeaders.Accept.Clear();
            this.httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
                );
        }


        public async Task<Note> create(PostNote note)
        {
            string jsonContent = JsonConvert.SerializeObject(note, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });
            StringContent stringContent = new StringContent(jsonContent, Encoding.UTF8);
            HttpResponseMessage response = await this.httpClient.PostAsync("create", stringContent);
            String resp = await response.Content.ReadAsStringAsync();
            Debug.Write(resp.GetType());
            stringContent.Dispose();
            return JsonConvert.DeserializeObject<Note>(resp);
        }

        public async Task<PostResponse> update(PostNote postNote, Note note)
        {
            string jsonContent = JsonConvert.SerializeObject(postNote, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });
            StringContent stringContent = new StringContent(jsonContent, Encoding.UTF8);
            HttpResponseMessage response = await this.httpClient.PostAsync("update/" + note.Id, stringContent);
            String resp = await response.Content.ReadAsStringAsync();
            Debug.Write(resp.GetType());
            stringContent.Dispose();
            return JsonConvert.DeserializeObject<PostResponse>(resp);
        }

        public async Task<List<Note>> getNotes()
        {
            HttpResponseMessage response = await this.httpClient.GetAsync("get");
            String resp = await response.Content.ReadAsStringAsync();
            //List<Note> notes = new List<Note>();
            //foreach(JObject data in (JArray)JsonConvert.DeserializeObject(resp))
            //{
            //    Note note = 
            //}
            return JsonConvert.DeserializeObject<List<Note>>(resp);

        }

        public void Dispose()
        {
            this.httpClient.Dispose();
        }
    }
}
