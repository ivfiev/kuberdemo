﻿open Nancy
open Nancy.Hosting.Self
open System
open Mono.Unix
open Mono.Unix.Native


module UnixHelper =
    let isRunningOnMono() = not <| Object.ReferenceEquals(Type.GetType("Mono.Runtime"), null)

    let getUnixTerminationSignals() = [| 
        new UnixSignal(Signum.SIGINT); 
        new UnixSignal(Signum.SIGTERM); 
        new UnixSignal(Signum.SIGQUIT); 
        new UnixSignal(Signum.SIGHUP) |]

let rnd = System.Random()
let randSym() = rnd.NextDouble() * 10.0 |> int

let spin() =
    let row() = String.concat " " <| List.init 6 (fun _ -> randSym() |> string)
    String.concat "<br/>" [row(); row(); row(); row()]

type SixXFour() as this = 
    inherit NancyModule()
    do this.Get.["spin"] <- fun _ ->
        spin() |> box

[<EntryPoint>]
let main argv = 
    use host = new NancyHost(new Uri("http://localhost:8064/"))
    host.Start()
    Console.WriteLine("6x4")
    
    match UnixHelper.isRunningOnMono() with
    | true  -> UnixSignal.WaitAny (UnixHelper.getUnixTerminationSignals()) |> ignore
    | false -> Console.ReadLine() |> ignore
    
    host.Stop()
    0