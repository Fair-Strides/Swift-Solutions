using PopNGo.Models;

namespace PopNGo.DAL.Abstract
{
    public interface IPgUserRepository : IRepository<PgUser>
    {
        public PgUser GetPgUserFromIdentityId(string identityId);
    }
}
