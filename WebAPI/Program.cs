using Airport;
using Newtonsoft.Json.Converters;

namespace WebAPI
{
    public class Program
    {
        public static Pathfinder Pathfinder = new Pathfinder(new GraphCollection());

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers().AddNewtonsoftJson(opts =>
            {
                opts.SerializerSettings.Converters.Add(new StringEnumConverter(camelCaseText: true));
                opts.SerializerSettings.Converters.Add(new GraphCollectionConverter());
                opts.SerializerSettings.DateFormatString = "dd/MM/yyyy HH:mm:ss";
                opts.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen().AddSwaggerGenNewtonsoftSupport();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapControllers();

            app.Run();
        }
    }
}