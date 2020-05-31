using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankCore;
using BankCore.Models;
using BankCore.Dtos;
using System.Threading;
using BankCore.Services;
using BankAPI.Filters;
using BankAPI.Extentions;
using Microsoft.AspNetCore.Authorization;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrenciesController : ControllerBase
    {
        private readonly ITransferService transferService;


        private readonly IValidateUserFilter validateUserFilter;

        public CurrenciesController(ITransferService transferService, IValidateUserFilter validateUserFilter)
        {
            this.transferService = transferService;
            this.validateUserFilter = validateUserFilter;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<CurrencyDto>>> GetAllCurrencies(CancellationToken cancellationToken = default)
        {

            var currencies = await transferService.ShowCurrencies(cancellationToken);
            if (currencies == null)
            {
                return BadRequest("Failed to show currencies");
            }

            return Ok(currencies);
        }
    }
}
