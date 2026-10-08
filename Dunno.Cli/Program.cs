using Dunno.Cli;
using Dunno.Cli.App;
using Dunno.Cli.Commands;
using Dunno.Cli.Repositories;
using Dunno.Cli.Services;

var databasePath = DatabasePathResolver.Resolve();
var connectionString = $"Data Source={databasePath}";

var dbInitializer = new DatabaseInitializer(connectionString);
await dbInitializer.InitializeAsync();

var commandRepository = new CommandRepository(connectionString);
var tagRepository = new TagRepository(connectionString);

var tagService = new TagService(tagRepository);
var commandService = new CommandService(commandRepository, tagService);

var listCommand = new ListCommand(commandService);
var addCommand = new AddCommand(commandService);
var tagCommand = new TagCommand(tagService);

var app = new CliApp(listCommand, addCommand, tagCommand);

return await app.RunAsync(args);