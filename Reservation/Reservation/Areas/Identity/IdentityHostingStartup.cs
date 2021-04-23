using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reservation.Data;

[assembly: HostingStartup(typeof(Reservation.Areas.Identity.IdentityHostingStartup))]
namespace Reservation.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.AddAuthorization(options =>
              {
                  options.AddPolicy("readonlypolicy",
                      builder => builder.RequireRole("Admin", "Student"));
                  options.AddPolicy("writepolicy",
                      builder => builder.RequireRole("Admin"));

              });

            });
        }

    }

}