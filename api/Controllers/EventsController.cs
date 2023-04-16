using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTO.EventDto;
using api.Models;
using api.Repository.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly ICommonRepository<Event> _eventRepository;
        private readonly IMapper _mapper;

        public EventsController(ICommonRepository<Event> repsitory, IMapper mapper)
        {
            _eventRepository = repsitory;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Event>>> Get()
        {
            var events = await _eventRepository.GetAll();
            if (events.Count <= 0)
            {
                return NotFound();
            }
            return Ok(events);
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetEventDto>> GetDetails(int id)
        {
            var event_ = await _eventRepository.GetDetails(id);
            return event_ == null ? NotFound() : Ok(event_);
        }


        [HttpPost("CreateEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Create(CreateEventDto? events)
        {
            
            var eventModel = _mapper.Map<Event>(events);

			var result = await _eventRepository.Insert(eventModel);

			if (result != null)
			{
				var eventsDetails = _mapper.Map<GetEventDto>(eventModel);
				return CreatedAtAction("GetDetails", new { id = eventsDetails.EventId }, eventsDetails);
			}

			return BadRequest();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(UpdateEventDto events)
        {
            var eventModel = _mapper.Map<Event>(events);

			var result = await _eventRepository.Update(eventModel);

			if (result != null)
			{
				return Ok(result);
			}

			return BadRequest();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(int id)
        {
            var event_ = await _eventRepository.GetDetails(id);
            if (event_ == null)
            {
                 return NoContent();
            }

            await _eventRepository.Delete(event_.EventId);
            return Ok(new {message = "Event has been deleted successfully."});
        }
    }
}