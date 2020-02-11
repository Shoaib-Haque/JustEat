using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JustEat.Models;

namespace JustEat.Repository 
{
    public class CartRepository : Repository<Cart>
    {
        public CartRepository(JustEatEntities entity)
            : base(entity)
        {
    
        }
    }
}