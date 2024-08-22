using Bloggie.Web.Data;
using Bloggie.Web.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BloggieDbContext bloggieDbContext;
        public UserRepository(BloggieDbContext bloggieDbContext)
        {
            this.bloggieDbContext = bloggieDbContext;
        }

        public async Task<IEnumerable<IdentityUser>> GetAll()
        {
            var users = await bloggieDbContext.Users.ToListAsync();

            var superAdminUsers = await bloggieDbContext
                .Users
                .FirstOrDefaultAsync(data => data.Email == "superadmin@bloggie.com");

            if (superAdminUsers is not null)
            {
                users.Remove(superAdminUsers);
            }

            return users;
        }
    }
}
