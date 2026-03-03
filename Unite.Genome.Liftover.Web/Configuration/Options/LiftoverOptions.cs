namespace Unite.Genome.Liftover.Web.Configuration.Options;

public class LiftoverOptions
{
    public string ChainsDirectoryPath => Environment.GetEnvironmentVariable("UNITE_CHAINS_DIRECTORY_PATH");
}
