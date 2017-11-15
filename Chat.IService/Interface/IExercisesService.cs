﻿using Chat.DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.IService.Interface
{
    public interface IExercisesService:IServiceSupport
    {
        long AddNew(string title,long testPaperId,string optionA,string optionB,string optionC,string optionD,long rightKeyId,int point=0);
        long GetPaperExercisesCount(long testPaperId);
        ExercisesDTO GetById(long id);
        ExercisesDTO[] GetExercisesByPaperId(long testPaperId);
        bool DelExercisesById(long id);
    }
}
