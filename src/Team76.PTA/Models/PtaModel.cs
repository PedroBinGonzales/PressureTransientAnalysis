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
    /// Mock pta model for inf hom reservoir
    /// </summary>
    public class PtaModel
    {
        public Fluid Fluid { get; set; }
        public Well Well { get; set; }
        public Reservoir Reservoir { get; set; }


        /// <summary>
        /// Dimensionless wellbore storage, [dimensionless]
        /// </summary>
        public double Cd => 0.8936 * Well.C / (Reservoir.Porosity * Reservoir.Ct * Reservoir.H * Well.Rw * Well.Rw);


        ///// <summary>
        ///// Flow rate at surface, [STB/D]
        ///// </summary>
        //public double Q { get; set; }


        /// <summary>
        /// Dimensionless wellbore pressure
        /// </summary>
        /// <param name="td">dimensionless time</param>
        /// <returns></returns>
        public double Pwd(double td)
        {
            return Laplace.InverseTransform(Fs_Pwd, td);
        }

        /// <summary>
        /// Dimensionless wellbore pressure derivative
        /// </summary>
        /// <param name="td"></param>
        /// <returns></returns>
        public double PwdDerivative(double td)
        {
            var nd = new NumericalDerivative();
            var d = nd.EvaluateDerivative(Pwd, td, 1);
            return d * td;
        }

        private double Fs_Pwd(double s)
        {
            var rz = Math.Sqrt(s);
            var p1 = SpecialFunctions.BesselK0(rz) + Well.SkinFactor * rz * SpecialFunctions.BesselK1(rz);
            var p2 = s * (rz * SpecialFunctions.BesselK1(rz) + Cd * s * p1);
            return p1 / p2;
        }

        /// <summary>
        /// Dimensionless pressure as defined for constant-rate production
        /// </summary>
        /// <param name="p">pressure, psi</param>
        /// <param name="q">flow rate at surface, [STB/D]</param>
        /// <returns></returns>
        public double DimensionlessPressure(double p, double q)
        {
            return Reservoir.K * Reservoir.H * (Reservoir.Pi - p) / (141.2 * q * Fluid.B * Fluid.Mu);
        }

        /// <summary>
        /// Dimensionless pressure as defined for constant-rate production
        /// </summary>
        /// <param name="p">pressure, psi</param>
        /// <param name="q">flow rate at surface, [STB/D]</param>
        /// <returns></returns>
        public double DimensionlessPressureDrop(double p, double q)
        {
            return Reservoir.K * Reservoir.H * (0 - p) / (141.2 * q * Fluid.B * Fluid.Mu);
        }




        public double FromDimensionlessPressure(double dp, double q)
        {
            return Reservoir.Pi - dp * (141.2 * q * Fluid.B * Fluid.Mu) / (Reservoir.K * Reservoir.H);
        }

        public double FromDimensionlessPressureDrop(double dp, double q)
        {
            return 0 - dp * (141.2 * q * Fluid.B * Fluid.Mu) / (Reservoir.K * Reservoir.H);
        }

        /// <summary>
        /// Dimensionless time
        /// </summary>
        /// <param name="t">elapsed time, hours</param>
        /// <returns></returns>
        public double DimensionlessTime(double t)
        {
            return 0.0002637 * Reservoir.K * t / (Reservoir.Porosity * Fluid.Mu * Reservoir.Ct * Well.Rw * Well.Rw);
        }

        public double FromDimensionlessTime(double dt)
        {
            return dt * (Reservoir.Porosity * Fluid.Mu * Reservoir.Ct * Well.Rw * Well.Rw) / (0.0002637 * Reservoir.K);
        }


        public double Pressure(double time, double q)
        {
            var dt = DimensionlessTime(time);
            var pwd = Pwd(dt);
            var pw = FromDimensionlessPressure(pwd, q);
            return pw;
        }

        public double PressureDrop(double time, double q)
        {
            var dt = DimensionlessTime(time);
            var pwd = Pwd(dt);
            var pw = FromDimensionlessPressureDrop(pwd, q);
            return pw;
        }

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
    }
}