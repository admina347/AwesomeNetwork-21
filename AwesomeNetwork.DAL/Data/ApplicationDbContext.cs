using AwesomeNetwork.DAL.Models.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AwesomeNetwork.DAL
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration<Friend>(new FriendConfiguration());

            foreach(var foreignKey in builder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
            //builder.Entity<Friend>().HasKey(uf => new {uf.UserId, uf.CurrentFriendId});
            /* builder.Entity<Friend>()
                .HasOne(u => u.UserId)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Friend>()
                .HasOne(f => f.CurrentFriendId)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict); */

            /* builder.Entity<Friend>()
                .HasOne<Friend>(u => u.UserId)
                .WithMany(f => f.CurrentFriendId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Friend>()
                .HasOne(u => u.CurrentFriendId)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict); */
        }

    }
}