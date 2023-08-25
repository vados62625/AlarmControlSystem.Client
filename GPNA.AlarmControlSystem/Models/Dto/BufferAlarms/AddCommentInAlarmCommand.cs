namespace GPNA.AlarmControlSystem.Models.Dto.BufferAlarms
{
    public class AddCommentInAlarmCommand
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Комментарии
        /// </summary>
        public string? Comment { get; set; } = String.Empty;
    }
}
