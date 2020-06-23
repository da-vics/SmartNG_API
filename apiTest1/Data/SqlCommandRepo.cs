using apiTest1.DataProfiles;
using apiTest1.Helpers;
using apiTest1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartNG.DataProfiles;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace apiTest1.Data
{
    public class SqlCommandRepo : BaseCommandRepo
    {

        #region PrivateProps
        private readonly apiDBContext _commandDbContext;
        private readonly DataEncryptionHelper _dataEncryptionHelper;
        #endregion

        #region PublicProps
        public readonly UserRegisterModel RegisterModel;
        public readonly UserServicesModel servicesModel;
        public readonly UserServiceDataModel serviceDataModel;
        #endregion


        #region Constructor
        public SqlCommandRepo(apiDBContext commandDbContext)
        {
            _commandDbContext = commandDbContext;
            RegisterModel = new UserRegisterModel();
            servicesModel = new UserServicesModel();
            serviceDataModel = new UserServiceDataModel();
            _dataEncryptionHelper = new DataEncryptionHelper();
        }
        #endregion


        #region CreateNewUser Function
        public override async Task CreateUser(UserRegisterProfile userRegister)
        {
            if (userRegister == null)
                throw new ArgumentNullException("null parameter Detected!");

            try
            {
                var check = await _commandDbContext.RegisterUser.FirstOrDefaultAsync(user => user.Email == userRegister.Email);
                if (check != null)
                    throw new ArgumentException($"a user with this email {userRegister.Email} exsits");
            }

            catch (ArgumentException)
            {
                throw;
            }

            var newUser = new UserRegisterModel
            {
                Email = userRegister.Email,
                ApiKeyId = _dataEncryptionHelper.Encrypt(userRegister.Email, "smarterliving"),
                PassWordHash = _dataEncryptionHelper.Encrypt(userRegister.PassWordHash, "smarterliving", userRegister.Email),
                HomeAddress = userRegister.HomeAddress,
                PhoneNumber = userRegister.PhoneNumber,
                FullName = userRegister.FullName
            };


            await _commandDbContext.RegisterUser.AddAsync(newUser);
            await SaveChanges();
        }

        #endregion


        #region GetCollectionOfUserData

        public override async Task<List<UserDataProfileConsumption>> GetUserCollatedServiceData(GetCollectionUserDataProfile getUserData)
        {
            CultureInfo MyCultureInfo = new CultureInfo("de-DE");
            DateTime dateTime;
            bool checkdate = DateTime.TryParse(getUserData.setRecord, out dateTime);

            if (checkdate == false)
                return null;

            IQueryable<UserServiceDataModel> result = null;

            await Task.Run(() =>
            {
                result = from data in _commandDbContext.UserData
                         where data.ApiKeyId == getUserData.apikey && data.DeviceId == getUserData.DeviceId && data.DataInsertDat >= dateTime
                         orderby data.DataInsertDat descending
                         select data;

            });

            if (result == null)
                return null;

            else
            {
                List<UserDataProfileConsumption> convertResult = new List<UserDataProfileConsumption>();

                await Task.Run(() =>
                {
                    foreach (var data in result)
                    {
                        convertResult.Add(new UserDataProfileConsumption { Userdata = data.ServiceData, DateInserted = data.DataInsertDat.ToString() });
                    }

                });

                return convertResult;
            }

        }

        #endregion


        #region Save Changes to DataBase
        public override async Task<bool> SaveChanges()
        {
            int bb = 0;
            try
            {

                bb = await _commandDbContext.SaveChangesAsync();

            }

            catch (NullReferenceException)
            {
                throw;
            }

            finally
            {

            }
            bool a = (bb > 0) ? true : false;
            return a;
        }
        #endregion


        #region VerifyUser

        public override async Task<string> VerifyUser(UserLoginProfile confirmUserDetails)
        {

            if (confirmUserDetails == null)
                return string.Empty;

            var result = await _commandDbContext.RegisterUser.FirstOrDefaultAsync(user => user.Email == confirmUserDetails.Email);
            if (result == null)
                return string.Empty;

            if (_dataEncryptionHelper.VerifyMd5Hash(confirmUserDetails.Password, result.PassWordHash, "smarterliving", result.Email))
            {
                return result.ApiKeyId;
            }

            else
            {
                return string.Empty;
            }
        }

        #endregion


        #region CreateNewService

        public override async Task<bool> CreateNewUserService(UserServicesProfile userServices)
        {

            if (string.IsNullOrEmpty(userServices.ApiKey) || string.IsNullOrEmpty(userServices.ServiceName))
                return false;

            UserRegisterModel checkKey = null;

            try
            {
                checkKey = await _commandDbContext.RegisterUser.FirstOrDefaultAsync(c => c.ApiKeyId == userServices.ApiKey);
                if (checkKey == null)
                    throw new ArgumentException("Access denied user not found!");
            }

            catch (ArgumentException)
            {
                throw;
            }


            try
            {
                var deviceCheck = await _commandDbContext.SetupModels.FirstOrDefaultAsync(device => device.Id == userServices.DeviceId);
                if (deviceCheck == null)
                    throw new ArgumentException("Device not Found!");
            }

            catch (ArgumentException)
            {
                throw;
            }

            var convertToDBName = $"{userServices.ServiceName}_{checkKey.Email}";

            try
            {
                var checkService = await _commandDbContext.UserServices.FirstOrDefaultAsync(c => c.ServiceName == convertToDBName);
                if (checkService != null)
                    throw new ArgumentException("ServiceName already exist");

                var checkServiceId = await _commandDbContext.UserServices.FirstOrDefaultAsync(c => c.DeviceId == userServices.DeviceId);
                if (checkServiceId != null)
                    throw new ArgumentException("DeviceId has been registered");
            }

            catch (ArgumentException)
            {
                throw;
            }

            var newservice = new UserServicesModel
            {
                ApiKeyId = userServices.ApiKey,
                ServiceName = convertToDBName,
                DeviceId = userServices.DeviceId,
                DeviceType = (short)userServices.DeviceType
            };
            await _commandDbContext.UserServices.AddAsync(newservice);


            _ = await this.SaveChanges();


            return true;

        }

        #endregion

        #region AddUserServiceData

        public override async Task<bool> AddUserServiceData(UserDataProfile userData)
        {
            var confirmID = await _commandDbContext.UserServices.FirstOrDefaultAsync(dat => dat.DeviceId == userData.DeviceId && dat.ApiKeyId == userData.apikey);


            if (confirmID == null)
                return false;

            var userServiceData = new UserServiceDataModel();

            #region setDataFields
            userServiceData.ApiKeyId = userData.apikey;
            userServiceData.DeviceId = userData.DeviceId;
            userServiceData.ServiceData = (decimal)userData.Userdata;
            userServiceData.DataInsertDat = DateTime.UtcNow;
            #endregion

            await _commandDbContext.UserData.AddAsync(userServiceData);
            _ = await this.SaveChanges();

            return true;
        }

        #endregion


        #region CreateNewFieldDevice

        public override async Task<string> CreateFieldDevice(FieldRegisterProfile fieldRegister)
        {
            if (fieldRegister == null)
                return string.Empty;

            fieldRegister.MasterKey = _dataEncryptionHelper.Encrypt(fieldRegister.MasterKey);

            var check = await _commandDbContext.FieldMasterKey.FirstOrDefaultAsync(keys => keys.AccessKey == fieldRegister.MasterKey);

            if (check != null)
            {
                var newfieldDevice = new DeviceSetupModel();
                var result = await _commandDbContext.SetupModels.AddAsync(newfieldDevice);
                _ = await this.SaveChanges();

                return result.Entity.Id.ToString();
            }

            return string.Empty;
        }

        #endregion

        #region GetFIeldKey

        public async override Task<DeviceCalibrationProfile> GetFieldUserKey(FiledDeivceProfile deviceConfig)
        {

            UserServicesModel check = null;
            try
            {
                check = await _commandDbContext.UserServices.FirstOrDefaultAsync(c => c.DeviceId == deviceConfig.DeviceId);
                if (check == null)
                    throw new ArgumentException($"no user assigned to {deviceConfig.DeviceId}");
            }

            catch (ArgumentException)
            {
                throw;
            }

            DeviceCalibrationProfile deviceCalibration = new DeviceCalibrationProfile();


            return new DeviceCalibrationProfile()
            {
                ApiKey = check.ApiKeyId,
                DeviceType = check.DeviceType
            };

        }

        #endregion

        #region GetUserServiceData

        public override async Task<UserDataProfileConsumption> GetUserServiceData(GetUserDataProfile getUserData)
        {

            List<UserServiceDataModel> result = new List<UserServiceDataModel>();

            await Task.Run(() =>
             {
                 result = (from data in _commandDbContext.UserData
                           where data.ApiKeyId == getUserData.apikey && data.DeviceId == getUserData.DeviceId
                           orderby data.Id descending
                           select data).Take(1).ToList<UserServiceDataModel>();
             });

            if (result.Count <= 0)
                return null;

            return new UserDataProfileConsumption { Userdata = result[0].ServiceData, DateInserted = result[0].DataInsertDat.ToString() };

        }

        #endregion

        #region UpdateUserService

        public override async Task<bool> UpdateUserService(UserServicesProfile userServices)
        {

            UserRegisterModel checkKey = null;

            try
            {
                checkKey = await _commandDbContext.RegisterUser.FirstOrDefaultAsync(c => c.ApiKeyId == userServices.ApiKey);
                if (checkKey == null)
                    throw new ArgumentException("Access denied user not found!");
            }

            catch (ArgumentException)
            {
                throw;
            }

            var convertToDBName = $"{userServices.ServiceName}_{checkKey.Email}";

            UserServicesModel checkService = null;

            try
            {
                checkService = await _commandDbContext.UserServices.FirstOrDefaultAsync(c => c.DeviceId == userServices.DeviceId);
                if (checkService == null)
                    throw new ArgumentException("Device not Found!");
            }

            catch (ArgumentException)
            {
                throw;
            }

            checkService.DeviceType = (short)userServices.DeviceType;

            _commandDbContext.UserServices.Update(checkService);

            _ = await this.SaveChanges();

            return true;
        }
        #endregion

        #region GetUserServices
        public override async Task<List<GetUserServiceDataProfile>> GetUserServices(GetUserServicesProfile getUserServices)
        {
            if (string.IsNullOrEmpty(getUserServices.ApiKey) || getUserServices == null)
                return null;

            IQueryable<UserServicesModel> result = null;
            await Task.Run(() =>
             {
                 result = from service in _commandDbContext.UserServices
                          where getUserServices.ApiKey == service.ApiKeyId
                          select service;
             });

            try
            {
                if (result == null || result.Count<UserServicesModel>() <= 0)
                    throw new ArgumentException("No Services");
            }

            catch (ArgumentException)
            {
                throw;
            }

            List<GetUserServiceDataProfile> dataProfiles = new List<GetUserServiceDataProfile>();

            await result.ForEachAsync((service) =>
             {
                 dataProfiles.Add(new GetUserServiceDataProfile { DeviceId = service.DeviceId, ServiceName = service.ServiceName });
             });

            return dataProfiles;
        }
        #endregion

    }
}
