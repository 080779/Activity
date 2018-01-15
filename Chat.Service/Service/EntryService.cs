using Chat.IService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.DTO.DTO;
using Chat.Service.Entities;

namespace Chat.Service.Service
{
    public class EntryService : IEntryService
    {
        public long Add(EntryDTO dto)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                //CommonService<EntryEntity> cs = new CommonService<EntryEntity>(dbc);
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
                //entity.Workplace = dto.Workplace;
                entity.WorkUnits = dto.WorkUnits;
                dbc.Entries.Add(entity);
                dbc.SaveChanges();
                return entity.Id;
            }                
        }
    }
}
