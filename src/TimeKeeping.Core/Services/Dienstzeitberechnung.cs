using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Th11s.TimeKeeping.Data;

namespace Th11s.TimeKeeping.Services
{
    public class Dienstzeitberechnung
    {
        private readonly ApplicationDbContext _dbContext;

        public Dienstzeitberechnung(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task ExecuteAsync()
        {
            //TODO: Maintenance Task mit der Aufgabe Tagesdienstzeiten zu korrigieren
            return Task.CompletedTask;
        }
    }
}
