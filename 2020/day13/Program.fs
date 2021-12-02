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

//TODO need to use chinese remainder theorem. THe numbers are too big to brute force.
let findBusTimestamp2 (buses: (int*int) seq) =
    let rec findStartTime startTime =
        let isCorrect = 
            buses
            |> Seq.forall (fun (interval, bus) -> (startTime + interval) % bus = 0)
        match isCorrect with 
        | true -> startTime 
        | false ->
            let (_ ,firstBus) = buses |> Seq.head
            findStartTime (startTime + firstBus)
    findStartTime 0

let parseLines (lines : (string array)) =
    let minTimestamp = int lines.[0]
    let buses = lines.[1].Split ',' |> Seq.choose (fun s -> match Int32.TryParse s with | (true,i) -> Some i | _ -> None) 
    (minTimestamp, buses)

let parseLines2 (lines : (string array)) =
    let buses = 
        lines.[1].Split ',' 
        |> Seq.mapi (fun index busString -> match Int32.TryParse busString with | (true, bus) -> Some (index, bus) | _ -> None) 
        |> Seq.choose id
    buses

[<EntryPoint>]
let main argv =

    printfn "Running..."
    let file = argv.[0]

    let (minTimestamp, buses) = file |> File.ReadAllLines |> parseLines
    let (waitTime, bus) = findBusTimestamp minTimestamp buses
    printfn "Part 1: Bus: %d, wait time: %d, Result: %d" bus waitTime (waitTime * bus)


    let buses2 = file |> File.ReadAllLines |> parseLines2
    let smallestStartTime = findBusTimestamp2 buses2

    printfn "Part 2: Bus: %d, min wait time: %d" bus smallestStartTime
    
    0 // return an integer exit code