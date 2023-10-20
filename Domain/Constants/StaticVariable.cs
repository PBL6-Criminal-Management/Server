namespace Domain.Constants
{
    public class StaticVariable
    {
        // System
        public const string SHORT_NAME_APP = "CRL_MGT";

        // Permission
        public const string ACCESS = "Access";
        public const string ADD = "Add";
        public const string EDIT = "Edit";
        public const string DELETE = "Delete";

        //Messages
        public const string NOT_FOUND_MSG = "Không tìm thấy dữ liệu phù hợp!";
        public const string DELETE_USER = "Xóa người dùng thành công";
        public const string SYS_ERROR = "Lỗi hệ thông!!!";
        public const string NOT_FOUND_EMAIL = "Không tìm thấy email!";
        public const string NOT_FOUND_ROLE = "Not found match role!";
        public const string UNKNOWN_ERROR = "Data can't be added into database because unknown error!";

        public const string INVALID_PHONE_NUMBER = "Phone number is invalid";
        public const string INVALID_EMAIL = "Email is invalid";
        public const string INVALID_USER_NAME = "User name is invalid";
        public const string INVALID_PASSWORD = "Password is invalid";

        public const string PHONE_NUMBER_EXISTS_MSG = "Phone number already exists in the database.";
        public const string EMAIL_EXISTS_MSG = "Email already exists in the database.";
        public const string USERNAME_EXISTS_MSG = "Username already exists in the database.";
        public const string CMND_CCCD_EXISTS_MSG = "CMND or CCCD already exists in the database.";

        //Limit length
        public const string LIMIT_NAME = "The name should not exceed 100 characters.";
        public const string LIMIT_CMND_CCCD = "The CMND_CCCD should not exceed 15 characters.";
        public const string LIMIT_ADDRESS = "The address should not exceed 200 characters.";
        public const string LIMIT_PHONENUMBER = "The phone number should not exceed 15 characters.";
        public const string LIMIT_EMAIL = "The email should not exceed 100 characters.";
        public const string LIMIT_USERNAME = "The username should not exceed 50 characters.";
        public const string LIMIT_PASSWORD = "The password can only be between 8 and 100 characters.";
        public const string LIMIT_IMAGE = "The image should not exceed 500 characters.";
        public const string LIMIT_DESCRIPTION = "The description should not exceed 500 characters.";
        public const string LIMIT_NOTE = "The note should not exceed 500 characters.";
    }
}