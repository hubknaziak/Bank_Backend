﻿using System;
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
using BankCore.Dtos;
using System.Threading;
using BankAPI.Extentions;
using BankAPI.Filters;

namespace BankAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {

        private readonly IAccountService accountService;

        private readonly IValidateUserFilter validateUserFilter;


        public AccountsController(IAccountService accountService, IValidateUserFilter validateUserFilter)
        {
            this.accountService = accountService;
            this.validateUserFilter = validateUserFilter;
        }


        [HttpPost("register/client")]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> RegisterClient([FromBody] ClientDto clientDto,
           CancellationToken cancellationToken = default)
        {
            string login = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(login, cancellationToken);

            if(access == "client" || access == "null")
            {
                return Unauthorized("ERROR, Access denied");
            }

            var response = await accountService.CreateClientAccount(clientDto, cancellationToken);
         
            if (response == "null")
            {
                return UnprocessableEntity("ERROR, Account cannot be created");
            }
            return Ok();
        }

        [HttpPost("register/admin")]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> RegisterAdmin([FromBody] AdministratorDto administratorDto,
          CancellationToken cancellationToken = default)
        {
            string login = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(login, cancellationToken);

            if (access == "client" || access == "null")
            {
                return Unauthorized("ERROR, Access denied");
            }

            var response = await accountService.CreateAdminAccount(administratorDto, cancellationToken);

            if (response == "null")
            {
                return UnprocessableEntity("ERROR, Account cannot be created");
            }

            return Ok();
        }

         [AllowAnonymous]
         [HttpPost("authenticate")]
         [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
         [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
         public async Task<IActionResult> AuthenticateUser([FromBody] AccountDto accountDto, 
             CancellationToken cancellationToken = default)
         {
             var success = await accountService.VerifyPassword(accountDto, cancellationToken);
             if (success.Equals("null"))
             {
                 return Unauthorized("Unauthorized access!");
             }

             var token = accountService.GenerateJwt(accountDto);
             return Ok(new {accountType = success, token });
         }

       
        [HttpPut("modify")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateClient(GetClientDto clientDto, 
            CancellationToken cancellationToken = default)
        {
            string login = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(login, cancellationToken);

            if (access == "client" || access == "null")
            {
                return Unauthorized("ERROR, Access denied");
            }


            var success = await accountService.ModifyAccount(clientDto, cancellationToken);
            if (!success)
            {
                return UnprocessableEntity("Failed to modify account");
            }

            return NoContent();
        }

        [HttpPut("block")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> BlockAccount(string login,
          CancellationToken cancellationToken = default)
        {
            string l = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(l, cancellationToken);

            if (access == "client" || access == "null")
            {
                return Unauthorized("ERROR, Access denied");
            }

            var success = await accountService.BlockAccount(login, cancellationToken);
            if (!success)
            {
                return UnprocessableEntity("Failed to block an account");
            }

            return NoContent();
        }

        [HttpPut("unblock")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UnblockAccount(string login,
         CancellationToken cancellationToken = default)
        {
            string l = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(l, cancellationToken);

            if (access == "client" || access == "null")
            {
                return Unauthorized("ERROR, Access denied");
            }

            var success = await accountService.UnblockAccount(login, cancellationToken);
            if (!success)
            {
                return UnprocessableEntity("Failed to block an account");
            }

            return NoContent();
        }


        [HttpDelete("{login}")]
        public async Task<IActionResult> DeleteAccount(string login, CancellationToken cancellationToken)
        {

            string l = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(l, cancellationToken);

            if (access == "client" || access == "null")
            {
                return Unauthorized("ERROR, Access denied");
            }

            var account = await accountService.DeleteAccount(login, cancellationToken);
            if (account == null)
            {
                return NotFound();
            }

            if (account is string)
            {
                return Unauthorized(account);
            }
            else
            {
                return (bool)account ? (IActionResult)NoContent() : NotFound();
            }
        }

        [HttpGet("client")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<GetClientDto>>> ShowAllAccounts(CancellationToken cancellationToken = default)
        {
            string l = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(l, cancellationToken);

            if (access == "client" || access == "null")
            {
                return Unauthorized("ERROR, Access denied");
            }

            var accounts = await accountService.ShowAllAccounts(cancellationToken);
            if (accounts == null)
            {
                return BadRequest("Failed to show accounts");
            }
            return Ok(accounts);
        }

          [HttpGet("{login}")]
          [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
          [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
          public async Task<ActionResult<Account>> GetAccountType(string login,
              CancellationToken cancellationToken = default)
          {
            string l = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(l, cancellationToken);

            if (access == "client" || access == "null")
            {
                return Unauthorized("ERROR, Access denied");
            }

            var result = await accountService.GetAccountType(login, cancellationToken);

              if (result == "null")
              {
                  return NotFound();
              }
              return Ok(new { accountType = result });
          }

        [HttpGet]
        [ProducesResponseType(typeof(GetAccountDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetAccountDto>> GetAccount(CancellationToken cancellationToken = default)
        {
            var login = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(login, cancellationToken);

            object result = null;

            if (access == "admin")
            {
                 result = await accountService.GetAdminAccount(login, cancellationToken);
            }
            else if(access == "client") 
            {
                 result = await accountService.GetClientAccount(login, cancellationToken);
            }
           
            if (result == null)
            {
                return NotFound();
            }

            else if (result is string)
            {
                return Unauthorized(result);
            }
            return Ok(result);
        }

        [HttpGet("client/{login}")]
        [ProducesResponseType(typeof(GetAccountDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetAccountDto>> GetClientData(string login, CancellationToken cancellationToken = default)
        {
            string l = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(l, cancellationToken);

            if (access == "client" || access == "null")
            {
                return Unauthorized("ERROR, Access denied");
            }

            var result = await accountService.GetClientAccount(login, cancellationToken);

            if (result == null)
            {
                return NotFound();
            }
            return Ok( result );
        }

    }
}
