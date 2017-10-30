namespace Team76.PTA.Models
{
    /// <summary>
    /// Well properties
    /// </summary>
    public class Well
    {
        /// <summary>
        /// Skin factor, [dimensionless]
        /// </summary>
        public double SkinFactor { get; set; }

        /// <summary>
        /// Wellbore radius, [ft]
        /// </summary>
        public double Rw { get; set; }


        /// <summary>
        /// Wellbore storage coefficient, [bbl/psi]
        /// </summary>
        public double C { get; set; }

        /// <summary>
        /// Dimensionless radius
        /// </summary>
        /// <param name="r">distance from the center of wellbore, ft</param>
        /// <returns></returns>
        public double DimensionlessRadius(double r) => r / Rw;

    }
}