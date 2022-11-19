open System.IO
open System

// Board utility //

type CellStatus =
    | UnMarked
    | Marked

let BOARD_WIDTH = 5
let BOARD_HEIGHT = 5

let getRow (row: int) (board: (int * CellStatus) []) =
  seq {
    for i in 0 .. (BOARD_WIDTH - 1) do
      yield board.[BOARD_HEIGHT*row + i]
  }

let getCol (col: int) (board: (int * CellStatus) []) =
  seq {
    for i in 0 .. (BOARD_HEIGHT - 1) do
      yield board.[col + i*BOARD_WIDTH]
  }

// Main Solution -- Helpers //

let findScore drawNumber board =
  let sumOfUnmarked = 
    board 
    |> Seq.filter (fun (_, status) -> status = CellStatus.UnMarked) 
    |> Seq.map fst
    |> Seq.sum
  sumOfUnmarked * drawNumber

// mutates the board
let markBoard drawNumber (board: (int * CellStatus) []) =
  for i in 0 .. (BOARD_HEIGHT*BOARD_WIDTH - 1) do
    if (fst board.[i]) = drawNumber then
      Array.set board i (fst board.[i], CellStatus.Marked)

let isWinningBoard (board: (int * CellStatus) []) =
  let isWinningSequence = Seq.exists (fun (_, status) -> status = CellStatus.UnMarked) >> not
  seq {
    for i in 0 .. (BOARD_HEIGHT - 1) do
      yield getRow i board
    for i in 0 .. (BOARD_WIDTH - 1) do
      yield getCol i board
  } 
  |> Seq.exists isWinningSequence

// Main Solution //

let findFirstWinningBoardScore drawNumbers boards =
  let listboard = Seq.toList boards
  seq {
    for number in drawNumbers do
      listboard |> List.iter (fun board -> markBoard number board)
      yield! listboard |> List.map (fun b -> (number, b))
  } 
  |> Seq.find (fun (_, board) -> board |> isWinningBoard) // assumes there will be a solution at some point - will short circuit
  ||> findScore

//this is a little messy
let findLastWinningBoardScore drawNumbers boards =
  drawNumbers
  |> Seq.fold (fun (lastWinningBoard, remainingBoards) number ->
    remainingBoards 
    |> List.iter (fun board -> markBoard number board);
    let (winningBoards, losingBoards) = remainingBoards |> List.partition (isWinningBoard)
    match winningBoards with 
    | [] -> (lastWinningBoard, losingBoards)
    | head :: rest -> (Some((number, head)), losingBoards)
  ) (None , Seq.toList boards)
  |> (fun (lastWinner, _) -> 
    match lastWinner with 
    | Some((number, board)) -> findScore number board
    | None -> 0) // maybe I should even 'failwith' here



// Input utility //

let readlines file =
  let splitIntoBoards = Seq.chunkBySize 6
  let linesToBoard lines =
    lines
    |> Seq.fold (fun boardArray line -> 
      match line with 
      | "" -> boardArray
      | _ ->
        let parsedLine = 
          line.Split(" ", StringSplitOptions.RemoveEmptyEntries) 
          |> Array.map (fun s -> ((int (s.Trim())), CellStatus.UnMarked))
        Array.append boardArray parsedLine //this might not be great in terms of efficiency
    ) [||]

  let lines = file |> File.ReadAllLines

  let draws = 
    lines
    |> Seq.head 
    |> fun (line : string) -> line.Split "," 
    |> Seq.map (fun el -> int el)

  let boards =
    lines
    |> Seq.skip 1
    |> splitIntoBoards
    |> Seq.map linesToBoard
  
  (draws, boards)



// Entry //

let (drawNumbers, boards) = readlines "data/day04/input.txt"
let firstWinningBoardS = findFirstWinningBoardScore drawNumbers boards
printfn "%d" firstWinningBoardS

let lastWinningBoardScore = findLastWinningBoardScore drawNumbers boards
printfn "%d" lastWinningBoardScore





  