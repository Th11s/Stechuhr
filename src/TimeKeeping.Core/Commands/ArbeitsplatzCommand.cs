using JGUZDV.CQRS;
using JGUZDV.CQRS.Commands;
using Microsoft.EntityFrameworkCore;
using Th11s.TimeKeeping.Data;
using Th11s.TimeKeeping.Data.Entities;

namespace Th11s.TimeKeeping.Commands;

public record ArbeitsplatzCommand(Guid ArbeitsplatzId) : ICommand;

public class ArbeitsplatzCommandContext(Arbeitsplatz arbeitsplatz)
{
    public Arbeitsplatz Arbeitsplatz { get; } = arbeitsplatz;
}

public abstract class ArbeitsplatzCommandHandler<TCommand, TContext> : CommandHandler<TCommand, TContext> 
    where TCommand : ArbeitsplatzCommand
    where TContext : ArbeitsplatzCommandContext
{
    public ArbeitsplatzCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected ApplicationDbContext _dbContext { get; }

    protected async Task<Arbeitsplatz> LoadArbeitsplatz(Guid arbeitsplatzId, CancellationToken ct)
    {
        var arbeitsplatz = await _dbContext.Arbeitsplaetze
            .Include(x => x.Abteilung)
            .Where(x => x.Id == arbeitsplatzId)
            .FirstOrDefaultAsync(ct);

        if (arbeitsplatz == null)
            throw new CommandException(HandlerResult.NotValid("Arbeitsplatz:NotFound"));

        return arbeitsplatz;
    }
}