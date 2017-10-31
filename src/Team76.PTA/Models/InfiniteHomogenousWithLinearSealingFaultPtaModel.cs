using System;
using MathNet.Numerics;
using Team76.PTA.MathFunctions;

namespace Team76.PTA.Models
{
    /// <summary>
    /// Infinite Homogenous PTA Model
    /// </summary>
    public class InfiniteHomogenousWithLinearSealingFaultPtaModel : PtaModelBase
    {
        private readonly double _l;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fluid"></param>
        /// <param name="well"></param>
        /// <param name="reservoir"></param>
        /// <param name="l">distance to linear sealing fault, [ft]</param>
        public InfiniteHomogenousWithLinearSealingFaultPtaModel(Fluid fluid, Well well, Reservoir reservoir, double l) : base(fluid, well, reservoir)
        {
            _l = l;
        }

        /// <inheritdoc />
        public override double PressureDrop(double time, double q)
        {
            var dt = Td(time);
            var pwd = Pwd(dt);
            var pressureDrop = FromDimensionlessPressureDrop(pwd, q);
            return pressureDrop;
        }

        private double PwdRinLaplaceSpace(double s)
        {
            var rz = Math.Sqrt(s);
            var p1 = SpecialFunctions.BesselK0(rz) + Well.SkinFactor * rz * SpecialFunctions.BesselK1(rz);
            var p2 = s * (rz * SpecialFunctions.BesselK1(rz) + Cd() * s * p1);
            return p1 / p2;
        }

        private double PwDbLinearSealingFault(double td)
        {
            var ld = Well.DimensionlessDistance(_l);
            return -0.5 * ExponentialIntegral.Evaluate(-ld * ld / td);
        }

        /// <summary>
        /// Dimensionless wellbore pressure
        /// </summary>
        /// <param name="td">dimensionless time</param>
        /// <returns></returns>
        private double Pwd(double td)
        {
            return Laplace.InverseTransform(PwdRinLaplaceSpace, td) + PwDbLinearSealingFault(td);
        }
    }
}