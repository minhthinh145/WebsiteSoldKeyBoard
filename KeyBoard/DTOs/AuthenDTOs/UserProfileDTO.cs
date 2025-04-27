namespace KeyBoard.DTOs.AuthenDTOs
{
    public class UserProfileDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthDateFormatted => BirthDate?.ToString("dd/MM/yyyy");
    }
}
