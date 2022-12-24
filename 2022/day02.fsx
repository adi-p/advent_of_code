open System.IO
open System


type Move =
| Rock
| Paper
| Scissors

let compareMove move1 move2 =
  match (move1, move2) with
  | (x, y) when x = y  -> 0
  | (Rock, Paper) -> -1
  | (Rock, Scissors) -> 1
  | (Paper, Rock) -> 1
  | (Paper, Scissors) -> -1
  | (Scissors, Rock) -> -1
  | (Scissors, Paper) -> 1

let getMove letter =
  match letter with
  | "A" | "X" -> Rock
  | "B" | "Y" -> Paper
  | "C" | "Z" -> Scissors
  | _ -> failwith "Incorrect input character" 

let getRequiredMove opponentMove requiredResult =
  match requiredResult with
  | "Y" -> opponentMove // Draw
  | "X" -> // Lose
    match opponentMove with
    | Rock -> Scissors
    | Paper -> Rock
    | Scissors -> Paper
  | "Z" -> // Win
    match opponentMove with
    | Rock -> Paper
    | Paper -> Scissors
    | Scissors -> Rock
  | _ -> failwith "non existing requirement"

let getMoveScore move =
  match move with
  | Rock -> 1
  | Paper -> 2
  | Scissors -> 3

let getWinScore opponentMove move =
  match compareMove move opponentMove with
  | -1 -> 0
  | 0 -> 3
  | 1 -> 6
  | _ -> failwith "impossible compare score"

let calculateScore rounds =
  rounds
  |> Seq.map (fun (move1, move2) -> (getMoveScore move2) + (getWinScore move1 move2))
  |> Seq.sum



let readlines file =
  file 
  |> File.ReadAllLines
  |> Seq.fold (fun rounds line ->
    let [| move1; move2; |] = line.Split(" ")

    (move1, move2)::rounds
  ) []

let rounds = readlines "data/day02/input.txt"
printfn "Part1: %d" (rounds |> Seq.map (fun (m1, m2) -> (getMove m1, getMove m2)) |> calculateScore)
printfn "Part2: %d" (rounds |> Seq.map (fun (move, requirement) -> (getMove move, getRequiredMove (getMove move) requirement)) |> calculateScore)
