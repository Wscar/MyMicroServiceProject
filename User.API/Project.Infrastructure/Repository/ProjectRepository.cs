using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Project.Domain.AggregatesModel;
using Project.Domain.SeedWork;

namespace Project.Infrastructure.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        public IUnitOfWork UnitOfWork => throw new NotImplementedException();

        public Task<Domain.AggregatesModel.Project> AddAsync(Domain.AggregatesModel.Project project)
        {
            throw new NotImplementedException();
        }

        public Task<Domain.AggregatesModel.Project> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Domain.AggregatesModel.Project> UpdateAsync(Domain.AggregatesModel.Project project)
        {
            throw new NotImplementedException();
        }
    }
}
