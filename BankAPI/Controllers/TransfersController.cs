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
    public class TransfersController : ControllerBase
    {
        private readonly ITransferService transferService;


        private readonly IValidateUserFilter validateUserFilter;

        public TransfersController(ITransferService transferService, IValidateUserFilter validateUserFilter)
        {
            this.transferService = transferService;
            this.validateUserFilter = validateUserFilter;
        }

        [HttpPost("createTransfer")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateTransfer([FromBody] TransferDto transferDto,
          CancellationToken cancellationToken = default)
        {
            string l = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(l, cancellationToken);

            if (access == "admin" || access == "null")
            {
                return UnprocessableEntity("ERROR, Access denied");
            }

            var success = await transferService.CreateTransfer(transferDto, cancellationToken);

            if (!success)
            {
                return UnprocessableEntity("ERROR, Transfer cannot be done");
            }

            //var token = operationService.GenerateJwt(createAccountDto.AccountDto);
            //return Ok(new { token });
            return Ok();
        }

        [HttpPut("cancelTransaction")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CancelTransaction(int Id_Transaction,
         CancellationToken cancellationToken = default)
        {
            string l = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(l, cancellationToken);

            if (access == "client" || access == "null")
            {
                return UnprocessableEntity("ERROR, Access denied");
            }

            var success = await transferService.CancelTransaction(Id_Transaction, cancellationToken);
            if (!success)
            {
                return UnprocessableEntity("Failed to cancel a transaction");
            }

            return NoContent();
        }

        [HttpGet("checkTransactionHistory")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Transfer>>> CheckTransactionHistory([FromQuery]int Sender_Bank_Account,
         [FromQuery] int takeCount, [FromQuery]int skipCount, CancellationToken cancellationToken = default)
        {
            if (takeCount < 1 || skipCount < 0)
            {
                return BadRequest("Failed to check loan applocations");
            }

            var transfers = await transferService.CheckTransactionHistory(Sender_Bank_Account, takeCount, skipCount, cancellationToken);
            if (transfers == null)
            {
                return BadRequest("Failed to check transaction history");
            }

            return Ok(new { transfers });
        }

        [HttpGet("showAwaitingTransfers")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Transfer>>> ShowAwaitingTransfers([FromQuery] int takeCount, 
            [FromQuery]int skipCount, CancellationToken cancellationToken = default)
        {
            if (takeCount < 1 || skipCount < 0)
            {
                return BadRequest("Failed to show awaiting transfers");
            }

            var transfers = await transferService.ShowAwaitingTransfers(takeCount, skipCount, cancellationToken);
            if (transfers == null)
            {
                return BadRequest("Failed to show awaiting transfers");
            }

            return Ok(new { transfers });
        }

        [HttpGet("showCurrencies")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Currency>>> ShowCurrencies( [FromQuery] int takeCount, 
            [FromQuery]int skipCount, CancellationToken cancellationToken = default)
        {
            if (takeCount < 1 || skipCount < 0)
            {
                return BadRequest("Failed to show currencies");
            }

            var currencies = await transferService.ShowCurrencies(takeCount, skipCount, cancellationToken);
            if (currencies == null)
            {
                return BadRequest("Failed to show currencies");
            }

            return Ok(new { currencies });
        }


        [HttpPut("makeTransfers")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> MakeTransfers(CancellationToken cancellationToken = default)
        {
            string l = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(l, cancellationToken);

            if (access == "client" || access == "null")
            {
                return UnprocessableEntity("ERROR, Access denied");
            }

            var success = await transferService.MakeTransfers(cancellationToken);

            if (!success)
            {
                return UnprocessableEntity("ERROR, transfers cannot be made or there are not any transfers to be made");
            }

            return NoContent();
        }

    }
}
