using System;
using CuttingEdge.Conditions;
using MathNet.Numerics;
using MathNet.Numerics.Differentiation;

namespace Team76.PTA.MathFunctions
{
    /// <summary>
    /// Infinite Homogenous Reservoir
    /// </summary>
    public static class InfiniteHomogenousReservoir
    {
        /// <summary>
        ///     The expression of dimensionless line source solution pressure of infinite homogeneous reservoir.
        /// </summary>
        /// <param name="td">dimensionless time</param>
        /// <param name="rd">dimensionless radius</param>
        /// <returns>System.Double.</returns>
        public static double Pd(double td, double rd)
        {
            Condition.Requires(td, nameof(td)).IsGreaterOrEqual(0.0);
            Condition.Requires(rd, nameof(rd)).IsGreaterOrEqual(0.0);

            return -0.5 * ExponentialIntegral.Evaluate(-rd * rd / (4 * td));
        }

        /// <summary>
        ///     The calculation Of the dimensionless wellbore pressure drop  with skin for homogeneous reservoir.
        /// </summary>
        /// <param name="td">dimensionless time</param>
        /// <param name="cd">dimensionless wellbore storage</param>
        /// <param name="skinFactor">skin factor</param>
        /// <returns></returns>
        public static double PwdR(double td, double cd, double skinFactor)
        {
            Condition.Requires(td, nameof(td)).IsGreaterOrEqual(0.0);
            Condition.Requires(cd, nameof(cd)).IsGreaterOrEqual(0.0);

            return Laplace.InverseTransform((x) => PwdRinLaplaceSpace(x, cd, skinFactor), td);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="td"></param>
        /// <param name="cd"></param>
        /// <param name="skinFactor"></param>
        /// <returns></returns>
        public static double PwdRDerivative(double td, double cd, double skinFactor)
        {
            Condition.Requires(td, nameof(td)).IsGreaterOrEqual(0.0);
            Condition.Requires(cd, nameof(cd)).IsGreaterOrEqual(0.0);

            var nd = new NumericalDerivative();
            var d = nd.EvaluateDerivative(c => PwdR(c, cd, skinFactor), td, 1);
            return d * td;
        }

        private static double PwdRinLaplaceSpace(double s, double cd, double skinFactor)
        {
            var rz = Math.Sqrt(s);
            var p1 = SpecialFunctions.BesselK0(rz) + skinFactor * rz * SpecialFunctions.BesselK1(rz);
            var p2 = s * (rz * SpecialFunctions.BesselK1(rz) + cd * s * p1);
            return p1 / p2;
        }
    }
}
