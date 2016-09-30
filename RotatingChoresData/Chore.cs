using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotatingChoresData
{
    public class Chore
    {
        public int ChoreId { get; set; }

        public string Name { get; set; }

        public int Difficulty { get; set; }

        public string Description { get; set; }

        public virtual ICollection<string> Steps { get; set; }

        public DateTime? LastCompleted { get; set; }

        public virtual ChoreDoer LastCompletedBy { get; set; }
    }
}
