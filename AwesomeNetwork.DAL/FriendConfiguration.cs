using AwesomeNetwork.DAL.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AwesomeNetwork.DAL
{
    public class FriendConfiguration : IEntityTypeConfiguration<Friend>
    {
        public void Configure(EntityTypeBuilder<Friend> builder)
        {
            builder.ToTable("UserFriends").HasKey(p => p.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            /* builder.HasOne(u => u.UserId)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(f => f.CurrentFriendId)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict); */
        }
    }
}