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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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

            var success = await transferService.CreateTransfer(transferDto, l, cancellationToken);

            if (!success)
            {
                return UnprocessableEntity("ERROR, Transfer cannot be done");
            }

            //var token = operationService.GenerateJwt(createAccountDto.AccountDto);
            //return Ok(new { token });
            return Ok();
        }

        [HttpPost("exchange")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> ExchangeMoney([FromBody] CurrencyExchangeDto currencyExchangeDto,
         CancellationToken cancellationToken = default)
        {
            string l = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(l, cancellationToken);

            if (access == "admin" || access == "null")
            {
                return UnprocessableEntity("ERROR, Access denied");
            }

            var success = await transferService.ExchangeMoney(currencyExchangeDto, cancellationToken);

            if (!success)
            {
                return UnprocessableEntity("ERROR, Transfer cannot be done");
            }

            //var token = operationService.GenerateJwt(createAccountDto.AccountDto);
            //return Ok(new { token });
            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> updateTransfer([FromBody] TransferDto transferDto,
         CancellationToken cancellationToken = default)
        {
            string l = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(l, cancellationToken);

            if (access == "client" || access == "null")
            {
                return UnprocessableEntity("ERROR, Access denied");
            }

            var success = await transferService.CancelTransaction(transferDto, cancellationToken);
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

        [HttpGet("currencies")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<CurrencyDto>>> GetAllCurrencies(CancellationToken cancellationToken = default)
        {
            string l = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(l, cancellationToken);

            if (access == "client" || access == "null")
            {
                return UnprocessableEntity("ERROR, Access denied");
            }

            var currencies = await transferService.ShowCurrencies(cancellationToken);
            if (currencies == null)
            {
                return BadRequest("Failed to show currencies");
            }

            return Ok(new { currencies });
        }

        [HttpGet("{transferId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TransferDto>> GetTransfer([FromRoute]int transferId,
            CancellationToken cancellationToken = default)
        {
            string l = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(l, cancellationToken);

            if (access == "client" || access == "null")
            {
                return UnprocessableEntity("ERROR, Access denied");
            }

            var transfer = await transferService.GetTransfer(transferId, cancellationToken);
            if (transfer == null)
            {
                return BadRequest("Transfer do not exists or is has been executed");
            }

            return Ok(new { transfer });
        }

        [HttpGet("bankAccount/{bankAccountId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TransferDto>>> GetTransfers([FromRoute]int bankAccountId,
            CancellationToken cancellationToken = default)
        {
            string l = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(l, cancellationToken);

            if (access == "admin" || access == "null")
            {
                return UnprocessableEntity("ERROR, Access denied");
            }

            var transfer = await transferService.GetTransfers(bankAccountId, cancellationToken);
            if (transfer == null)
            {
                return BadRequest("Transfer do not exists or is has been executed");
            }

            return Ok(new { transfer });
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<TransferDto>>> GetTransfers([FromBody]TransferRequestDto transferRequestDto,
           CancellationToken cancellationToken = default)
        {
            string l = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(l, cancellationToken);

            if (access == "client" || access == "null")
            {
                return UnprocessableEntity("ERROR, Access denied");
            }

            var transfer = await transferService.GetAdminTransfers(transferRequestDto, cancellationToken);
            if (transfer == null)
            {
                return BadRequest("Transfer do not exists or is has been executed");
            }

            return Ok(new { transfer });
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
