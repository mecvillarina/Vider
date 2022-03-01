using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.App.Infrastructure.Routes
{
    public static class CreatorEndpoints
    {
        public const string GetProfile = "api/creatorportal/creator?username={0}";
        public const string Search = "api/creatorportal/creator/search?query={0}&take={1}";
        public const string UploadDump = "api/creatorportal/creator/uploaddump";
    }
}
