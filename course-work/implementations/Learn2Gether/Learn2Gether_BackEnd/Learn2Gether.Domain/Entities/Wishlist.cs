namespace Learn2Gether.Domain.Entities
{
    public class Wishlist
    {
        public Guid StudentId { get; set; }
        public User Student { get; set; } = null!;
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;

    }
}
