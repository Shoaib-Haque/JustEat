using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JustEat.Models;

namespace JustEat.Repository
{
    public class AdminRepository : Repository<Admin>
    {
        public AdminRepository(JustEatEntities entity)
            : base(entity)
        {
        }
    }
}