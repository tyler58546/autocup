using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace autocup
{
    class Updater
    {
        static readonly HttpClient client = new HttpClient();
        public async Task<Update> checkForUpdatesAsync()
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://tyler58546.com/autocup-version.txt");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // Above three lines can be replaced with new helper method below
                // string responseBody = await client.GetStringAsync(uri);
                responseBody = "a";
                if (responseBody.Substring(0,5).Equals(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString().Substring(0,5)))
                {
                    return null;
                }
                return new Update()
                {
                    version = responseBody,
                    downloadURL = "https://tyler58546.com/autocup"
                };
                
                
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            return null;
        }
    }
    class Update
    {
        public String version { get; set; }
        public String downloadURL { get; set; }
    }
}
