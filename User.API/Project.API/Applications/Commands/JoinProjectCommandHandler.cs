using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Project.Domain.AggregatesModel;
namespace Project.API.Applications.Commands
{
    public class JoinProjectCommandHandler : IRequestHandler<JoinProjectCommand>
    {
        public readonly IProjectRepository projectRepository;
        public JoinProjectCommandHandler(IProjectRepository _projectRepository)
        {
            projectRepository = _projectRepository;
        }

        public async Task Handle(JoinProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await projectRepository.GetAsync(request.ProjectContributor.ProjectId);
            if (project == null)
            {
                throw new Domain.Exceptions.ProjectDomainException($"project not found:{request.ProjectContributor.ProjectId}");
            }
            project.AddContributor(request.ProjectContributor);
            await projectRepository.UnitOfWork.SaveChangesAsync();
        }
    }
}
