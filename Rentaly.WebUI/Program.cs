
using Microsoft.EntityFrameworkCore;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.BusinessLayer.Concrete;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.Context;
using Rentaly.DataAccessLayer.EntityFrameWork;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// MVC
builder.Services.AddControllersWithViews();

// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Database
builder.Services.AddDbContext<RentalyContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddScoped<ICarService, CarManager>();

// Category
builder.Services.AddScoped<ICategoryService, CategoryManager>();

// Brand
builder.Services.AddScoped<IBrandService, BrandManager>();

// Branch
builder.Services.AddScoped<IBranchService, BranchManager>();

// Car Model
builder.Services.AddScoped<ICarModelService, CarModelManager>();

// Customer
builder.Services.AddScoped<ICustomerService, CustomerManager>();

// Rental
builder.Services.AddScoped<IRentalService, RentalManager>();
builder.Services.AddScoped<IEmailService, EmailManager>();


builder.Services.AddScoped<IAboutService, AboutManager>();

builder.Services.AddScoped<IProcessService, ProcessManager>();

builder.Services.AddScoped<IFeatureService, FeatureManager>();

builder.Services.AddScoped<IWhyChooseUsService, WhyChooseUsManager>();

builder.Services.AddScoped<ITestimonialService, TestimonialManager>();

builder.Services.AddScoped<IFaqService, FaqManager>();

builder.Services.AddScoped<IOurFutureService, OurFutureManager>();

builder.Services.AddScoped<IAwardService, AwardManager>();


// Car
builder.Services.AddScoped<ICarDal, EfCarDal>();

// Category
builder.Services.AddScoped<ICategoryDal, EfCategoryDal>();

// Brand
builder.Services.AddScoped<IBrandDal, EfBrandDal>();

// Branch
builder.Services.AddScoped<IBranchDal, EfBranchDal>();

// Car Model
builder.Services.AddScoped<ICarModelDal, EfCarModelDal>();

// Customer
builder.Services.AddScoped<ICustomerDal, EfCustomerDal>();

// Rental
builder.Services.AddScoped<IRentalDal, EfRentalDal>();



builder.Services.AddScoped<IAboutDal, EfAboutDal>();

builder.Services.AddScoped<IProcessDal, EfProcessDal>();

builder.Services.AddScoped<IFeatureDal, EfFeatureDal>();

builder.Services.AddScoped<IWhyChooseUsDal, EfWhyChooseUsDal>();

builder.Services.AddScoped<ITestimonialDal, EfTestimonialDal>();

builder.Services.AddScoped<IFaqDal, EfFaqDal>();

builder.Services.AddScoped<IOurFutureDal, EfOurFutureDal>();

builder.Services.AddScoped<IAwardDal, EfAwardDal>();


var app = builder.Build();
Console.WriteLine("ÇALIÞAN PROJE: " + app.Environment.ContentRootPath);


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseStatusCodePagesWithReExecute("/Home/NotFound");

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

// NORMAL SAYFALAR
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);


app.Run();
