using System;
using MathNet.Numerics;
using MathNet.Numerics.Differentiation;
using Team76.PTA.MathFunctions;

namespace Team76.PTA.Models
{
    public class InfiniteHomogenousModel
    {
        /// <summary>
        /// Skin factor, [dimensionless]
        /// </summary>
        public double SkinFactor { get; set; }

        /// <summary>
        /// Dimensionless wellbore storage, [dimensionless]
        /// </summary>
        public double Cd => 0.8936 * C / (Porosity * Ct * H * Rw * Rw);

        /// <summary>
        /// Wellbore radius, [ft]
        /// </summary>
        public double Rw { get; set; }

        /// <summary>
        /// Porosity, [dimensionless]
        /// </summary>
        public double Porosity { get; set; }

        /// <summary>
        /// Net formation thickness, [ft]
        /// </summary>
        public double H { get; set; }

        /// <summary>
        /// Total compressibility, [1/psi]
        /// </summary>
        public double Ct { get; set; }

        /// <summary>
        /// Wellbore storage coefficient, [bbl/psi]
        /// </summary>
        public double C { get; set; }

        /// <summary>
        /// Viscosity, [cp]
        /// </summary>
        public double Mu { get; set; }

        /// <summary>
        /// Formation volume factor, [res vol/surface vol]
        /// </summary>
        public double B { get; set; }

        /// <summary>
        /// Matrix permeability, [md]
        /// </summary>
        public double K { get; set; }

        /// <summary>
        /// Original reservoir pressure, [psi]
        /// </summary>
        public double  Pi { get; set; }

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
            var p1 = SpecialFunctions.BesselK0(rz) + SkinFactor * rz * SpecialFunctions.BesselK1(rz);
            var p2 = s * (rz * SpecialFunctions.BesselK1(rz) + Cd * s * p1);
            return p1 / p2;
        }

        /// <summary>
        /// Dimensionless pressure as defined for constant-rate production
        /// </summary>
        /// <param name="p">pressure, psi</param>
        /// <returns></returns>
        public double DimensionlessPressure(double p) => K * H * (Pi - p) / (141.2 * Q * B * Mu);

        /// <summary>
        /// Dimensionless radius
        /// </summary>
        /// <param name="r">distance from the center of wellbore, ft</param>
        /// <returns></returns>
        public double DimensionlessRadius(double r) => r / Rw;

        /// <summary>
        /// Dimensionless time
        /// </summary>
        /// <param name="t">elapsed time, hours</param>
        /// <returns></returns>
        public double DimensionlessTime(double t)
        {
            return 0.0002637 * K * t / (Porosity * Mu * Ct * Rw * Rw);
        }
    }
}
