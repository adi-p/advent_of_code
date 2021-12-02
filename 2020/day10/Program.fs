open System
open System.IO

let updateMap updater (defaultVal : 'Value) (key : 'Key) (map : Map<'Key, 'Value>) =
    match map.TryFind key with
    | None -> Map.add key defaultVal map
    | Some i -> Map.add key (updater i) map

// TODO: this could probably be done with a zip -> map -> fold. It would be more elegant
let countDifferences (adapters : int array) =
    let rec countDiffHelper index map =
        if index >= adapters.Length then
            map
        else                
            let diff = adapters.[index] - adapters.[index - 1]
            map
            |> updateMap (fun n -> n + 1) 1 diff
            |> countDiffHelper (index + 1)

    Map.empty 
    |> Map.add (adapters.[0]) 1 // initial adapter
    |> countDiffHelper 1
    |> updateMap (fun n -> n + 1) 1 3 // device

[<EntryPoint>]
let main argv =
    let file = argv.[0]

    let adapters =
        file 
        |> File.ReadAllLines
        |> Array.map int
        |> Array.sort

    let diffMap = countDifferences adapters

    let defaultVal = function | Some i -> i | None -> 0 

    let count1 =  defaultVal <| diffMap.TryFind 1
    let count2 = defaultVal <| diffMap.TryFind 2
    let count3 = defaultVal <| diffMap.TryFind 3

    printfn "1 diff count: %d, 2 diff count: %d, 3 diff count: %d" count1 count2 count3
    printfn "Result: %d" (count1 * count3)
         
    0 // return an integer exit code