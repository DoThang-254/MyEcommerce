using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;
using Shared.Application.Common.Interfaces;
using Shared.Infrastructure.Persistence;

namespace OrderService.Infrastructure.Data
{
    public class OrderDbContext : BaseDbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options, IMediator mediator, ICurrentUserService currentUserService) : base(options, mediator, currentUserService)
        {
        }

        public DbSet<Order> Orders => Set<Order>();

        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(OrderDbContext).Assembly);

            base.OnModelCreating(builder);
        }
    }
}
