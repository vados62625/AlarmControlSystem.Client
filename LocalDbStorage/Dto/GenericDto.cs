namespace LocalDbStorage.Dto
{
    public class GenericDto<T> where T : class
    {
        public T Payload { get; set; }
    }
}
