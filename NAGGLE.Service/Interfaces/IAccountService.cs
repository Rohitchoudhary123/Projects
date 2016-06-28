using NAGGLE.Common.ServiceResponse;
using NAGGLE.Common.Utility;
using NAGGLE.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAGGLE.Service.Interfaces
{
    public interface IAccountService
    {


        Task<List<CountryModel>> GetAllCountries();
        Task<List<StateModel>> GetStatesByCountryId(int countryId);
        List<CategoryModel> GetAllParentCategories();
        List<AccountTypeModel> GetAllAccountTypes();
        
        


        #region Account
        List<AccountModel> GetAccountList();
        AccountModel GetAccountById(int accountId);        
        ServiceResponse SaveAccountDetail(AccountModel accountModel);
        ServiceResponse UpdateAccountDetail(AccountModel account);
        bool DeleteAccountById(int accountId = 0);
        List<AccountModel> AccountFilter(FilterParameter parameters);
        #endregion


        #region Contact

        UserModel GetContactById(int Id);
        List<UserModel> GetContactList(FilterParameter searchParam = null);
        ServiceResponse AddEditContact(UserModel userModel, int accountId);
        bool DeleteContactByUserId(int userId = 0);


        #endregion








        List<AutoCompleteModel> SearchCategories(string searchText);

        List<CategoryModel> GetChildCategory(int CategoryId);
    }
}
