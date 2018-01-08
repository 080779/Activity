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
        long AddNew(string Title, string Img, string Address, DateTime? startTime, DateTime? endTime, decimal entryFee, string upToOne, string description);
        TrainDTO[] GetAll();
    }
}
