# blazoract

:warning: Blazoract is currently under-development.

Blazoract is an interactive notebook user interface implemented in Blazor WebAssembly. It combines some features in Blazor released in .NET 5, such as virtualization and CSS isolation, and includes a kernel backend powered by .NET Interactive.

## Development Setup

1. Fork and clone this repository locally using Git.

```
$ git clone https://github.com/{yourusername}/blazoract
```

2. Restore the project's dependencies by running `dotnet restore` in the root.

3. Run `dotnet run --project Server` to launch the application server.

4. Navigate to https://localhost:5001 where the contents of the default notebook should load.

![Screen Shot 2020-10-25 at 10 03 24 PM](https://user-images.githubusercontent.com/1857993/97135602-f194b300-170d-11eb-87e4-af81bda68ad5.png)

## License

[MIT](https://choosealicense.com/licenses/mit/)