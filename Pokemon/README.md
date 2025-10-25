# Pokemon type application
This application uses API calls to the [PokeAPI](https://pokeapi.co/) to display type weaknesses/strengths for the selected Pokemon.

## Running the application
With [.NET SDK 9.0.306](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) installed:
1. Clone this repository
2. Navigate to the root directory of the repository (it should have the `Pokemon.csproj` file in it) from the command line
3. Run the command `dotnet build`
4. Double click the `Pokemon\bin\Debug\net9.0\Pokemon.exe` file

## Using the application
- On starting the application, you will be given the following commands to choose from:
    - `c. check pokemon name` - confirms that the name selected is a valid pokemon, but does not print type information
    - `t. check pokemon damage types` - prints type information for a valid pokemon name
    - `q. quit` - terminates the program 
- The application will not stop running when given erroneous commands or pokemon names. It will only stop if given the `q` command or with a kill signal (`ctrl + c`)