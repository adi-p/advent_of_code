// Learn more about F# at http://fsharp.org

open System
open System.IO

let parseQuestions_part1 lines =
    let parseQuestionsHelper (acc, curSet) line =
        if line = "" then
            (curSet::acc, Set.empty)
        else
            (acc, Seq.fold (fun set c -> Set.add c set) curSet line)

    // this pattern again... I should look up if there is a better way of doing this
    let (sets, lastSet) = List.fold parseQuestionsHelper ([], Set.empty) lines
    (lastSet::sets)

let parseQuestions lines =
    let parseQuestionsHelper (acc, curAcc) line =
        if line = "" then
            let curSet = Set.intersectMany curAcc //idk how efficient this is
            (curSet::acc, [])
        else
            let newSet = Seq.fold (fun set c -> Set.add c set) Set.empty line
            (acc, newSet::curAcc)

    // this pattern again... I should look up if there is a better way of doing this
    let (sets, lastList) = List.fold parseQuestionsHelper ([], []) lines
    ((Set.intersectMany lastList)::sets)

[<EntryPoint>]
let main argv =
    let file = argv.[0]
    let sets = 
        file
        |> File.ReadAllLines
        |> List.ofArray
        |> parseQuestions

    // for set in sets do
    //     printfn "%d" set.Count

    printfn "total count: %d" (sets |> List.fold (fun sum set -> sum + set.Count) 0)

    0 // return an integer exit code
