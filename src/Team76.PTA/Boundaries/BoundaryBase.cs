namespace Team76.PTA.Boundaries
{
    /// <summary>
    /// Boundary Base
    /// </summary>
    public abstract class BoundaryBase
    {
        /// <summary>
        /// Dimensionless bottom hole pressure affected by outer boundary
        /// </summary>
        /// <param name="td">Dimensionless time</param>
        /// <returns></returns>
        public abstract double PwDb(double td);

    }
}