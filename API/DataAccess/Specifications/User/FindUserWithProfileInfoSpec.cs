using API.Entities;

namespace API.DataAccess.Specifications.User
{
    public class FindUserWithProfileInfoSpec: BaseSpecification<AppUser>
    {
        public FindUserWithProfileInfoSpec(int id)
            :base(x => x.Id == id)
        {
            AddIncludes("Profile.Address");
            AddIncludes("Profile.Photos");
        }
    }
}
