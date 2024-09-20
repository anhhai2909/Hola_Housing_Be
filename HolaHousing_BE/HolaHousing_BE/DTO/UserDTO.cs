namespace HolaHousing_BE.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string? Fullname { get; set; }
        public string? PhoneNum { get; set; }
        public string? Email { get; set; }
        public byte? Status { get; set; }
        public int? RoleId { get; set; }
    }
}
