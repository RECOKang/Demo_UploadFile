using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sample_UploadFiles
{
    class Program
    {
        static void Main(string[] args)
        {
			var imgPath = @"A:\CSFFramework.rar";
			var response = Task.Run(() => Upload(imgPath));
			Console.WriteLine($"please any key to continue......");
			Console.WriteLine($"{response.Result}");
			Console.ReadLine();
		}

		public static async Task<string> Upload(string filename)
		{
			var fs = File.ReadAllBytes(filename);

			using (var client = new HttpClient())
			{
				System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
				using (var content = new MultipartFormDataContent())
				{
					var myfilename = Path.GetFileName(filename); ;
					var streamContent = new StreamContent(new MemoryStream(fs));
					streamContent.Headers.Add("Content-Type", "application/octet-stream");
					streamContent.Headers.Add("Content-Disposition", "form-data; name=\"files\"; filename=\"" + myfilename + "\"");

					content.Add(streamContent, "file", filename);

					using (var message = await client.PostAsync("https://localhost:44379/API/Upload", content))
					{
						var input = await message.Content.ReadAsStringAsync();

						return !string.IsNullOrWhiteSpace(input) ? input : null;
					}
				}
			}
		}

	}
}
