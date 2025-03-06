

namespace RepositoryPatternWithUOW.BL.Models
{
    public class Relative
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int Age { get; set; }

        public string? Job { get; set; }

        [ForeignKey("Employee")]
        public int EmolyeeId { get; set; }

       public virtual Employee Employee { get; set; }
    }
}
