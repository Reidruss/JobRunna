namespace JobRunna.Models;

public class SubmissionModel
{
    public required string Name { get; init; }
    public required string Output { get; init; }
    public required string Error { get; init; }
    public required string Command { get; init; }
}