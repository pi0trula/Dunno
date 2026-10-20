# Dunno

A CLI for saving shell commands that are easy to forget, with a description and tags, so there's no need to google them again.

.NET 10, SQLite, Dapper.

## Try it

```bash
dotnet run --project Dunno.Cli -- add docker --args "ps -a" --desc "all containers, even stopped" --tags "docker,containers"
dotnet run --project Dunno.Cli -- list
dotnet run --project Dunno.Cli -- list docker
dotnet run --project Dunno.Cli -- list tag docker
dotnet run --project Dunno.Cli -- tag
```

Output:

```
1. docker ps -a
   all containers, even stopped
   #docker #containers
```

The database is created automatically at `~/.local/share/dunno/dunno.db`. Set `DUNNO_DB` to put it somewhere else.

## Tests

```bash
dotnet test
```
