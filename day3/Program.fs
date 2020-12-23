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

    let count = countTree (3,1) (0,0) landArray    
    printfn "Tree count: %d" count
    0 // return an integer exit code
