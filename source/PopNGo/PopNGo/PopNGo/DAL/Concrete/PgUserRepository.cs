using PopNGo.DAL.Abstract;
using PopNGo.Models;
using Microsoft.EntityFrameworkCore;

namespace PopNGo.DAL.Concrete
{
    public class PgUserRepository : Repository<PgUser>, IPgUserRepository
    {
        private readonly DbSet<PgUser> _pgUsers;
        public PgUserRepository(PopNGoDB context) : base(context)
        {
            _pgUsers = context.PgUsers;
        }

        public PgUser GetPgUserFromIdentityId(string identityId)
        {
            return _pgUsers.FirstOrDefault(u => u.AspnetuserId == identityId);
        }
    }
}
