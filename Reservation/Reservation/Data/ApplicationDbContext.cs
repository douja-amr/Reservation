using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Reservation.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<TypeReservations> TypeReservations { get; set; }
        public DbSet<ReservationModel> Reservations { get; set; }
        //
        public DbSet<Reservation.Models.ReservationViewModel> ReservationViewModel { get; set; }
        //public DbSet<ReservationViewModel> ReservationViewModel { get; set; }
    }
}
