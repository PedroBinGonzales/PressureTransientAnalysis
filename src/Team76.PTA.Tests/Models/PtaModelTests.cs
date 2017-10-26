using System;
using System.Linq;
using NUnit.Framework;
using Team76.PTA.Models;

namespace Team76.PTA.Tests.Models
{
    [TestFixture]
    public class PtaModelTests
    {
        [Test]
        public void PressureTest()
        {
            var fluid = new Fluid(){B = 1, Mu = 1};
            var well = new Well() {C = 1, Rw = 0.15, SkinFactor = 0};
            var reservoir = new Reservoir() {Ct = 0.00001, Porosity = 0.2, H = 10, K = 10, Pi = 2000};
            var pta = new PtaModel() {Fluid = fluid, Well = well, Reservoir = reservoir};
            var q = 500;
            var times = Enumerable.Range(0, 10);
            foreach (var time in times)
            {
                var p = pta.Pressure(time, q);
                Console.WriteLine($"{time} : {p}");
            }

        }
    }
}
