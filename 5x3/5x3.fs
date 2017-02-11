open Nancy
open Nancy.Hosting.Self
open System

let rnd = System.Random()
let randSym() = rnd.NextDouble() * 10.0 |> int

let spin() =
    let row() = String.concat " " <| List.init 5 (fun _ -> randSym() |> string)
    String.concat "<br/>" [row(); row(); row()]

type FiveXThree() as this = 
    inherit NancyModule()
    do this.Get.["spin"] <- fun _ ->
        spin() |> box

[<EntryPoint>]
let main argv = 
    use host = new NancyHost(new Uri("http://localhost:8053/"))
    host.Start()
    Console.WriteLine("5x3")
    Console.ReadLine() |> ignore
    host.Stop()
    0