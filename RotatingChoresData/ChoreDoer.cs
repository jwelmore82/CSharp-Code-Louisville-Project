using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotatingChoresData
{
    public class ChoreDoer : ChoreBase
    {
        public int ChoreDoerId { get; set; }

        public string Name { get; set; }


        [MaxLength(120)]
        [Index(IsUnique = true)]
        public string Email { get; set; }

        public DifficultyLevel MaxDifficulty { get; set; }

        public int GroupId { get; set; }

    }
}
