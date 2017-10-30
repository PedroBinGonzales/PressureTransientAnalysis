using System;
using MathNet.Numerics.Differentiation;

namespace Team76.PTA.MathFunctions
{
    public static class Helpers
    {
        public static double PressureDropDerivative(Func<double,double> pressureDropFunc, double t)
        {
            var nd = new NumericalDerivative();
            var dP = nd.EvaluateDerivative(pressureDropFunc, t, 1);
            return dP * t;
        }
    }
}
