run: 
	if [ ! -d "frontend/node_modules" ]; then cd frontend && npm ci && cd ..; fi
	dotnet run --project backend/backend.csproj
test:
	cd frontend && npm test -- --watchAll=false && cd .. && dotnet test
build:
	dotnet build 
clean:
	dotnet clean && rm -r frontend/node_modules/
restore:
	dotnet restore
