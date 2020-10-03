run:
	dotnet run --project backend/backend.csproj
test:
	dotnet test && cd frontend && npm test -- --watchAll=false && cd ..
build:
	dotnet build
clean:
	dotnet clean
restore:
	dotnet restore
