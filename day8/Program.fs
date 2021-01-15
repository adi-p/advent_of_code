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

    printfn "Acc: %d" acc
    


    0 // return an integer exit code