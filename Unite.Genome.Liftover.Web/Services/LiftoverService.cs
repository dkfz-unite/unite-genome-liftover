using System.Diagnostics;
using Unite.Genome.Liftover.Web.Configuration.Options;

namespace Unite.Genome.Liftover.Web.Services;

public class LiftoverService
{
    private readonly LiftoverOptions _options;

    public LiftoverService(LiftoverOptions options)
    {
        _options = options;
    }

    public async Task Liftover(string from, string to, string inputFile, string outputFile, string unmappedFile)
    {
        var chainFile = GetChainFile(from, to);

        try
        {
            using var process = PrepareProcess(inputFile, outputFile, unmappedFile, chainFile);

            process.Start();

            await process.WaitForExitAsync();

            if (process.ExitCode != 0)
            {
                var errorOutput = await process.StandardError.ReadToEndAsync();
                
                throw new Exception($"Liftover process failed with exit code {process.ExitCode}. Error output: {errorOutput}");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Liftover process failed: {ex.Message}", ex);
        }
    }

    private string GetChainFile(string from, string to)
    {
        // Example chain files:
        // hg38ToAilMel1.over.chain.gz
        // hg38ToAllMis1.over.chain.gz
        // hg38ToAmaVit1.over.chain.gz

        if (!Directory.Exists(_options.ChainsDirectoryPath))
            throw new DirectoryNotFoundException($"Chains directory not found: {_options.ChainsDirectoryPath}");

        var chainFilePath = Directory
            .EnumerateFiles(_options.ChainsDirectoryPath, "*.over.chain.gz")
            .FirstOrDefault(filePath => Path.GetFileName(filePath).StartsWith($"{from}To{to}", StringComparison.OrdinalIgnoreCase)); 
            

        if (chainFilePath == null)
            throw new FileNotFoundException($"Chain file not found for liftover from {from} to {to} in directory {_options.ChainsDirectoryPath}");

        return chainFilePath;
    }

    private static Process PrepareProcess(string inputFile, string outputFile, string unmappedFile, string chainFile)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "liftover",
            Arguments = $"{inputFile} {chainFile} {outputFile} {unmappedFile}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        return new Process { StartInfo = startInfo };
    }
}
