
open System
open System.IO
open System.Text.RegularExpressions

type PaswordEntry = {
    RequiredLetter : char
    Min : int
    Max : int
    Password : string
}

let isValid_old entry =
    let count = 
        entry.Password
        |> String.filter (fun c -> c = entry.RequiredLetter)
        |> String.length
    (count >= entry.Min) && (count <= entry.Max)

let isValid entry =
    let cond1 = entry.Password.[entry.Min - 1] = entry.RequiredLetter
    let cond2 = entry.Password.[entry.Max - 1] = entry.RequiredLetter
    (not(cond1 && cond2)) && (cond1 || cond2)


// this function assumes quite a bit of things 
let parse_line line =
    let r = new Regex("(\d*)-(\d*) (.): (.*)")
    let m = r.Match line
    let items = m.Groups |> Array.ofSeq

    {
        Min = int items.[1].Value
        Max = int items.[2].Value
        RequiredLetter = char items.[3].Value
        Password = string items.[4].Value
    }


[<EntryPoint>]
let main argv =
    let file = argv.[0]

    let entries =
        file
        |> File.ReadAllLines
        |> Array.map (fun str -> parse_line str)
        |> Array.toList

    let result = entries |> List.filter isValid |> List.length
          

    printfn "Valid count: %d" result

    
    

    0 // return an integer exit code
