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
        bool Update(EntryDTO dto);
        bool Delete(long id);
        long ImportAdd(EntryImportDTO dto);
        bool EntryImport(long trainId,long cityId,long entryChannelId,DataTable dt);
        bool IsJoinined(long trainId,string mobile);
        EntryListDTO[] GetAll();
        EntryListDTO[] GetByTrainId(long id);
        EntrySearchResult GetPageByTrainId(EntryGetPageDTO dto);
        EntryListDTO[] GetByTrainIdCityId(long id, long cityId);
        EntryListDTO GetById(long id);
        EntryDTO GetByEntryId(long id);
    }

    public class EntrySearchResult
    {
        public EntryListDTO[] Entries { get; set; }
        public long TotalCount { get; set; }
    }
}
