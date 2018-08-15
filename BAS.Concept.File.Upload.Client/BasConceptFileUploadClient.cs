using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BAS.Concept.File.Upload.Client
{
    public class BasConceptFileUploadClient
    {
        private const string URL_DEFAULT = "https://bas-concept-file-upload-api.herokuapp.com";
        private ClientOptions _options;

        public BasConceptFileUploadClient()
            : this(new ClientOptions { Url = URL_DEFAULT })
        { }

        public BasConceptFileUploadClient(ClientOptions options)
        {
            _options = options;
        }

        public WebResponse<WebFileInfo> SendFile(Stream fileStream, string fileName)
        {
            var response = new WebResponse<WebFileInfo>();

            using (var client = new HttpClient())
            using (var fsr = new BinaryReader(fileStream))
            {
                fileStream.Position = 0;

                var form = new MultipartFormDataContent();
                var fileContent = new ByteArrayContent(fsr.ReadBytes((int)fileStream.Length));
                fileContent.Headers.ContentType = 
                    MediaTypeHeaderValue.Parse(ContentTypeMap.ComputeMimeType(fileName));

                form.Add(fileContent, "file", fileName);

                using (var result = client.PostAsync(_options.UrlUpload, form))
                {
                    result.Wait();

                    response.StatusCode = (int)result.Result.StatusCode;
                    response.StatusText = result.Result.StatusCode.ToString();

                    var content = result.Result.Content.ReadAsStringAsync();

                    content.Wait();

                    var resultList = JsonConvert.DeserializeObject<WebFileInfo[]>(content.Result);

                    response.Data = resultList[0];
                    response.Data.MimeType = fileContent.Headers.ContentType.MediaType;
                }
            }

            return response;
        }

        public WebResponse<IEnumerable<WebFileInfo>> GetAllFiles()
        {
            var response = new WebResponse<IEnumerable<WebFileInfo>>
            {
                Data = new List<WebFileInfo>()
            };

            using (var client = new HttpClient())
            {
                using (var result = client.GetAsync(_options.UrlInfoAllFiles))
                {
                    result.Wait();

                    response.StatusCode = (int)result.Result.StatusCode;
                    response.StatusText = result.Result.StatusCode.ToString();

                    var content = result.Result.Content.ReadAsStringAsync();

                    content.Wait();

                    response.Data = JsonConvert.DeserializeObject<List<WebFileInfo>>(content.Result);
                }
            }

            return response;
        }

        public WebResponse<WebFileInfo> GetFileInfo(string fileId)
        {
            var response = new WebResponse<WebFileInfo>();

            using (var client = new HttpClient())
            {
                using (var result = client.GetAsync(string.Format(_options.UrlInfo, fileId)))
                {
                    result.Wait();

                    response.StatusCode = (int)result.Result.StatusCode;
                    response.StatusText = result.Result.StatusCode.ToString();

                    var content = result.Result.Content.ReadAsStringAsync();

                    content.Wait();

                    response.Data = JsonConvert.DeserializeObject<WebFileInfo>(content.Result);
                }
            }

            return response;
        }

        public Stream GetFileContent(string fileId)
        {
            throw new NotImplementedException();
        }
    }
}
