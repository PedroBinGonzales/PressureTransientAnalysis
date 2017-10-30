using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics;
using MathNet.Numerics.Differentiation;
using Team76.PTA.MathFunctions;
#pragma warning disable 1591

namespace Team76.PTA.Models
{
    /// <summary>
    /// Infinite Homogenous PTA Model
    /// </summary>
    public class InfiniteHomogenousPtaModel
    {
        private readonly Fluid _fluid;
        private readonly Well _well;
        private readonly Reservoir _reservoir;

        public InfiniteHomogenousPtaModel(Fluid fluid, Well well, Reservoir reservoir)
        {
            _fluid = fluid;
            _well = well;
            _reservoir = reservoir;
        }

        /// <summary>
        /// Pressure Drop, [psi]
        /// </summary>
        /// <param name="time">elapsed time, hours</param>
        /// <param name="q">flow rate at surface, [STB/D]</param>
        /// <returns></returns>
        public double PressureDrop(double time, double q)
        {
            var dt = Td(time);
            var pwd = Pwd(dt);
            var pressureDrop = FromDimensionlessPressureDrop(pwd, q);
            return pressureDrop;
        }

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
        private double Cd() => 0.8936 * _well.C / (_reservoir.Porosity * _reservoir.Ct * _reservoir.H * _well.Rw * _well.Rw);

        private double PwdRinLaplaceSpace(double s)
        {
            var rz = Math.Sqrt(s);
            var p1 = SpecialFunctions.BesselK0(rz) + _well.SkinFactor * rz * SpecialFunctions.BesselK1(rz);
            var p2 = s * (rz * SpecialFunctions.BesselK1(rz) + Cd() * s * p1);
            return p1 / p2;
        }

        /// <summary>
        /// Dimensionless wellbore pressure
        /// </summary>
        /// <param name="td">dimensionless time</param>
        /// <returns></returns>
        private double Pwd(double td)
        {
            return Laplace.InverseTransform(PwdRinLaplaceSpace, td);
        }

        /// <summary>
        /// Pressure drop, [psi]
        /// </summary>
        /// <param name="dp">dimensionless pressure drop</param>
        /// <param name="q">flow rate at surface, [STB/D]</param>
        /// <returns></returns>
        private double FromDimensionlessPressureDrop(double dp, double q)
        {
            return dp * (141.2 * q * _fluid.B * _fluid.Mu) / (_reservoir.K * _reservoir.H);
        }

        /// <summary>
        /// Dimensionless time
        /// </summary>
        /// <param name="t">elapsed time, hours</param>
        /// <returns></returns>
        private double Td(double t)
        {
            return 0.0002637 * _reservoir.K * t / (_reservoir.Porosity * _fluid.Mu * _reservoir.Ct * _well.Rw * _well.Rw);
        }
    }
}