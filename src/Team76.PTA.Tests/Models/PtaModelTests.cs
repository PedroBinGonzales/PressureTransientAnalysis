using System;
using System.Collections.Generic;
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


        [Test]
        public void PtaExtensionsPressureTest()
        {
            var fluid = new Fluid() { B = 1, Mu = 1 };
            var well = new Well() { C = 1, Rw = 0.15, SkinFactor = 0 };
            var reservoir = new Reservoir() { Ct = 0.00001, Porosity = 0.2, H = 10, K = 10, Pi = 2000 };
            var pta = new PtaModel() { Fluid = fluid, Well = well, Reservoir = reservoir };

            var times = new List<double>() {0, 5};
            var rates = new List<double>() {500, 0};
            
            
            var steps = Enumerable.Range(0, 10);
            foreach (var step in steps)
            {
                var p = pta.Pressure(step, times, rates);
                Console.WriteLine($"{step} : {p}");
            }

        }


    }

    public static class PtaExtensions
    {
        public  static double Pressure(this PtaModel pta, double time, List<double> times, List<double> rates)
        {
            throw new NotImplementedException();
        }
    }
}
