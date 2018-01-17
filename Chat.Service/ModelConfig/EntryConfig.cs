using Chat.Service.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Service.ModelConfig
{
    class EntryConfig : EntityTypeConfiguration<EntryEntity>
    {
        public EntryConfig()
        {
            ToTable("T_Entries");

            Property(e => e.Name).HasMaxLength(20).IsRequired();
            Property(e => e.Mobile).HasMaxLength(30).IsRequired().IsUnicode();
            Property(e => e.Workplace).HasMaxLength(10);
            Property(e => e.WorkUnits).HasMaxLength(100).IsRequired();
            Property(e => e.Duty).HasMaxLength(10).IsRequired();
            HasRequired(e => e.Stays).WithMany().HasForeignKey(e => e.StayId).WillCascadeOnDelete(false);
            HasRequired(e => e.Pays).WithMany().HasForeignKey(e => e.PayId).WillCascadeOnDelete(false);
            HasRequired(e => e.EntryChannels).WithMany().HasForeignKey(e => e.EntryChannelId).WillCascadeOnDelete(false);
            HasRequired(e => e.Cities).WithMany().HasForeignKey(e => e.CityId).WillCascadeOnDelete(false);
            Property(e => e.InvoiceUp).HasMaxLength(20).IsRequired();
            Property(e => e.Ein).HasMaxLength(30).IsRequired().IsUnicode();
            Property(e => e.Address).HasMaxLength(150).IsRequired();
            Property(e => e.Contact).HasMaxLength(20).IsRequired();
            Property(e => e.OpenBank).HasMaxLength(10).IsRequired();
            Property(e => e.BankAccount).HasMaxLength(30).IsRequired().IsUnicode();
            HasMany(e => e.Trains).WithMany(t => t.Entries).Map(m => m.ToTable("T_EntriesTrains").MapLeftKey("EntryId").MapRightKey("TrainId"));
        }
    }
}
