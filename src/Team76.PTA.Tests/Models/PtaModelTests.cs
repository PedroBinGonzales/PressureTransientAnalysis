using System;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Should;
using Team76.PTA.Boundaries;
using Team76.PTA.Models;


namespace Team76.PTA.Tests.Models
{
    [TestFixture]
    public class PtaModelTests
    {
        [Test]
        public void UseCase()
        {
            var fluid = new Fluid() { B = 1, Ul = 1 };
            var well = new Well() { C = 1, Rw = 0.15, SkinFactor = 0 };
            var reservoir = new Reservoir() { Ct = 0.00001, Porosity = 0.2, H = 10, K = 10 };
            var boundary = new LinearSealingFaultBoundary(well.Rw, 1000);
            var pta = new PtaModel(fluid, well, reservoir, boundary);

            var q = 500;
            var time = 10;

            var pressureDrop = pta.PressureDrop(time, q);
        }

        [Test]
        public void UseCase2()
        {
            var fluid = new Fluid() { B = 1, Ul = 1 };
            var well = new Well() { C = 1, Rw = 0.15, SkinFactor = 0 };
            var reservoir = new Reservoir() { Ct = 0.00001, Porosity = 0.2, H = 10, K = 10 };
            var l1 = 1000;
            var l2 = 1000;

            var q = 500;
            var time = 10;

            var boundary = new NoBoundary();
            var pta1 = new PtaModel(fluid, well, reservoir, boundary);
            var pta2 = new PtaModel(fluid, well, reservoir);

            pta1.PressureDrop(time, q).ShouldEqual(pta2.PressureDrop(time, q));
        }

        [Test, Ignore("")]
        public void PrintPtaPressureDropTest()
        {
            var fluid = new Fluid() { B = 1, Ul = 1 };
            var well = new Well() { C = 1, Rw = 0.15, SkinFactor = 0 };
            var reservoir = new Reservoir() { Ct = 0.00001, Porosity = 0.2, H = 10, K = 10 };
            var l1 = 1000;
            var l2 = 1000;
            var pta1 = new PtaModel(fluid, well, reservoir);
            var pta2 = new PtaModel(fluid, well, reservoir, new LinearSealingFaultBoundary(well.Rw, l1));
            var pta3 = new PtaModel(fluid, well, reservoir, new LinearConstantPressureBoundary(well.Rw, l1));
            var pta4 = new PtaModel(fluid, well, reservoir, new PerpendicularSealingFaultsBoundary(well.Rw, l1, l2));
            var pta5 = new PtaModel(fluid, well, reservoir, new PerpendicularConstantPressuresBoundary(well.Rw, l1, l2));
            var pta6 = new PtaModel(fluid, well, reservoir, new PerpendicularMixedBoundaries(well.Rw, l1, l2));


            var q = 500;
            var times = Enumerable.Range(0, 10000);
            foreach (var time in times)
            {
                StringBuilder sb = new StringBuilder();
                sb
                    .Append(time + " ")
                    .Append(pta1.PressureDrop(time, q) + " ")
                    .Append(pta2.PressureDrop(time, q) + " ")
                    .Append(pta3.PressureDrop(time, q) + " ")
                    .Append(pta4.PressureDrop(time, q) + " ")
                    .Append(pta5.PressureDrop(time, q) + " ")
                    .Append(pta5.PressureDrop(time, q) + " ")
                    .Append(pta6.PressureDrop(time, q) + " ")
                    .Append(pta1.PressureDropDerivative(time, q) + " ")
                    .Append(pta2.PressureDropDerivative(time, q) + " ")
                    .Append(pta3.PressureDropDerivative(time, q) + " ")
                    .Append(pta4.PressureDropDerivative(time, q) + " ")
                    .Append(pta5.PressureDropDerivative(time, q) + " ")
                    .Append(pta5.PressureDropDerivative(time, q) + " ")
                    .Append(pta6.PressureDropDerivative(time, q) + " ")
                    ;
                Console.WriteLine(sb.ToString());
            }
        }

        [Test]
        public void PressureDropTest2()
        {
            var fluid = new Fluid() { B = 1, Ul = 1 };
            var well = new Well() { C = 1, Rw = 0.15, SkinFactor = 0 };
            var reservoir = new Reservoir() { Ct = 0.00001, Porosity = 0.2, H = 10, K = 10 };
            var pta = new PtaModel(fluid, well, reservoir);

            var data = new[] { new Tuple<double, double>(0, 200), new Tuple<double, double>(5, 0) };

            var steps = Enumerable.Range(0, 20);
            foreach (var step in steps)
            {
                var p = pta.PressureDrop(step, data);
                Console.WriteLine($"{step} : {p:F1}");
            }

            pta.PressureDrop(1, data).ShouldEqual(pta.PressureDrop(1, 200));
            pta.PressureDrop(6, data).ShouldEqual(pta.PressureDrop(6, 200) + pta.PressureDrop(6 - 5, 0 - 200));
        }

        [Test]
        public void PressureDropDerivativeTest()
        {
            var well = new Well() { C = 0.01, Rw = 0.15, SkinFactor = 0 };
            var fluid = new Fluid() { B = 1, Ul = 1 };
            var reservoir = new Reservoir() { Ct = 0.00001, Porosity = 0.2, H = 10, K = 10 };
            var l1 = 1000;
            var l2 = 1000;
            var q = 500;

            var pta = new PtaModel(fluid, well, reservoir);

            var xvalues = Enumerable.Range(-60, 91).Select(x => x / 10.0).Select(c => Math.Pow(10, c)).ToList();

            var xs = xvalues;

            Assert.DoesNotThrow(()=> xs.Select(x => new Tuple<double, double>(x, pta.PressureDrop(x, q))));
            Assert.DoesNotThrow(() => xs.Select(x => new Tuple<double, double>(x, pta.PressureDropDerivative(x, q))));
            //var dt1 = xs.Select(x => new Tuple<double, double>(x, pta.PressureDrop(x, q))).ToList();
            //var dt2 = xs.Select(x => new Tuple<double, double>(x, pta.PressureDropDerivative(x, q))).ToList();


        }
    }
}