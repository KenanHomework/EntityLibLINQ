using EntityLibLINQ.DbClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibLINQ.Comparers
{
    public class IdBookComparer : IComparer<SCard>
    {
        public int Compare(SCard? x, SCard? y)
        {
            int countX = Program.NumberOfTakedBookByStudent(x.IdBook);
            int countY = Program.NumberOfTakedBookByStudent(y.IdBook);
            return countX.CompareTo(countY);
        }
    }
}
