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
using Microsoft.AspNetCore.Authorization;
using System.Threading;
using BankCore.Dtos;
using BankAPI.Extentions;
using BankAPI.Filters;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Bank_AccountsController : ControllerBase
    {
        private readonly IBank_AccountService bank_AccountService;

        private readonly IValidateUserFilter validateUserFilter;

        public Bank_AccountsController(IBank_AccountService bank_AccountService, IValidateUserFilter validateUserFilter)
        {
            this.bank_AccountService = bank_AccountService;
            this.validateUserFilter = validateUserFilter;
        }


        [HttpPost("createBankAccount")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateBankAccount([FromBody] Bank_AccountDto bank_AccountDto,
           CancellationToken cancellationToken = default)
        {
            string l = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(l, cancellationToken);

            if (access == "client" || access == "null")
            {
                return UnprocessableEntity("ERROR, Access denied");
            }

            var success = await bank_AccountService.CreateBankAccount(bank_AccountDto, cancellationToken);

            if (!success)
            {
                return UnprocessableEntity("ERROR, Bank account cannot be created");
            }

            return Ok();
        }

        [HttpPut("block")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> BlockBankAccount(int id_Bank_Account,
         CancellationToken cancellationToken = default)
        {
            string l = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(l, cancellationToken);

            if (access == "client" || access == "null")
            {
                return UnprocessableEntity("ERROR, Access denied");
            }

            var success = await bank_AccountService.BlockBankAccount(id_Bank_Account, cancellationToken);
            if (!success)
            {
                return UnprocessableEntity("Failed to block an account");
            }

            return NoContent();
        }


        [HttpPut("unblock")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UnblockBankAccount(int id_Bank_Account,
          CancellationToken cancellationToken = default)
        {
            string l = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(l, cancellationToken);

            if (access == "client" || access == "null")
            {
                return UnprocessableEntity("ERROR, Access denied");
            }

            var success = await bank_AccountService.UnblockBankAccount(id_Bank_Account, cancellationToken);
            if (!success)
            {
                return UnprocessableEntity("Failed to unblock an account");
            }

            return NoContent();
        }

        [HttpGet("checkAmount")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<decimal>> CheckAccountAmount(int id_Bank_Account,
        CancellationToken cancellationToken = default)
        {
            var bank_Amount = await bank_AccountService.CheckAccountAmount(id_Bank_Account, cancellationToken);
            /*if (bank_Account == null)
            {
                return UnprocessableEntity("Failed to check account amount");
            }*/

            return bank_Amount;
        }

        [HttpGet("showBankAccounts")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Bank_Account>>> ShowBankAccounts([FromQuery]int client,
       [FromQuery] int takeCount, [FromQuery]int skipCount, CancellationToken cancellationToken = default)
        {
            if (takeCount < 1 || skipCount < 0)
            {
                return BadRequest("Failed to show bank accounts");
            }

            var bankAccounts = await bank_AccountService.ShowBankAccounts(takeCount, skipCount, client, cancellationToken);
            if (bankAccounts == null)
            {
                return BadRequest("Failed to show bank accounts");
            }

            return Ok(new { bankAccounts });
        }
    }
}
