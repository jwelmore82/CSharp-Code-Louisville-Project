using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RotatingChoresData
{
    public class Chore
    {
        public int ChoreId { get; set; }

        public string Name { get; set; }

        public int GroupId { get; set; }

        public int Difficulty { get; set; }

        public string Description { get; set; }

       
        public ChoreDoer AssignedTo { get; set; }

        public DateTime? LastCompleted { get; set; }

        //[ForeignKey("LastCompletedById")]
        //public virtual ChoreDoer LastCompletedByDoer { get; set; }

        //public int? LastCompletedById { get; set; }

        public ChoreDoer LastCompletedBy { get; set; }

        

       
    }
}
