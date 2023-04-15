using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly ICommonRepository<Event> _eventRepository;

        public EventsController(ICommonRepository<Event> repsitory)
        {
            _eventRepository = repsitory;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Event>> Get()
        {
            var events = _eventRepository.GetAll();
            if (events.Count <= 0)
            {
                return NotFound();
            }
            return Ok(events);
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Event> GetDetails(int id)
        {
            var event_ = _eventRepository.GetDetails(id);
            return event_ == null ? NotFound() : Ok(event_);
        }


        [HttpPost("CreateEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Create(Event event_)
        {
            _eventRepository.Insert(event_);
            var result = _eventRepository.SaveChanges();
            if (result > 0)
            {
                // return CreatedAtAction("Getetails", new { id = employee.EmployeeId }, employee);
                return CreatedAtAction("GetDetails", new { id = event_.EventId }, event_);

            }
            return BadRequest();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Update(Event event_)
        {
            _eventRepository.Update(event_);
            var result = _eventRepository.SaveChanges();
            if (result > 0)
            {
                // return CreatedAtAction("Getetails", new { id = employee.EmployeeId }, employee);
                return NoContent();

            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Delete(int id)
        {
            var event_ = _eventRepository.GetDetails(id);
            if (event_ == null)
            {
                 return NoContent();
            }

            _eventRepository.Delete(event_);
            _eventRepository.SaveChanges();
            return NoContent();
        }
        
    }
}