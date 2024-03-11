namespace GPNA.AlarmControlSystem.Models.Dto.TagChange;

public class AddTagChangeCommand
{
    public int ExecutorUserId { get; set; }

    public int InitiatorUserId { get; set; }

    public int TagId { get; set; }

    public string Comment { get; set; } = "";
}