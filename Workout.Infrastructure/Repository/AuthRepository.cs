using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Workout.Application.Common.Interfaces;
using Workout.Domain.Entities;
using Workout.Infrastructure.Data;

namespace Workout.Infrastructure.Repository
{
    public class AuthRepository : Repository<User>, IAuthRepository
    {
        private readonly AppDbContext _db;
        public AuthRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(User model)
        {
            _db.users.Update(model);
        }
    }
}
