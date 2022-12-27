open System.IO
open System


let intersect m1 m2 = //returns a sequence, not a map
  seq {
    for KeyValue(key,v1) in m1 do
      match Map.tryFind key m2 with
      | Some v2 -> yield (key, v1) //only passing one value
      | None -> ()
  }

let findIntersectPriority rucksacks =
  rucksacks
  |> Seq.map (fun compartments -> 
    compartments
    ||> intersect
    |> Seq.head //assumes there will be one (And no more)
    |> fun (key, priority) -> priority)

let findIntersectOfChunk rucksacks =
  rucksacks
  |> Seq.chunkBySize 3
  |> Seq.map (fun group ->
    group
    |> Seq.reduce (fun rucksack1 rucksack2 -> intersect rucksack1 rucksack2 |> Map.ofSeq)
    |> Map.toSeq
    |> Seq.head
    |> fun (key, priority) -> priority
  )

let charToPriority c =
  if int c >= int 'a' then
    ((int c) - (int 'a')) + 1
  else
    ((int c) - (int 'A')) + 27

let readlines file =
  file 
  |> File.ReadAllLines
  |> Seq.fold (fun rucksacks line ->
    let lineAsTuple = line |> Seq.map (fun c -> (c, charToPriority c))
    let compartmentMap1= lineAsTuple |> Seq.take (line.Length / 2) |> Map.ofSeq
    let compartmentMap2 = lineAsTuple |> Seq.rev |> Seq.take (line.Length / 2) |> Map.ofSeq
    (compartmentMap1 , compartmentMap2)::rucksacks
  ) []

let readlines2 file =
  file 
  |> File.ReadAllLines
  |> Seq.fold (fun rucksacks line ->
    line 
    |> Seq.map (fun c -> (c, charToPriority c)) 
    |> Map.ofSeq
    |> fun rucksackMap -> rucksackMap::rucksacks
  ) []


let file = "data/day03/input.txt"
let rucksacks = readlines file
let priorities = rucksacks |> findIntersectPriority

let rucksacks2 = readlines2 file
let priorities2 = rucksacks2 |> findIntersectOfChunk


printfn "Part1: %d" (priorities |> Seq.sum)
printfn "Part 2: %d" (priorities2 |> Seq.sum)
