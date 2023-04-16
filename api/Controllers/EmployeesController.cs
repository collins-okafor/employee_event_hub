using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTO.EmployeeDto;
using api.Models;
using api.Repository.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("PublicPolicy")]
    public class EmployeesController : ControllerBase
    {
        private readonly ICommonRepository<Employee> _employeeRepository;
        private readonly IMapper _mapper;

        public EmployeesController(ICommonRepository<Employee> repository, IMapper mapper)
        {
            _employeeRepository = repository;
            _mapper = mapper;
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Employee,Hr")]
        public async Task<ActionResult<IEnumerable<GetEmployeeDto>>> Get()
        {
            var employees = await _employeeRepository.GetAll();
            if (employees.Count <= 0)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<IEnumerable<GetEmployeeDto>>(employees));
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Employee,Hr")]
        public async Task<ActionResult<GetEmployeeDto>> GetDetails(int id)
        {
            var employee = await _employeeRepository.GetDetails(id);
            return employee == null ? NotFound() : Ok(_mapper.Map<GetEmployeeDto>(employee));
        }


        [HttpPost("CreateEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Employee,Hr")]
        public async Task<ActionResult> Create(CreateEmployeeDto employee)
        {
            var employeeModel = _mapper.Map<Employee>(employee);
            var result =  await _employeeRepository.Insert(employeeModel);
            if (result != null)
            {
                var employeeDetails = _mapper.Map<Employee>(employeeModel);
                return CreatedAtAction("GetDetails", new { id = employeeDetails.EmployeeId }, employeeDetails);

            }
            return BadRequest();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Employee,Hr")]
        public async Task<ActionResult> Update(UpdateEmployeeDto employee)
        {
            var employeeModel = _mapper.Map<Employee>(employee);
            var result =  await _employeeRepository.Update(employeeModel);
            if (result != null)
            {
                
                return Ok(result);

            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Hr")]
        public async Task<ActionResult> Delete(int id)
        {
            var employee = await _employeeRepository.GetDetails(id);
            if (employee == null)
            {
                 return NoContent();
            }
          
            await _employeeRepository.Delete(employee.EmployeeId);
            return NoContent();
        }
    }
}