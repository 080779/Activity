using Chat.IService.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chat.DTO.DTO;

namespace Chat.Service.Service
{
    public class TrainService : ITrainService
    {
        public long AddNew(string Title, string Img, string Address, DateTime? startTime, DateTime? endTime, decimal entryFee, string upToOne, string description)
        {
            throw new NotImplementedException();
        }

        public TrainDTO[] GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
