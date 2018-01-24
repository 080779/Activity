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
    public class TrainService : ITrainService
    {
        public long AddNew(string title, string img, string address, DateTime? startTime, DateTime? endTime, decimal entryFee, long upToOne, string description)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                TrainEntity train = new TrainEntity();
                train.Address = address;
                train.Description = description;
                train.EndTime = (DateTime)endTime;
                train.EntryFee = entryFee;
                train.Img = img;
                train.StartTime = (DateTime)startTime;
                train.UpToOne = upToOne;
                train.Title = title;
                if(train.StartTime>DateTime.Now)
                {
                    train.StatusId = 34;
                }
                else if(train.StartTime<=DateTime.Now && train.EndTime>=DateTime.Now)
                {
                    train.StatusId = 35;
                }
                else if(train.EndTime<DateTime.Now)
                {
                    train.StatusId = 36;
                }
                dbc.Trains.Add(train);
                dbc.SaveChanges();
                return train.Id;
            }
        }

        public bool Update(long id, string title, string img, string address, DateTime? startTime, DateTime? endTime, decimal entryFee, long upToOne, string description)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<TrainEntity> cs = new CommonService<TrainEntity>(dbc);
                var train = cs.GetAll().SingleOrDefault(t=>t.Id==id);
                if(train==null)
                {
                    return false;
                }
                train.Address = address;
                train.Description = description;
                train.EndTime = (DateTime)endTime;
                train.EntryFee = entryFee;
                train.Img = img;
                train.StartTime = (DateTime)startTime;
                train.UpToOne = upToOne;
                train.Title = title;
                if (train.StartTime > DateTime.Now)
                {
                    train.StatusId = 34;
                }
                else if (train.StartTime <= DateTime.Now && train.EndTime >= DateTime.Now)
                {
                    train.StatusId = 35;
                }
                else if (train.EndTime < DateTime.Now)
                {
                    train.StatusId = 36;
                }
                dbc.SaveChanges();
                return true;
            }
        }

        public bool Delete(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<TrainEntity> cs = new CommonService<TrainEntity>(dbc);
                var train=cs.GetAll().SingleOrDefault(t=>t.Id==id);
                if(train==null)
                {
                    return false;
                }
                train.IsDeleted = true;
                dbc.SaveChanges();
                return true;
            }                
        }

        public TrainDTO[] GetAll()
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<TrainEntity> cs = new CommonService<TrainEntity>(dbc);
                foreach(var train in cs.GetAll())
                {
                    if (train.StartTime > DateTime.Now)
                    {
                        train.StatusId = 34;
                    }
                    else if (train.StartTime <= DateTime.Now && train.EndTime >= DateTime.Now)
                    {
                        train.StatusId = 35;
                    }
                    else if (train.EndTime < DateTime.Now)
                    {
                        train.StatusId = 36;
                    }
                }
                dbc.SaveChanges();
                return cs.GetAll().Include(t => t.Status).Include(t => t.Entries).OrderByDescending(t => t.CreateDateTime).Take(20).ToList().Select(t => ToDTO(t)).ToArray();
            }
        }

        public TrainSearchResult Search(long? statusId, DateTime? startTime, DateTime? endTime, string keyWord,int currentIndex,int pageSize)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                TrainSearchResult result = new TrainSearchResult();
                CommonService<TrainEntity> cs = new CommonService<TrainEntity>(dbc);
                foreach (var train in cs.GetAll())
                {
                    if (train.StartTime > DateTime.Now)
                    {
                        train.StatusId = 34;
                    }
                    else if (train.StartTime <= DateTime.Now && train.EndTime >= DateTime.Now)
                    {
                        train.StatusId = 35;
                    }
                    else if (train.EndTime < DateTime.Now)
                    {
                        train.StatusId = 36;
                    }
                }
                dbc.SaveChanges();
                var trains = cs.GetAll().Include(t=>t.Status).Include(t=>t.Entries);
                if(statusId!=null)
                {
                    trains= trains.Where(t => t.StatusId == statusId);
                }
                if(startTime!=null)
                {
                    trains = trains.Where(t => t.CreateDateTime > startTime);
                }
                if(endTime!=null)
                {
                    trains = trains.Where(t => t.CreateDateTime < endTime);
                }
                if(!string.IsNullOrEmpty(keyWord))
                {
                    trains = trains.Where(t => t.Title.Contains(keyWord));
                }
                result.TotalCount = trains.LongCount();
                result.Trains=trains.OrderByDescending(t => t.CreateDateTime).Skip(currentIndex).Take(pageSize).ToList().Select(t => ToDTO(t)).ToArray();
                return result;
            }
        }

        public TrainDTO GetById(long id)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<TrainEntity> cs = new CommonService<TrainEntity>(dbc);
                var train= cs.GetAll().SingleOrDefault(t => t.Id == id);
                if(train==null)
                {
                    return null;
                }
                if (train.StartTime > DateTime.Now)
                {
                    train.StatusId = 34;
                }
                else if (train.StartTime <= DateTime.Now && train.EndTime >= DateTime.Now)
                {
                    train.StatusId = 35;
                }
                else if (train.EndTime < DateTime.Now)
                {
                    train.StatusId = 36;
                }
                dbc.SaveChanges();
                return ToDTO(train);
            }
        }        

        private TrainDTO ToDTO(TrainEntity entity)
        {
            TrainDTO dto = new TrainDTO();
            dto.Address = entity.Address;
            dto.CreateDateTime = entity.CreateDateTime;
            dto.Description = entity.Description;
            dto.EndTime = entity.EndTime.ToString("yyyy-MM-dd HH:mm:ss");
            dto.EntryFee = entity.EntryFee;
            dto.Id = entity.Id;
            dto.Img = entity.Img;
            dto.StartTime = entity.StartTime.ToString("yyyy-MM-dd HH:mm:ss");
            dto.StatusName = entity.Status.Name;
            dto.Title = entity.Title;
            dto.UpToOne = entity.UpToOne;
            dto.EntryCount = entity.EntryCount;
            dto.VisitCount = entity.VisitCount;
            return dto;
        }
    }
}
