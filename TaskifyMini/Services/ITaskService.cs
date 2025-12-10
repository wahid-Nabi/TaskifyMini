using TaskifyMini.Helpers;
using TaskifyMini.Helpers.Extensions;
using TaskifyMini.Models.DTOs;
using TaskifyMini.Models.Entities;

namespace TaskifyMini.Services
{
    public interface ITaskService
    {
        Task<ApiResponse<TaskItem>> CreateTaskAsync(CreateTaskRequestDto request);
        Task<ApiResponse<UpdateTaskResponseDto>> UpdateTaskAsync(UpdateTaskRequestDto request);
        Task<ApiResponse<bool>> DeleteTaskAsync(int id);
        Task<ApiResponse<PagedResult<TaskItem>>> GetTaskItems(int pageNumber, int PageSize);
        Task<ApiResponse<TaskItem>> GetTaskById(int id);
    }
}
