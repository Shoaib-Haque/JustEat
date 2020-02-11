using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JustEat.Models;

namespace JustEat.Repository
{
    public class TransactionRepository : Repository<Transaction>
    {
        public TransactionRepository(JustEatEntities entity)
            : base(entity)
        {
        }
    
    }
}