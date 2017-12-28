using Team76.PTA.MathFunctions;

namespace Team76.PTA.Boundaries
{
    /// <summary>
    /// Perpendicular Sealing Faults Boundary
    /// </summary>
    public class PerpendicularSealingFaultsBoundary : BoundaryBase
    {
        private readonly double _ld1;
        private readonly double _ld2;

        /// <summary>
        /// Perpendicular Sealing Faults Boundary
        /// </summary>
        /// <param name="rw">Wellbore radius, [ft]</param>
        /// <param name="l1">Distance to boundary 1, [ft]</param>
        /// <param name="l2">Distance to boundary 2, [ft]</param>
        public PerpendicularSealingFaultsBoundary(double rw, double l1, double l2)
        {
            _ld1 = l1 / rw;
            _ld2 = l2 / rw;
        }

        /// <inheritdoc />
        public override double PwDb(double td)
        {
            return - 0.5 * (
                       + ExponentialIntegral.Evaluate(-_ld1 * _ld1 / td)
                       + ExponentialIntegral.Evaluate(-_ld2 * _ld2 / td)
                       + ExponentialIntegral.Evaluate(-(_ld1 * _ld1 + _ld2 * _ld2) / td)
                   );
        }
    }
}