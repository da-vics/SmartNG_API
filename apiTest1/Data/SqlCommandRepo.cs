using apiTest1.DataProfiles;
using apiTest1.Helpers;
using apiTest1.Models;
using Microsoft.EntityFrameworkCore;
using System;
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
        public override async Task CreateUser(UserRegisterModel userRegister)
        {
            if (userRegister == null)
                throw new ArgumentNullException(nameof(userRegister));

            #region data-Encryrtion

            userRegister.PassWordHash = _dataEncryptionHelper.Encrypt(userRegister.PassWordHash, "smarterliving", userRegister.Email); /// password
            userRegister.ApiKeyId = _dataEncryptionHelper.Encrypt(userRegister.Email, "smarterliving");  /// apikey
            #endregion

            await _commandDbContext.RegisterUser.AddAsync(userRegister);
            await SaveChanges();
        }

        #endregion


        public override Task<UserServiceDataModel> GetUserData()
        {
            throw new NotImplementedException();
        }


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

        public override async Task<string> VerifyUser(UserLoginProfile confirmUserDetails)
        {

            if (confirmUserDetails == null)
                return string.Empty;

            var result = await _commandDbContext.RegisterUser.FirstOrDefaultAsync(user => user.Email == confirmUserDetails.Email);
            if (result == null)
                return string.Empty;

            if (_dataEncryptionHelper.VerifyMd5Hash(confirmUserDetails.Password, result.PassWordHash, result.Email))
            {
                return result.ApiKeyId;
            }

            else
            {
                return string.Empty;
            }
        }

        public override async Task<bool> CreateNewUserService(UserServicesProfile userServices)
        {

            if (string.IsNullOrEmpty(userServices.ApiKey) || string.IsNullOrEmpty(userServices.ServiceName))
                return false;

            var checkKey = await _commandDbContext.RegisterUser.FirstOrDefaultAsync(c => c.ApiKeyId == userServices.ApiKey);

            try
            {

                var deviceCheck = await _commandDbContext.SetupModels.FindAsync(userServices.DeviceId);
                if (deviceCheck == null)
                    throw new ArgumentException("Device not Found!");
            }

            catch (ArgumentException)
            {
                throw;
            }

            //var check2 = from user in _commandDbContext.RegisterUser
            //                           join service in _commandDbContext.UserServices
            //                           on user.ApiKeyId equals apikey
            //                           where service.ServiceName == NewServiceName
            //                           select service;


            /// create a new user service by appending custome new plus email
            var convertToDBName = $"{userServices.ServiceName}_{checkKey.Email}";

            if (checkKey == null)
                return false;

            try
            {
                var checkService = await _commandDbContext.UserServices.FirstOrDefaultAsync(c => c.ServiceName == convertToDBName);
                if (checkService != null)
                    throw new ArgumentException("Service already exist");
            }

            catch (ArgumentException)
            {
                throw;
            }

            var newservice = new UserServicesModel { ApiKeyId = userServices.ApiKey, ServiceName = convertToDBName, DeviceId = userServices.DeviceId };
            await _commandDbContext.UserServices.AddAsync(newservice);


            _ = await this.SaveChanges();


            return true;

        }

        public override async Task<bool> AddUserServiceData(UserDataProfile userData)
        {
            var confirmID = await _commandDbContext.UserServices.FirstOrDefaultAsync(dat => dat.DeviceId == userData.DeviceId && dat.ApiKeyId == userData.apikey);


            if (confirmID == null)
                return false;

            var userServiceData = new UserServiceDataModel();

            #region setDataFields
            userServiceData.ApiKeyId = userData.apikey;
            userServiceData.DeviceId = userData.DeviceId;
            userServiceData.ServiceData = userData.Userdata;
            userServiceData.DataInsertDat = DateTime.Now;
            #endregion

            await _commandDbContext.UserData.AddAsync(userServiceData);
            _ = await this.SaveChanges();

            return true;
        }

        public override async Task<string> CreateFieldDevice(FieldRegisterProfile fieldRegister)
        {
            if (fieldRegister == null)
                return string.Empty;

            fieldRegister.MasterKey = _dataEncryptionHelper.Encrypt(fieldRegister.MasterKey);

            var check = await _commandDbContext.FieldMasterKey.FirstOrDefaultAsync(keys => keys.accessKey == fieldRegister.MasterKey);

            if (check != null)
            {
                var newfieldDevice = new DeviceSetupModel();
                var result = await _commandDbContext.SetupModels.AddAsync(newfieldDevice);
                _ = await this.SaveChanges();

                return result.Entity.Id.ToString();
            }

            return string.Empty;
        }

        public async override Task<string> GetFieldUserKey(FiledDeivceProfile deviceConfig)
        {

            var check = await _commandDbContext.UserServices.FirstOrDefaultAsync(c => c.DeviceId == deviceConfig.DeviceId);

            if (check != null)
                return check.ApiKeyId;

            else return string.Empty;

        }
    }
}
