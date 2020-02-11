using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JustEat.Models;

namespace JustEat.Repository
{
    public class NotificationRepository : Repository<Notification>
    {
        public NotificationRepository(JustEatEntities entity)
            : base(entity)
        {
        }
    }
}