﻿using MediatR;
using Zindagi.Domain.UserAggregate.ViewModels;
using Zindagi.SeedWork;

#nullable disable

namespace Zindagi.Domain.UserAggregate.Queries
{
    public class GetCurrentUser : IRequest<Result<UserDto>> { }
}
