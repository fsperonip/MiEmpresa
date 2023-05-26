Conexiones.Conexiones.ConfigurarEntorno("");
//try
//{

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();

    //Action filter home!
    //builder.Services.AddControllersWithViews(options =>
    //{
    //    options.Filters.Add<LibGenerica.InformationApiFilter>();
    //});

    //builder.Services.AddCors(p => p.AddDefaultPolicy(build =>
    //{
    //    build.WithOrigins("https://democratest.com/", "https://blue-flower-0ebcc9703.2.azurestaticapps.net", "https://zealous-coast-060dacf03.2.azurestaticapps.net");
    //    build.AllowAnyMethod();
    //    build.AllowAnyHeader();
    //}));

    var app = builder.Build();

    //Home para api!
    app.MapGet("/", () => Results.Redirect("/noKey"));
    app.MapGet("/{key}", (string key) =>
    {
        if (key == Conexiones.Conexiones.apiKey)
            return "Authorized!";
        else
            return "Unauthorized!";
    });

    // Configure the HTTP request pipeline.

    app.UseHttpsRedirection();

    app.UseCors();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
//}
//catch (Exception ex) { Herramientas.RequestTelemetryHelper.TrackException(ex); }
