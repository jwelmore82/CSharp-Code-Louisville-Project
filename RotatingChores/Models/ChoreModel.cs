using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RotatingChoresData;
using System.ComponentModel.DataAnnotations;
using static RotatingChoresData.ChoreBase;

namespace RotatingChores.Models
{
    public class ChoreModel
    {
        
        public int? ChoreId { get; set; }

        public string Name { get; set; }

        public DifficultyLevel Difficulty { get; set; }

        public string Description { get; set; }

        public DateTime? LastCompleted { get; set; }

        public ChoreDoer LastCompletedBy { get; set; }

        public ChoreDoer AssignedTo { get; set; }

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
                model.LastCompletedBy = chore.LastCompletedBy;
            }

            model.AssignedTo = chore.AssignedTo;
            

            return model;
        }

        public Chore ConvertToChore()
        {
            var chore = new Chore();
            this.ChoreId = chore.ChoreId;
            this.Description = chore.Description;
            this.Name = chore.Name;
            this.Difficulty = chore.Difficulty;
            this.AssignedTo = chore.AssignedTo;
            this.LastCompleted = chore.LastCompleted;
            this.LastCompletedBy = chore.LastCompletedBy;
    
            return chore;
        }

    }

}