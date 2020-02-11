using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JustEat.Models;

namespace JustEat.Repository
{
    public class CommentRepository : Repository<Comment>
    {
        public CommentRepository(JustEatEntities entity)
            : base(entity)
        {
        }
    }
}