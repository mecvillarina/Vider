using System;

namespace Domain.Entities
{
    public class Creator
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string UsernameNormalize { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }

        public string Salt { get; set; }

        //XRPL Accounts
        public string AccountXAddress { get; set; }
        public string AccountSecret { get; set; }
        public string AccountClassicAddress { get; set; }
        public string AccountAddress { get; set; }
        public bool IsAccountValid { get; set; }

        public string ProfilePictureFilename { get; set; }

        public DateTime DateAccountAcquired { get; set; }
        public DateTime DateRegistered { get; set; }
    }
}
