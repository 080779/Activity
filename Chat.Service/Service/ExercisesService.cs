using Chat.IService.Interface;
using Chat.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Service.Service
{
    public class ExercisesService : IExercisesService
    {
        public long AddNew(string title, long testPaperId, string optionA, string optionB, string optionC, string optionD, long rightKeyId, int point = 0)
        {
            using (MyDbContext dbc = new MyDbContext())
            {
                CommonService<ExercisesEntity> cs = new CommonService<ExercisesEntity>(dbc);
                if(cs.GetAll().Any(e => e.Title == title))
                {
                    return -1;
                }
                ExercisesEntity exercises = new ExercisesEntity();
                exercises.Title = title;
                exercises.TestPaperId = testPaperId;
                exercises.OptionA = optionA;
                exercises.OptionB = optionB;
                exercises.OptionC = optionC;
                exercises.OptionD = optionD;
                exercises.RightKeyId = rightKeyId;
                exercises.Point = point;
                dbc.Exercises.Add(exercises);
                dbc.SaveChanges();
                return exercises.Id;
            }
        }
    }
}
