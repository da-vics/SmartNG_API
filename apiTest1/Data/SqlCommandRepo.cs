using apiTest1.DataProfiles;
using apiTest1.Helpers;
using apiTest1.Models;
using Microsoft.EntityFrameworkCore;
using System;
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
            userRegister.PassWordHash = _dataEncryptionHelper.Encrypt(userRegister.PassWordHash, userRegister.Email); /// password
            userRegister.ApiKeyId = _dataEncryptionHelper.Encrypt(userRegister.Email);  /// apikey
            #endregion

            await _commandDbContext.RegisterUser.AddAsync(userRegister);
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

            var result = await _commandDbContext.RegisterUser.FindAsync(confirmUserDetails.Email);
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

        public override async Task<string> CreateNewUserService(string apikey, string NewServiceName)
        {

            if (apikey == null || NewServiceName == null)
                return string.Empty;

            var checkKey = await _commandDbContext.RegisterUser.FirstOrDefaultAsync(c => c.ApiKeyId == apikey);


            /// create a new user service by appending custome new plus email
            var convertToDBName = $"{NewServiceName}_{checkKey.Email}";


            if (checkKey == null)
                return string.Empty;

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

            var newservice = new UserServicesModel { ApiKeyId = apikey, ServiceName = convertToDBName };
            await _commandDbContext.UserServices.AddAsync(newservice);


            _ = await this.SaveChanges();


            return NewServiceName;

        }

        public override async Task<bool> AddUserServiceData(UserDataProfile userData)
        {

            var checkdat = await _commandDbContext.UserServices.FindAsync(userData.ServiceName);

            if (checkdat == null)
                return false;

            var userServiceData = new UserServiceDataModel();

            #region setDataFields
            userServiceData.ServiceName = userData.ServiceName;
            userServiceData.ServiceData = userData.Userdata;
            userServiceData.DataInsertDat = DateTime.Now;
            #endregion

            await _commandDbContext.UserData.AddAsync(userServiceData);
            _ = await this.SaveChanges();

            return true;


        }

    }
}
