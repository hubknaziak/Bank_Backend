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
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> RegisterClient([FromBody] CreateAccountDto createAccountDto,
           CancellationToken cancellationToken = default)
        {
            string login = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(login, cancellationToken);

            if(access == "client" || access == "null")
            {
                return UnprocessableEntity("ERROR, Access denied");
            }

            var response = await accountService.CreateClientAccount(createAccountDto, cancellationToken);
         
            if (response == "null")
            {
                return UnprocessableEntity("ERROR, Account cannot be created");
            }

            return Ok(new { login = response });
        }

        [HttpPost("register/admin")]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> RegisterAdmin([FromBody] CreateAccountDto createAccountDto,
          CancellationToken cancellationToken = default)
        {
            string login = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(login, cancellationToken);

            if (access == "client" || access == "null")
            {
                return UnprocessableEntity("ERROR, Access denied");
            }

            var response = await accountService.CreateAdminAccount(createAccountDto, cancellationToken);

            if (response == "null")
            {
                return UnprocessableEntity("ERROR, Account cannot be created");
            }

            return Ok(new { login = response });
        }

         [AllowAnonymous]
         [HttpPost("authenticate")]
         [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
         [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
         public async Task<IActionResult> Authenticate([FromBody] AccountDto accountDto, 
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
        public async Task<IActionResult> ModifyAccount(CreateAccountDto modifyAccountDto, 
            CancellationToken cancellationToken = default)
        {
            string login = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(login, cancellationToken);

            if (access == "client" || access == "null")
            {
                return UnprocessableEntity("ERROR, Access denied");
            }


            var success = await accountService.ModifyAccount(modifyAccountDto, cancellationToken);
            if (!success)
            {
                return UnprocessableEntity("Failed to modify account");
            }

            return NoContent();
        }

        [HttpPut("change")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> ChangePassword(AccountDto accountDto,
           CancellationToken cancellationToken = default)
        {
            var success = await accountService.ChangePassword(accountDto, cancellationToken);
            if (!success)
            {
                return UnprocessableEntity("Failed to change password");
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
                return UnprocessableEntity("ERROR, Access denied");
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
                return UnprocessableEntity("ERROR, Access denied");
            }

            var success = await accountService.UnblockAccount(login, cancellationToken);
            if (!success)
            {
                return UnprocessableEntity("Failed to block an account");
            }

            return NoContent();
        }

        /* //[AllowAnonymous]
         [HttpGet("{login}")]
         [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
         [ProducesResponseType(StatusCodes.Status404NotFound)]
         public async Task<ActionResult<AccountDto>> GetAccount(string login,
             CancellationToken cancellationToken = default)
         {
             var result = await accountService.GetAccount(login, cancellationToken);

             if (result == null)
             {
                 return NotFound();
             }
             else if (result is string)
             {
                 return Unauthorized(result);
             }
             return Ok(result);
         }*/

        // DELETE: api/Accounts/5
        //[AllowAnonymous]
        [HttpDelete("{login}")]
        public async Task<IActionResult> DeleteAccount(string login, CancellationToken cancellationToken)
        {

            string l = HttpContext.GetLoginFromClaims();

            var access = await validateUserFilter.ValidateUser(l, cancellationToken);

            if (access == "client" || access == "null")
            {
                return UnprocessableEntity("ERROR, Access denied");
            }

            var account = await accountService.DeleteAccount(login, cancellationToken);
            if (account == null)
            {
                return NotFound();
            }

            //_context.Accounts.Remove(account);
            if (account is string)
            {
                return Unauthorized(account);
            }
            else
            {
                return (bool)account ? (IActionResult)NoContent() : NotFound();
            }
        }

        [HttpGet("showAllAccounts")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<Account>>> ShowAllAccounts([FromQuery] int takeCount, [FromQuery]int skipCount, 
            CancellationToken cancellationToken = default)
        {
            if (takeCount < 1 || skipCount < 0)
            {
                return BadRequest("Failed to show accounts");
            }

            var accounts = await accountService.ShowAllAccounts(takeCount, skipCount, cancellationToken);
            if (accounts == null)
            {
                return BadRequest("Failed to show accounts");
            }

            //return Ok(new { count = loanApplications, notes = notes.Item2 });
            return Ok(new { accounts });
        }

          [HttpGet("{login}")]
          [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)]
          [ProducesResponseType(StatusCodes.Status404NotFound)]
          public async Task<ActionResult<Account>> GetAccount(int id_Account,
              CancellationToken cancellationToken = default)
          {
              var result = await accountService.GetAccount(id_Account, cancellationToken);

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

       /* //[AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(typeof(GetClientDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetClientDto>> GetAdminAccount(CancellationToken cancellationToken = default)
        {
            string login = HttpContext.GetLoginFromClaims();

            var result = await accountService.GetAdminAccount(login, cancellationToken);

            if (result == null)
            {
                return NotFound();
            }

            else if (result is string)
            {
                return Unauthorized(result);
            }
            return Ok(result);
        }*/

        /*private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.id_account == id);
        }*/
    }
}
