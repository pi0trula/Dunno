using Dunno.Cli.App;
using Dunno.Cli.Commands;
using Dunno.Cli.Repositories;
using Dunno.Cli.Services;

const string connectionString = "Data Source=dunno.db";

var dbInitializer = new DatabaseInitializer(connectionString);
await dbInitializer.InitializeAsync();

var commandRepository = new CommandRepository(connectionString);
var tagRepository = new TagRepository(connectionString);

var commandService = new CommandService(commandRepository);
var tagService = new TagService(tagRepository);

var listCommand = new ListCommand(commandService);
var tagCommand = new TagCommand(tagService);

var app = new CliApp(listCommand, tagCommand);

return await app.RunAsync(args);