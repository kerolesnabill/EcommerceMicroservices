﻿using AutoMapper;
using MediatR;
using UserService.Domain.Exceptions;
using UserService.Domain.Interfaces;

namespace UserService.Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler
    (ILogger<UpdateUserCommandHandler> logger,
    IUserRepository userRepository,
    IUserContext userContext,
    IMapper mapper) : IRequestHandler<UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();
        logger.LogInformation("Updating user with id {UserId}", currentUser.Id);

        var user = await userRepository.GetByIdAsync(currentUser.Id)
            ?? throw new UserNotFoundException(currentUser.Id.ToString());

        mapper.Map(request, user);
        user.UpdatedAt = DateTime.UtcNow;
        
        await userRepository.UpdateAsync(user);
    }
}
