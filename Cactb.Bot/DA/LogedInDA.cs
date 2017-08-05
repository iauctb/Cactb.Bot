using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cactb.Bot.Models;

namespace Cactb.Bot.DA
{
    public class LogedInDA
    {
        private static CactbBotContext db = new CactbBotContext();

        public static int Insert(LogedIn logedIn)
        {
            db.LogedIn.Add(logedIn);
            db.SaveChanges();
            return logedIn.id;
        }
    }
}