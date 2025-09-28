namespace RGit.InitCommand;

public sealed record InitCommandModel();

public static class InitCommand
{
    public static readonly RGitCommandType CommandType = RGitCommandType.Init;

    public static void Execute()
    {
        var rgitConfigPath = Path.Combine(Directory.GetCurrentDirectory(), ".rgit");
        if (File.Exists(rgitConfigPath))
        {
            Console.WriteLine("Folder already exists.");
            return;
        }

        Directory.CreateDirectory(rgitConfigPath);
    }
}