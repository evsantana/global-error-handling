using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
        if(contextFeature != null)
        {
            Console.WriteLine($"Error : {contextFeature.Error}");

            await context.Response.WriteAsJsonAsync(new
            {
                Status = context.Response.StatusCode,
                Message = "Internal server error. Please try again later",
                Error = contextFeature.Error.Message
            });
        }
    });
});

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    //try
    //{
    //    var forecast = Enumerable.Range(10, 15).Select(index =>
    //     new WeatherForecast
    //     (
    //         DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
    //         Random.Shared.Next(-20, 55),
    //         summaries[20]
    //     ))
    // .ToArray();
    //    return Results.Ok(forecast);
    //}
    //catch (Exception ex)
    //{
    //    return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
    //}

    var forecast = Enumerable.Range(10, 15).Select(index =>
     new WeatherForecast
     (
         DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
         Random.Shared.Next(-20, 55),
         summaries[20]
     ))
 .ToArray();
    return Results.Ok(forecast);

})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
