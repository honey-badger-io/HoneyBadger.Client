build:
	dotnet restore HoneyBadger.Client.sln
	dotnet build HoneyBadger.Client.sln -c $(c)

test: build
	dotnet test HoneyBadger.Client.sln

pack: build
	dotnet pack HoneyBadger.Client.sln --no-build -o ./nugets -c $(c)
