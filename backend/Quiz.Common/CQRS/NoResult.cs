namespace Quiz.Common.CQRS;

public record NoResult
{
    public static NoResult Instance = new NoResult();
}