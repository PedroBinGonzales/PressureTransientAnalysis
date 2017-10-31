using System;
using MathNet.Numerics;
using Team76.PTA.MathFunctions;
using Team76.PTA.Models;

#pragma warning disable 1591

namespace Team76.PTA.PtaModels
{
    /// <summary>
    /// Infinite Homogenous PTA Model
    /// </summary>
    public class InfHomPtaModel : PtaModelBase
    {
        public InfHomPtaModel(Fluid fluid, Well well, Reservoir reservoir): base(fluid,well,reservoir)
        {

        }

        public override double PressureDrop(double time, double q)
        {
            var dt = Td(time);
            var pwd = Pwd(dt);
            var pressureDrop = FromDimensionlessPressureDrop(pwd, q);
            return pressureDrop;
        }

        protected double PwdRinLaplaceSpace(double s)
        {
            var rz = Math.Sqrt(s);
            var p1 = SpecialFunctions.BesselK0(rz) + Well.SkinFactor * rz * SpecialFunctions.BesselK1(rz);
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
    }
}