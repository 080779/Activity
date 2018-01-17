using Chat.IService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.DTO.DTO;
using Chat.Service.Entities;
using System.Data.Entity;

namespace Chat.Service.Service
{
    public class EntryService : IEntryService
    {
        public long Add(EntryDTO dto)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                //CommonService<IdNameEntity> cs = new CommonService<IdNameEntity>(dbc);
                
                EntryEntity entity = new EntryEntity();
                entity.Address = dto.Address;
                entity.BankAccount = dto.BankAccount;
                entity.Contact = dto.Contact;
                entity.Duty = dto.Duty;
                entity.Ein = dto.Ein;
                entity.Gender = dto.Gender;
                entity.InvoiceUp = dto.InvoiceUp;
                entity.Mobile = dto.Mobile;
                entity.Name = dto.Name;
                entity.OpenBank = dto.OpenBank;
                entity.PayId = dto.PayId;
                entity.StayId = dto.StayId;
                entity.CityId = dto.CityId;
                entity.EntryChannelId = dto.EntryChannelId;
                //entity.Workplace = dto.Workplace;
                entity.WorkUnits = dto.WorkUnits;
                dbc.Entries.Add(entity);
                dbc.SaveChanges();
                return entity.Id;
            }                
        }

        public EntryListDTO[] GetAll()
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<EntryEntity> cs = new CommonService<EntryEntity>(dbc);
                return cs.GetAll().Include(e => e.Stays).Include(e => e.Pays).Include(e => e.EntryChannels).Include(e => e.Cities).Include(e => e.Trains).OrderByDescending(e => e.CreateDateTime).Take(20).ToList().Select(e => ToListDTO(e)).ToArray();
            }
        }

        private EntryListDTO ToListDTO(EntryEntity entity)
        {
            EntryListDTO dto = new EntryListDTO();
            dto.Address = entity.Address;
            dto.BankAccount = entity.BankAccount;
            dto.CityName = entity.Cities.Name;
            dto.Contact = entity.Contact;
            dto.CreateDateTime = entity.CreateDateTime;
            dto.Duty = entity.Duty;
            dto.Ein = entity.Ein;
            dto.EntryChannelName = entity.EntryChannels.Name;
            dto.Gender = entity.Gender;
            dto.Id = entity.Id;
            dto.InvoiceUp = entity.InvoiceUp;
            dto.Mobile = entity.Mobile;
            dto.Name = entity.Name;
            dto.OpenBank = entity.OpenBank;
            dto.PayName = entity.Pays.Name;
            dto.StayName = entity.Stays.Name;
            dto.WorkUnits = entity.WorkUnits;
            return dto;
        }
    }
}
