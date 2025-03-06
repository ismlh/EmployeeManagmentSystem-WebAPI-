

namespace RepositoryPatternWithUOW.BL.Dtos
{
    public class RelativeDto
    {
        public required string Name { get; set; }
        public int Age { get; set; }

        public string? Job { get; set; }

        [Required(ErrorMessage ="Employee Id Is Required")]
        public int EmolyeeId { get; set; }

    }
}
