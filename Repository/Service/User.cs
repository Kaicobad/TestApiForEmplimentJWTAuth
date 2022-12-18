using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using testapi.DataLayer;
using testapi.Repository.Interface;

namespace testapi.Repository.Service
{
    public class User : IUser
    {
        private readonly ApplicationDbContext applicationDbContext;

        public User(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public async Task<bool> CheckUserAvalilability(int id)
        {
            try
            {
                var user = applicationDbContext.users.FirstOrDefault(x => x.Id == id);

                if (user != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch ( Exception ex)
            {
                return false;
            }
        }

        public async Task<List<Model.User>> GetAllUserDetails()
        {
            try
            {
                var userList = await applicationDbContext.users.ToListAsync();
                if (userList != null)
                {
                    return userList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        //public async Task<Model.User> GetUserByCredentials(string userName)
        //{
        //    //            try
        //    //            {
        //    //                var user = await applicationDbContext.users.FindAsync(userName);

        //    //                if (user != null)
        //    //                {
        //    //                    return null;
        //    //                }
        //    //;            }
        //    //            catch (Exception)
        //    //            {

        //    //                throw;
        //    //            }

        //    return null;
        //}

        public async Task<Model.User> GetUserByCredentials(Model.User user)
        {
            
            try
            {

                var users = await applicationDbContext.users.Where(a => a.UserName == user.UserName || a.Password == user.Password).Select(a => new { a.Id, a.UserName, a.Password }).FirstAsync();

                user.UserName = users.UserName;
                user.Password = users.Password;

                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
