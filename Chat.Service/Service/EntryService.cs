using Chat.IService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.DTO.DTO;
using Chat.Service.Entities;
using System.Data.Entity;
using System.Data;

namespace Chat.Service.Service
{
    public class EntryService : IEntryService
    {
        //public IIdNameService idNameService { get; set; }

        public long Add(EntryDTO dto)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<TrainEntity> cs = new CommonService<TrainEntity>(dbc);
                var train= cs.GetAll().SingleOrDefault(t=>t.Id==dto.TrainId);
                if(train==null)
                {
                    return 0;
                }

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
                train.Entries.Add(entity);
                train.EntryCount++;
                dbc.SaveChanges();
                return entity.Id;
            }                
        }

        public long ImportAdd(EntryImportDTO dto)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<TrainEntity> cs = new CommonService<TrainEntity>(dbc);
                CommonService<IdNameEntity> ics = new CommonService<IdNameEntity>(dbc);
                var train = cs.GetAll().Include(t=>t.Entries).SingleOrDefault(t => t.Id == dto.TrainId);
                if (train == null)
                {
                    return 0;
                }

                EntryEntity entity = new EntryEntity();
                entity.Address = dto.Address;
                entity.BankAccount = dto.BankAccount;
                entity.Contact = dto.Contact;
                entity.Duty = dto.Duty;
                entity.Ein = dto.Ein;
                entity.Gender = dto.Gender=="男";
                entity.InvoiceUp = dto.InvoiceUp;
                entity.Mobile = dto.Mobile;
                entity.Name = dto.Name;
                entity.OpenBank = dto.OpenBank;
                entity.PayId =ics.GetAll().SingleOrDefault(i=>i.Name.Contains(dto.PayName)).Id;
                entity.StayId = ics.GetAll().SingleOrDefault(i => i.Name.Contains(dto.StayName)).Id;
                entity.CityId = dto.CityId;
                entity.EntryChannelId = dto.EntryChannelId;
                //entity.Workplace = dto.Workplace;
                entity.WorkUnits = dto.WorkUnits;
                dbc.Entries.Add(entity);
                train.Entries.Add(entity);
                dbc.SaveChanges();
                return entity.Id;
            }
        }

        public bool EntryImport(long trainId, long cityId, long entryChannelId, DataTable dt)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<TrainEntity> cs = new CommonService<TrainEntity>(dbc);
                CommonService<IdNameEntity> ics = new CommonService<IdNameEntity>(dbc);
                CommonService<EntryEntity> ecs = new CommonService<EntryEntity>(dbc);
                var train = cs.GetAll().Include(t => t.Entries).SingleOrDefault(t => t.Id == trainId);
                if (train == null)
                {
                    return false;
                }
                EntryEntity entity;
                string payName;
                string stayName;
                string moblie;
                foreach (DataRow row in dt.Rows)
                {
                    if(CheckData(row)==false)
                    {
                        continue;
                    }
                    moblie = row["手机号"].ToString();
                    var entry = ecs.GetAll().SingleOrDefault(e => e.Mobile == moblie);
                    if(entry==null)
                    {
                        entity = new EntryEntity();
                        entity.Address = row["地址"].ToString();
                        entity.BankAccount = row["银行账号"].ToString();
                        entity.Contact = row["联系方式"].ToString();
                        entity.Duty = row["职务"].ToString();
                        entity.Ein = row["税号"].ToString();
                        entity.Gender = row["性别"].ToString() == "男";
                        entity.InvoiceUp = row["发票抬头"].ToString();
                        entity.Mobile = moblie;
                        entity.Name = row["姓名"].ToString();
                        entity.OpenBank = row["开户行"].ToString();
                        payName = row["支付方式"].ToString();
                        stayName = row["住宿要求"].ToString();
                        entity.PayId = ics.GetAll().SingleOrDefault(i => i.Name.Contains(payName)).Id;
                        entity.StayId = ics.GetAll().SingleOrDefault(i => i.Name.Contains(stayName)).Id;
                        entity.CityId = cityId;
                        entity.EntryChannelId = entryChannelId;
                        //entity.Workplace = dto.Workplace;
                        entity.WorkUnits = row["工作单位"].ToString();
                        dbc.Entries.Add(entity);
                        train.Entries.Add(entity);
                        train.EntryCount++;
                    }
                    else
                    {
                        entry.Address = row["地址"].ToString();
                        entry.BankAccount = row["银行账号"].ToString();
                        entry.Contact = row["联系方式"].ToString();
                        entry.Duty = row["职务"].ToString();
                        entry.Ein = row["税号"].ToString();
                        entry.Gender = row["性别"].ToString() == "男";
                        entry.InvoiceUp = row["发票抬头"].ToString();
                        //entry.Mobile = moblie;
                        entry.Name = row["姓名"].ToString();
                        entry.OpenBank = row["开户行"].ToString();
                        payName = row["支付方式"].ToString();
                        stayName = row["住宿要求"].ToString();
                        entry.PayId = ics.GetAll().SingleOrDefault(i => i.Name.Contains(payName)).Id;
                        entry.StayId = ics.GetAll().SingleOrDefault(i => i.Name.Contains(stayName)).Id;
                        entry.CityId = cityId;
                        entry.EntryChannelId = entryChannelId;
                        //entity.Workplace = dto.Workplace;
                        entry.WorkUnits = row["工作单位"].ToString();
                        if(!train.Entries.Any(e => e.Id == entry.Id))
                        {
                            train.Entries.Add(entry);
                            train.EntryCount++;
                        }                                         
                    }
                }                
                dbc.SaveChanges();
                return true; 
            }
        }

        public bool IsJoinined(long trainId, string mobile)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<TrainEntity> cs = new CommonService<TrainEntity>(dbc);
                var train = cs.GetAll().SingleOrDefault(t=>t.Id==trainId);
                if(train==null)
                {
                    return false;
                }
                return train.Entries.Where(e => e.IsDeleted == false).Any(e=>e.Mobile==mobile);
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

        public EntryListDTO[] GetByTrainId(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<TrainEntity> cs = new CommonService<TrainEntity>(dbc);
                var train = cs.GetAll().SingleOrDefault(t=>t.Id==id);
                if(train==null)
                {
                    return null;
                }
                return train.Entries.Where(e=>e.IsDeleted==false).OrderByDescending(e => e.CreateDateTime).Take(20).ToList().Select(e => ToListDTO(e)).ToArray();
            }
        }

        public EntryListDTO[] GetByTrainIdCityId(long id, long cityId)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<TrainEntity> cs = new CommonService<TrainEntity>(dbc);
                var train = cs.GetAll().SingleOrDefault(t => t.Id == id);
                if (train == null)
                {
                    return null;
                }
                return train.Entries.Where(e => e.IsDeleted == false && e.CityId==cityId).OrderByDescending(e => e.CreateDateTime).ToList().Select(e => ToListDTO(e)).ToArray();
            }
        }

        public EntryListDTO GetById(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<EntryEntity> cs = new CommonService<EntryEntity>(dbc);
                var entry = cs.GetAll().SingleOrDefault(e => e.Id == id);
                if (entry == null)
                {
                    return null;
                }
                return ToListDTO(entry);
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

        private bool CheckData(DataRow row)
        {            
            if(row["地址"].ToString()=="")
            {
                return false;
            }
            if (row["银行账号"].ToString() == "")
            {
                return false;
            }
            if (row["联系方式"].ToString() == "")
            {
                return false;
            }
            if (row["职务"].ToString() == "")
            {
                return false;
            }
            if (row["税号"].ToString() == "")
            {
                return false;
            }
            if (row["性别"].ToString() == "")
            {
                return false;
            }
            if (row["发票抬头"].ToString() == "")
            {
                return false;
            }
            if (row["手机号"].ToString() == "")
            {
                return false;
            }
            string mobile = row["手机号"].ToString();
            long num;
            if(!long.TryParse(mobile,out num))
            {
                return false;
            }
            if(mobile.Length!=11)
            {
                return false;
            }
            if (row["姓名"].ToString() == "")
            {
                return false;
            }
            if (row["开户行"].ToString() == "")
            {
                return false;
            }
            if (row["支付方式"].ToString() == "")
            {
                return false;
            }
            if (row["住宿要求"].ToString() == "")
            {
                return false;
            }
            if (row["工作单位"].ToString() == "")
            {
                return false;
            }            
            return true;
        }
                
    }
}
