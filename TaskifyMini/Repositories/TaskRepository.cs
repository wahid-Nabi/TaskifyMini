using Microsoft.EntityFrameworkCore;
using TaskifyMini.Data;
using TaskifyMini.Models.Entities;
using TaskifyMini.Repositories.Interfaces;

namespace TaskifyMini.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskifyContext _context;
        public TaskRepository(TaskifyContext context)
        {
            _context = context;
        }
        public async Task<TaskItem> CreateTaskAsync(TaskItem item)
        {
            await _context.TaskItems.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;

        }
        public async Task<TaskItem> UpdateTaskAsync(TaskItem item)
        {
            await _context.SaveChangesAsync();
            return item;

        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            //soft delete
            var task = await  _context.TaskItems.FirstOrDefaultAsync(t => t.Id == id);
            if(task == null) return false;  
            task.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<TaskItem?> GetTaskByIdAsync(int id)
        {
            return await _context.TaskItems.AsNoTracking().Where(x => !x.IsDeleted).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TaskItem>> GetTaskItemsAsync(int pageNumber, int pageSize)
        {
            var tasks = await  _context.TaskItems
                .Where(t => !t.IsDeleted)
                .OrderBy(t => t.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return tasks;
        }
    }
}
