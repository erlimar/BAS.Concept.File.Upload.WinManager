using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAS.Concept.File.Upload.Client
{
    public class ClientOptions
    {
        public string Url { get; set; }

        public string UrlUpload
        {
            get
            {
                return Url + "/upload";
            }
        }

        public string UrlDownload
        {
            get
            {
                return Url + "/download/{0}";
            }
        }

        public string UrlInfo
        {
            get
            {
                return Url + "/file/{0}";
            }
        }

        public string UrlInfoAllFiles
        {
            get
            {
                return Url + "/files";
            }
        }
    }
}
