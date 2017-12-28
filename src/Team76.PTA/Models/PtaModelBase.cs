using System;
using System.Collections.Generic;
using System.Linq;
using CuttingEdge.Conditions;
using MathNet.Numerics.Differentiation;

namespace Team76.PTA.Models
{
    /// <summary>
    /// Base PTA model
    /// </summary>
    public abstract class PtaModelBase
    {
        /// <summary>
        /// Fluid
        /// </summary>
        protected readonly Fluid Fluid;

        /// <summary>
        /// Well
        /// </summary>
        protected readonly Well Well;

        /// <summary>
        /// Reservoir       
        ///  </summary>
        protected readonly Reservoir Reservoir;

        /// <summary>
        /// PTA model base class
        /// </summary>
        /// <param name="fluid">Fluid model</param>
        /// <param name="well">Well model</param>
        /// <param name="reservoir">Reservoir model</param>
        protected PtaModelBase(Fluid fluid, Well well, Reservoir reservoir)
        {
            Fluid = fluid;
            Well = well;
            Reservoir = reservoir;
        }


        /// <summary>
        /// Pressure Drop, [psi]
        /// </summary>
        /// <param name="time">elapsed time, [hours]</param>
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
            Condition.Requires(time, nameof(time)).IsGreaterOrEqual(0);
            if (time == 0) return 0;


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
        /// PTA derivative: dP * time
        /// </summary>
        /// <param name="time">elapsed time, hours</param>
        /// <param name="q">flow rate at surface, [STB/D]</param>
        /// <returns></returns>
        public double PressureDropDerivative(double time, double q)
        {
            var nd = new NumericalDerivative();

            if (time > 0.01)
            {
                nd.StepType = StepType.Relative;
                nd.Epsilon = 1e-6;
            }

            var dP = nd.EvaluateDerivative(x => PressureDrop(x, q), time, 1);
            return dP * time;
        }

        /// <summary>
        /// Dimensionless wellbore storage, [dimensionless]
        /// </summary>
        protected double Cd() => 0.8936 * Well.C / (Reservoir.Porosity * Reservoir.Ct * Reservoir.H * Well.Rw * Well.Rw);

        /// <summary>
        /// Dimensionless time
        /// </summary>
        /// <param name="time">elapsed time, hours</param>
        /// <returns></returns>
        protected double Td(double time)
        {
            return 0.0002637 * Reservoir.K * time / (Reservoir.Porosity * Fluid.Ul * Reservoir.Ct * Well.Rw * Well.Rw);
        }

        /// <summary>
        /// Pressure drop, [psi]
        /// </summary>
        /// <param name="dp">dimensionless pressure drop</param>
        /// <param name="q">flow rate at surface, [STB/D]</param>
        /// <returns></returns>
        protected double FromDimensionlessPressureDrop(double dp, double q)
        {
            return dp * (141.2 * q * Fluid.B * Fluid.Ul) / (Reservoir.K * Reservoir.H);
        }
    }
}