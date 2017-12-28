using Team76.PTA.MathFunctions;

namespace Team76.PTA.Boundaries
{
    /// <summary>
    /// Linear Constant Pressure Boundary
    /// </summary>
    public class LinearConstantPressureBoundary : BoundaryBase
    {
        private readonly double _ld;

        /// <summary>
        /// Linear Constant Pressure Boundary
        /// </summary>
        /// <param name="rw">Wellbore radius, [ft]</param>
        /// <param name="l">Distance to boundary, [ft]</param>
        public LinearConstantPressureBoundary(double rw, double l)
        {
            _ld = l / rw;
        }


        /// <inheritdoc />
        public override double PwDb(double td)
        {
            return 0.5 * ExponentialIntegral.Evaluate(-_ld * _ld / td);
        }
    }
}