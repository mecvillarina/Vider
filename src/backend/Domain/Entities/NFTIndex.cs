using System;

namespace Domain.Entities
{
    public class NFTIndex
    {
        public int Id { get; set; }
        public string TokenId { get; set; }
        public int TokenTaxon { get; set; }
        public string Uri { get; set; }
        public string UriHex { get; set; }
        public string Metadata { get; set; }
        public int TokenFlags { get; set; }
        public int NftSerial { get; set; }
        public DateTimeOffset Created { get; set; }
    }
}
