using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.Data.Configurations;
using Shared.Application.Common.Interfaces;
using Shared.Infrastructure.Persistence;

namespace ProductService.Infrastructure.Data
{
    public class ProductDbContext : BaseDbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options, IMediator mediator , ICurrentUserService currentUserService) : base(options, mediator, currentUserService)
        {
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ProductSeed.Seed(modelBuilder);

            base.OnModelCreating(modelBuilder);

        }
    }
}
