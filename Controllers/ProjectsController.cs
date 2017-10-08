using Microsoft.AspNetCore.Mvc;
using timetracker.Models;
using timetracker.Interfaces;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace timetracker.Controllers
{
    namespace timetracker.Controllers
    {
        [Authorize]
        [Route("api/[controller]")]
        public class ProjectsController : Controller
        {
            private IRepository<Project> _repository;
            public ProjectsController(IRepository<Project> repository)
            {
                _repository = repository;
            }

            [HttpGet]
            public IEnumerable<Project> GetAll()
            {
                return _repository.GetAll();
            }

            [HttpGet("{id}", Name = "GetProject")]
            public IActionResult GetProject(long id)
            {
                var project = _repository.Get(id);
                if(project == null)
                {
                    return NotFound();
                }

                return new ObjectResult(project);
            }

            [HttpPost]
            public IActionResult CreateProject([FromBody]Project value)
            {
                 if(value == null)
                {
                    return BadRequest();
                }

                _repository.Add(value);
                return CreatedAtRoute("GetProject", new { id = value.Id }, value);
            }

            [HttpPut("{id}")]
            public IActionResult UpdateProject(long id, [FromBody] Project value)
            {
                var project = _repository.Get(id);
                if (project == null)
                {
                    return NotFound();
                }
                if (value == null)
                {
                    return BadRequest();
                }

                _repository.Update(project);         
                return Ok();
            }

            [HttpDelete("{id}")]
            public IActionResult DeleteProject(long id)
            {
                var project = _repository.Get(id);
                if (project == null)
                {
                    return NotFound();
                }

                project.Deleted = true;
                _repository.Update(project);     
                return new NoContentResult();
            }
        }
    }
}