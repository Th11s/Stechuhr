using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Th11s.TimeKeeping.Data.Entities;

namespace Th11s.TimeKeeping.Data
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<RollingClockEntry> ClockRoll { get; set; }
    }
}
