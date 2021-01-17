open System
open System.IO
open System.Numerics


//adapted from day 1
let twoSum n (array : bigint array) =
    let rec twoSumHelper n (array : bigint array) map =
        if array.Length > 0 then
            let x = array.[0]
            let diff = n - x
            match Map.tryFind diff map with
            | Some(m) -> Some (x,m)
            | None -> twoSumHelper n array.[1..] (Map.add x x map)
        else
            None        
    twoSumHelper n array Map.empty


let findKey (array : bigint array) preambleSize =
    let rec findKeyHelper startIndex curIndex =
        match twoSum array.[curIndex] array.[startIndex..(startIndex + preambleSize)] with
        | Some _ -> findKeyHelper (startIndex + 1) (curIndex + 1)
        | None -> array.[curIndex]

    findKeyHelper 0 preambleSize


// not going best efficiency
let findConsecutiveSum (goal : bigint) (array : bigint array) =
    let rec findConsHelper startIndex currentIndex acc =
        if acc > goal then
            findConsHelper (startIndex + 1) (startIndex + 1) (bigint 0)
        else if acc = goal then
            array.[startIndex..(currentIndex - 1)]
        else
            let newAcc = acc + array.[currentIndex]
            findConsHelper startIndex (currentIndex + 1) newAcc
    findConsHelper 0 0 (bigint 0) 

[<EntryPoint>]
let main argv =
    let file = argv.[0]
    let nums = 
        file
        |> File.ReadAllLines
        |> Array.map BigInteger.Parse 

    let preambleSize = 25

    let n = findKey nums preambleSize

    let sumArray = findConsecutiveSum n nums

    let max = sumArray |> Array.fold (fun max n -> if n > max then n else max) (bigint 0)
    let min = sumArray |> Array.fold (fun min n -> if n < min then n else min) sumArray.[0]


    printfn "n: %A" n

    printfn "Min: %A  Max: %A Sum: %A" min max <| min + max
    
    0 // return an integer exit code