



namespace RepositoryPatternWithUOW.BL.Models
{
    public class ApplicationUser:IdentityUser
    {
        public required string FullName { get; set; }    
    }
}
