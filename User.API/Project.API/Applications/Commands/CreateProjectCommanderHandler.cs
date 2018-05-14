using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Project.Domain.AggregatesModel;

namespace Project.API.Applications.Commands
{
    public class CreateProjectCommanderHandler : IRequestHandler<CreateProjectCommand, Project.Domain.AggregatesModel.Project>
    {
        private readonly IProjectRepository projectRepository;
        public CreateProjectCommanderHandler(IProjectRepository _projectRepository)
        {
            projectRepository = _projectRepository;
        }
        public async Task<Domain.AggregatesModel.Project> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            await projectRepository.AddAsync(request.Project);
            await projectRepository.UnitOfWork.SaveEntitiesAsync();
            return request.Project;
        }
    }
}
