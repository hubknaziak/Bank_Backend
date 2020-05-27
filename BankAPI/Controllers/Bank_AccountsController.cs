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

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Bank_AccountsController : ControllerBase
    {
        private readonly Bank_AccountService bank_AccountService;

        public Bank_AccountsController(Bank_AccountService bank_AccountService)
        {
            this.bank_AccountService = bank_AccountService;
        }

        [AllowAnonymous]
        [HttpPost("createBankAccount")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateBankAccount([FromBody] Bank_AccountDto bank_AccountDto,
           CancellationToken cancellationToken = default)
        {
            var success = await bank_AccountService.CreateBankAccount(bank_AccountDto, cancellationToken);

            if (!success)
            {
                return UnprocessableEntity("ERROR, Bank account cannot be created");
            }

            //var token = operationService.GenerateJwt(createAccountDto.AccountDto);
            //return Ok(new { token });
            return Ok();
        }

        [AllowAnonymous]
        [HttpPut("block")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> BlockBankAccount(int id_Bank_Account,
         CancellationToken cancellationToken = default)
        {
            var success = await bank_AccountService.BlockBankAccount(id_Bank_Account, cancellationToken);
            if (!success)
            {
                return UnprocessableEntity("Failed to block an account");
            }

            return NoContent();
        }

        [AllowAnonymous]
        [HttpPut("unblock")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UnblockBankAccount(int id_Bank_Account,
          CancellationToken cancellationToken = default)
        {
            var success = await bank_AccountService.UnblockBankAccount(id_Bank_Account, cancellationToken);
            if (!success)
            {
                return UnprocessableEntity("Failed to unblock an account");
            }

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("checkAmount")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<ActionResult<Bank_Account>> CheckAccountAmount(int id_Bank_Account,
        CancellationToken cancellationToken = default)
        {
            var bank_Account = await bank_AccountService.CheckAccountAmount(id_Bank_Account, cancellationToken);
            if (bank_Account == null)
            {
                return UnprocessableEntity("Failed to check account amount");
            }

            return bank_Account;
        }
    }
}
