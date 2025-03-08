using System.ComponentModel;

namespace Tabibak.Api.BLL.Constants
{


    #region DbSchemas

    public class DXConstants
    {
        public static class Fields
        {
            public const string Astr = "*";
        }
        #region DateFilters
        public static class DateFilters
        {
            public const string GeneralDateFormat = "yyyy-MM-dd HH:mm:ss";
            public const string OutputFilterFormat = "yyyy-MM-dd";

            public const string FieldName = "%FieldName%";
            public const string ValueName = "%Value%";
            public const string ValueStartName = "%SValue%";
            public const string ValueEndName = "%EValue%";
            public static readonly string[] Separators = { "and", "or" };


            public static double GetMultiplyFactor(string inputString) => inputString switch
            {
                "SpecificDate" => 0,
                "Minutes" => 0.016667,
                "Hour" => 1.000000,
                "Day" => 24.000000,
                "Week" => 168.000000,
                "Month" => 720.000000,
                "Year" => 8760.000000,
                _ => 0
            };
        }

        #endregion
        public static class DbSchemas
        {

            public const string DEXEF = "dbo";
            public const string POS = "POS";
            public const string SchemaAccounting = "Accounting";
            public const string StaticData = "StaticData";
            public const string Security = "Security";
            public const string Lang = "Lang";
            public static string ReferenceTableName = "[{0}].[{1}]";
            public static string OrganizationContainer = "organization1";
        }

        #endregion DbSchemas

    }
    #region BLL Responses MessageCodes

    public enum MessageCodes

    {
        [Description("Success")]
        Success = 1000,

        [Description("Internal Server Error")]
        Failed = 2000,

        [Description("Failed To Fetch Data")]
        FailedToFetchData = 2001,

        [Description("There is NoPermission to Perform this Action")]
        UnAuthorizedAccess = 4000,

        //Exception
        [Description("Some Error Occurred")]
        Exception = 5000,

        //InputValidation
        [Description("Failed : Input Validation Error")]
        InputValidationError = 6000,

        [Description("Failed : {0} Is Required")]
        Required = 6001,

        [Description("Failed: {0}  Must Be Greater Than Zero")]
        GreaterThanZero = 6002,

        [Description("Length Validation Error")]
        LengthValidationError = 6003,

        [Description("Failed : {2} Must Be Between {0} And {1}")]
        InbetweenValue = 6004,

        [Description("Failed : {1} Must Be GreaterThan {0}")]
        InvalidMinLength = 6005,

        [Description("Failed : {1} Must Be LessThan {0}")]
        InvalidMaxLength = 6006,

        [Description("Failed : Invalid Email")]
        InvalidEmail = 6007,

        [Description("Failed :Invalid Items Count")]
        InvalidItemsCount = 6008,

        [Description("Failed :Invalid Logo")]
        InvalidLogo = 6009,

        [Description("Failed :Invalid Json")]
        InvalidJson = 6010,

        [Description("Failed :Invalid Json Empty Value")]
        InvalidJsonEmptyValue = 6011,

        [Description("Failed :Failed To Deserialize")]
        FailedToDeserialize = 6012,

        [Description("Failed :Missing Default Value")]
        MissingDefaultValue = 6013,

        [Description("Failed :Missing Arabic Value")]
        MissingArabicValue = 6014,

        [Description("Failed :Password should contain at least 1 digit")]
        MissingPasswordDigits = 6015,

        [Description("Failed :Password should contain at least one alphabetic character")]
        MissingPasswordAlphabetic = 6016,

        [Description("Failed :Password should contain at least one special characters Like { ., $, ~ ,&}")]
        MissingPasswordSpecialCharacters = 6017,

        [Description("Failed :Invalid Https Url")]
        InvalidHttpsUrl = 6018,

        [Description("Failed :Invalid File Type")]
        InvalidFileType = 6019,

        [Description("Failed :Invalid File Content Type")]
        InvalidFileContentType = 6020,

        [Description("Failed :Invalid File Size,, Must be less than 2 MB")]
        InvalidFileSize = 6021,

        [Description("Failed :Invalid Rate, Must be within 1 to 5")]
        InvalidRate = 6022,

        [Description("Failed :You Must select one at least")]
        InvalidItemsSelect = 6023,

        //Business Validation
        [Description("Failed : Business Validation Error")]
        BusinessValidationError = 7000,

        [Description("Failed : {0} Already Exists")]
        AlreadyExists = 7001,

        [Description("Failed : {0} Not Found")]
        NotFound = 7002,

        [Description("Failed : {0} Is DefaultForOther")]
        DefaultForOther = 7003,

        [Description("There're related data to this item")]
        RelatedDataExist = 7004,

        [Description("File type Is Not supported")]
        FileTypeNotSupported = 7005,

        [Description("Failed : Name Already Exists")]
        NameAlreadyExists = 7006,

        [Description("Failed : UserName Already Exists")]
        UserNameAlreadyExists = 7007,

        [Description("Failed : Email Already Exists")]
        EmailAlreadyExists = 7008,

        [Description("Failed : Can't Delete Admin User ")]
        CanNotDeleteAdminUser = 7009,

