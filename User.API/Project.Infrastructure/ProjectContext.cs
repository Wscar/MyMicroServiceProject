using Microsoft.EntityFrameworkCore;
using Project.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Project.Domain.AggregatesModel;
using MediatR;
using Microsoft.EntityFrameworkCore.Design;
using Project.Infrastructure.EntityConfiguration;
namespace Project.Infrastructure
{
    public class ProjectContext : DbContext, IUnitOfWork
    {
        public const string DEFAULT_SCHEMA = "Project";
        public DbSet<Project.Domain.AggregatesModel.Project> Project { get; set; }
        public DbSet<ProjectContributor> ProjectContributor { get; set; }
        public DbSet<ProjectProperties> ProjectProperties { get; set; }
        public DbSet<ProjectViewer> ProjectViewer { get; set; }
        public DbSet<ProjectVisibleRule> ProjectVisibleRule { get; set; }
      

        private readonly IMediator _mediator;

        private ProjectContext(DbContextOptions<ProjectContext> options) : base(options) { }

        public ProjectContext(DbContextOptions<ProjectContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));


            System.Diagnostics.Debug.WriteLine("OrderingContext::ctor ->" + this.GetHashCode());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProjectEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectViewEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectVisibleEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectContributorEntityConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectPropertyEntityConfiguation());
        }


        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _mediator.DispatchDomainEventsAsync(this);
            await base.SaveChangesAsync();
            return true;
        }
    }
    public class OrderingContextDesignFactory : IDesignTimeDbContextFactory<ProjectContext>
    {
        public ProjectContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProjectContext>()
                .UseMySQL("server=47.93.232.105;uid=yemobai;pwd=Jolly1128@;database=beta_user;Charset=utf8;port=3306");

            return new ProjectContext(optionsBuilder.Options, new NoMediator());
        }

        class NoMediator : IMediator
        {
            public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default(CancellationToken)) where TNotification : INotification
            {
                return Task.CompletedTask;
            }

            public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult<TResponse>(default(TResponse));
            }

            public Task Send(IRequest request, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.CompletedTask;
            }
        }
    }

}
