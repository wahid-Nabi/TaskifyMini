using TaskifyMini.Helpers;
using TaskifyMini.Helpers.Extensions;
using TaskifyMini.Models.Entities;

namespace TaskifyMini.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<TaskItem> CreateTaskAsync(TaskItem item);
        Task<TaskItem> UpdateTaskAsync(TaskItem item);
        Task<bool> DeleteTaskAsync(int id);
        Task<IEnumerable<TaskItem>> GetTaskItemsAsync(int pageNumber, int pageSize);
        Task<TaskItem?> GetTaskByIdAsync(int id);

    }
}
