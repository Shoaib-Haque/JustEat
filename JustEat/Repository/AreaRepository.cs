using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JustEat.Models;

namespace JustEat.Repository
{
    public class AreaRepository : Repository<Area>
    {
        public AreaRepository(JustEatEntities entity)
            : base(entity)
        {

        }
    }
}