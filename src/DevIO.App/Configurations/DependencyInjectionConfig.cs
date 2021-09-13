using AutoMapper.Configuration;
using DevIO.App.Extensions;
using DevIO.Data.Context;
using DevIO.Data.Repository;
using DevIO.Business.Interface;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using DevIO.Business.Notifications;
using System;
using System.Linq;
using System.Threading.Tasks;
using DevIO.Business.Services;

namespace DevIO.App.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(
            this IServiceCollection services)
        {
            services.AddScoped<MyDbContext>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<ISupplierRepository, SupplierRepository>();


            services.AddScoped<INotifier, Notifier>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IProductService, ProductService>();



            //Não existe ainda
            services.AddSingleton<IValidationAttributeAdapterProvider, 
                CurrencyAttributeAdapterProvider>();




            return services;
        }
    }
}
