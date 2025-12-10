using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskifyMini.Models.DTOs;
using TaskifyMini.Models.Entities;
using TaskifyMini.Services;

namespace TaskifyMini.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }
        [Authorize(Roles ="Admin,User")]
        [HttpPost("Create")]
        [ProducesResponseType(typeof(TaskItem),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTask(CreateTaskRequestDto request)
        {
            var result = await _taskService.CreateTaskAsync(request);
            return Ok(result);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> UpdateTask(UpdateTaskRequestDto request)
        {
            var result = await _taskService.UpdateTaskAsync(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


        [HttpGet("Tasks")]
        public async Task<IActionResult> GetTasks(int PageNumber, int PageSize)
        {
            var result = await _taskService.GetTaskItems(PageNumber, PageSize);
            return Ok(result);
        }
        [HttpGet("Task")]
        public async Task<IActionResult> GetTask(int id)
        {
            var result = await _taskService.GetTaskById(id);

            return Ok(result);
        }
        [HttpGet("DeleteTask")]
        public async Task<IActionResult> DeleteTask(int TaskId)
        {
            var result = await _taskService.DeleteTaskAsync(TaskId);
            return Ok(result);
        }

    }
}
