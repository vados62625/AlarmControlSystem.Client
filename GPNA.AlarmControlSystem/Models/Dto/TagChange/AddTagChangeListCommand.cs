namespace GPNA.AlarmControlSystem.Models.Dto.TagChange;

public class AddTagChangeListCommand
{
    public int ExecutorUserId { get; set; }

    public int InitiatorUserId { get; set; }

    public int[] TagIds { get; set; } = Array.Empty<int>();

    public string Comment { get; set; } = "";
}