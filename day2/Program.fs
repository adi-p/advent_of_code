
open System
open System.IO
open System.Text.RegularExpressions

type PaswordEntry = {
    RequiredLetter : char
    Min : int
    Max : int
    Password : string
}

// let newPasswordEntry letter min max password =
//     {
//         RequiredLetter = letter
//         Min = min
//         Max = max
//         Password = password
//     }

let isValid entry =
    let count = 
        entry.Password
        |> String.filter (fun c -> c = entry.RequiredLetter)
        |> String.length
    (count >= entry.Min) && (count <= entry.Max)

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
