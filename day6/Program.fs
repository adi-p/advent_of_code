// Learn more about F# at http://fsharp.org

open System
open System.IO

let parseQuestions lines =
    let parseQuestionsHelper (acc, curMap) line =
        if line = "" then
            (curMap::acc, Map.empty)
        else
            (acc, Seq.fold (fun m c -> Map.add c c m) curMap line)

    // this pattern again... I should look up if there is a better way of doing this
    let (maps, lastMap) = List.fold parseQuestionsHelper ([], Map.empty) lines
    (lastMap::maps)

[<EntryPoint>]
let main argv =
    let file = argv.[0]
    let maps = 
        file
        |> File.ReadAllLines
        |> List.ofArray
        |> parseQuestions

    // for map in maps do
    //     printfn "%d" map.Count

    printfn "total count: %d" (maps |> List.fold (fun sum m -> sum + m.Count) 0)

    0 // return an integer exit code
