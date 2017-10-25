using System;
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


        /// <summary>
        /// Flow rate at surface, [STB/D]
        /// </summary>
        public double Q { get; set; }


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
        /// <returns></returns>
        public double DimensionlessPressure(double p)
        {
            return Reservoir.K * Reservoir.H * (Reservoir.Pi - p) / (141.2 * Q * Fluid.B * Fluid.Mu);
        }

        public double FromDimensionlessPressure(double dp)
        {
            return Reservoir.Pi - dp * (141.2 * Q * Fluid.B * Fluid.Mu) / (Reservoir.K * Reservoir.H);
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
            return dt * (Reservoir.Porosity * Fluid.Mu * Reservoir.Ct * Well.Rw * Well.Rw) /( 0.0002637 * Reservoir.K);
        }


        public double Pressure(double time)
        {
            var dt = DimensionlessTime(time);
            var pwd = Pwd(dt);
            var pw = FromDimensionlessPressure(pwd);
            return pw;
        }
    }
}