using apiTest1.DataProfiles;
using apiTest1.Models;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiTest1.Data
{
    public abstract class BaseCommandRepo
    {

        public abstract Task<bool> SaveChanges();

        public abstract Task<bool> AddUserServiceData(UserDataProfile userData);

        public abstract Task CreateUser(UserRegisterModel RegNewUser);

        public abstract Task<string> CreateNewUserService(string apikey, string NewServiceName);

        public abstract Task<string> VerifyUser(UserLoginProfile confirmUserDetails);

        public abstract Task<UserServiceDataModel> GetUserData();

    }
}
