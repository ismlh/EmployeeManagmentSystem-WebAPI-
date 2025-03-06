

namespace RepositoryPatternWithUOW.BL.EntittesConfigurations
{
   public class EmployeeEntityConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
           builder
        .HasOne(e => e.Manager)
        .WithMany(e => e.Subordinates)
        .HasForeignKey(e => e.ManagerId)
        .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
