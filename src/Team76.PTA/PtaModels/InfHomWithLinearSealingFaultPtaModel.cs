using Team76.PTA.MathFunctions;
using Team76.PTA.Models;

namespace Team76.PTA.PtaModels
{
    /// <summary>
    /// Infinite Homogenous With Linear Sealing Fault PTA Model
    /// </summary>
    public class InfHomWithLinearSealingFaultPtaModel : InfHomPtaModel
    {
        private readonly double _l;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fluid"></param>
        /// <param name="well"></param>
        /// <param name="reservoir"></param>
        /// <param name="l">distance to linear sealing fault, [ft]</param>
        public InfHomWithLinearSealingFaultPtaModel(Fluid fluid, Well well, Reservoir reservoir, double l) : base(fluid, well, reservoir)
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