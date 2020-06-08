using apiTest1.DataProfiles;
using apiTest1.Models;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SmartNG.DataProfiles;
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

        public abstract Task CreateUser(UserRegisterProfile RegNewUser);

        public abstract Task<string> GetFieldUserKey(FiledDeivceProfile deviceid);

        public abstract Task<string> CreateFieldDevice(FieldRegisterProfile fieldRegister);

        public abstract Task<bool> CreateNewUserService(UserServicesProfile userServices);

        public abstract Task<string> VerifyUser(UserLoginProfile confirmUserDetails);

        public abstract Task<List<UserDataProfileConsumption>> GetUserCollatedServiceData(GetCollectionUserDataProfile getUserData);

        public abstract Task<UserDataProfileConsumption> GetUserServiceData(GetUserDataProfile getUserData);



    }
}
