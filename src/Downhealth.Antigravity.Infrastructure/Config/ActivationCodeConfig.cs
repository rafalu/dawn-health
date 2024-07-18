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
        builder.Property(x => x.AssignedToEmail).IsRequired(true).HasMaxLength(256);
        builder.Property(x => x.Code).IsRequired();
        builder.Property(x => x.ExpiryDate).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.CreatedBy).IsRequired();
        builder.Property(x => x.IsUsed).IsRequired();

        builder.HasIndex(x => new { x.Code, x.AssignedToEmail });
    }
}
