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
        TrainDTO[] GetAll();
        TrainDTO GetById(long id);
    }
}
