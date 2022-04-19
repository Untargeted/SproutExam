using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common.Enums;
using Microsoft.Extensions.Configuration;
using Sprout.Exam.DataAccess.Interface;
using Dapper;
using Sprout.Exam.DataAccess;
using Sprout.Exam.DataAccess.CalculateFactory;

namespace Sprout.Exam.WebApp.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IDapperClass dapr;
        public EmployeesController(IDapperClass dapr_)
        {
            dapr = dapr_;
        }
        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //Setting up Parameters
            DynamicParameters param = new DynamicParameters();
            param.Add("@Code", "GetAll");

            List<EmployeeDto> res = dapr.ExecStoredProcReturnListWithParam<EmployeeDto>(param, "EmployeeQueries");
            //Formatting Birth Date
            res.ToList().ForEach(x => x.Birthdate = Convert.ToDateTime(x.Birthdate).ToString("yyyy-MM-dd"));

            var result = await Task.FromResult(res);
            return Ok(result);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and fetch from the DB.
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@Code", "GetByID");
            param.Add("@Id", id);

            List<EmployeeDto> res = dapr.ExecStoredProcReturnListWithParam<EmployeeDto>(param, "EmployeeQueries");
            //Re-formatting Date
            res[0].Birthdate = Convert.ToDateTime(res[0].Birthdate).ToString("yyyy-MM-dd");

            var result = await Task.FromResult(res[0]);
            return Ok(result);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and update changes to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(EditEmployeeDto input)
        {

            DynamicParameters param = new DynamicParameters();
            param.Add("@Code", "Update");
            param.Add("@Id", input.Id);
            param.Add("@FullName", input.FullName);
            param.Add("@Birthdate", input.Birthdate.ToString("yyyy-MM-dd"));
            param.Add("@TIN", input.Tin);
            param.Add("@EmployeeTypeId", input.EmployeeTypeId);

            var res = await Task.FromResult(dapr.ExecStoredProcWithParam(param, "EmployeeQueries"));

            return Ok(res);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and insert employees to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Post(CreateEmployeeDto input)
        {
            int ID = await Task.FromResult(dapr.ExecStoredProcReturnINT("GetLastID"));

            DynamicParameters param = new DynamicParameters();
            param.Add("@Code", "Insert");
            param.Add("@FullName", input.FullName);
            param.Add("@Birthdate", input.Birthdate.ToString("yyyy-MM-dd"));
            param.Add("@TIN", input.Tin);
            param.Add("@EmployeeTypeId", input.EmployeeTypeId);

            dapr.ExecStoredProcWithParam(param, "EmployeeQueries");

            return Created($"/api/employees/{ID}", ID);
        }


        /// <summary>
        /// Refactor this method to go through proper layers and perform soft deletion of an employee to the DB.
        /// </summary>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            DynamicParameters param = new DynamicParameters();
            param.Add("@Code", "Delete");
            param.Add("@Id", id);

            var res = await Task.FromResult(dapr.ExecStoredProcWithParam(param, "EmployeeQueries"));

            return Ok(id);
        }

        /// <summary>
        /// Refactor this method to go through proper layers and use Factory pattern
        /// </summary>
        /// <param name="id"></param>
        /// <param name="absentDays"></param>
        /// <param name="workedDays"></param>
        /// <returns></returns>
        [HttpPost("{id}/calculate")]
        public async Task<IActionResult> Calculate(CalculateEmployeeDto input)
        {
            CalculateFactory factory = null;
            DynamicParameters param = new DynamicParameters();
            param.Add("@Code", "GetByID");
            param.Add("@Id", input.Id);

            EmployeeDto res = await Task.FromResult(dapr.ExecStoredProcReturnListWithParam<EmployeeDto>(param, "EmployeeQueries")[0]);

            if (res == null) return NotFound();

            var type = (EmployeeType) res.EmployeeTypeId;

            switch (type)
            {
                case EmployeeType.Regular:
                    //create computation for regular.
                    factory = new RegularFactory(input.absentDays);
                    break;
                case EmployeeType.Contractual:
                    //create computation for contractual.
                    factory = new ContractualFactory(input.workedDays);
                    break;
                default: break;

            }
            return Ok(await Task.FromResult(factory.GetFinalSalary().FinalSalary));

        }

    }
}
