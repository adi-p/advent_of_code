
open System
open System.IO

let max a b = if a > b then a else b

let getSeatId (str : string) =
    let COL_COUNT = 8
    let rec getRow (rstr : string) startpoint len =
        if len = 1 then
            startpoint
        else
            match rstr.[0] with
            | 'F' -> getRow rstr.[1..] startpoint (len/2)
            | 'B' -> getRow rstr.[1..] (startpoint + (len/2)) (len/2)
            | _ -> invalidArg "str" "index 0 to 6 of the argument 'str' should only contain characters 'F' and 'B'"

    let rec getCol (cstr : string) startpoint len =
        if len = 1 then
            startpoint
        else
            match cstr.[0] with
            | 'L' -> getCol cstr.[1..] startpoint (len/2)
            | 'R' -> getCol cstr.[1..] (startpoint + (len/2)) (len/2)
            | _ -> invalidArg "str" "index 7 to 9 of the argument 'str' should only contain characters 'R' and 'L'"        

    let row = getRow str.[0..6] 0 128
    let col = getCol str.[7..] 0 8
    (row * COL_COUNT) + col

[<EntryPoint>]
let main argv =
    
    let file = argv.[0]
    let seatIds = 
        file
        |> File.ReadAllLines
        |> List.ofArray
        |> List.map getSeatId

    printfn "The highest seat ID is: %d" (seatIds |> List.fold max 0 )

    0 // return an integer exit code
