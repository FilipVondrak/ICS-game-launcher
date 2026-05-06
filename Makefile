SOLUTION = ICSGameLauncher.slnx
DATA_PROJECT = ICSGameLauncher.Data
APP_PROJECT = ICSGameLauncher.App
CONFIGURATION = Release

.PHONY: all build clean restore test run win-publish migrate db-update remove-db

all: build

restore:
	dotnet restore $(SOLUTION)

build: restore
	dotnet build $(SOLUTION) -c $(CONFIGURATION) --no-restore

clean:
	dotnet clean $(SOLUTION)
	rm -rf ./artifacts*

test: restore
	dotnet test ICSGameLauncher.Tests -c $(CONFIGURATION) --verbosity normal --logger trx
	dotnet test ICSGameLauncher.BL.Tests -c $(CONFIGURATION) --verbosity normal --logger trx
	dotnet test ICSGameLauncher.DAL.Tests -c $(CONFIGURATION) --verbosity normal --logger trx

run: build
	dotnet run --project $(APP_PROJECT) -c $(CONFIGURATION) --no-build

win-publish: build
	dotnet publish $(APP_PROJECT)/$(APP_PROJECT).csproj -f net10.0-windows10.0.19041.0 -r win-x64 --self-contained -o ./artifacts-windows/publish-win
	./artifacts-windows/publish-win/$(APP_PROJECT).exe

# Usage: make migrate NAME=MyMigration
migrate:
	dotnet ef migrations add $(NAME) --project $(DATA_PROJECT) --startup-project $(DATA_PROJECT)

db-update:
	dotnet ef database update --project $(DATA_PROJECT) --startup-project $(DATA_PROJECT)

remove-db:
	dotnet ef database drop --project $(DATA_PROJECT) --startup-project $(DATA_PROJECT) --force

copy-to-vm:
	rsync -aP . /VirtualMachines/shared_folder/win11/ICSGameLauncher