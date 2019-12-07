using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Savaglow.Data.Interfaces;
using Savaglow.Dtos;
using Savaglow.Models.Ledger;
using Savaglow.Params;
using Microsoft.AspNetCore.Mvc;

namespace Savaglow.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/[controller]")]
    public class LedgerController : ControllerBase
    {
        private readonly ILedgerRepository _ledgerRepo;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepo;
        public LedgerController(IUserRepository userRepo, ILedgerRepository ledgerRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _ledgerRepo = ledgerRepo;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetLedgerItems(string userId, [FromQuery]LedgerParams ledgerParams)
        {
            var user = await _userRepo.GetUser(userId);
            if (user == null)
            {
                return NotFound("User could not be located");
            }
            if (ledgerParams.Recurring)
            {
                var ledgerItems = await _ledgerRepo.GetRecurringLedgerForUser(userId);
                var ledgerItemsToReturn = _mapper.Map<RecurringLedgerItemDto[]>(ledgerItems);
                return Ok(ledgerItemsToReturn);
            }
            else
            {
                var recurringLedgerItems = await _ledgerRepo.GetLedgerForUser(userId);
                var ledgerItemsToReturn = _mapper.Map<LedgerItemDto[]>(recurringLedgerItems);
                return Ok(ledgerItemsToReturn);
            }
        }

        [HttpGet("{id}", Name = "GetLedgerItem")]
        public async Task<IActionResult> GetLedgerItem(string userId, int id, [FromQuery]LedgerParams ledgerParams)
        {
            var user = await _userRepo.GetUser(userId);
            if (user == null)
            {
                return NotFound("User could not be located");
            }
            if (ledgerParams.Recurring)
            {
                var recurringledgerItem = await _ledgerRepo.GetRecurringLedgerItem(id);
                var recurringLedgerItemToReturn = _mapper.Map<RecurringLedgerItemDto>(recurringledgerItem);
                return Ok(recurringLedgerItemToReturn);
            }
            else
            {
                var ledgerItem = await _ledgerRepo.GetLedgerItem(id);
                var ledgerItemToReturn = _mapper.Map<LedgerItemDto>(ledgerItem);
                return Ok(ledgerItemToReturn);
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddLedgerItem(string userId, LedgerItemCreationDto ledgerItem)
        {
            var user = await _userRepo.GetUser(userId);
            if (user == null)
            {
                return NotFound("User could not be located");
            }
            ledgerItem.UserId = userId;
            if (ledgerItem.Recurring == null)
            {
                var ledgerItemToSave = _mapper.Map<LedgerItem>(ledgerItem);
                await _ledgerRepo.AddLedgerItem(ledgerItemToSave);
                var ledgerItemToReturn = _mapper.Map<LedgerItemDto>(ledgerItemToSave);
                await _ledgerRepo.Save();
                return CreatedAtRoute("GetLedgerItem", new { userId = userId, id = ledgerItemToSave.Id }, ledgerItemToReturn);
            }
            else
            {
                var ledgerItemToSave = _mapper.Map<RecurringLedgerItem>(ledgerItem);
                await _ledgerRepo.AddLedgerItem(ledgerItemToSave);
                var ledgerItemToReturn = _mapper.Map<RecurringLedgerItemDto>(ledgerItemToSave);
                await _ledgerRepo.Save();
                return CreatedAtRoute("GetLedgerItem", new { userId = userId, id = ledgerItemToSave.Id}, ledgerItemToReturn);
            }
        }
    }
}