namespace MyApplication


open System.Net
open System.Net.Http

module HttpFuncs = 
    // The route type
    type Route = { Path: string; Handler: HttpListenerContext -> unit }

    // Applies a uint32 code to a response, and closes
    let flushResponse (ctx: HttpListenerContext) code =
        ctx.Response.StatusCode <- code
        ctx.Response.Close()

    // Creates a "text only" response. Lazy function
    let textResponse (context: HttpListenerContext) code str = 
        let content = (new StringContent(str))
        content.CopyToAsync context.Response.OutputStream
        |> Async.AwaitTask
        |> Async.RunSynchronously
        flushResponse context code

    // The handler that takes care of errors
    let errorHandler (context: HttpListenerContext) =
        ("ERROR! Your path " + context.Request.Url.AbsolutePath + " was not valid") 
        |> textResponse context 404 

    // The Route instance that is called whenever there's an error
    let ErrorRoute = {
        Path = "ERR";
        Handler = errorHandler
    }
    
    // trim excess slashes on both sides of a route
    let pathFormat (itm: Route) =
        match itm.Path.Length with
        | x when x = 0 -> itm
        | _ -> { Handler = itm.Handler; Path = itm.Path.Trim '/' }

    // A string with its edges trimmed and the path with its edges trimmed equal each other
    let pathRoughlyMatches (other: string) (path: Route) =
        match other.Trim '/' with 
        | x when (pathFormat path).Path = x -> true
        | _ -> false

    // First route in a list of routes, or the error provided
    let headRouteOrError (err: Route) (route: Route list) =
        match route with
        | x when List.isEmpty x -> err
        | _ -> List.head route

    // Actually does the routing. 
    let performRoute (route: Route list) (context: HttpListenerContext) =
        route
        |> List.filter (pathRoughlyMatches context.Request.Url.AbsolutePath)
        |> headRouteOrError ErrorRoute
        |> fun rt -> rt.Handler context

    // To be used in the main loop for 
    let rec processContext (http: HttpListener) routes =
        http.GetContextAsync()
        |> Async.AwaitTask
        |> Async.RunSynchronously
        |> performRoute routes

    // Add a route to the prefixes on port 8080
    let applyRoute (http: HttpListener) (str: Route) =
        http.Prefixes.Add ("http://localhost:8080/" + str.Path)

    // Start the httplistener and add all routes given
    let initializeHttpListener (http: HttpListener) (routes: Route list) =
        http.Start()
        routes |> List.iter (applyRoute http)

        
