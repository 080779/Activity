using Chat.DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.IService.Interface
{
    public interface ITrainService: IServiceSupport
    {
        long AddNew(string title, string img, string address, DateTime? startTime, DateTime? endTime, decimal entryFee, long upToOne, string description);
        bool Update(long id, string title, string img, string address, DateTime? startTime, DateTime? endTime, decimal entryFee, long upToOne, string description);
        bool Delete(long id);
        TrainDTO[] GetAll();
        TrainSearchResult Search(long? statusId,DateTime? startTime,DateTime? endTime,string keyWord,int currentIndex,int pageSize);
        TrainDTO GetById(long id);
    }
    public class TrainSearchResult
    {
        public TrainDTO[] Trains { get; set; }
        public int TotalCount { get; set; }
    }
}
