open System
open System.IO

type Seat =
    | Floor
    | Empty
    | Occupied

// let printSeats seats =
//     for row in seats do
//             row |>
//             Array.map (function | Floor -> '.' | Empty -> 'L' | Occupied -> '#')
//             |> String
//             |> printfn "%A"

//     printfn ""        


let findEquilibrium seats =
    let rec findEquilibriumHelper seats =
        let countAdjacent row col (array : Seat [][]) =
            [(row - 1, col - 1); (row - 1, col); (row - 1, col + 1);
             (row, col - 1); (row, col + 1);
             (row + 1, col - 1); (row + 1, col); (row + 1, col + 1);] // Could this be done with a seq?
            
            |> List.filter (fun (r,c) -> r >= 0 && r < array.Length && c >= 0 && c < array.[r].Length)
            |> List.map (fun (r,c) -> array.[r].[c])
            |> List.filter (function 
                | Floor | Empty -> false 
                | Occupied -> true)
            |> List.length          
             
        let newSeats =
            Array.mapi (fun idx row -> 
                Array.mapi (fun jdx seat ->
                    let count = countAdjacent idx jdx seats
                    match seat with
                    | Empty -> if count = 0 then Occupied else Empty
                    | Occupied -> if count >= 4 then Empty else Occupied
                    | Floor -> Floor                        
                ) row) seats

        if seats = newSeats then
            newSeats
        else
            findEquilibriumHelper newSeats        

    findEquilibriumHelper seats

let parseLine line =
    line
    |> Seq.map (function
        | 'L' -> Empty
        | '.' -> Floor
        | '#' -> Occupied
        | _ -> failwith "Invalid character input")
    |> Array.ofSeq


[<EntryPoint>]
let main argv =

    let file = argv.[0]

    let seats =
        file 
        |> File.ReadAllLines
        |> Array.map parseLine
        |> findEquilibrium     


    // This is a little ugly
    let occCount =
        seats 
        |> Array.fold (fun count row -> (Array.filter (function Occupied -> true | _ -> false) row).Length + count) 0

    printfn "Number of occupied seats: %d" occCount

    0 // return an integer exit code