using alumni.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.IServices
{
    public interface IBadgeInformationService
    {
        Task<CreationResult<BadgeInformation>> CreateAsync(BadgeInformation badgeInformation);
        Task<BadgeInformation> GetAsync(string id);
    }
}
