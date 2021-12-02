// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp

open System
open System.IO


type Instruction = 
    | Nop of int
    | Acc of int
    | Jmp of int


let runInstructions (instructions : Instruction array) =
    let rec runHelper (acc, index, seenSet) =
        match Set.contains index seenSet with
        | true -> acc
        | false -> 
            let seenSet = Set.add index seenSet
            (match instructions.[index] with
            | Nop _ -> runHelper (acc, index + 1, seenSet)
            | Acc n -> runHelper (acc + n, index + 1, seenSet)
            | Jmp n -> runHelper (acc, index + n, seenSet))

    runHelper (0, 0, Set.empty)


let runUncorrupt (instructions : Instruction array) =
    let rec uncorruptHelper (acc, index, seenSet, isChangeMade) =
        if index >= instructions.Length then
            Some acc
        else        
            match Set.contains index seenSet with
            | true -> None
            | false -> 
                let seenSet = Set.add index seenSet
                // this is messy but w/e
                (match instructions.[index] with
                | Nop n ->
                    if isChangeMade then
                        uncorruptHelper (acc, index + 1, seenSet, true)
                    else
                        (match uncorruptHelper (acc, index + 1, seenSet, false) with
                        | None -> uncorruptHelper (acc, index + n, seenSet, true)
                        | Some i -> Some i)

                | Jmp n -> 
                    if isChangeMade then
                        uncorruptHelper (acc, index + n, seenSet, true)
                    else
                        (match uncorruptHelper (acc, index + n, seenSet, false) with
                        | None -> uncorruptHelper (acc, index + 1, seenSet, true)
                        | Some i -> Some i)

                | Acc n -> uncorruptHelper (acc + n, index + 1, seenSet, isChangeMade))

    uncorruptHelper (0, 0, Set.empty, false)

let parseLine (line : string) =
    let instruction = line.Split ' '
    let value = int instruction.[1] //this might not work
    match instruction.[0] with
    | "jmp" -> Jmp value
    | "acc" -> Acc value
    | "nop" -> Nop value
    | _ -> failwith "unknown instruction"

[<EntryPoint>]
let main argv =
    let file = argv.[0]
    let instructions = 
        file
        |> File.ReadAllLines
        |> Array.map parseLine

    let acc = runInstructions instructions
    let acc2 = runUncorrupt instructions

    printfn "Acc: %d" acc

    match acc2 with
    | Some i -> printfn "uncorrupt acc: %d" i
    | None -> printfn "couldn't uncorrupt the program"
    


    0 // return an integer exit code