var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();// добавляем сервисы MVC
// Services. имееет еще куча методов(Add'ов) для добавления каких-то хуевин для проекта

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

app.UseAuthorization();

// добавляем поддержку контроллеров, которые располагаются в области
app.MapControllerRoute(
            name: "Account",
            // {area:exists}, это означает ограничение на параметр area, которое требует, чтобы значение параметра area было указано и не являлось пустым
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
          );
// устанавливаем сопоставление маршрутов с контроллерами
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");// здесь задается контроллер который будет отрабатывать при запуске программы



app.Run();
