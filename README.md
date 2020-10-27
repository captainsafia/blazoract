# blazoract

:warning: Blazoract is currently under-development.

Blazoract is an interactive notebook user interface implemented in Blazor WebAssembly. It combines some features in Blazor released in .NET 5, such as virtualization and CSS isolation, and includes a kernel backend powered by .NET Interactive.

## Development Setup

Before starting development, be sure that you have the following installed:

- [.NET Core](https://dotnet.microsoft.com/download)
- [Azure Functions Core Tools](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local#install-the-azure-functions-core-tools)

1. Fork and clone this repository locally using Git.

```
$ git clone https://github.com/{yourusername}/blazoract
```

1. Restore the project's dependencies by running `dotnet restore` in the root.

1. Copy the sample configuration file for running Azure functions locally.

```
$ cp `Api/local.settings.example.json` file into `Api/local.settings.json`
```

1. Open a terminal and run the following to launch a local instance of the Azure Functions for this app. You will need to the Azure Functions Core Tools mentioned above to enable this.

```
$ cd Api
$ func start --build
```

1. In another terminal window, run `dotnet run --project Client` to start the client application.

1. Navigate to https://localhost:5001 where the contents of the default notebook should load.

![Screen Shot 2020-10-25 at 10 03 24 PM](https://user-images.githubusercontent.com/1857993/97135602-f194b300-170d-11eb-87e4-af81bda68ad5.png)

## License

[MIT](https://choosealicense.com/licenses/mit/)