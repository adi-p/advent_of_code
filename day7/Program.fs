// Learn more about F# at http://fsharp.org

open System
open System.IO
open System.Text.RegularExpressions


type Bag = {
    Name : string
    Contents : BagContent
}
and BagContent = 
    | Empty
    | Bags of (int * string) list

let findAllPossibleParents goalBag bagMap =
    let rec findHelper (canReachGoal ,seenMap) bag =
        if bag = goalBag then
            (true, seenMap)
        else
            match Map.tryFind bag seenMap with
            | Some canReach ->
                (canReach, seenMap)
            | None ->
                (match (Map.find bag bagMap) with
                | Empty -> (false, seenMap)
                | Bags content ->
                    let childrenBags = [ for b in content -> snd b ]
                    let (canReachGoal ,seenMap) = 
                        List.fold (fun (cr, sm) b ->
                            let (newcr, seenbag) = findHelper (false, sm) b
                            (newcr || cr, seenbag)
                            ) 
                        <| (false, seenMap) <| childrenBags

                    let seenMap = Map.add bag canReachGoal seenMap
                    (canReachGoal, seenMap))

    let (_ , seenMap ) = Map.fold (fun acc b _ -> findHelper acc b) (false, Map.empty) bagMap
    seenMap |> Map.toList |> List.filter (fun (_, reach) -> reach) |> List.map fst


// Note: we assume that the input is correctly formatted (I'm ignoring warnings :/ )
let createMap bagAcc line =
    let parseLine line =
        let r = Regex("(.+) bags contain( (\d+|no) .+ bags?)")
        let m = r.Match line
        let [ name; contentStr; firstBagCount ] = List.tail [ for g in m.Groups -> g.Value ]
        
        let parseContents contentStr firstBagCount=
            match firstBagCount with
            | "no" -> Empty
            | _ -> 
                let r = Regex("(\d+) (.+?) bags?")
                let content = 
                    r.Matches contentStr
                    |> Seq.map (fun m -> 
                        let [ count; name ] = List.tail [ for g in m.Groups -> g.Value]
                        (int count, name))
                    |> List.ofSeq               
                Bags content

        {
            Name = name
            Contents = parseContents contentStr firstBagCount
        }        

    let bag = line |> parseLine
    Map.add bag.Name bag.Contents bagAcc


    

[<EntryPoint>]
let main argv =
    let file = argv.[0]
    let bagMap = 
        file
        |> File.ReadAllLines
        |> Array.fold createMap Map.empty


    let goalBag = "shiny gold"

    let possibleParents = findAllPossibleParents goalBag bagMap

    printfn "Total: %d" <| List.length possibleParents    

    0 // return an integer exit code
