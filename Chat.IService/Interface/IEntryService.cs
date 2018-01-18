using Chat.DTO.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.IService.Interface
{
    public interface IEntryService: IServiceSupport
    {
        long Add(EntryDTO dto);
        long ImportAdd(EntryImportDTO dto);
        bool EntryImport(long trainId,long cityId,long entryChannelId,DataTable dt);
        EntryListDTO[] GetAll();
        EntryListDTO[] GetByTrainId(long id);
    }
}
