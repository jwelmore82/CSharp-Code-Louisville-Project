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
        
        public int? ChoreDoerId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public ChoreBase.DifficultyLevel? MaxDifficulty { get; set; }

        public ICollection<ChoreModel> Chores { get; set; }

        public void UpdateDoer(ChoreDoer doer)
        {         
            doer.Name = Name;
            doer.Email = Email;
            doer.MaxDifficulty =(ChoreBase.DifficultyLevel) MaxDifficulty;
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

        public void AddChoresList(ChoreDoer doer, Group group)
        {
            var choreModels = new List<ChoreModel>();

            var chores = group.Chores.Where(c => c.AssignedTo == doer);
            if (chores.Count() > 0)
            {
                foreach (var chore in chores)
                {                 
                        choreModels.Add(ChoreModel.ConvertFromChore(chore));                    
                }
            }

            Chores = choreModels;
        }

        public ChoreDoer GetRepresentedDoer(RotatingChoresContext context)
        {
            var doer = context.ChoreDoers.SingleOrDefault(d => d.ChoreDoerId == ChoreDoerId);
            return doer;
        }
    }
}