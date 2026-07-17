using Dunno.Cli.CLI;
using Dunno.Cli.Commands;
using Dunno.Cli.Services;

var service = new CommandService();

var listCommand = new ListCommand(service);

var app = new CliApp(listCommand);

return app.Run(args);