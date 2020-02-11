using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JustEat.Models;

namespace JustEat.Repository
{
    public class CustomerNotificationRepository : Repository<CustomerNotification>
    {
        public CustomerNotificationRepository(JustEatEntities entity)
            : base(entity)
        {
    
        }
    }
}