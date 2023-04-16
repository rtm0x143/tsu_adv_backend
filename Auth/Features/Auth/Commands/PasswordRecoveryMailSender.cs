using System.Net.Mail;
using Auth.Infra.Data.Entities;
using Common.App.Models.Results;
using Common.App.Services;
using OneOf;

namespace Auth.Features.Auth.Commands;

public class PasswordRecoveryMailSender
{
    public required IMailSender Sender { get; init; }

    /// <returns>
    /// <exception cref="ArgumentException">When <paramref name="user"/>s <see cref="AppUser.Email"/> is null</exception>
    /// </returns>
    public Task<OneOf<EmptyResult, ArgumentException>> SendRecoveryMessageFor(AppUser user, string recoveryToken)
    {
        if (user.Email == null)
            return Task.FromResult<OneOf<EmptyResult, ArgumentException>>(
                new ArgumentException($"{nameof(user)}.{nameof(user.Email)} == null"));

        return Sender.Send(new MailMessage
        {
            To = { user.Email },
            Subject = "Password recovery",
            Body =
                $"""
                Hi! {user.UserName},
                that is your password recovery code {recoveryToken}.
                (course it should be link to user's frontend, but it is missing).
                
                """
        }).ContinueWith<OneOf<EmptyResult, ArgumentException>>(t => new EmptyResult());
    }
}