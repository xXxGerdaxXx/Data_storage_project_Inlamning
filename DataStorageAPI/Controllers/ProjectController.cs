using Data_storage_project_library.Dtos;
using Data_storage_project_library.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DataStorageAPI.Controllers
{
    [ApiController]
    [Route("api/project")]
    public class ProjectController(IProjectService projectService) : ControllerBase
    {
        private readonly IProjectService _projectService = projectService;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectsDto>>> GetAllProjects()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return Ok(projects);
        }

        [HttpGet("{projectId}")]
        public async Task<ActionResult<ProjectsDto>> GetProjectById(string projectId)
        {
            var project = await _projectService.GetProjectByIdAsync(projectId);

            if (project == null)
                return NotFound();

            return Ok(project);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectsDto>> RegisterProject([FromBody] ProjectRegistrationForm form)
        {
            if (string.IsNullOrEmpty(form.Title))
                return BadRequest("Project title is required.");

            var createdProject = await _projectService.RegisterProjectAsync(form);

            if (createdProject == null)
                return BadRequest("Project registration failed.");

            return CreatedAtAction(nameof(GetProjectById), new { projectId = createdProject.Id }, createdProject);
        }

        [HttpPut("{projectId}")]
        public async Task<ActionResult<ProjectsDto>> UpdateProject(string projectId, [FromBody] ProjectRegistrationForm form)
        {
            if (string.IsNullOrEmpty(form.Title))
                return BadRequest("Project title is required.");

            var updatedProject = await _projectService.UpdateProjectAsync(projectId, form);

            if (updatedProject == null)
                return NotFound();

            return Ok(updatedProject);
        }

        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteProject(string projectId)
        {
            var deleted = await _projectService.DeleteProjectAsync(projectId);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
