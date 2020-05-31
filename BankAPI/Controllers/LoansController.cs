using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankCore;
using BankCore.Models;
using BankCore.Services;
using BankCore.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Threading;
using BankAPI.Filters;
using BankAPI.Extentions;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService loanService;

        private readonly IValidateUserFilter validateUserFilter;

        public LoansController(ILoanService loanService, IValidateUserFilter validateUserFilter)
        {
            this.loanService = loanService;
            this.validateUserFilter = validateUserFilter;
        }

        [HttpPost("application")]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateLoanApplication([FromBody] Loan_ApplicationDto loan_ApplicationDto,
         CancellationToken cancellationToken = default)
        {
            string l = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(l, cancellationToken);

            if (access == "admin" || access == "null")
            {
                return UnprocessableEntity("ERROR, Access denied");
            }

            var success = await loanService.ApplyForLoan(loan_ApplicationDto, cancellationToken);

            if (!success)
            {
                return UnprocessableEntity("ERROR, Loan application cannot be send");
            }

            return Ok();
        }

        [HttpGet("showAllLoanApplications")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Loan_Application>>> ShowAllLoanApplications([FromQuery]int administrator,
          [FromQuery] int takeCount, [FromQuery]int skipCount, CancellationToken cancellationToken = default)
        {
            string l = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(l, cancellationToken);

            if (access == "client" || access == "null")
            {
                return UnprocessableEntity("ERROR, Access denied");
            }

            if (takeCount < 1 || skipCount < 0)
            {
                return BadRequest("Failed to check loan applocations");
            }

            var loanApplications = await loanService.ShowAllLoanApplications(takeCount, skipCount, administrator, cancellationToken);
            if (loanApplications == null)
            {
                return BadRequest("Failed to check loan applocations");
            }

            return Ok(loanApplications);
        }

        [HttpGet("application/{login}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Loan_ApplicationDto>>> GetClientLoanApplications([FromRoute]string login,
          CancellationToken cancellationToken = default)
        {
            string l = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(l, cancellationToken);

            if (access == "admin" || access == "null")
            {
                return UnprocessableEntity("ERROR, Access denied");
            }

            var loanApplications = await loanService.ShowLoanApplication(login, cancellationToken);
            if (loanApplications == null)
            {
                return BadRequest("Failed to check loan applocations");
            }

            return Ok(loanApplications);
        }

        [HttpGet("application/admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<AdminLoanApplicationDto>>> GetAdminLoanApplications(
         CancellationToken cancellationToken = default)
        {
            string l = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(l, cancellationToken);

            if (access == "client" || access == "null")
            {
                return UnprocessableEntity("ERROR, Access denied");
            }

            var loanApplications = await loanService.GetAdminLoanApplications(l, cancellationToken);
            if (loanApplications == null)
            {
                return BadRequest("Failed to check loan applocations, or there are no loan applications for given administrator");
            }

            return Ok( loanApplications );
        }

        [HttpGet("{login}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<LoanDto>>> GetClientLoans([FromRoute]string login,
        CancellationToken cancellationToken = default)
        {
            string l = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(l, cancellationToken);

            if (access == "admin" || access == "null")
            {
                return UnprocessableEntity("ERROR, Access denied");
            }

            var loanApplications = await loanService.ShowLoan(login, cancellationToken);
            if (loanApplications == null)
            {
                return BadRequest("Failed to check loans");
            }

            //return Ok(new { count = loanApplications, notes = notes.Item2 });
            return Ok(loanApplications );
        }

        [HttpPut("discardLoan")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> DiscardLoanApplication(int Id_Loan_Application,
       CancellationToken cancellationToken = default)
        {
            string l = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(l, cancellationToken);

            if (access == "client" || access == "null")
            {
                return UnprocessableEntity("ERROR, Access denied");
            }


            var success = await loanService.DiscardLoanApplication(Id_Loan_Application, cancellationToken);

            if (!success)
            {
                return UnprocessableEntity("ERROR, Loan application cannot be considered");
            }

            //var token = operationService.GenerateJwt(createAccountDto.AccountDto);
            //return Ok(new { token });
            return NoContent();
        }

        [HttpPut("application")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> updateLoanApplication([FromBody] AdminLoanApplicationDto adminLoanApplicationDto,
       CancellationToken cancellationToken = default)
        {
            string l = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(l, cancellationToken);

            if (access == "client" || access == "null")
            {
                return UnprocessableEntity("ERROR, Access denied");
            }

            var success = await loanService.ConfirmLoanApplication(adminLoanApplicationDto, cancellationToken);

            if (!success)
            {
                return UnprocessableEntity("ERROR, Loan application cannot be update");
            }

            //var token = operationService.GenerateJwt(createAccountDto.AccountDto);
            //return Ok(new { token });
            return Ok();
        }
    }
}
