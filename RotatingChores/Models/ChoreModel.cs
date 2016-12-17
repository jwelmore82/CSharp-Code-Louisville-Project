using System;
using System.Linq;
using RotatingChoresData;
using System.ComponentModel.DataAnnotations;

namespace RotatingChores.Models
{
    public class ChoreModel
    {
        
        public int? ChoreId { get; set; }

        public string Name { get; set; }

        public ChoreBase.DifficultyLevel? Difficulty { get; set; }

        public string Description { get; set; }
        [Display(Name = "Last Completed")]
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


        //This method uses the ChoreModel to update the given chore in the given context.
        //Be sure to call SaveChanges() after updating.
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

        //Uses the ChoreId of the model to get the represented data entity Chore.
        public Chore GetRepresentedChore(RotatingChoresContext context)
        {
            Chore chore = context.Chores.SingleOrDefault(c => c.ChoreId == ChoreId);
            
            return chore;
        }

        public void MarkComplete()
        {
            LastCompleted = DateTime.Now;
            LastCompletedById = AssignedToId;
            LastCompletedBy = AssignedTo;            
        }

        public void AdvanceChore(Chore chore, Group group)
        {
            //Gets a list of ChoreDoers this chore can be assigned to.
            var membersAvailable = group.Members.Where(c => c.MaxDifficulty >= chore.Difficulty).ToList();
            //If there is more than one ChoreDoer in the list                     
            if (membersAvailable.Count > 1)
            {
                var position = membersAvailable.IndexOf(chore.AssignedTo);
                //If the ChoreDoer is the last in the list assign the chore to the first person in the list.
                if (position == membersAvailable.Count - 1)
                {
                    chore.AssignedTo = membersAvailable[0];
                }
                //If not assign the chore to the next person in the list.
                else
                {
                    chore.AssignedTo = membersAvailable[position + 1];
                }
            }
            else
            {
                //If only one person in the list they must be assigned the chore.  
                chore.AssignedTo = membersAvailable[0];
            }
            

        }
    }

}