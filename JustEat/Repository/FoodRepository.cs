using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JustEat.Models;

namespace JustEat.Repository
{
    public class FoodRepository : Repository<Food>
    {
        public FoodRepository(JustEatEntities entity)
            : base(entity)
        {

        }
    }
}