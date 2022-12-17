using testapi.Model;

namespace testapi.Repository.Interface
{
    public interface IUser
    {
        public Task<List<User>> GetAllUserDetails();
        public Task<bool> CheckUserAvalilability(int id);
        public Task<User> GetUserByCredentials(Model.User user);
    }
}
