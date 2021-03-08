open System
open System.IO
open System.Text.RegularExpressions

type Action = Direction * int 
and Direction =
    | North 
    | South
    | East
    | West
    | Left
    | Right
    | Forward

type BoatState = {
    Orientation : Direction
    Coordinates : int * int
}
with
    static member Initial = { Orientation = East; Coordinates = (0,0)}

let turn90Deg = function
    | North -> East
    | East -> South
    | South -> West
    | West -> North
    | _ -> failwith "incorrect orientation provided"

let rec updatePosition boatState action =
    let (x,y) = boatState.Coordinates 
    match action with
    | (Forward, n) -> updatePosition boatState (boatState.Orientation, n)
    | (North , n) -> { boatState with Coordinates = (x, y + n)}
    | (South, n) -> { boatState with Coordinates = (x, y - n)}
    | (East, n) -> { boatState with Coordinates = (x + n, y)}
    | (West, n) -> { boatState with Coordinates = (x - n, y)}
    
    | (turnDirection, n) -> 
        // normalize the turn to be a Right turn
        let n = if turnDirection = Left then 360 - n 
                else n
        let rotationCount = (n % 360) / 90
        let newOrientation = List.fold (fun orient _ -> turn90Deg orient) boatState.Orientation [1 .. rotationCount] //this is a little sketch
        { boatState with Orientation = newOrientation}


let turnWaypoint (wpx, wpy) = (wpy, -1 * wpx)

let rec updatePosition2 (state : {| BoatState : int*int; WaypointState: int*int |})  action =
    let (wpx, wpy) = state.WaypointState
    match action with
    | (Forward, n) -> 
        let (bx,by) = state.BoatState
        {| BoatState = (bx + (n*wpx), by + (n*wpy)); WaypointState = state.WaypointState |}
    | (North , n) -> {| BoatState = state.BoatState; WaypointState = (wpx, wpy + n) |}  
    | (South, n) ->  {| BoatState = state.BoatState; WaypointState = (wpx, wpy - n) |}
    | (East, n) -> {| BoatState = state.BoatState; WaypointState = (wpx + n, wpy) |}
    | (West, n) -> {| BoatState = state.BoatState; WaypointState = (wpx - n, wpy) |}
    | (turnDirection, n) ->
        // normalize the turn to be a Right turn
        let n = if turnDirection = Left then 360 - n 
                else n
        let rotationCount = (n % 360) / 90
        let newWaypointState = List.fold (fun wpi _ -> turnWaypoint wpi) state.WaypointState  [1 .. rotationCount]
        {| BoatState = state.BoatState; WaypointState = newWaypointState |}

    
let parseLine (line : string) =
    let r = Regex("(?<Direction>[NSEWLRF])(?<Number>\d+)")
    let m = r.Match line
    let n = int m.Groups.["Number"].Value
    match m.Groups.["Direction"].Value with
    | "N" -> (North, n)
    | "S" -> (South, n)
    | "E" -> (East, n)
    | "W" -> (West , n)
    | "L" -> (Left, n)
    | "R" -> (Right, n)
    | "F" -> (Forward, n)
    | _ -> failwith "Incorrect direction provided as input"

let parseFile file = file |> File.ReadAllLines |> Seq.map parseLine

[<EntryPoint>]
let main argv =
    
    let file = argv.[0]
    let actions = parseFile file

    let finalCoord = 
        actions 
        |> Seq.fold updatePosition BoatState.Initial
        |> fun boatState -> boatState.Coordinates

    let finalCoord2 = 
        actions 
        |> Seq.fold updatePosition2 {| BoatState = (0,0); WaypointState = (10,1)|}
        |> fun state -> state.BoatState

    
    finalCoord
    |> function (x,y) -> (abs x) + (abs y)
    |> printfn "Part 1 - Result %d" 

    finalCoord2
    |> function (x,y) -> (abs x) + (abs y)
    |> printfn "Part 2 - Result %d" 

    0 