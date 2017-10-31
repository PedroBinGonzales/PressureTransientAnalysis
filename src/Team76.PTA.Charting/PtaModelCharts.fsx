#load @"..\packages\FSharp.Charting.0.91.1\lib\net45\FSharp.Charting.fsx"
#r    @"bin\Debug\Team76.PTA.dll"


open FSharp.Charting
open System
open System.Drawing
open Team76.PTA
open Team76.PTA.Models
open FSharp.Charting.ChartTypes

let fluid = Fluid(B = 1.0, Mu = 1.0)
let well = Well(C=0.01, Rw = 0.3, SkinFactor = 0.0)
let reservoir = Reservoir(Ct=1e-5, Porosity = 0.1, H = 100.0, K= 10.0)
let ptaModel = InfiniteHomogenousPtaModel(fluid, well, reservoir)
let q = 1000.0
let pi = 5000.0

let time = 
    [ for x in 0.0..1.0..2000.0 -> x ]

let timePressure = time |> Seq.map (fun x ->(x, pi- ptaModel.PressureDrop(x, q)))

let chart =  Chart.Line (timePressure, Name ="pressure vs time", Color = Color.Blue)
            |> Chart.WithXAxis(Min = 0.0, Log = false, LabelStyle = LabelStyle(Format = "F0"))
            |> Chart.WithXAxis(Enabled = true, Title = "time, [hr]")             
            |> Chart.WithYAxis(Min = 0.0, Log = false, LabelStyle = LabelStyle(Format = "F0"))
            |> Chart.WithYAxis(Enabled = true, Title = "pressure, [psi]") 
            

let data = [|(0.0,1000.0);(1000.0,0.0)|]

let timePressure2 = time |> Seq.map (fun x ->(x, pi- ptaModel.PressureDrop(x, data)))

let chart2 =  Chart.Line (timePressure2, Name ="pressure vs time 2", Color = Color.Blue)
            |> Chart.WithXAxis(Min = 0.0, Log = false, LabelStyle = LabelStyle(Format = "F0"))
            |> Chart.WithXAxis(Enabled = true, Title = "time, [hr]")             
            |> Chart.WithYAxis(Min = 0.0, Log = false, LabelStyle = LabelStyle(Format = "F0"))
            |> Chart.WithYAxis(Enabled = true, Title = "pressure, [psi]")
