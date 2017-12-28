using System;
using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Prism.Mvvm;
using Team76.PTA.Boundaries;
using Team76.PTA.Models;

namespace Team76.PTA.WPF.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public string Title => "Prism Application";
        public Well Well => new Well() { C = 0.01, Rw = 0.15, SkinFactor = 0 };
        public Fluid Fluid => new Fluid() { B = 1, Ul = 1 };
        public Reservoir Reservoir => new Reservoir() { Ct = 0.00001, Porosity = 0.2, H = 10, K = 10 };
        public double L1 => 1000;
        public double L2 => 1000;
        public double Q => 500;

        public PlotModel NoBoundary { get; set; }
        public PlotModel LinearSealingFaultBoundary { get; set; }
        public PlotModel LinearConstantPressureBoundary { get; set; }
        public PlotModel PerpendicularSealingFaultsBoundary { get; set; }
        public PlotModel PerpendicularConstantPressuresBoundary { get; set; }
        public PlotModel PerpendicularMixedBoundary { get; set; }

        public MainWindowViewModel()
        {
            UpdatePlots();
        }

        private void UpdatePlots()
        {
            NoBoundary = GetPtaPlot(new PtaModel(Fluid, Well, Reservoir));
            NoBoundary.Title = "No Boundary";

            LinearSealingFaultBoundary =
                GetPtaPlot(new PtaModel(Fluid, Well, Reservoir,
                    new LinearSealingFaultBoundary(Well.Rw, L1)));
            LinearSealingFaultBoundary.Title = "Sealing Fault Boundary";

            LinearConstantPressureBoundary =
                GetPtaPlot(new PtaModel(Fluid, Well, Reservoir,
                    new LinearConstantPressureBoundary(Well.Rw, L1)));
            LinearConstantPressureBoundary.Title = "Constant Pressure Boundary";

            PerpendicularSealingFaultsBoundary =
                GetPtaPlot(new PtaModel(Fluid, Well, Reservoir,
                    new PerpendicularSealingFaultsBoundary(Well.Rw, L1, L2)));
            PerpendicularSealingFaultsBoundary.Title = "Perpendicular Sealing Faults";

            PerpendicularConstantPressuresBoundary =
                GetPtaPlot(new PtaModel(Fluid, Well, Reservoir,
                    new PerpendicularConstantPressuresBoundary(Well.Rw, L1, L2)));
            PerpendicularConstantPressuresBoundary.Title = "Perpendicular Constant Pressure Boundaries";

            PerpendicularMixedBoundary =
                GetPtaPlot(new PtaModel(Fluid, Well, Reservoir,
                    new PerpendicularMixedBoundaries(Well.Rw, L1, L2)));
            PerpendicularMixedBoundary.Title = "Perpendicular Mixed Boundaries";
        }

        private PlotModel GetPtaPlot(PtaModelBase pta)
        {
            var pm = GetLogLogPlotModel();
            
            var ls1 = GetLineSeries((x) => pta.PressureDrop(x, Q));
            ls1.Title = "Pressure Drop";
            var ls2 = GetLineSeries((x) => pta.PressureDropDerivative(x, Q));
            ls2.Title = "Pressure Drop Derivative";

            pm.Series.Add(ls1);
            pm.Series.Add(ls2);

            return pm;
        }

        private PlotModel GetLogLogPlotModel()
        {
            var pm = new PlotModel(); ;
            pm.Axes.Add(new LogarithmicAxis() { Position = AxisPosition.Bottom, Minimum = 0.001, Maximum = 10e6 });
            pm.Axes.Add(new LogarithmicAxis() { Position = AxisPosition.Left, Minimum = 0.001, Maximum = 10e4 });
            pm.LegendPlacement = LegendPlacement.Outside;
            pm.LegendPosition = LegendPosition.BottomCenter;
            return pm;
        }

        private List<double> GetXValues()
        {
            return Enumerable.Range(-30, 91).Select(x => x / 10.0).Select(c => Math.Pow(10, c)).ToList();
        }

        private LineSeries GetLineSeries(Func<double, double> f)
        {
            var ls = new LineSeries();
            var dt = GetXValues().Select(x => new DataPoint(x, f(x)));
            ls.Points.AddRange(dt);
            return ls;
        }
    }
}
