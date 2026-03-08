# README

## Projects description
- uses classical layered architecture
### *ICSGameLauncher.App*
- MAUI frontend
### *ICSGameLauncher.Core*
- bussiness layer for the app
### *ICSGameLauncher.Data*
- data access layer with EF Core
- uses SQLite database
### *ICSGameLauncher.Tests*
- unit tests
### *ICSGameLauncher.Common*
- shared code

## Running the app
- create a `.env` file based on `.env.example`
- run `make win-publish` to build the app and run it

## Migrations
- run `make db-update` to update the database
- run `make migrate NAME=MyMigration` to create a new migration
- run `make remove-db` to drop the database