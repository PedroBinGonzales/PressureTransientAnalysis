#load @"..\packages\FSharp.Charting.0.91.1\lib\net45\FSharp.Charting.fsx"
#r    @"bin\Debug\Team76.PTA.dll"  

open FSharp.Charting
open System
open System.Drawing
open Team76.PTA.PtaModels
open Team76.PTA.Models
open FSharp.Charting.ChartTypes

let fluid = Fluid(B = 1.0, Mu = 1.0)
let well = Well(C=0.01, Rw = 0.3, SkinFactor = 0.0)
let reservoir = Reservoir(Ct=1e-5, Porosity = 0.1, H = 100.0, K= 10.0)

let q = 1000.0
let pi = 5000.0
let l1 = 1000.0
let l2 = 1000.0

let ptaModel1 = InfHomPtaModel(fluid, well, reservoir)
let ptaModel2 = InfHomWithLinearSealingFaultPtaModel(fluid, well, reservoir, l1)
let ptaModel3 = InfHomWithLinearConstantPressurePtaModel(fluid, well, reservoir, l1)
let ptaModel4 = InfHomWithPerpendicularSealingFaultsPtaModel(fluid, well, reservoir, l1, l2)
let ptaModel5 = InfHomWithPerpendicularConstantPressuresPtaModel(fluid, well, reservoir, l1, l2)
let ptaModel6 = InfHomWithPerpendicularMixedBoundariesPtaModel(fluid, well, reservoir, l1, l2)


//let time = [ for x in 0.0..1.0..2000.0 -> x ]
let time = [ for x in -2.0..0.1..6.0 -> 10.0**x ]
let data = [|(0.0,1000.0);(1000.0,0.0)|]  

let timePressure1 = time |> Seq.map (fun x ->(x, ptaModel1.PressureDrop(x, q)))
let timePressure2 = time |> Seq.map (fun x ->(x, ptaModel2.PressureDrop(x, q)))
let timePressure3 = time |> Seq.map (fun x ->(x, ptaModel3.PressureDrop(x, q)))
let timePressure4 = time |> Seq.map (fun x ->(x, ptaModel4.PressureDrop(x, q)))
let timePressure5 = time |> Seq.map (fun x ->(x, ptaModel5.PressureDrop(x, q)))
let timePressure6 = time |> Seq.map (fun x ->(x, ptaModel6.PressureDrop(x, q))) 

let timePressureChart = Chart.Combine(
   [ Chart.Line(timePressure1,Name="InfHomPtaModel")
     Chart.Line(timePressure2,Name="InfHomWithLinearSealingFaultPtaModel") 
     Chart.Line(timePressure3,Name="InfHomWithLinearConstantPressurePtaModel")
     Chart.Line(timePressure4,Name="InfHomWithPerpendicularSealingFaultsPtaModel")
     Chart.Line(timePressure5,Name="InfHomWithPerpendicularConstantPressuresPtaModel")
     Chart.Line(timePressure6,Name="InfHomWithPerpendicularMixedBoundariesPtaModel")])
                        |> Chart.WithXAxis(Min = 0.01, Log = true, LabelStyle = LabelStyle(Format = "F0"))
                        |> Chart.WithXAxis(Enabled = true, Title = "time, [hr]")             
                        |> Chart.WithYAxis(Min = 0.01, Log = true, LabelStyle = LabelStyle(Format = "F0"))
                        |> Chart.WithYAxis(Enabled = true, Title = "pressure, [psi]") 
                        |> Chart.WithLegend(Enabled = true, Docking = Docking.Bottom, InsideArea = false, Alignment = StringAlignment.Center)

//does not work

//let timeDPressure1 = time |> Seq.map (fun x ->(x, ptaModel1.PressureDropDerivative(x, q)))
//let timeDPressure2 = time |> Seq.map (fun x ->(x, ptaModel2.PressureDropDerivative(x, q)))
//let timeDPressure3 = time |> Seq.map (fun x ->(x, ptaModel3.PressureDropDerivative(x, q)))
//let timeDPressure4 = time |> Seq.map (fun x ->(x, ptaModel4.PressureDropDerivative(x, q)))
//let timeDPressure5 = time |> Seq.map (fun x ->(x, ptaModel5.PressureDropDerivative(x, q)))
//let timeDPressure6 = time |> Seq.map (fun x ->(x, ptaModel6.PressureDropDerivative(x, q)))

//let timeDPressureChart =
//    Chart.Combine(
//   [ Chart.Line(timeDPressure1,Name="InfHomPtaModel")
//     Chart.Line(timeDPressure2,Name="InfHomWithLinearSealingFaultPtaModel") 
//     Chart.Line(timeDPressure3,Name="InfHomWithLinearConstantPressurePtaModel")
//     Chart.Line(timeDPressure4,Name="InfHomWithPerpendicularSealingFaultsPtaModel")
//     Chart.Line(timeDPressure5,Name="InfHomWithPerpendicularConstantPressuresPtaModel")
//     Chart.Line(timeDPressure6,Name="InfHomWithPerpendicularMixedBoundariesPtaModel")
//     ])
//    |> Chart.WithXAxis(Min = 0.01, Log = true, LabelStyle = LabelStyle(Format = "F0"))
//    |> Chart.WithXAxis(Enabled = true, Title = "time, [hr]")             
//    |> Chart.WithYAxis(Min = 0.01, Log = true, LabelStyle = LabelStyle(Format = "F0"))
//    |> Chart.WithYAxis(Enabled = true, Title = "pressure, [psi]") 
//    |> Chart.WithLegend(Enabled = true, Docking = Docking.Bottom, InsideArea = false, Alignment = StringAlignment.Center)
