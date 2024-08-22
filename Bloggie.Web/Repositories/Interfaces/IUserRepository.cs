using Microsoft.AspNetCore.Identity;

namespace Bloggie.Web.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAll();
    }
}
