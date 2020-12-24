// Learn more about F# at http://fsharp.org

open System
open System.IO
open System.Text.RegularExpressions


// idk if we really need this...
type Field =
    | BirthYear
    | IssueYear
    | ExpirationYear
    | Height
    | HairColour
    | EyeColour
    | PassportId
    | CountryId

    static member GetRequired() =
        [
            yield BirthYear
            yield IssueYear
            yield ExpirationYear
            yield Height
            yield HairColour
            yield EyeColour
            yield PassportId
        ]

let fieldToAbriviation = function
    | BirthYear -> "byr"
    | IssueYear -> "iyr"
    | ExpirationYear -> "eyr"
    | Height -> "hgt"
    | HairColour -> "hcl"
    | EyeColour -> "ecl"
    | PassportId -> "pid"
    | CountryId -> "cid"


let isValid fields =
    let parseField str =
        let r = new Regex("([^\s]*):([^\s]*)")
        let m = r.Match str
        let groupVals = m.Groups |> Array.ofSeq |> Array.map (fun g -> g.Value)
        (groupVals.[1], groupVals.[2])

    let rec checkRequired requiredList map =
        match requiredList with
        | (x::xs) ->
            match Map.tryFind x map with
            | Some(_) -> checkRequired xs map // Should do validation for each field here.
            | None -> false
        | [] -> true

    fields
    |> List.map parseField
    |> List.fold (fun m (field, value) -> Map.add field value m) Map.empty
    |> checkRequired (List.map fieldToAbriviation (Field.GetRequired()) )


let parseFile file =
    let accumulatePassportFields (passports, curPassport) line =
        // printfn "curpass: %A" curPassport 
        if line = "" then
            (curPassport::passports, [])
        else 
            let additionalFields =
                line.Split ' ' 
                |> Seq.toList           
            (passports, additionalFields @ curPassport)

    // the 'lastPassport' part is kinda ugly ...
    let (passports, lastPassport) =
        file
        |> File.ReadAllLines
        |> Array.fold accumulatePassportFields ([],[])
    lastPassport::passports    
     

[<EntryPoint>]
let main argv =
    
    let file = argv.[0]
    
    let passport = file |> parseFile
        
    printfn "Valid passport count: %d" (passport |> List.filter isValid |> List.length)

    0 // return an integer exit code
