using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JustEat.Models;

namespace JustEat.Repository
{
    public class CustomerRepository : Repository<Customer>
    {
        public CustomerRepository(JustEatEntities entity)
            : base(entity)
        {

        }
    
    }
}