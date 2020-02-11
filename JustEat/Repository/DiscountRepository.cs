using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JustEat.Models;

namespace JustEat.Repository
{
    public class DiscountRepository : Repository<Discount>
    {
        public DiscountRepository(JustEatEntities entity)
            : base(entity)
        {

        }
    }
}