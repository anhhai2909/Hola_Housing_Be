using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace HolaHousing_BE.Services.ImageService
{
    public class ImageService
    {
        private readonly string CLIENT_ID = "f260b84e886a6c3";
        private readonly string ACCESS_TOKEN = "b2055b90ce99f1634fddcfddbdb819d89fd3086a";
        private readonly string ALBUM_ID = "Q5vKEcs";

        public ImageService() {}

        public async Task<string> UploadImageAsync1(IFormFile file, string name)
        {
            using (var memoryStream = new MemoryStream())
            {
                string clientId = "";
                await file.CopyToAsync(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();

                using (var client = new HttpClient())
                {
                    // Add Client ID to the Authorization header
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + ACCESS_TOKEN);

                    // Create multipart form content to upload the file
                    var content = new MultipartFormDataContent();
                    content.Add(new ByteArrayContent(fileBytes), "image", name);
                    content.Add(new StringContent(ALBUM_ID), "album");
                    // Send POST request to Imgur API
                    var response = await client.PostAsync("https://api.imgur.com/3/upload", content);
                    var responseData = await response.Content.ReadAsStringAsync();
                    var imageUrl = JsonConvert.DeserializeObject<ImgurResponseData>(responseData);
                    return imageUrl.Data.Link; // Return the image URL
                }
            }
        }

        public async Task<string> UploadImageAsync2(string imageDataBase64String)
        {
            byte[] response;
            using (var client = new WebClient())
            {
                client.Headers.Add("Authorization", "Client-ID " + CLIENT_ID);
                var values = new NameValueCollection { { "image", imageDataBase64String } };
                response = await client.UploadValuesTaskAsync("https://api.imgur.com/3/upload", values);
            }

            var result = JsonConvert.DeserializeObject<ImgurResponseData>(Encoding.ASCII.GetString(response));

            return result.Data.Link;
        }
    }

    class ImgurResponseData
    {
        public ImgurImageDataViewModel Data { get; set; }
        public bool Success { get; set; }
        public int Status { get; set; }
    }

    class ImgurImageDataViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Datetime { get; set; }
        public string Type { get; set; }
        public bool Animated { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Size { get; set; }
        public int Views { get; set; }
        public int Bandwidth { get; set; }
        public string Deletehash { get; set; }
        public string Section { get; set; }
        public string Link { get; set; }
        public string Account_url { get; set; }
        public int Account_id { get; set; }
        public override string ToString()
        {
            return $"ImgurImageDataViewModel: \n" +
                   $"Id: {Id}\n" +
                   $"Title: {Title}\n" +
                   $"Description: {Description}\n" +
                   $"Datetime: {Datetime}\n" +
                   $"Type: {Type}\n" +
                   $"Animated: {Animated}\n" +
                   $"Width: {Width}\n" +
                   $"Height: {Height}\n" +
                   $"Size: {Size} bytes\n" +
                   $"Views: {Views}\n" +
                   $"Bandwidth: {Bandwidth} bytes\n" +
                   $"Deletehash: {Deletehash}\n" +
                   $"Section: {Section}\n" +
                   $"Link: {Link}\n" +
                   $"Account URL: {Account_url}\n" +
                   $"Account ID: {Account_id}\n";
        }
    }
}
