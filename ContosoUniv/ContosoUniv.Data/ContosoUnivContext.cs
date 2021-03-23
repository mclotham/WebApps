using Microsoft.EntityFrameworkCore;
using System;

namespace ContosoUniv.Data
{
    public partial class ContosoUnivContextBase
    {
        public ContosoUnivContextBase( DbContextOptions<ContosoUnivContext> options )
            : base( options )
        {
        }
    }

    public class ContosoUnivContext : ContosoUnivContextBase
    {
        public ContosoUnivContext( DbContextOptions<ContosoUnivContext> options )
            : base( options )
        {
        }
    }
}
