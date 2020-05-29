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

        [HttpPost("loan_application")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> LoanApplication([FromBody] Loan_ApplicationDto loan_ApplicationDto,
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

            return Ok(new { loanApplications });
        }

        [HttpGet("showLoanApplication")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Loan_Application>>> ShowLoanApplication([FromQuery]int id_client,
         [FromQuery] int takeCount, [FromQuery]int skipCount, CancellationToken cancellationToken = default)
        {
            if (takeCount < 1 || skipCount < 0)
            {
                return BadRequest("Failed to check loan applocations");
            }

            var loanApplications = await loanService.ShowLoanApplication(takeCount, skipCount, id_client, cancellationToken);
            if (loanApplications == null)
            {
                return BadRequest("Failed to check loan applocations");
            }

            return Ok(new { loanApplications });
        }

        [HttpGet("showLoan")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Loan>>> ShowLoan([FromQuery]int id_client,
        [FromQuery] int takeCount, [FromQuery]int skipCount, CancellationToken cancellationToken = default)
        {
            if (takeCount < 1 || skipCount < 0)
            {
                return BadRequest("Failed to check loans");
            }

            var loanApplications = await loanService.ShowLoan(takeCount, skipCount, id_client, cancellationToken);
            if (loanApplications == null)
            {
                return BadRequest("Failed to check loans");
            }

            //return Ok(new { count = loanApplications, notes = notes.Item2 });
            return Ok(new { loanApplications });
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

        [HttpPost("confirmLoan")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> ConfirmLoanApplication([FromBody] LoanDto loanDto,
       CancellationToken cancellationToken = default)
        {
            string l = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(l, cancellationToken);

            if (access == "client" || access == "null")
            {
                return UnprocessableEntity("ERROR, Access denied");
            }

            var success = await loanService.ConfirmLoanApplication(loanDto, cancellationToken);

            if (!success)
            {
                return UnprocessableEntity("ERROR, Loan application cannot be send");
            }

            //var token = operationService.GenerateJwt(createAccountDto.AccountDto);
            //return Ok(new { token });
            return Ok();
        }
    }
}
