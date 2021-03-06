﻿using Chat.DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.IService.Interface
{
    public interface IUserService:IServiceSupport
    {
        long AddNew(string name, long actId, string photoUrl,string mobile,bool gender,string address);
        UserDTO[] GetAll();
        UserSearchResult Search(bool? gender,bool? isWon,DateTime? startTime,DateTime? endTime,string keyWord, int currentIndex, int pageSize);
        UserDTO[] GetByActivityId(long id);
        UserSearchResult GetUsersByActivityId(long id, int currentIndex, int pageSize);
        UserSearchResult GetUsersByActivityId(long id, DateTime? startTime, DateTime? endTime, string keyWord, int currentIndex, int pageSize);
        //UserSearchResult PrizeSearch(long id, DateTime? startTime, DateTime? endTime, string keyWord, int currentIndex, int pageSize);
        UserDTO[] PrizeSearch1(long id, DateTime? startTime, DateTime? endTime, string keyWord, int currentIndex, int pageSize);
        bool SetWon(long id, long activityId);
        bool ReSetWon(long id, long activityId);
        bool RandSetWon(int count, long actId);
        UserSearchResult  GetByActivityIdHavePrize(long id, DateTime? startTime, DateTime? endTime, string keyWord, int currentIndex, int pageSize);
        UserDTO[] GetByActivityIdHavePrize1(long id);
        UserDTO[] GetByActivityIdIsWon1(long id);
        UserSearchResult GetByActivityIdIsWon(long id, int currentIndex, int pageSize);
        UserSearchResult GetByActivityIdIsWon(long id, int? currentIndex, int? pageSize);
        UserSearchResult SearchIsWon(long activityId,string lastM, int currentIndex, int pageSize);
        bool UserIsWonByMobile(string mobile);
        bool UpdateUser(string mobile, string name, bool gender, string address);
        bool RetSetWon(long id);
        bool IsHavePrizeChance(long id);
        bool ReSetPrizeChance(long id);
        bool Del(long id);
    }
    public class UserSearchResult
    {
        public UserDTO[] Users { get; set; }
        public long TotalCount { get; set; }
        public int WinCount { get; set; }
    }
}
