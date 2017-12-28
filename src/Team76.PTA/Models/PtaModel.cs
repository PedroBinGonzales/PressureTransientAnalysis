using System;
using CuttingEdge.Conditions;
using Team76.PTA.Boundaries;
using Team76.PTA.MathFunctions;

namespace Team76.PTA.Models
{

    /// <summary>
    /// PTA model
    /// </summary>
    public class PtaModel : PtaModelBase
    {
        private readonly BoundaryBase _boundary;

        /// <summary>
        /// PTA model with boundary
        /// </summary>
        /// <param name="fluid">Fluid model</param>
        /// <param name="well">Well model</param>
        /// <param name="reservoir">Reservoir model</param>
        /// <param name="boundary">Boundary model</param>
        public PtaModel(Fluid fluid, Well well, Reservoir reservoir, BoundaryBase boundary) : base(fluid, well, reservoir)
        {
            Condition.Requires(boundary, nameof(boundary)).IsNotNull();
            _boundary = boundary;
        }

        /// <summary>
        /// PTA model with no boundary
        /// </summary>
        /// <param name="fluid">Fluid model</param>
        /// <param name="well">Well model</param>
        /// <param name="reservoir">Reservoir model</param>
        public PtaModel(Fluid fluid, Well well, Reservoir reservoir) : this(fluid, well, reservoir, new NoBoundary())
        {
            Condition.Requires(fluid, nameof(fluid)).IsNotNull();
            Condition.Requires(well, nameof(well)).IsNotNull();
            Condition.Requires(reservoir, nameof(reservoir)).IsNotNull();
        }

        /// <inheritdoc />
        public override double PressureDrop(double time, double q)
        {
            Condition.Requires(time, nameof(time)).IsGreaterOrEqual(0);
            if (time == 0) return 0;

            var dt = Td(time);
            var pwdTotal = Pwd(dt) + _boundary.PwDb(dt);
            var pressureDrop = FromDimensionlessPressureDrop(pwdTotal, q);
            return pressureDrop;
        }

        private double PwdRinLaplaceSpace(double s)
        {
            var rz = Math.Sqrt(s);
            var p1 = MathNet.Numerics.SpecialFunctions.BesselK0(rz) + Well.SkinFactor * rz * MathNet.Numerics.SpecialFunctions.BesselK1(rz);
            var p2 = s * (rz * MathNet.Numerics.SpecialFunctions.BesselK1(rz) + Cd() * s * p1);
            return p1 / p2;
        }

        /// <summary>
        /// Dimensionless wellbore pressure
        /// </summary>
        /// <param name="td">dimensionless time</param>
        /// <returns></returns>
        private double Pwd(double td)
        {
            return Laplace.InverseTransform(PwdRinLaplaceSpace, td);
        }
    }
}