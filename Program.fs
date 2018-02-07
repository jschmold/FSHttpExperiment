// Learn more about F# at http://fsharp.org

open MyApplication
open MyApplication.HttpFuncs
open System.Net


let rec httpLoop (http: HttpListener) (routes: Route list)=
    processContext http routes
    httpLoop http routes

[<EntryPoint>]
let main argv =
    argv |> ignore
    let listener = new HttpListener()
    initializeHttpListener listener Configuration.Routes
    httpLoop listener Configuration.Routes
    0 // return an integer exit code
