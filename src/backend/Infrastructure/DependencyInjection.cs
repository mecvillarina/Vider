﻿using Application.Common.Interfaces;
using Infrastructure.Context;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure
{
    [ExcludeFromCodeCoverage]
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IDateTime, DateTimeService>();

            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();

            var connectionString = configuration.GetConnectionString("DataDbContext");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddScoped<IDomainEventService, DomainEventService>();

            services.AddTransient<IAuthTokenService, AuthTokenService>();
            services.AddTransient<ICreatorIdentityService, CreatorIdentityService>();
            services.AddTransient<IFaucetService, FaucetService>();
            services.AddTransient<IXrplAccountService, XrplAccountService>();
            services.AddTransient<IXrplPaymentService, XrplPaymentService>();
            services.AddTransient<IXrplNFTTokenService, XrplNFTTokenService>();

            services.AddTransient<IAzureStorageAccountService, AzureStorageAccountService>();
            services.AddTransient<IAzureStorageBlobService, AzureStorageBlobService>();
            services.AddTransient<IAzureStorageQueueService, AzureStorageQueueService>();

            // note: the below dependencies use a scope context (per call scope)
            services.AddScoped<ICallContext, MutableCallContext>();
            return services;
        }
    }
}