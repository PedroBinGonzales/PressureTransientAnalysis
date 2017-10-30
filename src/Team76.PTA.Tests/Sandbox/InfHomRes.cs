using System;
using CuttingEdge.Conditions;

namespace Team76.PTA.Tests.Sandbox
{
    public class InfHomRes
    {
        /// <summary>
        ///     The calculation Of the dimensionless wellbore pressure drop  with skin for homogeneous reservoir.
        /// </summary>
        /// <param name="td">dimensionless time</param>
        /// <param name="cd">dimensionless wellbore storage</param>
        /// <param name="skinFactor">skin factor</param>
        /// <returns>System.Double.</returns>
        public static double PwdR(double td, double cd, double skinFactor)
        {

            Condition.Requires(td, nameof(td)).IsGreaterOrEqual(0.0);
            Condition.Requires(cd, nameof(cd)).IsGreaterOrEqual(0.0);

            var n = 14;

            double pwdR = 0;

            for (int i = 1; i <= n; i++)
            {
                double u = i * Math.Log(2.0) / td;

                double sru = Math.Sqrt(u);

                double p1 = MathNet.Numerics.SpecialFunctions.BesselK0(sru)
                            + skinFactor * sru * MathNet.Numerics.SpecialFunctions.BesselK1(sru);

                double p2 = u * sru * MathNet.Numerics.SpecialFunctions.BesselK1(sru)
                            + cd * Math.Pow(u, 2) * MathNet.Numerics.SpecialFunctions.BesselK0(sru)
                            + cd * Math.Pow(u, 2) * skinFactor * MathNet.Numerics.SpecialFunctions.BesselK1(sru) * sru;

                pwdR = StehfestCoefficients.Vi(i, n) * p1 / p2 + pwdR;
            }
            pwdR = (Math.Log(2) / td) * pwdR;

            return pwdR;
        }
    }
}