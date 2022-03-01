using Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Context
{
    [ExcludeFromCodeCoverage]
    public class MutableCallContext : ICallContext
    {
        public Guid CorrelationId { get; set; }
        public string AuthenticationType { get; set; }
        public string FunctionName { get; set; }
        public IDictionary<string, string> AdditionalProperties { get; } = new Dictionary<string, string>();

        public bool UserRequiresAuthorization { get; set; }
        public string UserBearerAuthorizationToken { get; set; }
        
        public string Username { get; set; }
        public int UserId { get; set; }
        public string UserAccountXAddress { get; set; }
        public string UserAccountSecret { get; set; }
        public string UserAccountClassicAddress { get; set; }
        public string UserAccountAddress { get; set; }

    }
}