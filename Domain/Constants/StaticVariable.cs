namespace Domain.Constants
{
    public class StaticVariable
    {
        // System
        public const string SHORT_NAME_APP = "CRL_MGT";

        // Permission
        public const string ACCESS = "Truy cập";
        public const string ADD = "Thêm";
        public const string EDIT = "Sửa";
        public const string DELETE = "Xóa";

        //Messages
        public const string NOT_FOUND_MSG = "Không thể tìm thấy dữ liệu phù hợp!";
        public const string DELETE_USER = "Xóa người dùng thành công!";
        public const string SYS_ERROR = "Lỗi hệ thống!";
        public const string NOT_FOUND_EMAIL = "Không tìm thấy email!";
        public const string NOT_FOUND_ROLE = "Không tìm thấy vai trò phù hợp!";
        public const string UNKNOWN_ERROR = "Không thể thêm dữ liệu vào cơ sở dữ liệu vì lỗi không xác định!";

        public const string INVALID_PHONE_NUMBER = "Số điện thoại không hợp lệ!";
        public const string INVALID_EMAIL = "Email không hợp lệ!";
        public const string INVALID_USER_NAME = "Tên người dùng không hợp lệ!";
        public const string INVALID_PASSWORD = "Mật khẩu không hợp lệ!";

        public const string PHONE_NUMBER_EXISTS_MSG = "Số điện thoại đã tồn tại trong cơ sở dữ liệu!";
        public const string EMAIL_EXISTS_MSG = "Email đã tồn tại trong cơ sở dữ liệu!";
        public const string USERNAME_EXISTS_MSG = "Tên người dùng đã tồn tại trong cơ sở dữ liệu!";
        public const string CMND_CCCD_EXISTS_MSG = "CMND hoặc CCCD đã tồn tại trong cơ sở dữ liệu!";
        public const string CHANGE_ROLE_FAIL = "Thay đổi role không thành công";

        //Limit length
        public const string LIMIT_NAME = "Tên không được vượt quá 100 ký tự!";
        public const string LIMIT_CMND_CCCD = "CMND_CCCD không được vượt quá 15 ký tự!";
        public const string LIMIT_ADDRESS = "Địa chỉ không được vượt quá 200 ký tự!";
        public const string LIMIT_PHONENUMBER = "Số điện thoại không được vượt quá 15 ký tự!";
        public const string LIMIT_EMAIL = "Email không được vượt quá 100 ký tự!";
        public const string LIMIT_USERNAME = "Tên người dùng không được vượt quá 50 ký tự!";
        public const string LIMIT_PASSWORD = "Mật khẩu chỉ có thể có từ 8 đến 100 ký tự!";
        public const string LIMIT_IMAGE = "Đường dẫn ảnh không được vượt quá 500 ký tự!";
        public const string LIMIT_DESCRIPTION = "Mô tả không được vượt quá 500 ký tự!";
        public const string LIMIT_NOTE = "Nội dung không được vượt quá 500 ký tự!";
    }
}