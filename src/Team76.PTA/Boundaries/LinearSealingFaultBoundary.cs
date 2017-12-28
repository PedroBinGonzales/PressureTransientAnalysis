using Team76.PTA.MathFunctions;

namespace Team76.PTA.Boundaries
{
    /// <summary>
    /// Linear Sealing Fault Boundary
    /// </summary>
    public class LinearSealingFaultBoundary : BoundaryBase
    {
        private readonly double _ld;

        /// <summary>
        /// Linear Sealing Fault Boundary
        /// </summary>
        /// <param name="rw">Wellbore radius, [ft]</param>
        /// <param name="l">Distance to boundary, [ft]</param>
        public LinearSealingFaultBoundary(double rw, double l)
        {
            _ld = l / rw;
        }

        /// <inheritdoc />
        public override double PwDb(double td)
        {
            return -0.5 * ExponentialIntegral.Evaluate(-_ld * _ld / td);
        }
    }
}