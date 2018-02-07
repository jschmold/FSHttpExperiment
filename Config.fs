namespace MyApplication

open MyApplication.HttpFuncs
open System.Net

module Configuration =
    let HomePath (context: HttpListenerContext) = 
        "Welcome home" |> textResponse context 200
    let AboutPath (ctx: HttpListenerContext) =
        "Well that's sure something else" |> textResponse ctx 200
    let CuriousPath (ctx: HttpListenerContext) = 
        "Well, aren't you are curious thing" |> textResponse ctx 200

    let Routes = [ 
        { Path = "home/"; Handler = HomePath }; 
        { Path = "about/"; Handler = AboutPath };
        { Path = "curious/"; Handler = CuriousPath };
        { Path = ""; Handler = HomePath };
    ]
