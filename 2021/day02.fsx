open System.IO


type Command = Direction * int
and Direction = 
  | Forward
  | Down
  | Up

let calculateCoordinates commands =
  commands
  |> Seq.fold 
    (fun coordinates command ->
      let (horizontal ,depth) = coordinates
      match command with
      | (Direction.Forward, n) -> (horizontal + n, depth)
      | (Direction.Down, n) -> (horizontal, depth + n)
      | (Direction.Up, n) -> (horizontal, depth - n)
    ) (0,0)


let calculateCoordinates2 commands =
  commands
  |> Seq.fold 
    (fun coordinates command ->
      let (horizontal, depth, aim) = coordinates
      match command with
      | (Direction.Forward, n) -> (horizontal + n, depth + (n*aim), aim)
      | (Direction.Down, n) -> (horizontal, depth, aim + n)
      | (Direction.Up, n) -> (horizontal, depth, aim - n)
    ) (0,0,0)

let readlines file =
  file
  |> File.ReadAllLines
  |> Seq.map (fun s -> 
    let [| direction; amount |] = s.Split ' '
    match direction with
    | "forward" -> (Direction.Forward, int amount)
    | "up" -> (Direction.Up, int amount)
    | "down" -> (Direction.Down, int amount)
    | _ -> failwith "Incorrect direction provided as input"
  )

let list = readlines "data/day02/input.txt"


let (x1,z1) = calculateCoordinates list
printfn "%d" (x1 * z1)
let (x2,z2, _) = calculateCoordinates2 list
printfn "%d" (x2 * z2)



  