using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryPatternWithUOW.BL.Dtos;
using RepositoryPatternWithUOW.BL.Models;
using RepositoryPatternWithUOW.BL.Repositories;
using RepositoryPatternWithUOW.DataAccessLayer.Services;

namespace RepositoryPatternWithUOW.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class EmployeesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var employees= await _unitOfWork.Employees.GetEmployeesWithRelatvies();
            var result= employees.Select(e => new EmployeeDto 
            { 
                FName = e.FName ,
                LName = e.LName ,
                
                Salary = e.Salary
            });
            return Ok(result);
        }
        [HttpGet("GetEmployeesWithRelatvies")]
        public async Task<IActionResult> GetEmployeesWithRelatvies()
        {
            var employees = await _unitOfWork.Employees.GetEmployeesWithRelatvies();
            var result = employees.Select(e => new
            {
                FName = e.FName,
                LName = e.LName,
                DepartmentId = e.DepartmentId,
                ManagerId = e.ManagerId,
                Salary = e.Salary,
                Relatives = e.Relatives.Select(u => new
                {
                    u.Name,
                    u.Age,
                    u.Job
                })
            });
            return Ok(result);
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var employee = await _unitOfWork.Employees.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }
            else
            {
                var result= new EmployeeDto
                {
                    FName = employee.FName,
                    LName = employee.LName,
                    DepartmentId = employee.DepartmentId,
                    ManagerId = employee.ManagerId,
                    Salary = employee.Salary
                };
                return Ok(result);
            }
        }

        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByName(string Name)
        {
            var employees = await _unitOfWork.Employees.Filter(e=>e.FName==Name);
            if (!employees.Any())
            {
                return NotFound();
            }
            else
            {
                var result =employees.Select(e=> new EmployeeDto
                {
                    FName = e.FName,
                    LName = e.LName,
                    DepartmentId = e.DepartmentId,
                    ManagerId = e.ManagerId,
                    Salary = e.Salary,
                    ProjectId=e.ProjectEmployees.Select(pe => pe.ProjectId).FirstOrDefault()
                });
                return Ok(result);
            }
        }
        [HttpPost]
        public async Task<IActionResult> Add(EmployeeDto employeeModel)
        {
            if (string.IsNullOrEmpty(employeeModel.FName)|| 
                string.IsNullOrEmpty(employeeModel.LName))
                return BadRequest("Name Is Required");
            var employee = new Employee()
            { 
                FName = employeeModel.FName,
                LName=employeeModel.LName,
                DepartmentId=employeeModel.DepartmentId ,
                ManagerId= employeeModel.ManagerId,
                Salary=employeeModel.Salary,
            };
           
            try
            {
                await _unitOfWork.Employees.Add(employee);
                _unitOfWork.Complete();

                var ProjectEmployee = new ProjectEmployees() { ProjectId = (int)employeeModel.ProjectId, EmployeeId = employee.Id };
                await _unitOfWork.ProjectEmployees.Add(ProjectEmployee);
                _unitOfWork.Complete();
                return Ok("Created");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Edit(EmployeeDto employeeModel,int id)
        {
            if (string.IsNullOrEmpty(employeeModel.FName) ||
                string.IsNullOrEmpty(employeeModel.LName))
                return BadRequest("Name Is Required");
            var employee=await _unitOfWork.Employees.GetById(id);
            if (employee == null)
            {
                return NotFound();
            }
            employee.LName = employeeModel.LName;
            employee.FName = employeeModel.FName;
            if (employee.Salary != employeeModel.Salary)
            {
                employee.Salary = employeeModel.Salary;
            }

            if (employee.DepartmentId != employeeModel.DepartmentId && employeeModel.DepartmentId>0)
            {
                employee.DepartmentId = employeeModel.DepartmentId;
            }


            try
            {
                await _unitOfWork.Employees.Update(employee);
                _unitOfWork.Complete();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var employee = _unitOfWork.Employees.GetById(id).Result;
            if (employee == null)
            {
                return NotFound();
            }
            try
            {
                _unitOfWork.Employees.Delete(employee);
                _unitOfWork.Complete();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
