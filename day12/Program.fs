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

type BoatInformation = {
    Orientation : Direction
    Coordinates : int * int
}

let turn90Deg = function
    | North -> East
    | East -> South
    | South -> West
    | West -> North
    | _ -> failwith "incorrect orientation provided"

let rec updatePosition boatInfo action =
    let (x,y) = boatInfo.Coordinates 
    match action with
    | (Forward, n) -> updatePosition boatInfo (boatInfo.Orientation, n)
    | (North , n) -> { boatInfo with Coordinates = (x, y + n)}
    | (South, n) -> { boatInfo with Coordinates = (x, y - n)}
    | (East, n) -> { boatInfo with Coordinates = (x + n, y)}
    | (West, n) -> { boatInfo with Coordinates = (x - n, y)}
    
    | (turnDirection, n) -> 
        // normalize the turn to be a Right turn
        let n = if turnDirection = Left then 360 - n 
                else n
        let rotationCount = (n % 360) / 90
        let newOrientation = List.fold (fun orient _ -> turn90Deg orient) boatInfo.Orientation [1 .. rotationCount] //this is a little sketch
        { boatInfo with Orientation = newOrientation}


let turnWaypoint (wpx, wpy) = (wpy, -1 * wpx)

let rec updatePosition2 boatInfo waypointInfo action =
    let (wpx, wpy) = waypointInfo
    match action with
    | (Forward, n) -> 
        let (bx,by) = boatInfo
        {| BoatInfo = (bx + (n*wpx), by + (n*wpy)); WaypointInfo = waypointInfo |}
    | (North , n) -> {| BoatInfo = boatInfo; WaypointInfo = (wpx, wpy + n) |}  
    | (South, n) ->  {| BoatInfo = boatInfo; WaypointInfo = (wpx, wpy - n) |}
    | (East, n) -> {| BoatInfo = boatInfo; WaypointInfo = (wpx + n, wpy) |}
    | (West, n) -> {| BoatInfo = boatInfo; WaypointInfo = (wpx - n, wpy) |}
    | (turnDirection, n) ->
        // normalize the turn to be a Right turn
        let n = if turnDirection = Left then 360 - n 
                else n
        let rotationCount = (n % 360) / 90
        let newWaypointInfo = List.fold (fun wpi _ -> turnWaypoint wpi) waypointInfo  [1 .. rotationCount]
        {| BoatInfo = boatInfo; WaypointInfo = newWaypointInfo |}

let getFinalCoordinates actions =
    let rec applyActions boatInfo actions  =
        match actions with
        | [] -> boatInfo.Coordinates
        | firstAction::rest ->
            let newBoatInfo = updatePosition boatInfo firstAction
            applyActions newBoatInfo rest
    applyActions { Orientation = East; Coordinates = (0,0)} (List.ofSeq actions)

let getFinalCoordinates2 actions =
    let rec applyActions2 boatInfo waypointInfo actions =
        match actions with
        | [] -> boatInfo
        | firstAction::rest ->
            let newInfos = updatePosition2 boatInfo waypointInfo firstAction
            applyActions2 newInfos.BoatInfo newInfos.WaypointInfo rest
   
    applyActions2 (0,0) (10,1) (Seq.toList actions)

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
    let finalCoord = getFinalCoordinates actions

    let finalCoord2 = getFinalCoordinates2 actions

    // let (x,y) = finalCoord
    // printfn "x : %d y : %d" x y

    finalCoord
    |> function (x,y) -> (abs x) + (abs y)
    |> printfn "Part 1 - Result %d" 

    finalCoord2
    |> function (x,y) -> (abs x) + (abs y)
    |> printfn "Part 2 - Result %d" 

    0 