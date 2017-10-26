#load @"..\packages\FSharp.Charting.0.91.1\lib\net45\FSharp.Charting.fsx"
#r    @"bin\Debug\Team76.PTA.dll"


open FSharp.Charting
open System
open System.Drawing
open Team76.PTA
open Team76.PTA.Models
open FSharp.Charting.ChartTypes

let fluid = Fluid(B = 1.0, Mu = 1.0)
let well = Well(C=1.0, Rw = 0.3, SkinFactor = 0.0)
let reservoir = Reservoir(Ct=1e-5, Porosity = 0.2, H = 10.0, K= 10.0, Pi = 5000.0)
let ptaModel = PtaModel(Fluid = fluid, Well = well, Reservoir = reservoir)
let q = 200.0

let time = 
    [ for x in -2.0..0.1..1000.0 -> x ]

let timePressure = time |> Seq.map (fun x ->(x, ptaModel.Pressure(x, q)))

let chart =  Chart.Line (timePressure, Name ="pressure vs time", Color = Color.Blue)
            |> Chart.WithXAxis(Min = 0.0, Log = false, LabelStyle = LabelStyle(Format = "F0"))
            |> Chart.WithXAxis(Enabled = true, Title = "time, [hr]")             
            |> Chart.WithYAxis(Min = 0.00, Log = false, LabelStyle = LabelStyle(Format = "F0"))
            |> Chart.WithYAxis(Enabled = true, Title = "pressure, [psi]")           

