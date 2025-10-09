using Blazored.LocalStorage;
using DevExpress.AspNetCore.Reporting;
using DevExpress.Blazor.Reporting;
using DevExpress.XtraReports.Web.Extensions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using MyWebERP.Authentication;
using MyWebERP.Data;
using MyWebERP.Main.Components;
using MyWebERP.Main.Services;
using MyWebERP.Model;
using MyWebERP.Services;
using System.Globalization;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents(options =>
        options.DetailedErrors = builder.Environment.IsDevelopment())
    .AddInteractiveServerComponents();


builder.Services.AddDevExpressBlazor(options =>
{
    options.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5;
});
builder.Services.AddMvc();

// Xử lý chuyển ngôn ngữ trong cookie
//Trường hợp VẪN CẦN giữ lại:
//Bạn có controller hoặc middleware tùy chỉnh
//Bạn có service nào đó (singleton hoặc scoped) cần truy cập HttpContext
//Bạn muốn log thông tin request, IP, header v.v.
//⇒ Nên giữ lại AddHttpContextAccessor()
//builder.Services.AddHttpContextAccessor();


//// test thôi
//builder.Services.AddSingleton<WeatherForecastService>();

#region Cài đặt file ngôn ngữ json
var locFolder = Path.Combine(Directory.GetCurrentDirectory(), "LocalizationFiles");
builder.Services.AddSingleton(new JsonLocalizationProvider(locFolder));

// Đăng ký IStringLocalizer dùng toàn cục (không generic)
builder.Services.AddSingleton<IStringLocalizer>(sp =>
{
    var provider = sp.GetRequiredService<JsonLocalizationProvider>();
    return new JsonStringLocalizer<object>(provider); // tất cả dùng object
});
#endregion

//builder.Services.AddScoped(IServiceProvider => new HttpClient { BaseAddress = new Uri("http://45.119.82.176:9400") });
// Thêm file config
IConfiguration config = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddEnvironmentVariables().Build();
builder.Services.AddScoped(IServiceProvider => new HttpClient { BaseAddress = new Uri(config.GetValue<string>("RootApi")) });

builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IDataService, DataService>();

// Lưu thông tin người dùng
builder.Services.AddBlazoredLocalStorage();

// Lưu trạng thái đăng nhập
builder.Services.AddScoped<AuthenticationStateProvider, MyAuthenticationStateProvider>();


// Lưu thông tin đăng nhập, năm làm việc, thông tin chi nhánh làm việc
builder.Services.AddScoped<AppStateManager>();

builder.Services.AddDevExpressServerSideBlazorReportViewer();
builder.Services.AddDevExpressBlazor();
builder.Services.Configure<DevExpress.Blazor.Configuration.GlobalOptions>(options => {
    options.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5;
});

// Thiết kế báo cáo
builder.Services.AddDevExpressBlazorReporting();
builder.Services.AddScoped<ReportStorageWebExtension, MyReportStorageWebExtension>();
//builder.Services.AddTransient<IWebDocumentViewerReportResolver, MyWebDocumentViewerReportResolver>();

