



// Demo - module 1










// assignments
let x = 10
x = 20



































// mutability
let mutable y = 10
y <- 20





































// tuples
open System
let tuple = 10, "tomas"
let num, str = tuple
let result, value = Int32.TryParse("99")





































// records
type Person = {Age: int; Name: string}
let me = {Age = 33; Name = "Tomas"} // type inference!
let modifiedMe = {me with Age = 32}
me = modifiedMe
let otherMe = {Age = 33; Name = "Tomas"}
me = otherMe

























// functions
let add x y = x + y
add
add 1 2






















// Demo - module 2



// higher order functions

let printer printFun x = sprintf "%s" (printFun x)
let spacePrinterFun x = "                     " + x.ToString() 

let spacePrinter x = printer spacePrinterFun x
spacePrinter "tomas"

let doublePrinter = printer (fun x -> (2*x).ToString()) // <-- returns a new function
doublePrinter 10

















let add x y = x + y
let mul x y = x * y

// partial applications
let add10 = add 10
let mul2 = mul 2

// pipelining
2 |> add10 |> mul2 |> mul 3

// (mul 3 (mul2 (add10 2)) --> (mul 3 (mul 2 (add 10 2)))






















// compositions
let add10mul2 = add10 >> mul2

add10mul2 2
2 |> add10mul2

































// Demo - module 3


// Options
let divide x y = if y = 0 then None else Some (x/y)
let result = divide 10 0

if result |> Option.isNone then printfn "division error" else printfn "The result is %i"  result.Value




























// Discriminated union

type Result<'TVal> = 
    | Success of 'TVal
    | Error of string

let bind func value =
    match value with
    | Success x -> func x
    | Error str -> Error str

let div100 y =
    match y with
    | 0 -> Error "Divsion error"
    | i -> Success (100/y)

let validator x =
    match x with
    | i when i > 10 -> Error "Wow, take it easy! The input is not 10"
    | i -> Success i

let print x =
    match x with
    | Success yolo -> printfn "The result is: %i" yolo
    | Error s -> printfn "Something bad happened %s" s


10 |> validator |> (bind div100) |> (bind (validator)) |> print;;
















// Measure types

type [<Measure>] SEK
type [<Measure>] NOK

let nok2Sek (nok:float<NOK>) = 1.<SEK> * System.Math.Round((1.1 * nok / 1.<NOK>), 1)

100.<NOK> = 100.<SEK>
nok2Sek 100.<NOK> = 100.<SEK>

































// Demo - Module 4


// Lists and prime calculation
let numbers = [1 .. 2 .. 100]

// Not optimal prime implementation
let isPrime x = 
    [2 .. (x - 1)/2] 
    |> List.tryFind (fun i -> x % i = 0)
    |> Option.isNone

numbers |> List.filter isPrime

// List comprehension
let primes = [for n in 1 .. 2 .. 100 do if isPrime n then yield n]



















// Recursion
let takeEverySnd list = 
    let rec takeEverySnd tookLast xs result =
        match tookLast, xs with
        | _, [] -> result |> List.rev
        | false, head::tail -> takeEverySnd true tail (head::result)
        | true, _::tail -> takeEverySnd false tail result
    takeEverySnd false list []

primes |> takeEverySnd






























// Classes, why oh why?
type DoThisOnYourOwnRisk(x) = 
    member this.X = x

type IDoReturnInt = 
    abstract member ReturnInt: int -> int

type ReturnIntImpl() = 
    let z = 54
    member this.OneMoreNumber = 2
    interface IDoReturnInt with
        member this.ReturnInt x = 100 - x + z + this.OneMoreNumber

let intInstance = 
    {
        new IDoReturnInt with
        member this.ReturnInt x = 100 + x
    }

intInstance.ReturnInt(12)
(ReturnIntImpl() :> IDoReturnInt).ReturnInt(20)


















// type providers - CSV
#r "lib/FSharp.Data.dll"
open FSharp.Data

[<Literal>]
let sample = """animal,legs,canfly
dog,4,false
cow,4,false
bird,2,true
human,2,false
fish,0,false
dragon,4,yes
"""

type MyCSVType = CsvProvider<sample,",">
let data = MyCSVType.Parse(sample)
data.Rows 
|> Seq.toList 
|> List.filter (fun i -> not i.Canfly) 
|> List.filter (fun i -> i.Legs > 2)
|> List.map (fun i -> printfn "%s %i" i.Animal i.Legs)























































































































