
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
        public const string NOT_VIEW_CRIMINAL_INFOR_PERMISSION = "Bạn không có quyền xem thông tin tội phạm này!";
        public const string NOT_EDIT_ACCOUNT_PERMISSION = "Bạn không có quyền chỉnh sửa thông tin tài khoản này!";
        public const string NOT_EDIT_ROLE_PERMISSION = "Bạn không có quyền chỉnh sửa chức vụ cho tài khoản này!";
        public const string NOT_EDIT_IS_ACTIVE_PERMISSION = "Bạn không có quyền chỉnh sửa trạng thái kích hoạt của tài khoản này!";
        public const string NOT_VIEW_ACCOUNT_INFOR_PERMISSION = "Bạn không có quyền xem thông tin tài khoản này!";

        //Messages
        public const string ACCOUNT_IS_NOT_CORRECT = "Tên người dùng hoặc mật khẩu không đúng.";
        public const string ACCOUNT_IS_NOT_EXIST = "Tài khoản không tồn tại hoặc đã bị xoá.";
        public const string ACCOUNT_IS_NOT_ACTIVE = "Tài khoản chưa được kích hoạt để đăng nhập vào hệ thống.";

        public const string INVALID_TOKEN = "Token không hợp lệ";
        public const string INVALID_REFRESH_TOKEN = "RefreshToken không hợp lệ hoặc đã hết hạn! Vui lòng đăng nhập lại!";

        public const string NOT_FOUND_MSG = "Không thể tìm thấy dữ liệu phù hợp!";
        public const string DELETE_USER = "Xóa người dùng thành công!";
        public const string DELETE_SUCCESS = "Xóa thành công!";
        public const string DELETE_CASE = "Xóa vụ án thành công";
        public const string DELETE_REPORT = "Xóa bản tố giác tội phạm thành công";
        public const string SYS_ERROR = "Lỗi hệ thống!";
        public const string NOT_FOUND_EMAIL = "Không tìm thấy email!";
        public const string NOT_FOUND_ROLE = "Không tìm thấy vai trò phù hợp!";
        public const string NOT_FOUND_INVESTIGATOR = "Không tìm thấy dữ liệu điều tra viên phù hợp!";
        public const string UNKNOWN_ERROR = "Không thể thêm dữ liệu vào cơ sở dữ liệu vì lỗi không xác định!";

        public const string INVALID_PHONE_NUMBER = "Số điện thoại không hợp lệ!";
        public const string INVALID_EMAIL = "Email không hợp lệ!";
        public const string INVALID_USER_NAME = "Tên người dùng không hợp lệ!";
        public const string INVALID_PASSWORD = "Mật khẩu không hợp lệ!";
        public const string NOT_MATCH_PASSWORD = "Mật khẩu xác nhận không khớp với mật khẩu mới!";
        public const string PHONE_NUMBER_EXISTS_MSG = "Số điện thoại đã tồn tại trong cơ sở dữ liệu!";
        public const string FACEBOOK_EXISTS_MSG = "Facebook đã tồn tại trong cơ sở dữ liệu";
        public const string PHONE_NUMBER_WITNESS_EXISTS_MSG = "Số điện thoại của nhân chứng đã tồn tại trong cơ sở dữ liệu!";
        public const string PHONE_NUMBER_VICTIM_EXISTS_MSG = "Số điện thoại của nạn nhân đã tồn tại trong cơ sở dữ liệu!";
        public const string EMAIL_EXISTS_MSG = "Email đã tồn tại trong cơ sở dữ liệu!";
        public const string USERNAME_EXISTS_MSG = "Tên người dùng đã tồn tại trong cơ sở dữ liệu!";
        public const string CITIZEN_ID_EXISTS_MSG = "CMND hoặc CCCD đã tồn tại trong cơ sở dữ liệu!";
        public const string CITIZEN_ID_REPEAT = "CMMD hoặc CCCD bị trùng";
        public const string CHANGE_ROLE_FAIL = "Thay đổi role không thành công";
        public const string NOT_FOUND_CRIMINAL = "Không tìm thấy tội phạm phù hợp!";

        public const string NOT_CORRECT_DATE_FORMAT = "Ngày nhập không đúng định dạng!";
        public const string INCORRECT_PASSWORD = "Mật khẩu cũ không đúng!";
        public const string PASSWORD_TOO_SHORT = "Mật khẩu mới quá ngắn, hãy nhập mật khẩu từ 8 ký tự trở lên!";
        public const string PASSWORD_REQUIRE_DIGIT = "Mật khẩu phải có ít nhất 1 chữ số!";
        public const string PASSWORD_REQUIRE_LOWER = "Mật khẩu phải có ít nhất 1 kí tự viết thường!";
        public const string PASSWORD_REQUIRE_NON_ALPHANUMERIC = "Mật khẩu không thể chứa chữ và số!";
        public const string PASSWORD_REQUIRE_UPPER = "Mật khẩu phải có ít nhất 1 kí tự viết hoa!";

        public const string CHANGE_PASSWORD_SUCCESSFULLY = "Thay đổi mật khẩu thành công!";
        public const string SEND_EMAIL_SUCCESSFULLY = "Đã gửi thông tin thay đổi mật khẩu qua email của bạn!";

        public const string RESET_PASSWORD_SUCCESSFULLY = "Thiết lập lại mật khẩu thành công!";

        public const string YEAR_MUST_NOT_BE_NEGATIVE = "Năm không thể là số âm!";
        public const string INVALID_MONTH = "Tháng chỉ có giá trị từ 1 - 12!";

        public const string USER_HAVE_NOT_ROLE = "Người dùng này chưa có chức vụ nên tự động bỏ qua cập nhật trường chức vụ và trường kích hoạt!";

        public const string USER_HAVE_NOT_EMAIL = "Người dùng này không có email để thông báo mật khẩu sau khi thiết lập lại!";

        public const string NUMBER_IMAGES_EACH_SECOND_MUST_BE_POSITIVE = "Số ảnh mỗi giây phải là 1 số dương!";

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
        //public const string AI_SERVER_BASE_URL = "https://face-recognition-z0vz.onrender.com";
        public const string AI_SERVER_BASE_URL = "https://face-recognition-sv2p.onrender.com";
        //public const string AI_SERVER_BASE_URL = "http://localhost:8000";
        public const string TRAINED_IMAGES_FOLDER_ID = "1FcPN4UNVUZHO7JL5MPSfCgIxmhmY9LC5";
        public const string TRAINED_IMAGES_FOLDER_NAME = "TrainedImages";
        public const int MAX_IMAGE_SIZE_FOR_FREE_AISERVER = 2300000;
        public const int DEFAULT_FRAMES_NUMBER_EACH_SECOND = 3;

        //Limit length
        public const string LIMIT_NAME = "Tên không được vượt quá 100 ký tự!";
        public const string LIMIT_CITIZEN_ID = "Chứng minh nhân dân không được vượt quá 12 ký tự!";
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
        public const string LIMIT_CHARACTERISTICS = "Tính cách không được vượt quá 500 ký tự!";
        public const string LIMIT_HOME_TOWN = "Quê quán không được vượt quá 200 ký tự!";
        public const string LIMIT_ETHNICITY = "Dân tộc không được vượt quá 50 ký tự!";
        public const string LIMIT_RELIGION = "Tôn giáo không được vượt quá 50 ký tự!";
        public const string LIMIT_NATIONALITY = "Quốc tịch không được vượt quá 50 ký tự!";
        public const string LIMIT_FATHER_NAME = "Tên bố của tội phạm không được vượt quá 100 ký tự!";
        public const string LIMIT_FATHER_CITIZEN_ID = "Chứng minh nhân dân của bố tội phạm không được vượt quá 12 ký tự!";
        public const string LIMIT_MOTHER_NAME = "Tên mẹ của tội phạm không được vượt quá 100 ký tự!";
        public const string LIMIT_MOTHER_CITIZEN_ID = "Chứng minh nhân dân của mẹ tội phạm không được vượt quá 12 ký tự!";
        public const string LIMIT_PERMANENT_RESIDENCE = "Địa chỉ thường trú không được vượt quá 200 ký tự!";
        public const string LIMIT_CURRENT_ACCOMMODATION = "Địa chỉ hiện tại không được vượt quá 200 ký tự!";
        public const string LIMIT_CURRENT_ACTIVITY = "Hoạt động hiện hành không được vượt quá 200 ký tự!";
        public const string LIMIT_WANTED_DECISION_NO = "Số ra quyết định không được vượt quá 50 ký tự!";
        public const string LIMIT_DECISION_MAKING_UNIT = "Đơn vị ra quyết định không được vượt quá 100 ký tự!";
        public const string LIMIT_ENTRY_AND_EXIT_INFORMATION = "Thông tin xuất nhập cảnh không được vượt quá 500 ký tự!";
        public const string LIMIT_FACEBOOK = "Facebook không được vượt quá 100 ký tự!";
        public const string LIMIT_ZALO = "Zalo không được vượt quá 100 ký tự!";
        public const string LIMIT_OTHER_SOCIAL_NETWORKS = "Mạng xã hội khác không được vượt quá 300 ký tự!";
        public const string LIMIT_GAME_ACCOUNT = "Tài khoản game không được vượt quá 100 ký tự!";
        public const string LIMIT_BANK_ACCOUNT = "Tài khoản ngân hàng không được vượt quá 30 ký tự!";
        public const string LIMIT_VEHICLES = "Phương tiện không được vượt quá 100 ký tự!";
        public const string LIMIT_DANGEROUS_LEVEL = "Mức độ nguy hiểm không được vượt quá 200 ký tự!";
        public const string LIMIT_OTHER_INFORMATION = "Thông tin khác không được vượt quá 500 ký tự!";
        public const string LIMIT_REASON = "Lí do không được vượt quá 500 ký tự";
        public const string LIMIT_MURDER_WEAPON = "Hung khí không được vượt quá 100 ký tự";
        public const string LIMIT_WEAPON = "Vũ khí không được vượt quá 100 ký tự";
        public const string LIMIT_CHARGE = "Tội danh không được vượt quá 100 ký tự";
        public const string LIMIT_CRIME_SCENE = "Địa điểm xảy ra vụ án không được vượt quá 200 ký tự";
        public const string LIMIT_REPORTER_NAME = "Họ và tên của người tố giác tội phạm không được vượt quá 100 ký tự";
        public const string LIMIT_REPORTER_EMAIL = "Email của người tố giác tội phạm không được vượt quá 100 ký tự";
        public const string LIMIT_REPORTER_PHONE = "Số điện thoại của người tố giác tội phạm không được vượt quá 15 ký tự";
        public const string LIMIT_REPORTER_ADDRESS = "Địa chỉ của người tố giác tội phạm không được vượt quá 200 ký tự";
        //Error message
        public const string ERROR_DELETE_IMAGE = "Xóa ảnh không thành công!!!";
        public const string INTERNAL_SERVER_ERROR = "Lỗi server";

        //Prefix format
        public const string CASE = "VA";
        public const string CRIMINAL = "TP";
        public const string REPORTING = "BC";
        public const string TITLE_CONTAINS_SPECIAL_CHARACTERS = "Nội dung chứa ký tự đặc biệt";
        public const string NAME_CONTAINS_VALID_CHARACTER = "Tên chỉ chứa ký tự chữ, khoảng trắng và dấu nháy đơn";
        public const string ANOTHER_NAME_CONTAINS_VALID_CHARACTER = "Tên khác chỉ chứa ký tự chữ, khoảng trắng và dấu nháy đơn";
        public const string CITIZEN_ID_VALID_CHARACTER = "CMMD hoặc CCCD chỉ chứa ký tự số";
        public const string USERNAME_VALID_CHARACTER = "Username chỉ chứa ký tự số, ký tự chữ và khoảng trắng";
        public const string ADDRESS_VALID_CHARACTER = "Địa chỉ không được chứa ký tự đặc biệt";
        public const string CHARGE_VALID_CHARACTER = "Tội danh chỉ chứa ký tự chữ, khoảng trắng và dấu phẩy";
        public const string EVIDENCE_NAME_VALID_CHARACTER = "Tên vật chứng chỉ chứa ký tự chữ, ký tự số và khoảng trắng";
        public const string WEAPON_NAME_VALID_CHARACTER = "Vũ khí chỉ chứa ký tự chữ, ký tự số, khoảng trắng và dấu phẩy";
        public const string WANTED_DECISION_NO_VALID_CHARACTER = "Số quyết định truy nã không chứa ký tự đặc biệt";
        public const string DECISION_MAKING_UNIT_VALID_CHARACTER = "Đơn vị ra quyết định không chứa ký tự đặc biệt";
        public const string PHONE_MODE_VALID_CHARACTER = "Model điện thoại chỉ chứa ký tự số, ký tự chữ và khoảng trắng";
        public const string CAREER_AND_WORKPLACE_VALID_CHARACTER = "Nơi làm việc và chỗ ở không được chứa ký tự đặc biệt";
        public const string CHARACTERISTICS_VALID_CHARACTER = "Tính cách chỉ chứa ký tự chữ, khoảng trắng và dấu phẩy";
        public const string HOME_TOWN_VALID_CHARACTER = "Quê quán không được chứa ký tự đặc biệt";
        public const string ETHNICITY_VALID_CHARACTER = "Dân tộc chỉ chứa ký tự chữ";
        public const string RELIGION_VALID_CHARACTER = "Tôn giáo chỉ chứa ký tự chữ";
        public const string NATIONALITY_VALID_CHARACTER = "Quốc tịch chỉ chứa ký tự chữ";
        public const string PERMANENT_RESIDENCE_VALID_CHARACTER = "Địa chỉ thường trú không được chứa ký tự đặc biệt";
        public const string CURRENT_ACCOMMODATION_VALID_CHARACTER = "Địa chỉ hiện tại không được chứa ký tự đặc biệt";
        public const string ENTRY_AND_EXIT_INFORMATION_VALID_CHARACTER = "Thông tin xuất nhập cảnh không được chứa ký tự đặc biệt";
        public const string FACEBOOK_VALID_CHARACTER = "Facebook không được chứa ký tự đặc biệt";
        public const string ZALO_VALID_CHARACTER = "Zalo chỉ chứa ký tự số";
        public const string BANK_ACCOUNT_VALID_CHARACTER = "Tài khoản ngân hàng chỉ chứa ký tự chữ, ký tự số và khoảng trắng";
        public const string DANGEROUS_LEVEL_VALID_CHARACTER = "Mức độ nguy hiểm không được chứa ký tự đặt biệt";
        public const string VEHICLES_VALID_CHARACTER = "Phương tiện không được chứa ký tự đặt biệt";



    }
}