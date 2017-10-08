using Microsoft.AspNetCore.Mvc;
using timetracker.Models;
using timetracker.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace timetracker.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class TimeEntriesControllers : Controller
    {
        private IRepository<TimeEntry> _repository;

        public TimeEntriesControllers(IRepository<TimeEntry> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<TimeEntry> GetAllForProject(long id)
        {
            return _repository.GetAll().Where(_ => _.ProjectId == id);
        }

        [HttpGet("{id}", Name = "GetTimeEntry")]
        public IActionResult GetTimeEntry(long id)
        {
            var timeEntry = _repository.Get(id);
            if(timeEntry == null)
            {
                return NotFound();
            }

            return new ObjectResult(timeEntry);
        }

        [HttpPost]
        public IActionResult CreateTimeEntry([FromBody] TimeEntry value)
        {
            if(value == null)
            {
                return BadRequest();
            }

            _repository.Add(value);
            return CreatedAtRoute("GetTimeEntry", new {id = value.Id }, value);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTimeEntry(long id, [FromBody] TimeEntry value)
        {
            var timeEntry = _repository.Get(id);
            if (timeEntry == null)
            {
                return NotFound();
            }
            if (value == null)
            {
                return BadRequest();
            }

            _repository.Update(timeEntry);         
            return Ok();
        }

        [HttpDelete]
        public IActionResult DeleteTimeEntry(long id)
        {
            var timeEntry = _repository.Get(id);
            if (timeEntry == null)
            {
                return NotFound();
            }

            timeEntry.Deleted = true;
            _repository.Update(timeEntry);     
            return new NoContentResult();
        }
    }
}