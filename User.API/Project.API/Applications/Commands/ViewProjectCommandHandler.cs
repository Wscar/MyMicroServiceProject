using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Project.Domain.AggregatesModel;
namespace Project.API.Applications.Commands
{
    public class ViewProjectCommandHandler : IRequestHandler<ViewProjectCommand>
    {
        public readonly IProjectRepository projectRepository;
        public ViewProjectCommandHandler(IProjectRepository _projectRepository)
        {
            projectRepository = _projectRepository;
        }
        public async Task Handle(ViewProjectCommand request, CancellationToken cancellationToken)
        {
            var project = await this.projectRepository.GetAsync(request.ProjectId);
            if (project == null)
            {
                throw new Domain.Exceptions.ProjectDomainException($"project not found:{request.ProjectId}");
            }
            project.AddViewer(request.UserId, request.UserName, request.Avatar);
            await projectRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
