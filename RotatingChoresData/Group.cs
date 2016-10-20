using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotatingChoresData
{
    public class Group
    {
        public int GroupId { get; set; }

        public ICollection<ChoreDoer> Members { get; set; }

        public ICollection<Chore> Chores { get; set; }
    }
}
