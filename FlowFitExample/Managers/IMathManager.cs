using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlowFitExample.Managers
{
    // This is obviously absurd - you wouldn't segregate such simple operations like this, but it's just for an example!
    public interface IMathManager
    {
        int Add(int a, int b);
        double SquareRoot(double a);
        long Factorial(int a);
    }
}