        [Description("Failed : {0}  Already Exists")]
        AlreadyExistsEn = 7010,

        [Description("Failed : {0}  Already Exists")]
        AlreadyExistsAr = 7011,

        [Description("Failed : Invalid UserName or Password")]
        InvalidUserNameOrPassword = 7012,

        [Description("Failed : Invalid Password")]
        InvalidPassword = 7013,

        [Description("Failed :Employee Is InActive .. Activate then try again")]
        EmployeeIsInActive = 7014,

        [Description("Failed : {0} Is HiglightedVersion")]
        HiglightedVersion = 7015,

        [Description("Failed :{0} Is InActive .. Activate then try again")]
        InActiveEntity = 7016,

        [Description("Failed :{0} Is Default.. Not allowed delete")]
        DefaultEntityDelete = 7017,

        [Description("Failed :You Already have 3 active versions,Please deactive one")]
        MaxActiveVersionsCountLimitExceeded = 7018,

        [Description("Failed :DownloadUrl or ReleaseNumber is already exisit")]
        VersionReleaseExisit = 7019,

        [Description("Failed :Mobile already verified")]
        MobileAlreadyVerified = 7020,

        [Description("Failed :Email already verified")]
        EmailAlreadyVerified = 7021,

        [Description("Failed :Send count exceeded")]
        SendCountExceeded = 7022,

        [Description("Failed :Phone code expired")]
        PhoneCodeExpired = 7023,

        [Description("Failed :Verification link expired")]
        VerificationLinkExpired = 7024,

        [Description("Failed :Invalid verification link")]
        InvalidVerificationLink = 7025,

        [Description("Failed :Invalid login credentials")]
        InvalidLoginCredentials = 7026,

        [Description("Failed :Invalid token")]
        InvalidToken = 7027,

        [Description("Failed :Token not expired yet")]
        TokenNotExpiredYet = 7028,

        [Description("Failed :Token and refresh token don't match")]
        TokensDoNotMatch = 7029,

        [Description("Failed :Email not verified")]
        EmailNotVerified = 7030,

        [Description("Failed :Password already defined for you before")]
        NewPasswordAlreadyDefined = 7031,

        [Description("Failed :Email is already primary")]
        EmailIsAlreadyPrimary = 7032,

        [Description("Failed :{0} Is InActive .. You must activate one at least")]
        ActiveEntityCount = 7033,

        [Description("Failed :{0} Is Default .. You must select another one to be default first")]
        DefaultEntityCount = 7034,

        [Description("Failed : Invalid password or link")]
        InvalidPasswordOrLink = 7035,

        [Description("Failed :{0} has unpaid invoices")]
        HasUnPaidInvoicesCount = 7036,

        [Description("Failed :There is an Unpaid {0} ")]
        HasUnPaidInvoices = 7037,

        [Description("Failed :Please, correct device serial")]
        OldEqualNewDevice = 7038,

        [Description("Failed : licenses limit has been exceeded")]
        ExceededLicensesLimit = 7039,

        [Description("Failed :You have exceeded the validity period")]
        ExceededPeriod = 7040,

        [Description("Failed : {0} status is Invalid")]
        InvalidInvoiceStatus = 7041,

        [Description("Failed : {0} Not Available in your Country")]
        NotAvailableInYourCountry = 7042,

        [Description("Failed : {0} Is Not Generated Yet")]
        IsNotGenerated = 7043,

        [Description("Failed : New Password not identical to Confirm Password")]
        MismatchNewConfirmPassword = 7044,

        [Description("Failed : license is not expired yet")]
        LicenseNotExpiredYet = 7045,

        [Description("Failed : There's no paid invoice.")]
        NoPaidInvoice = 7046,

        [Description("Failed : Must Purchase Version First.")]
        MustPurchaseVersionFirst = 7047,

        [Description("Failed : has more than one invoice")]
        HasMoreThanOneInvoice = 7048,

        [Description("Failed : Sorry the selected role is used.")]
        RoleAssign = 7049,

        [Description("Failed : request is already refunded.")]
        RequestAlreadyRefunded = 7050,

        [Description("Failed : request has been rejected.")]
        RequestAlreadyRejected = 7051,

        [Description("Failed : EndDate must be greater than StartDate.")]
        DatePeriod = 7052,

        [Description("Failed : Payment Failed.")]
        PaymentFailed = 7053,

        [Description("Failed : There're related addons have not been refunded yet.")]
        RelatedAddonNotRefunded = 7054,

        [Description("Failed : Invalid payment method.")]
        InvalidPaymentMethod = 7055,

        [Description("Failed : Can't Deactivate Featured Tag.")]
        DeactivateFeaturedTag = 7056,

        [Description("Failed :{0} Not Authroize show this page .")]
        PageNotAllowed = 7057,

        [Description("Failed :Page or Action not selected .")]
        PageNotSelected = 7058,

        [Description("Failed :You can not select partDeparment to it self .")]
        DepartmentSelf = 7059,

        [Description("Failed : Can't Deactivate Default {0}.")]
        DefaultEntityDeactivate = 7060,

