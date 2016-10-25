using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotatingChoresData
{
    public abstract class ChoreBase
    {
        public enum DifficultyLevel
        {
            None = 0,
            VeryEasy = 1,
            Easy = 2,
            Moderate = 3,
            Difficult = 4,
            VeryDifficult = 5
        }
    }
}
