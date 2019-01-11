using System.Collections.Generic;

namespace Taskboard.Queries.DTO
{
    public class ListDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<TaskDTO> Tasks { get; set; }
    }
}