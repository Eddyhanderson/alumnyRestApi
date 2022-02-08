using alumni.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace alumni.IServices
{
    public interface IOrganService
    {
        Task<CreationResult<Organ>> CreateAsync(Organ organ);
    }
}
