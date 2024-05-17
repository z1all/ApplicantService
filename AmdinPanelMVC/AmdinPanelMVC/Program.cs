using AmdinPanelMVC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

if(builder.Environment.IsDevelopment())
{
    //builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
}

// Services
builder.Services.AddServices();

var app = builder.Build();

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "Admission",
    pattern: "Applicant/{applicantId}/Admission/{admissionId}",
    defaults: new { controller = "Admission", action = "ApplicantAdmission" });

app.MapControllerRoute(
    name: "NotFound",
    pattern: "NotFound",
    defaults: new { controller = "Home", action = "NotFoundError" });

app.Run();
