using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RotatingChoresData
{
    public class Chore : ChoreBase
    {
        public int ChoreId { get; set; }

        public string Name { get; set; }

        public int GroupId { get; set; }

        public DifficultyLevel Difficulty { get; set; }

        public string Description { get; set; }

       
        public virtual ChoreDoer AssignedTo { get; set; }

        public DateTime? LastCompleted { get; set; }

        public virtual ChoreDoer LastCompletedBy { get; set; }

        

       
    }
}
