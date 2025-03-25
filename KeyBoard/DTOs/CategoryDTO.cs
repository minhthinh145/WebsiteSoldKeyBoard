namespace KeyBoard.DTOs
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
