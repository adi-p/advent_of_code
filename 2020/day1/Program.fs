open System
open System.IO

// Define a new function to print a name.
// It is defined above the main function.
// let printGreeting name =
//     printfn "Hello %s from F#!" name

let _2sum n list =
    let rec _2sum_helper n list map =
        match list with
        | [] -> None
        | (x::xs) ->
            let diff = n - x
            match Map.tryFind diff map with
            | Some(m) -> Some (x,m)
            | None -> _2sum_helper n xs (Map.add x x map)

    _2sum_helper n list Map.empty

let _3sum n list =
    let rec _3sum_helper n list seen = 
        match list with
        | [] -> None
        | (x::xs) ->
            let diff = n - x
            match _2sum diff seen with
            | Some(i,j) -> Some (x,i,j)
            | None -> _3sum_helper n xs (x::seen)

    _3sum_helper n list []


[<EntryPoint>]
let main argv =
    let GOAL = 2020

    // we should parse the input file - maybe based on the argv
    for file in argv do
        let readlines =
            file
            |> File.ReadAllLines
            |> Array.map (fun str -> int str ) 
    
        let result = 
            readlines
            |> Array.toList
            |> _3sum GOAL
        
        match result with
        | None -> Printf.eprintf "No matches found"
        | Some (n1, n2, n3) ->
            printfn "%d * %d * %d = %d" n1 n2 n3 (n1 * n2 * n3)
    0
