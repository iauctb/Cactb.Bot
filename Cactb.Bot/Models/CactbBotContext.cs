namespace Cactb.Bot.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CactbBotContext : DbContext
    {
        public CactbBotContext()
            : base("name=CactbBotContext")
        {
        }

        public virtual DbSet<LogedIn> LogedIn { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
