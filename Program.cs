using CosmosDbManagment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMvc();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ManagmentContext>(options =>
{
	options.UseCosmos(builder.Configuration.GetValue<string>("Azure:CosmosDb:Uri"),
		builder.Configuration.GetValue<string>("Azure:CosmosDb:Key"),
		builder.Configuration.GetValue<string>("Azure:CosmosDb:DatabaseName"));
});
builder.Services.AddIdentity<TeamMember, IdentityRole<string>>(options =>
{
	options.SignIn.RequireConfirmedPhoneNumber = false;
	options.SignIn.RequireConfirmedAccount = false;
	options.SignIn.RequireConfirmedEmail = false;

	options.Password.RequiredLength = 3;
	options.Password.RequiredUniqueChars = 0;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireDigit = false;
	options.Password.RequireLowercase = false;
	options.Password.RequireUppercase = false;
})
	.AddEntityFrameworkStores<ManagmentContext>();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}");
app.Run();
