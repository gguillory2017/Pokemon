# Pokemon type application
This application uses API calls to the [PokeAPI](https://pokeapi.co/) to display type weaknesses/strengths for the selected Pokemon.

## Running the application
- Clone this repository
- Double click `Pokemon\bin\Debug\net9.0\Pokemon.exe`
- Alternatively, open the repository in a C# IDE of your choice and run it from there. 

## Using the application
- On starting the application, you will be given the following commands to choose from:
    - `c. check pokemon name` - confirms that the name selected is a valid pokemon, but does not print type information
    - `t. check pokemon damage types` - prints type information for a valid pokemon name
    - `q. quit` - terminates the program 
- The application will not stop running when given erroneous commands or pokemon names. It will only stop if given the `q` command or with a kill signal (`ctrl + c`)