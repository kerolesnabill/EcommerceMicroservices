﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Addresses.Commands.AddAddress;
using UserService.Application.Addresses.Queries.GetAddresses;

namespace UserService.Presentation.Controllers;

[Route("api/users/me/addresses")]
[ApiController]
[Authorize]
public class AddressesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddAddress(AddAddressCommand command)
    {
        await mediator.Send(command);
        return NoContent();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAddress()
    {
        var addresses = await mediator.Send(new GetAddressesCommand());
        return Ok(addresses);
    }
}
