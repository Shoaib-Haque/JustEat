using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JustEat.Models;

namespace JustEat.Repository
{
    public class RatingsRepository : Repository<Rating>
    {
        public RatingsRepository(JustEatEntities entity)
            : base(entity)
        {
        }
    
    }
}