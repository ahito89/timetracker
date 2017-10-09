using Microsoft.AspNetCore.Mvc;
using timetracker.Models;
using timetracker.Interfaces;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System;

namespace timetracker.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ProjectsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private IRepository<Project> _repository;
        public ProjectsController(IRepository<Project> repository, UserManager<IdentityUser> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        [HttpGet]
        public IEnumerable<Project> GetAll()
        {       
            return _repository.GetAll().Where(project => project.UserId == GetUserId());
        }

        [HttpGet("{id}", Name = "GetProject")]
        public IActionResult GetProject(long id)
        {
            var project = _repository.Get(id);
            if (project == null || project.UserId != GetUserId())
            {
                return NotFound();
            }

            return new ObjectResult(project);
        }

        [HttpPost]
        public IActionResult CreateProject([FromBody]Project value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            value.UserId = GetUserId();
            _repository.Add(value);
            return CreatedAtRoute("GetProject", new { id = value.Id }, value);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateProject(long id, [FromBody] Project value)
        {
            var project = _repository.Get(id);
            if (project == null || project.UserId != GetUserId())
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
            if (project == null || project.UserId != GetUserId())
            {
                return NotFound();
            }

            project.Deleted = true;
            _repository.Update(project);
            return new NoContentResult();
        }

        private Guid GetUserId()
        {
            Guid id = Guid.Empty;
            var test = Guid.TryParse(_userManager.GetUserId(User), out id);
            return id;
        }
    }

}