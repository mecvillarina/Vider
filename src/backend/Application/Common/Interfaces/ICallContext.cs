using System;
using System.Collections.Generic;

namespace Application.Common.Interfaces
{
    public interface ICallContext
    {
        Guid CorrelationId { get; set; }

        string FunctionName { get; set; }


        string AuthenticationType { get; set; }

        IDictionary<string, string> AdditionalProperties { get; }

        bool UserRequiresAuthorization { get; set; }
        string UserBearerAuthorizationToken { get; set; }

        int UserId { get; set; }
        string Username { get; set; }
        string UserAccountXAddress { get; set; }
        string UserAccountSecret { get; set; }
        string UserAccountClassicAddress { get; set; }
        string UserAccountAddress { get; set; }
    }
}