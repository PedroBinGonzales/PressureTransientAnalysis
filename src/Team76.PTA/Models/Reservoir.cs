#pragma warning disable 1591

namespace Team76.PTA.Models
{
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

        /// <summary>
        /// Original reservoir pressure, [psi]
        /// </summary>
        public double Pi { get; set; }
    }
}
