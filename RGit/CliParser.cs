namespace RGit;

public sealed record CliParserModel(string[] CommandLineArguments);

public static class CliParser
{
    public static CliParserModel Create(string[] commandLineArguments) => 
        new(commandLineArguments);

    extension(CliParserModel cliParserModel)
    {
        public string GetCommand() =>
            cliParserModel.CommandLineArguments.First();
    }
}
