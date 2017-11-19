using Chat.DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.IService.Interface
{
    public interface IActivityService:IServiceSupport
    {
        long AddNew(string name,string description, long statusId, string imgUrl,DateTime startTime,DateTime examEndTime,DateTime rewardTime,long paperId,string prizeName,string prizeImgUrl);
        ActivityDTO[] GetAll();
        ActivityDTO[] GetPageData(int pageSize, int currentIndex);
        ActivityDTO GetNew();
        ActivityDTO GetById(long id);
        ActivityDTO GetByStatus(string statusName);
        bool Update(long id, string name, string description, long statusId, string imgUrl, DateTime startTime, DateTime examEndTime, DateTime rewardTime, long paperId, string prizeName, string prizeImgUrl);
        ActivityDTO[] Search(long? statusId, DateTime? startTime, DateTime? endTime, string keyWord);
        bool Delete(long id);
        bool AddUserId(long activityId, long userId);
        ActivityDTO[] GetByUserId(long id);
        long GetTotalCount();
        bool CheckByPaperId(long id);
        bool UpdateCount(long id, bool setVisitCount, bool setForwardCount, bool setAnswerCount, bool setHavePrizeCount, bool setPrizeCount);
    }
}
