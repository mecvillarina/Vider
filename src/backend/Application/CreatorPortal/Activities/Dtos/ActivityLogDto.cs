using Application.Common.Mappings;
using Domain.Entities;
using System;

namespace Application.CreatorPortal.Activities.Dtos
{
    public class ActivityLogDto : IMapFrom<ActivityLog>
    {
        public DateTime DateOccured { get; set; }
        public string Action { get; set; }
        public string TxHash { get; set; }
    }
}
