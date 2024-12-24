using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DAL.Entity
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=MyConnectionString")
        {
        }

        public virtual DbSet<buses> buses { get; set; }
        public virtual DbSet<drivers> drivers { get; set; }
        public virtual DbSet<routes> routes { get; set; }
        public virtual DbSet<schedules> schedules { get; set; }
        public virtual DbSet<tickets> tickets { get; set; }
        public virtual DbSet<trips> trips { get; set; }
        public virtual DbSet<user_cards> user_cards { get; set; }
        public virtual DbSet<users> users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<buses>()
                .HasMany(e => e.schedules)
                .WithOptional(e => e.buses)
                .WillCascadeOnDelete();

            modelBuilder.Entity<routes>()
                .HasMany(e => e.schedules)
                .WithOptional(e => e.routes)
                .WillCascadeOnDelete();

            modelBuilder.Entity<schedules>()
                .HasMany(e => e.trips)
                .WithOptional(e => e.schedules)
                .WillCascadeOnDelete();

            modelBuilder.Entity<tickets>()
                .Property(e => e.price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<trips>()
                .Property(e => e.price)
                .HasPrecision(10, 2);

            modelBuilder.Entity<trips>()
                .HasMany(e => e.tickets)
                .WithOptional(e => e.trips)
                .WillCascadeOnDelete();

            modelBuilder.Entity<users>()
                .HasMany(e => e.tickets)
                .WithOptional(e => e.users)
                .HasForeignKey(e => e.buyer_id);

            modelBuilder.Entity<users>()
                .HasMany(e => e.user_cards)
                .WithOptional(e => e.users)
                .WillCascadeOnDelete();
        }
    }
}
