open System.IO


let countIncrease numberList =
  Seq.zip numberList (Seq.tail numberList)
  |> Seq.filter (fun (x,y) -> y > x)
  |> Seq.length
  
let countIncreaseWindow numberList =
  Seq.zip3 numberList (Seq.tail numberList) (Seq.tail (Seq.tail numberList))
  |> Seq.map (fun (x,y,z) -> x + y + z)
  |> countIncrease

let readlines file =
  file
  |> File.ReadAllLines
  |> Seq.map int

let list = readlines "data/day01/input.txt"

printfn "%d" (countIncrease list)
printfn "%d" (countIncreaseWindow list)



  