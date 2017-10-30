using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Differentiation;

namespace Team76.PTA.Models
{
    public abstract class PtaModelBase
    {

        protected readonly Fluid Fluid;
        protected readonly Well Well;
        protected readonly Reservoir Reservoir;

        protected PtaModelBase(Fluid fluid, Well well, Reservoir reservoir)
        {
            Fluid = fluid;
            Well = well;
            Reservoir = reservoir;
        }
        
        
        /// <summary>
        /// Pressure Drop, [psi]
        /// </summary>
        /// <param name="time">elapsed time, hours</param>
        /// <param name="q">flow rate at surface, [STB/D]</param>
        /// <returns></returns>
        public abstract double PressureDrop(double time, double q);

        /// <summary>
        /// Pressure Drop, [psi]
        /// </summary>
        /// <param name="time">elapsed time, hours</param>
        /// <param name="data">Series of Time [hr]  vs Flow rates [STB/D] points</param>
        /// <returns></returns>
        public double PressureDrop(double time, Tuple<double, double>[] data)
        {
            var p = 0.0;

            var times = data.Where(x => x.Item1 < time).Select(x => x.Item1).ToList();
            var rates = data.Where(x => x.Item1 < time).Select(x => x.Item2).ToList();
            var n = times.Count;

            var stepRates = new List<double>();
            var stepTimes = new List<double>();

            for (int i = 0; i < n; i++)
            {
                stepRates.Add(i == 0 ? rates[i] : rates[i] - rates[i - 1]);
            }

            for (int i = 0; i < n; i++)
            {
                stepTimes.Add(i == 0 ? time : time - times[i]);
            }

            for (int i = 0; i < n; i++)
            {
                p = p + PressureDrop(stepTimes[i], stepRates[i]);
            }

            return p;
        }

        /// <summary>
        /// Dimensionless wellbore storage, [dimensionless]
        /// </summary>
        protected double Cd() => 0.8936 * Well.C / (Reservoir.Porosity * Reservoir.Ct * Reservoir.H * Well.Rw * Well.Rw);

        /// <summary>
        /// Dimensionless time
        /// </summary>
        /// <param name="t">elapsed time, hours</param>
        /// <returns></returns>
        protected double Td(double t)
        {
            return 0.0002637 * Reservoir.K * t / (Reservoir.Porosity * Fluid.Mu * Reservoir.Ct * Well.Rw * Well.Rw);
        }

        /// <summary>
        /// Pressure drop, [psi]
        /// </summary>
        /// <param name="dp">dimensionless pressure drop</param>
        /// <param name="q">flow rate at surface, [STB/D]</param>
        /// <returns></returns>
        protected double FromDimensionlessPressureDrop(double dp, double q)
        {
            return dp * (141.2 * q * Fluid.B * Fluid.Mu) / (Reservoir.K * Reservoir.H);
        }

        public double PressureDropDerivative(double time, double q)
        {
            var nd = new NumericalDerivative();
            var dP = nd.EvaluateDerivative(x=>PressureDrop(x,q), time, 1);
            return dP * time;
        }

    }
}