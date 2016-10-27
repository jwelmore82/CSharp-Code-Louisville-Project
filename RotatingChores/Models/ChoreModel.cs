using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RotatingChoresData;
using RotatingChores.Extensions;

namespace RotatingChores.Models
{
    public class ChoreModel
    {
        
        public int? ChoreId { get; set; }

        public string Name { get; set; }

        public ChoreBase.DifficultyLevel? Difficulty { get; set; }

        public string Description { get; set; }

        public DateTime? LastCompleted { get; set; }

        public int? LastCompletedById { get; set; }

        public int AssignedToId { get; set; }

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
            }

            model.AssignedToId = chore.AssignedTo.ChoreDoerId;
            

            return model;
        }

        public Chore ConvertToChore(RotatingChoresContext context)
        {
            Chore chore;
            
            //ChoreId on ChoreModel is of type int?, not set until value recieved from database
                
            if (ChoreId != null)
            {
                chore = context.Chores.SingleOrDefault(c => c.ChoreId == ChoreId.Value);
                return chore;
            }
           
            //If no reference to a database object, create a new Chore
            chore = new Chore();
                    
            
            //TODO Move this block of code to an Edit controller, it will never get hit here.    
            //if (LastCompletedById != null)
            //{
            //    chore.LastCompleted = LastCompleted;
            //    ChoreDoer last = context.ChoreDoers.SingleOrDefault(c => c.ChoreDoerId == LastCompletedById);
            //    chore.LastCompletedBy = last;
            //}
            ChoreDoer assignedTo = context.ChoreDoers.SingleOrDefault(c => c.ChoreDoerId == AssignedToId); 
            chore.AssignedTo = assignedTo;
            
            
            chore.Description = Description;
            chore.Name = Name;
            chore.Difficulty =(ChoreBase.DifficultyLevel) Difficulty; 


            
            
    
            return chore;
        }
    }

}