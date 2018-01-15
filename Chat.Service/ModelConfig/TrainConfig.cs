using Chat.Service.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Service.ModelConfig
{
    class TrainConfig : EntityTypeConfiguration<TrainEntity>
    {
        public TrainConfig()
        {
            ToTable("T_Trains");
            Property(t => t.Title).HasMaxLength(150).IsRequired();
            Property(t => t.Img).HasMaxLength(150).IsRequired().IsUnicode(false);
            Property(t => t.Address).HasMaxLength(256).IsRequired();
            Property(t => t.Description).HasMaxLength(256).IsRequired();
            HasRequired(t => t.Status).WithMany().HasForeignKey(e => e.StatusId).WillCascadeOnDelete(false);
        }
    }
}
