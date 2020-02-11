using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JustEat.Models;

namespace JustEat.Repository
{
    public class TrendingRepository : Repository<Trending>
    {
        public TrendingRepository(JustEatEntities entity)
            : base(entity)
        {
        }
    
    }
}