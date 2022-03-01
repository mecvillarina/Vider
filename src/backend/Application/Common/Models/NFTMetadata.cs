using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace Application.Common.Models
{
    public class NFTMetadata
    {
        public string Id { get; set; }
        public string Creator { get; set; }
        public string Uri { get; set; }
        public string Name { get; set; }
        public DateTimeOffset Created { get; set; }

        public Stream ToStream()
        {
            var json = JsonConvert.SerializeObject(this);
            return new MemoryStream(Encoding.UTF8.GetBytes(json ?? ""));
        }
    }
}
