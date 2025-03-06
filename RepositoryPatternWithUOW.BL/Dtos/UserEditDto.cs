

namespace RepositoryPatternWithUOW.BL.Dtos
{
   public  class UserEditDto
    {
        public required string FullName { get; set; }

        [RegularExpression("^01(1|2|0|5)[0-9]{8}", ErrorMessage = "Must Be Correct Phone Number")]
        public string? PhoneNumber { get; set; }

        [DataType(dataType: DataType.EmailAddress, ErrorMessage = "Must Be Valid Email")]
        public required string Email { get; set; }
        [DataType(dataType: DataType.Password, ErrorMessage = "Must Be Complex Password")]
        public string Password { get; set; }
    }
}
