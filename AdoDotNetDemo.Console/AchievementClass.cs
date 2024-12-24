namespace AdoDotNetDemo.Console;

public sealed class AchievementClass
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Unit { get; set; }

    public override string ToString() => $"{nameof(AchievementClass)} {{ Id: {Id}, Name: {Name}, Unit: {Unit} }}";
}