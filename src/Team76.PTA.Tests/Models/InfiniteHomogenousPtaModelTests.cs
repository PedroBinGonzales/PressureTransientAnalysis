using System;
using System.Linq;
using NUnit.Framework;
using Should;
using Team76.PTA.Models;

namespace Team76.PTA.Tests.Models
{
    [TestFixture]
    public class InfiniteHomogenousPtaModelTests
    {

        [Test]
        public void PressureDropTest()
        {
            var fluid = new Fluid() { B = 1, Mu = 1 };
            var well = new Well() { C = 1, Rw = 0.15, SkinFactor = 0 };
            var reservoir = new Reservoir() { Ct = 0.00001, Porosity = 0.2, H = 10, K = 10};
            var pta = new InfiniteHomogenousPtaModel(fluid, well, reservoir);
            var q = 500;
            var times = Enumerable.Range(0, 10);
            foreach (var time in times)
            {
                var p = pta.PressureDrop(time, q);
                Console.WriteLine($"{time} : {p}");
            }
        }

        [Test]
        public void PressureDropTest2()
        {
            var fluid = new Fluid() { B = 1, Mu = 1 };
            var well = new Well() { C = 1, Rw = 0.15, SkinFactor = 0 };
            var reservoir = new Reservoir() { Ct = 0.00001, Porosity = 0.2, H = 10, K = 10};
            var pta = new InfiniteHomogenousPtaModel(fluid, well, reservoir);

            var data = new[] { new Tuple<double, double>(0, 200), new Tuple<double, double>(5, 0)};

            var steps = Enumerable.Range(0, 20);
            foreach (var step in steps)
            {
                var p = pta.PressureDrop(step, data);
                Console.WriteLine($"{step} : {p :F1}");
            }

            pta.PressureDrop(1, data).ShouldEqual(pta.PressureDrop(1, 200));
            pta.PressureDrop(6, data).ShouldEqual(pta.PressureDrop(6, 200) + pta.PressureDrop(6-5, 0 - 200));
        }
    }
}