builder.Services.ConfigureReportingServices(configurator =>
{
    configurator.ConfigureReportDesigner(designerConfigurator =>
    {
    });
    configurator.ConfigureWebDocumentViewer(viewerConfigurator =>
    {
        viewerConfigurator.UseCachedReportSourceBuilder();
        //viewerConfigurator.RegisterConnectionProviderFactory<MyReportStorageWebExtension>();
    });
    // Đoạn này đăng ký class để có thể convert json ở trong file MyReportStorageWebExtension
    // Thiếu là báo security
    DevExpress.Utils.DeserializationSettings.RegisterTrustedClass(typeof(MenuReportFileItem));
    configurator.UseAsyncEngine();
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

#region Cấu hình localization
var supportedCultures = new[] { new CultureInfo("vi-VN"), new CultureInfo("en-US") };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("vi-VN"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

// Gọi localization middleware TRƯỚC
app.UseRequestLocalization(localizationOptions);

// Sau đó mới kiểm tra xem có cần gán cookie mặc định không
app.Use(async (context, next) =>
{
    var feature = context.Features.Get<IRequestCultureFeature>();
    var cultureResult = feature?.RequestCulture;

    if (cultureResult == null)
    {
        var defaultCulture = new RequestCulture("vi-VN");
        var cookieValue = CookieRequestCultureProvider.MakeCookieValue(defaultCulture);

        context.Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            cookieValue,
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                IsEssential = true,
                Path = "/"
            }
        );
    }

    await next();
});

/* ----- endpoint download file ngôn ngữ json----- */
app.MapGet("/localization-download/{culture}", async (string culture) =>
{
    var path = Path.Combine(locFolder, $"{culture}.json");
    if (!System.IO.File.Exists(path)) return Results.NotFound();
    return Results.File(path, "application/json", $"{culture}.json");
});

#endregion

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
//app.MapRazorComponents<App>().AddInteractiveServerRenderMode(); // Chuẩn.
// Đoạn này để xử lý lỗi cụ thể như ở form thêm đơn hàng, trong form đó gọi thêm khách, sản phẩm ở dll khác
// Nếu không có đoạn này thì lỗi render không load được dll customer, sku...
AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
{
    var name = new AssemblyName(args.Name).Name + ".dll";
    var path = Path.Combine(System.AppContext.BaseDirectory, name);
    return File.Exists(path) ? Assembly.LoadFrom(path) : null;
};

// Cho phép đổi ngôn ngữ thông qua URL: /set-culture?culture=vi-VN&returnUrl=/trang-hien-tai
app.MapGet("/set-culture", (HttpContext context) =>
{
    var query = context.Request.Query;
    var culture = query["culture"].ToString();
    var returnUrl = query["returnUrl"].ToString() ?? "/";

    if (!string.IsNullOrEmpty(culture))
    {
        var cookieValue = CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture));

        context.Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            cookieValue,
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddYears(1),
                IsEssential = true,
                Path = "/"
            });
    }

    return Results.Redirect(returnUrl);
});

// Đoạn này load để thì F5 ở các trang mới được, không có thì không F5 được.
string _sAssemblyNames = config.GetValue<string>("AdditionalAssemblies");
System.Reflection.Assembly[] _additionalAssemblies = MyLib.GetAdditionalAssemblies(_sAssemblyNames);
app.MapRazorComponents<App>().AddInteractiveServerRenderMode().AddAdditionalAssemblies(_additionalAssemblies);

// Nếu report dùng object thì phải đăng ký mấy cái này.
//DevExpress.Utils.DeserializationSettings.RegisterTrustedClass(typeof(Model.MenuItem));
//DevExpress.Utils.DeserializationSettings.RegisterTrustedClass(typeof(Model.MenuReportFileItem));
//DevExpress.Utils.DeserializationSettings.RegisterTrustedClass(typeof(Model.MenuPermissionItem));
//DevExpress.Utils.DeserializationSettings.RegisterTrustedClass(typeof(Model.ReportViewParamItem));
//DevExpress.Utils.DeserializationSettings.RegisterTrustedClass(typeof(Model.APIResultModel));
//DevExpress.Utils.DeserializationSettings.RegisterTrustedClass(typeof(Model.APIParameterModel));
//DevExpress.Utils.DeserializationSettings.RegisterTrustedClass(typeof(ExpandoObject));

//DeserializationSettings.RegisterTrustedAssembly("MyWebERP.Model");
//DevExpress.Utils.DeserializationSettings.RegisterTrustedAssembly(typeof(Model.ReportViewParamItem).Assembly);
//DevExpress.Utils.DeserializationSettings.RegisterTrustedAssembly(typeof(ExpandoObject).Assembly);

// 👉 
app.Run();