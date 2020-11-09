# blazoract

Blazoract is an interactive notebook user interface implemented in Blazor WebAssembly. It combines some features in Blazor released in .NET 5, such as virtualization and CSS isolation, and includes a kernel backend powered by [.NET Interactive](https://github.com/dotnet/interactive).

## Development Setup

Before starting development, be sure that you have the following installed:

- [.NET Core](https://dotnet.microsoft.com/download)
- [Azure Functions Core Tools](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local#install-the-azure-functions-core-tools)

1. Fork and clone this repository locally using Git.

```
$ git clone https://github.com/{yourusername}/blazoract
```

2. Restore the project's dependencies by running `dotnet restore` in the root.

3. Open a terminal and run the following to launch a local instance of the Azure Functions for this app. You will need to the Azure Functions Core Tools mentioned above to enable this.

```
$ cd Api
$ func start --build
```

4. In another terminal window, run `dotnet run --project Client` to start the client application.

5. Navigate to https://localhost:5001 where the contents of the default notebook should load.

![image](https://user-images.githubusercontent.com/1857993/98490947-18f87f00-21e8-11eb-889f-db78c79b5a9b.png)

## License

[MIT](https://choosealicense.com/licenses/mit/)
