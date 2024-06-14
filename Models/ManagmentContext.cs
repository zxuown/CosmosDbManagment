using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;

namespace CosmosDbManagment.Models;

public class ManagmentContext : IdentityDbContext<TeamMember, IdentityRole<string>, string>
{
	public ManagmentContext(DbContextOptions options) : base(options) { }

	public virtual DbSet<Project> Projects { get; set; }

	public virtual DbSet<ProjectDto> ProjectDtos { get; set; }

	public virtual DbSet<Task> Tasks { get; set; }

	public virtual DbSet<TeamMember> TeamMembers { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		builder.Entity<IdentityRole<string>>()
			.Property(b => b.ConcurrencyStamp)
			.IsETagConcurrency();
		builder.Entity<TeamMember>()
			.Property(b => b.ConcurrencyStamp)
			.IsETagConcurrency();

	}
}
