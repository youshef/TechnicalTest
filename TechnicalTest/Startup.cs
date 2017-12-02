using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using TechnicalTest.Api;
using TechnicalTest.Api.GraphQl;
using TechnicalTest.Data;
using TechnicalTest.Data.Services;

namespace TechnicalTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDbContext<AdsDbContext>(options=>options.UseSqlite("Data Source=TechnicalTestDb.db"));

            
            services.AddTransient<IFavoritesService, FavoritesService>();
            services.AddScoped<IDocumentExecuter, DocumentExecuter>();
            services.AddScoped<FavoritesQuery>();
            services.AddScoped<FavoritesMutation>();
            services.AddTransient<AdType>();
            services.AddTransient<FavoriteType>();
            services.AddTransient<FavoriteInputType>();
            var sp = services.BuildServiceProvider();
            services.AddScoped<ISchema>(_ => new FavoritesSchema(type => (GraphType)sp.GetService(type)) {
                Query = sp.GetService<FavoritesQuery>(),
                Mutation = sp.GetService<FavoritesMutation>()
            });
        }
    

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory,IFavoritesService favoritesService, AdsDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseMvc();
            dbContext.SeetTestData();
        }
    }
}
