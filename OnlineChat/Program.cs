using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineChat.Context;
using OnlineChat.Hubs;
using OnlineChat.Mappings;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
builder.Services.AddDbContext<ChatContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddSignalR();
builder.Services.AddScoped(provider => new MapperConfiguration(cfg => {
    cfg.AddProfile(new MessageProfile(provider.GetService<ChatContext>()));
    cfg.AddProfile(typeof(ConversationRoomProfile));
}).CreateMapper());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapHub<ChatHub>("/chatHub");

app.Run();
