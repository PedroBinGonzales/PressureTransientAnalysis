using Team76.PTA.MathFunctions;
using Team76.PTA.Models;

namespace Team76.PTA.PtaModels
{
    /// <summary>
    /// Infinite Homogenous With Perpendicular Sealing Faults PTA Model
    /// </summary>
    public class InfHomWithPerpendicularSealingFaultsPtaModel : InfHomPtaModel
    {
        private readonly double _l1;
        private readonly double _l2;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fluid"></param>
        /// <param name="well"></param>
        /// <param name="reservoir"></param>
        /// <param name="l1">distance to linear sealing fault 1, [ft]</param>
        /// <param name="l2">distance to linear sealing fault 2, [ft]</param>
        public InfHomWithPerpendicularSealingFaultsPtaModel(Fluid fluid, Well well, Reservoir reservoir, double l1, double l2) : base(fluid, well, reservoir)
        {
            _l1 = l1;
            _l2 = l2;
        }

        /// <inheritdoc />
        public override double PressureDrop(double time, double q)
        {
            var dt = Td(time);
            var pwd = Pwd(dt);
            var pressureDrop = FromDimensionlessPressureDrop(pwd, q);
            return pressureDrop;
        }

        private double PwDbPerpendicularSealingFaults(double td)
        {
            var ld1 = Well.DimensionlessDistance(_l1);
            var ld2 = Well.DimensionlessDistance(_l2);
            return -0.5 * (
                       +ExponentialIntegral.Evaluate(-ld1 * ld1 / td)
                       + ExponentialIntegral.Evaluate(-ld2 * ld2 / td)
                       + ExponentialIntegral.Evaluate(-(ld1 * ld1 + ld2 * ld2) / td)
                   );
        }


        /// <summary>
        /// Dimensionless wellbore pressure
        /// </summary>
        /// <param name="td">dimensionless time</param>
        /// <returns></returns>
        private double Pwd(double td)
        {
            return Laplace.InverseTransform(PwdRinLaplaceSpace, td) + PwDbPerpendicularSealingFaults(td);
        }
    }
}