using API.Entities;

namespace API.DataAccess.Specifications.User
{
    public class FindUserByUserNameSpec: BaseSpecification<AppUser>
    {
        public FindUserByUserNameSpec(string username)
            : base(u => u.UserName == username.ToLower())
        {
            AddIncludes("Profile.Address");
            AddIncludes("Profile.Photos");
        }
    }
}
