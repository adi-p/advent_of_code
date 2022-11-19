open System.IO
open System
open System.Text.RegularExpressions

type Point = { X: int; Y: int; }

let isHorizontal (p1, p2) = p1.X = p2.X
let isVertical (p1, p2) = p1.Y = p2.Y

//assumes 45deg for diagonal lines
let getLinePoints (p1: Point) (p2: Point) =
  let yDelta = p2.Y - p1.Y
  let xDelta = p2.X - p1.X
  let length = max (Math.Abs yDelta) (Math.Abs xDelta)
  seq {
    for i in 0..length do
      yield { X = p1.X + i * (xDelta/length); Y =  p1.Y + i * (yDelta/length)}
  }

let addLineCovering (covering: Map<Point, int>) (line: Point * Point) =
  line
  ||> getLinePoints
  |> Seq.fold (fun covering point -> 
    match Map.tryFind point covering with 
    | Some v -> covering.Add (point, (v + 1))
    | None -> covering.Add (point, 1)
  ) covering

let findCoverings lines =
  let coveringMap = 
    lines 
    |> Seq.fold addLineCovering Map.empty

  coveringMap
  |> Map.toSeq 
  |> Seq.filter (fun (_, coverCount) -> coverCount >= 2) 
  |> Seq.length


// Input utility //
let parseLine line =
    let r = Regex("(?<x1>\d+),(?<y1>\d+) -> (?<x2>\d+),(?<y2>\d+)")
    let m = r.Match line
    let p1 = { X = int m.Groups.["x1"].Value; Y = int m.Groups.["y1"].Value; }
    let p2 = { X = int m.Groups.["x2"].Value; Y = int m.Groups.["y2"].Value; }
    (p1,p2)

let readlines file =
  file 
  |> File.ReadAllLines
  |> Seq.map parseLine

// Entry //

let lines = readlines "data/day05/input.txt"

let verticalAndHorizontal = lines |> Seq.filter (fun line -> line |> isHorizontal || line |> isVertical)
let coverCount = findCoverings verticalAndHorizontal

printfn "%d" coverCount

let coverCount2 = findCoverings lines
printfn "%d" coverCount2