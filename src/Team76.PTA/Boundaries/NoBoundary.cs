namespace Team76.PTA.Boundaries
{
    /// <summary>
    /// No Boundary model
    /// </summary>
    public class NoBoundary : BoundaryBase
    {
        /// <inheritdoc />
        public override double PwDb(double td)
        {
            return 0;
        }
    }
}