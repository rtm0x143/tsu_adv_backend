﻿using System.ComponentModel.DataAnnotations;
using Common.App.Attributes;
using Common.App.Models.Results;
using Common.App.RequestHandlers;

namespace Auth.Features.Auth.Commands;

public sealed record RequestPasswordRecoverCommand
    : IRequestWithException<EmptyResult, KeyNotFoundException>
{
    [EmailAddress] public required string UsersEmail { get; set; }
}

[RequestHandlerInterface]
public interface IRequestPasswordRecover
    : IRequestHandlerWithException<RequestPasswordRecoverCommand, EmptyResult, KeyNotFoundException>
{
}