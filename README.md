# FSHttpExperiment

To build, ensure you are running .Net Core 2.0 +
`dotnet restore`
`dotnet build`

The routes registered are in Config.fs, and call simple helper functions to make returning text easier.
They generally do not do much more than demonstrate a single-threaded HTTP application. Will be updating this experiment in the future for multithreading.
