
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
        public const string DELETE_SUCCESS = "Xóa thành công!";
        public const string DELETE_CASE = "Xóa vụ án thành công";
        public const string SYS_ERROR = "Lỗi hệ thống!";
        public const string NOT_FOUND_EMAIL = "Không tìm thấy email!";
        public const string NOT_FOUND_ROLE = "Không tìm thấy vai trò phù hợp!";
        public const string NOT_FOUND_INVESTIGATOR = "Không tìm thấy dữ liệu điều tra viên phù hợp!";
        public const string UNKNOWN_ERROR = "Không thể thêm dữ liệu vào cơ sở dữ liệu vì lỗi không xác định!";

        public const string INVALID_PHONE_NUMBER = "Số điện thoại không hợp lệ!";
        public const string INVALID_EMAIL = "Email không hợp lệ!";
        public const string INVALID_USER_NAME = "Tên người dùng không hợp lệ!";
        public const string INVALID_PASSWORD = "Mật khẩu không hợp lệ!";

        public const string PHONE_NUMBER_EXISTS_MSG = "Số điện thoại đã tồn tại trong cơ sở dữ liệu!";
        public const string EMAIL_EXISTS_MSG = "Email đã tồn tại trong cơ sở dữ liệu!";
        public const string USERNAME_EXISTS_MSG = "Tên người dùng đã tồn tại trong cơ sở dữ liệu!";
        public const string CITIZEN_ID_EXISTS_MSG = "CMND hoặc CCCD đã tồn tại trong cơ sở dữ liệu!";
        public const string CHANGE_ROLE_FAIL = "Thay đổi role không thành công";
        public const string NOT_FOUND_CRIMINAL = "Không tìm thấy tội phạm phù hợp!";

        //File message
        public const string FILE_IS_NOT_IMAGE = "Tệp cung cấp không phải là ảnh hoặc có phần đuôi mở rộng không hợp lệ!";
        public const string FILE_IS_NOT_VIDEO = "Tệp cung cấp không phải là video hoặc có phần đuôi mở rộng không hợp lệ!";
        public const string FILE_TYPE_IS_INVALID = "Kiểu tệp không hợp lệ!";
        public const string IMAGE_LENGTH_IS_TOO_BIG = "Ảnh vượt quá kích thước tối đa cho phép!";
        public const string VIDEO_LENGTH_IS_TOO_BIG = "Video vượt quá kích thước tối đa cho phép!";

        //AI Model
        public const string UNKNOWN = "Không nhận diện ra";
        public const string AI_MODEL_HAS_NOT_TRAINED_YET = "Mô hình AI chưa được huấn luyện!";
        public const string NOT_FOUND_IMAGE_TO_TRAIN = "Không tìm thấy ảnh nào để huấn luyện!";
        public const string AI_MODEL_HAS_TRAINED_SUCCESSFULLY = "Mô hình AI được huấn luyện thành công!";

        //Limit length
        public const string LIMIT_NAME = "Tên không được vượt quá 100 ký tự!";
        public const string LIMIT_CITIZEN_ID = "CitizenID không được vượt quá 15 ký tự!";
        public const string LIMIT_ADDRESS = "Địa chỉ không được vượt quá 200 ký tự!";
        public const string LIMIT_PHONENUMBER = "Số điện thoại không được vượt quá 15 ký tự!";
        public const string LIMIT_EMAIL = "Email không được vượt quá 100 ký tự!";
        public const string LIMIT_USERNAME = "Tên người dùng không được vượt quá 50 ký tự!";
        public const string LIMIT_PASSWORD = "Mật khẩu chỉ có thể có từ 8 đến 100 ký tự!";
        public const string LIMIT_IMAGE = "Đường dẫn ảnh không được vượt quá 500 ký tự!";
        public const string LIMIT_DESCRIPTION = "Mô tả không được vượt quá 500 ký tự!";
        public const string LIMIT_NOTE = "Nội dung không được vượt quá 500 ký tự!";
        public const string LIMIT_ANOTHER_NAME = "Tên khác không được vượt quá 100 ký tự!";
        public const string LIMIT_PHONE_MODEL = "Model điện thoại không được vượt quá 100 ký tự!";
        public const string LIMIT_CAREER_AND_WORKPLACE = "Nghề nghiệp và nơi làm việc không được vượt quá 300 ký tự!";
        public const string LIMIT_CHRACTERISTICS = "Tính cách không được vượt quá 500 ký tự!";
        public const string LIMIT_HOME_TOWN = "Quê quán không được vượt quá 200 ký tự!";
        public const string LIMIT_ETHNICITY = "Dân tộc không được vượt quá 50 ký tự!";
        public const string LIMIT_RELIGION = "Tôn giáo không được vượt quá 50 ký tự!";
        public const string LIMIT_NATIONALITY = "Quốc tịch không được vượt quá 50 ký tự!";
        public const string LIMIT_FATHER_NAME = "Tên bố của tội phạm không được vượt quá 100 ký tự!";
        public const string LIMIT_FATHER_CITIZEN_ID = "Chứng minh nhân dân của bố tội phạm không được vượt quá 12 ký tự!";
        public const string LIMIT_MOTHER_NAME = "Tên mẹ của tội phạm không được vượt quá 100 ký tự!";
        public const string LIMIT_MOTHER_CITIZEN_ID = "Chứng minh nhân dân của mẹ tội phạm không được vượt quá 12 ký tự!";
        public const string LIMIT_PERMANENT_RESIDENCE = "Địa chỉ thường trú không được vượt quá 200 ký tự!";
        public const string LIMIT_CURRENT_ACCOMMODATION = "Địa chỉ hiện tại không được vượt quá 200 ký tự";
        public const string LIMIT_ENTRY_AND_EXITINFORMATION = "Thông tin xuất nhập cảnh không được vượt quá 500 ký tự!";
        public const string LIMIT_FACEBOOK = "Facebook không được vượt quá 100 ký tự!";
        public const string LIMIT_ZALO = "Zalo không được vượt quá 100 ký tự!";
        public const string LIMIT_OTHER_SOCIAL_NETWORKS = "Mạng xã hội khác không được vượt quá 300 ký tự!";
        public const string LIMIT_GAME_ACCOUNT = "Tài khoản game không được vượt quá 100 ký tự!";
        public const string LIMIT_BANK_ACCOUNT = "Tài khoản ngân hàng không được vượt quá 30 ký tự!";
        public const string LIMIT_VEHICLES = "Phương tiện không được vượt quá 100 ký tự!";
        public const string LIMIT_DANGEROUS_LEVEL = "Mức độ nguy hiểm không được vượt quá 200 ký tự!";
        public const string LIMIT_OTHER_INFORMATION = "Thông tin khác không được vượt quá 500 ký tự!";
        public const string LIMIT_REASON = "Lí do không được vượt quá 600 ký tự";
        public const string LIMIT_MURDER_WEAPON = "Hung khí không được vượt quá 100 ký tự";
        public const string LIMIT_CHARGE = "Tội danh không được vượt quá 100 ký tự";
        public const string LIMIT_CRIME_SCENE = "Địa điểm xảy ra vụ án không được vượt quá 200 ký tự";

        //Error message
        public const string ERROR_DELETE_IMAGE = "Xóa ảnh không thành công!!!";
        public const string INTERNAL_SERVER_ERROR = "Lỗi server";

        //public const string AI_SERVER_BASE_URL = "https://face-recognition-z0vz.onrender.com";
        public const string AI_SERVER_BASE_URL = "http://localhost:8000";
    }
}