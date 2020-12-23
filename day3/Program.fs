// Learn more about F# at http://fsharp.org

open System
open System.IO

type SquareType =
    | Tree
    | EmptyLand

let countTree (xstep, ystep) (xstart, ystart) landArray =
    let rec countTreeHelper (xcord, ycord) count =
        // Hit the bottom 
        if(ycord >= (Array.length landArray)) then 
            count
        else
            let xcord = xcord % (Array.length landArray.[0])
            let count =
                match landArray.[ycord].[xcord] with
                | EmptyLand -> count
                | Tree -> count + 1
            countTreeHelper (xcord + xstep, ycord + ystep) count
    countTreeHelper (xstart,ystart) 0


let parseLine line =
    line
    |> Seq.map (fun c -> if c = '.' then EmptyLand else Tree)
    |> Array.ofSeq
        

[<EntryPoint>]
let main argv =

    let file = argv.[0]
    let landArray =
        file
        |> File.ReadAllLines
        |> Array.map (fun str -> parseLine str)

    let steps = [ (1,1); (3,1); (5,1); (7,1); (1,2); ] 

    let counts =
        steps
        |> List.map countTree
        |> List.map (fun f -> f (0,0) landArray)   

    for count in counts do
        printfn "Tree count: %d" count

    // mult gets too big - we need to use bigint!
    let mult = counts |> List.fold (fun (acc:bigint) x -> acc*(bigint x))  (bigint 1)
    printfn "Multiplication result: %A" mult 
    0 // return an integer exit code
