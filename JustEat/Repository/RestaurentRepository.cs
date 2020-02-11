using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JustEat.Models;

namespace JustEat.Repository
{
    public class RestaurentRepository : Repository<Restaurent>
    {
        public RestaurentRepository(JustEatEntities entity)
            : base(entity)
        {
        }
    }
}