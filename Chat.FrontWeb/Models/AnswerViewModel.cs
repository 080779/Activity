using Chat.DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.FrontWeb.Models
{
    public class AnswerViewModel
    {
        public string ActivityName { get; set; }
        public ExercisesDTO[] Exercises { get; set; }
        public string Name { get; set; }
        public string Moblie { get; set; }
    }
}