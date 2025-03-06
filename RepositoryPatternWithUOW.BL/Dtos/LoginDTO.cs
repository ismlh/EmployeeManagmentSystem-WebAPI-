


namespace RepositoryPatternWithUOW.BL.Dtos
{
   public class LoginDTO
    {
        public required string UserName { get; set; }

        [DataType(dataType: DataType.Password, ErrorMessage = "Must Be Complex Password")]
        public string Password { get; set; }
    }
}
