namespace Team76.PTA.Models
{
    /// <summary>
    /// Fluid properties
    /// </summary>
    public class Fluid
    {

        /// <summary>
        /// Viscosity, [cp]
        /// </summary>
        public double Mu { get; set; }


        /// <summary>
        /// Formation volume factor, [res vol/surface vol]
        /// </summary>
        public double B { get; set; }
    }
}