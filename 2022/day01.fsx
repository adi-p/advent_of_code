open System.IO

let sumSublists (sublists: int list list) =
  sublists
  |> Seq.fold (fun sums sublist -> (Seq.sum sublist)::sums) []


let readlines file =
  file 
  |> File.ReadAllLines
  |> Seq.fold (fun (elfs, currentElf) calorieLine -> 
    match calorieLine with
    | "" -> (currentElf::elfs, [])
    | x -> (elfs, (int x)::currentElf)
  ) ([], [])
  |> (fun (elfs, lastElf) -> lastElf::elfs)


let caloriesByElf = readlines "data/day01/input.txt"
let summed = caloriesByElf |> sumSublists |> Seq.sortDescending
printfn "Part1: %d" (summed |> Seq.head)
printfn "Part2: %d" (summed |> Seq.take 3 |> Seq.sum)
