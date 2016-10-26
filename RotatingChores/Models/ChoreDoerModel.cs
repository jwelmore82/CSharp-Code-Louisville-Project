using Microsoft.AspNet.Identity;
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

        public ICollection<Chore> Chores { get; set; }

        public ChoreDoer CovertToDoer()
        {
            var doer = new ChoreDoer();
            doer.ChoreDoerId = ChoreDoerId;
            doer.Name = Name;
            doer.Email = Email;
            doer.MaxDifficulty = MaxDifficulty;
            return doer;
        }

        public static ChoreDoerModel ConvertFromDoer(ChoreDoer doer)
        {
            var model = new ChoreDoerModel();
            model.ChoreDoerId = doer.ChoreDoerId;
            model.Name = doer.Name;
            model.Email = doer.Email;
            model.MaxDifficulty = doer.MaxDifficulty;

            return model;
        }

        public override string ToString()
        {
            return $"{Name}({MaxDifficulty})";
        }
    }
}