using TaskifyMini.Helpers;
using TaskifyMini.Helpers.Extensions;
using TaskifyMini.Models.DTOs;
using TaskifyMini.Models.Entities;
using TaskifyMini.Repositories.Interfaces;

namespace TaskifyMini.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<ApiResponse<TaskItem>> CreateTaskAsync(CreateTaskRequestDto request)
        {
            var taskItem = new TaskItem
            {
                Title = request.Title,
                Description = request.Description,
                //DueDate = request.DueDate,
                DueDate = DateTime.SpecifyKind(request.DueDate.Value, DateTimeKind.Local)
                        .ToUniversalTime(),
                Priority = request.Priority,
                Status = Models.Enums.TaskStatus.New,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true,
                IsDeleted = false
            };
            var taskDetail = await _taskRepository.CreateTaskAsync(taskItem);
            if (taskDetail == null)
            {
                return new ApiResponse<TaskItem>()
                {
                    Data = null,
                    Message = "Failed to create task",
                    IsSuccess = false
                };
            }
            return new ApiResponse<TaskItem>()
            {
                Data = taskDetail,
                Message = "Task created successfully",
                IsSuccess = true
            };
        }

        public async Task<ApiResponse<bool>> DeleteTaskAsync(int id)
        {
            var isDeleted = await _taskRepository.DeleteTaskAsync(id);
            if (!isDeleted)
            {
                return new ApiResponse<bool>()
                {
                    Data = false,
                    Message = "Failed to delete task",
                    IsSuccess = false
                };
            }
            return new ApiResponse<bool>()
            {
                Data = true,
                Message = "Task deleted successfully",
                IsSuccess = true
            };
        }

        public async Task<ApiResponse<TaskItem>> GetTaskById(int id)
        {
            if (id <= 0)
            {
                return new ApiResponse<TaskItem>()
                {
                    Data = null,
                    Message = "Invalid task ID",
                    IsSuccess = false
                };
            }
            var taskItem = await _taskRepository.GetTaskByIdAsync(id);
            if (taskItem == null)
            {
                return new ApiResponse<TaskItem>()
                {
                    Data = null,
                    Message = "Task not found",
                    IsSuccess = false
                };
            }
            return new ApiResponse<TaskItem>()
            {
                Data = taskItem,
                Message = "Task retrieved successfully",
                IsSuccess = true
            };
        }

        public async Task<ApiResponse<PagedResult<TaskItem>>> GetTaskItems(int pageNumber, int PageSize)
        {
            if (pageNumber <= 0 || PageSize <= 0)
            {
                return new ApiResponse<PagedResult<TaskItem>>()
                {
                    Data = null,
                    Message = "Invalid pagination parameters",
                    IsSuccess = false
                };
            }

            var taskItems = await _taskRepository.GetTaskItemsAsync(pageNumber, PageSize);

            var result = new PagedResult<TaskItem>()
            {
                Items = taskItems,
                PageNumber = pageNumber,
                PageSize = PageSize,
                TotalCount = taskItems.Count()
            };
            return new ApiResponse<PagedResult<TaskItem>>()
            {
                Data = result,
                Message = taskItems.Any() ? "Tasks retrieved successfully" : "No tasks found",
                IsSuccess = true
            };

        }

        public async Task<ApiResponse<UpdateTaskResponseDto>> UpdateTaskAsync(UpdateTaskRequestDto request)
        {
            //find task by id
            var existingTask = await _taskRepository.GetTaskByIdAsync(request.Id);
            if (existingTask == null)
            {
                return new ApiResponse<UpdateTaskResponseDto>()
                {
                    Data = null,
                    Message = "Task not found",
                    IsSuccess = false
                };
            }
            existingTask.Title = request.Title ?? existingTask.Title;
            existingTask.Description = request.Description ?? existingTask.Description;
            existingTask.Status = request.Status;
            existingTask.UpdatedAt = DateTime.UtcNow;

            var updatedTask = await _taskRepository.UpdateTaskAsync(existingTask);
            if (updatedTask == null)
            {
                return new ApiResponse<UpdateTaskResponseDto>()
                {
                    Data = null,
                    Message = "Failed to update task",
                    IsSuccess = false
                };
            }
            return new ApiResponse<UpdateTaskResponseDto>()
            {
                Data = new UpdateTaskResponseDto
                {
                    UserId = updatedTask.Id,
                    UserName = updatedTask.Title,
                    Email = updatedTask.Description ?? string.Empty
                },
                Message = "Task updated successfully",
                IsSuccess = true
            };

        }
    }
}
