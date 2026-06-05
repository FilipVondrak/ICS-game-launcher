# README

## Projects description
- uses classical layered architecture
### *ICSGameLauncher.App*
- MAUI frontend
### *ICSGameLauncher.BL*
- bussiness layer for the app
- contains facades, services and mapster mappings
### *ICSGameLauncher.DAL*
- data access layer with EF Core
- contains db models and repositories
- uses SQLite database
### *Unit tests*
- ICSGameLauncher.Tests
- ICSGameLauncher.DAL.Tests
- ICSGameLauncher.BL.Tests
### *ICSGameLauncher.Common*
- shared code

## Running the app
- create a `.env` file based on `.env.example`
- run `make win-publish` to build the app and run it

## Migrations
- run `make db-update` to update the database
- run `make migrate NAME=MyMigration` to create a new migration
- run `make remove-db` to drop the database
