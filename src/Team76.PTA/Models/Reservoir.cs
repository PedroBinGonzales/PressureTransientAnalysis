namespace Team76.PTA.Models
{
    /// <summary>
    /// Reservoir properties
    /// </summary>
    public class Reservoir
    {
        /// <summary>
        /// Porosity, [dimensionless]
        /// </summary>
        public double Porosity { get; set; }

        /// <summary>
        /// Net formation thickness, [ft]
        /// </summary>
        public double H { get; set; }

        /// <summary>
        /// Total compressibility, [1/psi]
        /// </summary>
        public double Ct { get; set; }

        /// <summary>
        /// Matrix permeability, [md]
        /// </summary>
        public double K { get; set; }
    }
}
