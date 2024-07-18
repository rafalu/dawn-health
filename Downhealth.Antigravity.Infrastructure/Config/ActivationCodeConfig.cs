using Dawnhealth.Antigravity.Domain;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Downhealth.Antigravity.Infrastructure.Config;

public class ActivationCodeConfig : IEntityTypeConfiguration<ActivationCode>
{
    public void Configure(EntityTypeBuilder<ActivationCode> builder)
    {
        // calling these methods isn't strictly necessary in this case, but it's a good for clarity
        builder.HasKey(x => x.Id);
        builder.Property(x => x.UserId).IsRequired();
        builder.Property(x => x.Code).IsRequired();
        builder.Property(x => x.ExpiryDate).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.CreatedBy).IsRequired();
        builder.Property(x => x.IsUsed).IsRequired();

        // one-to-many: User -> ActivationCode
        builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);

        builder.HasIndex(x => new { x.UserId, x.Code });
    }
}
