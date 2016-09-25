using EWiki.Api.Models;

namespace EWiki.Api.DataAccess
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(IDbFactory dbFactory)
            : base(dbFactory)
        { }
    }
}