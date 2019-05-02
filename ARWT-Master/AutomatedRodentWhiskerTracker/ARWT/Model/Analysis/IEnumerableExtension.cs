using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARWT.Model.Analysis
{
    public static class IEnumerableExtension
    {
        public static double Mean(this IEnumerable<double> list)
        {
            int counter = 0;
            double total = 0;

            foreach (double value in list)
            {
                counter++;
                total += value;
            }

            return total/counter;
        }
    }
}
