using JGUZDV.CQRS.Commands;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Th11s.TimeKeeping.Commands;

// TODO AP: Wenn der Arbeitnehmer bereits "pro" Abteilung geführt wird (also nicht Arbeitnehmer is a User sondern User has many Arbeitnehmer),
// dann kann hier überall die Abteilungsid wegfallen, da die bereits impliziert wird
public record ArbeitnehmerCommand(string ArbeitnehmerId, string AbteilungsId) : ICommand;

public abstract class ArbeitnehmerCommandContext
{

}

public abstract class ArbeitnehmerCommandHandler<TCommand> : CommandHandler<TCommand, ArbeitnehmerCommandContext> 
    where TCommand : ArbeitnehmerCommand
{
    protected override async Task<List<ValidationResult>> ValidateAsync(TCommand command, ClaimsPrincipal? principal, CancellationToken ct)
    {
        var result = await base.ValidateAsync(command, principal, ct);

        if(command.A)
    }
}