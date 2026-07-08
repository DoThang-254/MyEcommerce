using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Application.Common.Interfaces;
using Shared.Infrastructure.Persistence;
using UserService.Domain.Entities;
using UserService.Infrastructure.Data.Configurations;

namespace UserService.Infrastructure.Data
{
    public class UserDbContext : BaseDbContext
    {
        public UserDbContext(DbContextOptions options, IMediator mediator, ICurrentUserService currentUserService) : base(options, mediator, currentUserService)
        {
        }

        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            UserSeed.Seed(modelBuilder);
            base.OnModelCreating(modelBuilder);

        }
    }
}
