namespace Taskboard.Queries.Queries
{
    public class GetTaskQuery : IQuery
    {
        public string ListId { get; set; }
        public string TaskId { get; set; }
    }
}