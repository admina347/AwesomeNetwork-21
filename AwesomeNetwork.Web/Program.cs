using AutoMapper;
using AwesomeNetwork;
using AwesomeNetwork.DAL;
using AwesomeNetwork.DAL.Data.Repository;
using AwesomeNetwork.DAL.Models.Users;
using AwesomeNetwork.Data.Repository;
using AwesomeNetwork.Extentions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//Db
builder.Services.AddDbContext<ApplicationDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
//add reppository
builder.Services.AddUnitOfWork()
.AddCustomRepository <Friend, FriendsRepository>()
.AddCustomRepository <Message, MessageRepository>()
//Identity password settings
.AddIdentity<User, IdentityRole>(opts => {
  opts.Password.RequiredLength = 5;   
  opts.Password.RequireNonAlphanumeric = false;  
  opts.Password.RequireLowercase = false; 
  opts.Password.RequireUppercase = false; 
  opts.Password.RequireDigit = false;
  })
  .AddEntityFrameworkStores<ApplicationDbContext>();


// Подключаем автомаппинг
var mapperConfig = new MapperConfiguration((v) => 
{
    v.AddProfile(new MappingProfile());
}
);
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
