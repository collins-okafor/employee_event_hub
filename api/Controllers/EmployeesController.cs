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
    public class EmployeesController : ControllerBase
    {
        private readonly ICommonRepository<Employee> _employeeRepository;
        public EmployeesController(ICommonRepository<Employee> repository)
        {
            _employeeRepository = repository;
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Employee>> Get()
        {
            var employees = _employeeRepository.GetAll();
            if (employees.Count <= 0)
            {
                return NotFound();
            }
            return Ok(employees);
        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Employee> GetDetails(int id)
        {
            var employee = _employeeRepository.GetDetails(id);
            return employee == null ? NotFound() : Ok(employee);
        }


        [HttpPost("CreateEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Create(Employee employee)
        {
            _employeeRepository.Insert(employee);
            var result = _employeeRepository.SaveChanges();
            if (result > 0)
            {
                // return CreatedAtAction("Getetails", new { id = employee.EmployeeId }, employee);
                return CreatedAtAction("GetDetails", new { id = employee.EmployeeId }, employee);

            }
            return BadRequest();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult Update(Employee employee)
        {
            _employeeRepository.Update(employee);
            var result = _employeeRepository.SaveChanges();
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
            var employee = _employeeRepository.GetDetails(id);
            if (employee == null)
            {
                 return NoContent();
            }

            _employeeRepository.Delete(employee);
            _employeeRepository.SaveChanges();
            return NoContent();
        }
    }
}