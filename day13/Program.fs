open System
open System.IO


let findBusTimestamp (minTimestamp : int) (buses: int seq) =
    let calcWaitTime bus = 
        let remainder = (minTimestamp % bus)
        match remainder with 
        | 0 -> 0 
        | n -> bus - n
    buses 
    |> Seq.map (fun bus -> (calcWaitTime bus, bus))
    |> Seq.minBy fst

let parseLines (lines : (string array)) =
    let minTimestamp = int lines.[0]
    let buses = lines.[1].Split ',' |> Seq.choose (fun s -> match Int32.TryParse s with | (true,i) -> Some i | _ -> None) 
    (minTimestamp, buses)

[<EntryPoint>]
let main argv =

    printfn "Running..."
    let file = argv.[0]

    let (minTimestamp, buses) = file |> File.ReadAllLines |> parseLines
    let (waitTime, bus) = findBusTimestamp minTimestamp buses

    printfn "Part 1: Bus: %d, wait time: %d, Result: %d" bus waitTime (waitTime * bus)
    0 // return an integer exit code