using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SimpleProductOrder.Dto;
using SimpleProductOrder.Models;
using SimpleProductOrder.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace SimpleProductOrder.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {

        private readonly AccountService _accountService;
        private readonly CustomerService _customerService;
        private readonly IMapper _mapper;

        public AccountController(AccountService accountService, IMapper mapper, CustomerService customerService)
        {
            _accountService = accountService;
            _mapper = mapper;
            _customerService = customerService;
        }

        /// <summary>
        /// Get all accounts
        /// </summary>
        /// <returns>
        /// returns list of accounts
        /// </returns>
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Account>))]
        public IActionResult GetAccounts()
        {
            var accounts = _mapper.Map<List<AccountDto>>(_accountService.GetAccounts());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(accounts);
        }

        /// <summary>
        /// Get account with selected id
        /// </summary>
        /// <returns>
        /// returns account 
        /// </returns>
        [HttpGet("{accountId}")]
        [ProducesResponseType(200, Type = typeof(Account))]
        [ProducesResponseType(400)]
        public IActionResult GetAccount(int accountId)
        {
            if (!_accountService.AccountExists(accountId))
                return NotFound();

            var account = _mapper.Map<AccountDto>(_accountService.GetAccount(accountId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(account);
        }

        /// <summary>
        /// Add account
        /// </summary>
        /// <returns>
        /// returns a newly created account
        /// </returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Account
        ///     {
        ///        "login": "John",
        ///        "password": "Doe"
        ///     }
        ///
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateAccount([FromQuery] int customerId, [FromBody] AccountDto accountCreate)
        {
            if (accountCreate == null)
                return BadRequest(ModelState);

            var account = _accountService.GetAccounts()
                .Where(c => c.Login.Trim().ToUpper() == accountCreate.Login.TrimEnd().ToUpper())
                .FirstOrDefault();

            var id = _customerService.GetCustomer(customerId);

            if (id == null)
            {
                ModelState.AddModelError("", $"customer id:{customerId} not found");
                return StatusCode(422, ModelState);
            }


            if (account != null)
            {
                ModelState.AddModelError("", "account already exists");
                return StatusCode(422, ModelState);
            }


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var accountMap = _mapper.Map<Account>(accountCreate);

            accountMap.Customer = _customerService.GetCustomer(customerId);

            if (!_accountService.CreateAccount(accountMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{accountId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateAccount(int accountId, [FromBody] AccountDto updatedAccount)
        {
            if (updatedAccount == null)
                return BadRequest(ModelState);

            if (accountId != updatedAccount.Id)
                return BadRequest(ModelState);

            if (!_accountService.AccountExists(accountId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var accountMap = _mapper.Map<Account>(updatedAccount);

            if (!_accountService.UpdateAccount(accountMap))
            {
                ModelState.AddModelError("", "Something went wrong while updating account");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{accountId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteAccount(int accountId)
        {
            if (!_accountService.AccountExists(accountId))
            {
                return NotFound();
            }

            var accountToDelete = _accountService.GetAccount(accountId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_accountService.DeleteAccount(accountToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting account");
                return BadRequest(ModelState);
            }

            return NoContent();
        }

    }
}
