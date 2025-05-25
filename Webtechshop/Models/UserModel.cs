using System.ComponentModel.DataAnnotations;

namespace Webtechshop.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập tên đăng nhập")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Yêu cầu nhập Email"),EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password),Required(ErrorMessage ="Vui lòng nhập mật khẩu")]
        public string Password { get; set; }
    }
}
