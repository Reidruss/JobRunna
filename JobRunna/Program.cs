using System.Diagnostics;
using JobRunna.Models;

namespace JobRunna;

public static class Program
{
	public static async Task<int> Main(string[] args)
    {
        if (args.Length is not 1)
        {
            Console.WriteLine("Usage: ./JobRunna <YAML>");
            return 1;
        }

        var userInput = string.Empty;
        do
        {
            Console.Write("> ");
            userInput = Console.ReadLine() ??  string.Empty;

            if (userInput.Equals("help", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("<INSERT HELP DOC HERE>");
            }
            else if (userInput.StartsWith("run ", StringComparison.OrdinalIgnoreCase))
            {
                var trimmedInput = userInput.Substring(3).TrimStart();
            }
            
        } while (!userInput.Equals("exit", StringComparison.OrdinalIgnoreCase));

        SubmissionModel submission;
        try
        {
            submission = await ArgParseService.Parse(args[0]);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        try
        {
            using var myProcess = new Process();
            myProcess.StartInfo.FileName = "/bin/bash";
            myProcess.StartInfo.ArgumentList.Add("-c");
            myProcess.StartInfo.ArgumentList.Add(submission.Command);
            myProcess.StartInfo.RedirectStandardOutput = true;
            myProcess.StartInfo.RedirectStandardError = true;
            
            myProcess.Start();
            var output =  await myProcess.StandardOutput.ReadToEndAsync();
            var error = await myProcess.StandardError.ReadToEndAsync();
            await myProcess.WaitForExitAsync();
            
            await File.WriteAllTextAsync(submission.Output, output);
            await File.WriteAllTextAsync(submission.Error, error);
            return myProcess.ExitCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to start process: {ex.Message}");
        }
        
        
        return 0;
    }
}