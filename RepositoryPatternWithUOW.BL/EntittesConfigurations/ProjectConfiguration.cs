

namespace RepositoryPatternWithUOW.BL.EntittesConfigurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
           builder.
                HasMany(p=>p.Employees).
                WithMany(e=>e.Projects).
                UsingEntity<ProjectEmployees>(
                    j => j.HasOne(pe => pe.Employee).
                    WithMany(e => e.ProjectEmployees).
                    HasForeignKey(pe => pe.EmployeeId),
                    j => j.HasOne(pe => pe.Project).
                    WithMany(p => p.ProjectEmployees).
                    HasForeignKey(pe => pe.ProjectId),
                    j => j.HasKey(pe => new { pe.EmployeeId, pe.ProjectId })
                );
            //builder .HasOne(p=>p.Employee).WithMany(p=> p.Projects).HasForeignKey(p => p.LeaderID);
        }
    }
}
