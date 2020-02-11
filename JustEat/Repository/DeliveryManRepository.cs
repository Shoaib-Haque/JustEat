using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JustEat.Models;

namespace JustEat.Repository
{
    public class DeliveryManRepository : Repository<DeliveryMan>
    {
        public DeliveryManRepository(JustEatEntities entity)
            : base(entity)
        {
        }
    }
}