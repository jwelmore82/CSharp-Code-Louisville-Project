using RotatingChoresData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RotatingChores.Models
{
    public class ChoreDoerModel
    {
        
        public int ChoreDoerId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public ChoreBase.DifficultyLevel MaxDifficulty { get; set; }

        public int GroupId { get; set; }

        public ICollection<Chore> Chores { get; set; }

        public static ChoreDoerModel ConvertFromDoer(ChoreDoer doer)
        {
            var model = new ChoreDoerModel();
            model.ChoreDoerId = doer.ChoreDoerId;
            model.Name = doer.Name;
            model.Email = doer.Email;
            model.MaxDifficulty = doer.MaxDifficulty;
            model.GroupId = doer.GroupId;

            return model;
        }
    }
}