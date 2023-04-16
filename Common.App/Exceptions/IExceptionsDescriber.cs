using Microsoft.AspNetCore.Mvc;

namespace Common.App.Exceptions;

public interface IExceptionsDescriber
{
    /// <summary>
    /// Describe an exception
    /// </summary>
    /// <param name="exception">Expects some <see cref="Exception"/> subclass</param>
    ActionResult Describe(object exception);
}