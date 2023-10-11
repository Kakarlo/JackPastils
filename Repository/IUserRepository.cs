using JackPastil.Model;
using System.Collections.Generic;

namespace JackPastil.Repository {
    public interface IUserRepository {
        // Sample palang po
        bool AuthenticateUser(string username, string password);
        void Add(UserModel userModel);
        void Edit(UserModel userModel);
        void Remove(UserModel userModel);
        UserModel GetById(int id);
        UserModel GetByUsername(string username);
        IEnumerable<UserModel> GetAll();
    }
}
