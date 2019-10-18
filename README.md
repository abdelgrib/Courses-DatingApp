# DatingApp
ASP.NET Core / Angular Web App

1. Install : `ASP.NET Core SDK`, `Node.js`, `Git`
2. Install Angular/CLI : `npm install -g @angular/cli`
3. Clone the solution
4. Build to check the solution and create the database (could be created when running migrations)
5. Run migrations after cd to API directory `dotnet ef database update`
6. Install `nodes_modules` after cd to SPA directory `npm install`
7. Run the API, it will also seed the database `dotnet run` or `dotnet watch run`
8. Finally run the SPA `ng serve` and get the url.
