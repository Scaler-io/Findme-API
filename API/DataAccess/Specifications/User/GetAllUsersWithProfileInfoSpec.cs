using API.Entities;

namespace API.DataAccess.Specifications.User
{
    public class GetAllUsersWithProfileInfoSpec: BaseSpecification<AppUser>
    {
        public GetAllUsersWithProfileInfoSpec()
        {
            AddIncludes("Profile.Address");
            AddIncludes("Profile.Photos");
        }
    }
}
