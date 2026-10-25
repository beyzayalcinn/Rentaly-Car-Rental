# Rentaly - Case 1 Ready

- Rentaly template homepage is served from `/` through `Views/Home/Index.cshtml`.
- Template assets remain under `wwwroot/RentalyTema` and are referenced with `/RentalyTema/...`.
- Home content services are wired into the homepage: About, Features, Why Choose Us, Process, Testimonials, FAQ, Our Future and Awards.
- Admin CRUD controllers/views are available for the case-1 content entities plus Brand and CarModel.
- Duplicate EfBrandDal/EfCarModel DAL definitions were removed to avoid ambiguous-reference errors.
- Category and Rental managers no longer contain `NotImplementedException`.

Open `Rentaly.sln` in Visual Studio and set `Rentaly.WebUI` as the startup project.
