open System.IO


let countIncrease numberList =
  numberList
  |> Seq.pairwise 
  |> Seq.filter (fun (x,y) -> y > x)
  |> Seq.length
  
let countIncreaseWindow numberList windowSize =
  numberList
  |> Seq.windowed windowSize
  |> Seq.map Seq.sum
  |> countIncrease

let readlines file =
  file
  |> File.ReadAllLines
  |> Seq.map int

let list = readlines "data/day01/input.txt"

printfn "%d" (countIncrease list)
printfn "%d" (countIncreaseWindow list 3)



  