        [Description("Failed :FromOrder is identical to ToOrder.")]
        InvalidOrderByInput = 7061,
        [Description("Failed :One or moreimported are not in completed state, Imported records can only bedeleted from completed jobs.")]
        FailedImport = 7062,
        [Description("Failed :There is details transactions.")]
        RelatedTreeData = 7063,
        [Description("Failed :old password not match.")]
        OldPasswordConfirm = 7064,
        [Description("Failed : The system could not log you on. Please Contact with your Administrator to Assign Default Accounts to your profile. .")]
        UserCompletedData = 7065,
        [Description("Failed : Cannot load dealing information from database. .")]
        DealingInformation = 7066,
        [Description("Failed : Cannot load account ids from database")]
        AccountLoadding = 7067,
        [Description("Failed : Item Name : {0} And ItemID :{1} will incur a loss, check price first.")]
        LostMoney = 7068,
        [Description("Error in Item Name : {0} And ItemID :{1} Available qty is less than required qty Available qty = {2}")]
        QuantityAvailable = 7070,
        [Description("Duplicate rows values in InvoiceTableEntryDto where (ItemId = {0}, Unit = {1} Qty = {2})")]
        DuplicatedValue = 7071,
        [Description("Please Enter a Customer Id (CustID)")]
        EnterCustomer = 7072,
        [Description("Error in Item Name : {0} And ItemID :{1} available quota qty to be used =  {2} {3} until now.")]
        MultipleErrorField = 7073,
        [Description("Failed : Check count permissions and try again ! . . .")]
        PermissionCount = 7074,

        [Description("Failed : Check count QuickPay and try again ! . . .")]
        QuickPayCount = 7075,

        [Description("Failed : Active musn't be more than one  ! . . .")]
        MoreActive = 7076,
        [Description("Failed : Appointment cannot be started")]
        InvalidOperation = 7077,
        [Description("Failed : You can only review doctors after completing an appointment")]
        ReviewPermission = 7078,



        #region
        //8000 -8099  for companies
        [Description("Failed :Company Number Or Name is Existed .")]
        CompanyNumberOrNameExisted = 8000,
        [Description("Failed :No Company Found With the submitted CategoryId .")]
        CompanyNotExisted = 8001,
        [Description("Failed :No Companies Found.")]
        NoCompaniesFound = 8002,
        [Description("Failed :You can not update the default currency of the company.")]
        CannotUpdateDefaultCurrency = 8003,
        [Description("Failed :You can not Create default currency of the company.")]
        CannotCreateDefaultCurrency = 8004,


        #endregion

        #region
        //8100 -8199  for Countriers

        [Description("Failed :No Country Currency Found With the submitted CategoryId .")]
        CountryCurrencyNotExisted = 8100,
        [Description("Failed :No Country Currency Found.")]
        NoCountriesFound = 8102,
        [Description("Failed :Country with the same name existed .")]
        CoountryExisted = 8101,
        [Description("Failed :Currency with the same name existed .")]
        CurrencyExisted = 8103,
        #endregion


        #region
        //8200 -8299  for FileManger

        [Description("Failed :No Not Supported File Type .")]
        NotSupportedFileType = 8200,
        [Description("Failed :No Files Found.")]
        NoFilesFound = 8201,
        [Description("Failed :No File Found With the submitted CategoryId .")]
        FileNotExisted = 8202,

        #endregion
        #region
        [Description("Failed :the submitted {0} value has no refrence on table  {1} column {2}.")]
        NoRefrenceExistedSqlException = 8300,
        #endregion
        #region
        //8000 -8099  for companies
        [Description("Failed :Uplaoded file is not matching our predefinded templates please download template!")]
        TemplateNotMatched = 8400,
        [Description("Failed :Uplaoded file is Empty")]
        NoDataInUploadedFile = 8401,
        #endregion



        [Description("Failed :No Data Found.")]
        NoDataFound = 9000,

        [Description("Failed :Theme Is Used For System.")]
        ThemeIsUsed = 9001,

        [Description("Failed :Can't Update System Defined Item.")]
        SystemDefinedUpdate = 9002,

        [Description("Failed :Can't Delete System Defined Item.")]
        SystemDefinedDelete = 9003,

        [Description("Failed: You reached max series, please call system admin.")]
        LimitExceeded = 9004,
        [Description("Failed :No series available, please call system admin.")]
        NoSeriesAvialable = 9005,


        [Description("Failed :You can't update IsAdmin to be Unchecked ")]
        CompanyMustContainOneIsAdminBranch = 9006,

        [Description("Failed :You can't Delete IsAdmin Branch ")]
        DeleteIsAdminBranch = 9007,

        [Description("Failed :You can't DeActivate IsAdmin Branch ")]
        DeactivateAdminBranch = 9008,

        [Description("Failed :LastNumber Must Be >= NextNumber")]
        LastNumberMustBGreaterThanNextNumber = 9009,

        [Description("Failed :link has been expired.")]
        LinkExpired = 9010,
        [Description("Failed :Link is invalid.")]
        InvalidLink = 9011,
        [Description("Failed :Please activate plugin.")]
        IsActivePlugin = 9012,
    }

    #endregion BLL Responses MessageCodes


}