using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RotatingChoresData;
using RotatingChores.Extensions;
using System.ComponentModel.DataAnnotations;

namespace RotatingChores.Models
{
    public class ChoreModel
    {
        
        public int? ChoreId { get; set; }

        public string Name { get; set; }

        public ChoreBase.DifficultyLevel? Difficulty { get; set; }

        public string Description { get; set; }
        [Display(Name = "Last Completed On")]
        public DateTime? LastCompleted { get; set; }

        [Display(Name = "Last Completed By")]
        public int? LastCompletedById { get; set; }
        [Display(Name = "Last Completed By")]
        public string LastCompletedBy { get; set;
        }
        [Display(Name = "Assigned To")]
        public int AssignedToId { get; set; }

        [Display(Name = "Assigned To")]
        public string AssignedTo { get; set; }

        public static ChoreModel ConvertFromChore(Chore chore)
        {
            var model = new ChoreModel();
            model.ChoreId = chore.ChoreId;
            model.Name = chore.Name;
            model.Difficulty = chore.Difficulty;
            model.Description = chore.Description;

            
            if (chore.LastCompleted != null)
            {
                model.LastCompleted = chore.LastCompleted;
                model.LastCompletedById = chore.LastCompletedBy.ChoreDoerId;
                model.LastCompletedBy = chore.LastCompletedBy.Name;
            }
            ChoreDoer doer = chore.AssignedTo;
            model.AssignedToId = doer.ChoreDoerId;
            model.AssignedTo = doer.Name;

            return model;
        }

        public void UpdateChore(RotatingChoresContext context, Chore chore)
        {            
            ChoreDoer assignedTo = context.ChoreDoers.SingleOrDefault(c => c.ChoreDoerId == AssignedToId);
            chore.AssignedTo = assignedTo;                        
            chore.Description = Description;
            chore.Name = Name;
            chore.Difficulty =(ChoreBase.DifficultyLevel) Difficulty;

            if (LastCompletedById != null)
            {
                chore.LastCompleted = LastCompleted;
                ChoreDoer last = context.ChoreDoers.SingleOrDefault(c => c.ChoreDoerId == LastCompletedById);
                chore.LastCompletedBy = last;
            }
        }

        public Chore GetRepresentedChore(RotatingChoresContext context)
        {
            Chore chore = context.Chores.SingleOrDefault(c => c.ChoreId == ChoreId);
            
            return chore;
        }
    }

}