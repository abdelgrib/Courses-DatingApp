# DatingApp
ASP.NET Core / Angular Web App

1. Install : `ASP.NET Core SDK`, `Node.js`, `Git`
2. Clone the solution
3. Build to check the solution and create the database (could be created when running migrations)
4. Run migrations after cd to API directory `dotnet ef database update`
5. Install `nodes_modules` after cd to SPA directory `npm install`
6. Run the API, it will also seed the database `dotnet run` or `dotnet watch run`
7. Finally run the SPA `ng serve` and get the url.
