open System.IO

let maxValue map = map |> Map.toSeq |> Seq.maxBy (fun (_, value) -> value)
let minValue map = map |> Map.toSeq |> Seq.minBy (fun (_, value) -> value)


let binaryStringToInt str =  System.Convert.ToInt32(str, 2)
let binaryCharSeqToInt (seq :seq<char>) = seq |> Seq.toArray |> System.String |> binaryStringToInt


let getBitCounts list = 
  let len = Seq.length (Seq.head list)
  let initMaps = Seq.init len (fun _ -> Map [ ('0', 0); ('1', 0); ])
  list
  |> Seq.fold (fun maps num -> 
    Seq.zip maps num
    |> Seq.map (fun (m, digit) -> 
      let curCount = Map.find digit m
      Map.add digit (curCount + 1) m
    ) 
  ) initMaps

let getGammaEpsilon list =
  let bitCounts = list |> getBitCounts
  // This could probably be cleaner, lol
  let gamma = bitCounts |> Seq.map maxValue |> Seq.map fst |> binaryCharSeqToInt
  let epsilon = bitCounts |> Seq.map minValue |> Seq.map fst |> binaryCharSeqToInt
  (gamma, epsilon)

let invertBit bit = match bit with '0' -> '1' | '1' -> '0'

let charToInt c = int c - int '0'

//this could be nicer
let getCO2andOxygen list =
  let getMostCommonFirstBit (list : seq<seq<char>>) =
    let firstBits = list |> Seq.map Seq.head
    let bitSum = firstBits |> Seq.fold (fun sum bit -> sum + (charToInt bit)) 0
    match (double)bitSum >= (double)(Seq.length list) / 2.0 with
    | true -> '1'
    | false -> '0'

  let rec getCO2andOxygenHelper bitFunction position remaining =
    match remaining |> Seq.toList with 
    | [el] -> el |> binaryCharSeqToInt
    | lst -> 
      let bit = remaining |> Seq.map (Seq.skip position) |> bitFunction

      let remaining = lst |> Seq.filter (fun el -> el |> Seq.skip position |> Seq.head |> fun el -> el = bit)
  
      getCO2andOxygenHelper bitFunction (position + 1) remaining

  let oxygen = list |> getCO2andOxygenHelper (getMostCommonFirstBit) 0
  let co2 = list |> getCO2andOxygenHelper (fun list -> list |> getMostCommonFirstBit |> invertBit) 0
  (oxygen, co2)


    

let readlines file =
  file
  |> File.ReadAllLines
  |> Seq.map Seq.toList


let list = readlines "data/day03/input.txt"

let (gamma, epsilon) = getGammaEpsilon list
printfn "%d * %d = %d" gamma epsilon (gamma * epsilon)

let (oxygen, co2) = getCO2andOxygen list
printfn "%d * %d = %d" oxygen co2 (oxygen * co2)



  