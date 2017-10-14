# Templating Engine

The templating engine reads templates, is passed a view model and returns a string.

TemplateEngine
    Compile(path) -> Template

Template
    string CompilerJSPath
    Task<string> ReadFromDisk
    Render(viewmodel) ->
        ReadFromDisk.Wait()
        Res => NodeServices.Run(CompilerJSPath, ReadFromDisk.Result, viewmodel)
        Res.Wait()
        return Res.Result
