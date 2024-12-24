using System.Text;

namespace AdoDotNetDemo.Console;

public sealed class Achievement
{
    public long Id { get; set; }
    public AchievementClass AchievementClass { get; set; }
    public DateOnly CompletedDate { get; set; }
    public int Quantity { get; set; }
    public override string ToString()
    {
        return new StringBuilder()
            .Append(nameof(Achievement))
            .Append(" { Id: ")
            .Append(Id)
            .Append(", Class: ")
            .Append(AchievementClass.Name)
            .Append(", Quantity: ")
            .Append(Quantity)
            .Append(" Unit: ")
            .Append(AchievementClass.Unit)
            .Append(" }")
            .ToString();
    }
}