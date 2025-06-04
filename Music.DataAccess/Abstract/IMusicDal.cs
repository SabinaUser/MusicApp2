using Music.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Music.Core.DataAccess.IEntityRepository;

namespace Music.DataAccess.Abstract
{
    public interface IMusicDal: IEntityRepository<Musicc>
    {
        Task<List<Musicc>> GetAllAsync();
        Task<Musicc> GetByIdAsync(int id);
        Task<List<Musicc>> GetByUserIdAsync(string userId);
        Task AddAsync(Musicc music);
        Task DeleteAsync(Musicc music);
    }
}
