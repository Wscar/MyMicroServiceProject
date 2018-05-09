using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Project.Domain.SeedWork;
using Project.Domain.Events;
namespace Project.Domain.AggregatesModel
{
    public class Project: Entity, IAggregateRoot 
    {   
       
        public Project()
        {
            this.Viewer = new List<ProjectViewer>();
            this.Contributors = new List<ProjectContributor>();
            this.AddDomainEvent(new ProjectCreateEvent() { Project = this });
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 项目头像
        /// </summary>
        public string Avatar { get; set; }
        /// <summary>
        /// 公司名称
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 原BF文件地址
        /// </summary>
        public string OriginBPFile { get; set; }

        /// <summary>
        /// 转换后的BP问价地址
        /// </summary>
        public string FormatBPFile { get; set; }

        /// <summary>
        /// 是否显示敏感信息
        /// </summary>
        public bool ShowSecurityInfo { get; set; }

        /// <summary>
        /// 公司所在省ID
        /// </summary>
        public int ProvineceId { get; set; }

        /// <summary>
        /// 公司所在省的名称
        /// </summary>
        public string Province { get; set; }


        /// <summary>
        /// 公司所在城市的ID
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// 公司所在城市的名称
        /// </summary>
        public string City { get; set; }


        /// <summary>
        /// 公司所在区域的ID
        /// </summary>
        public int AreaId { get; set; }

        /// <summary>
        /// 公司所在区域的名称
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 公司成立的日期
        /// </summary>
        public DateTime RegiseterTime { get; set; }


        /// <summary>
        /// 项目的基本信息
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 出让股比例
        /// </summary>
        public string FinPercentage { get; set; }

        /// <summary>
        /// 融资阶段
        /// </summary>
        public string FinStage { get; set; }

        /// <summary>
        /// 融资金额（单位--万）
        /// </summary>
        public int FinMoney { get; set; }

        /// <summary>
        /// 收入 单位(万)
        /// </summary>
        public int Income { get; set; }

        /// <summary>
        /// 利润 单位（万）
        /// </summary>
        public int Revenue { get; set; }

        /// <summary>
        /// 估值 单位（万）
        /// </summary>
        public int Valuation { get; set; }

        /// <summary>
        /// 佣金分配方式
        /// </summary>
        public int BrokerageOptions { get; set; }

        /// <summary>
        /// 是否委托给finbook
        /// </summary>
        public bool OnPlatform { get; set; }

        /// <summary>
        /// 是否可见
        /// </summary>
        public ProjectVisibleRule ProjectVisible { get; set; }

        /// <summary>
        /// 引用项目ID
        /// </summary>
        public int SourceId { get; set; }

        /// <summary>
        ///  上级引用项目ID
        /// </summary>
        public int ReferenceId { get; set; }

        /// <summary>
        /// 项目标签 
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// 项目属性：行业领域，融资币种
        /// </summary>
        public List<ProjectProperties> Properties { get; set; }

        /// <summary>
        /// 贡献者列表
        /// </summary>
        public List<ProjectContributor> Contributors { get; set; }

        /// <summary>
        /// 查看者
        /// </summary>
        public List<ProjectViewer> Viewer { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

         private Project CloneProject(Project source = null)
        {
            if (source == null)
            {
                source = this;
            }
            var newProject = new Project
            {
                AreaId = source.AreaId,
                BrokerageOptions = source.BrokerageOptions,
                Avatar = source.Avatar,
                City = source.City,
                CityId = source.CityId,
                Company = source.Company,
                Contributors = new List<ProjectContributor>(),
                Viewer = new List<ProjectViewer>(),
                CreateTime = DateTime.Now,
                FinMoney = source.FinMoney,
                FinPercentage = source.FinPercentage,
                FinStage = source.FinStage,
                FormatBPFile = source.FormatBPFile,
                Income = source.Income,
                Introduction = source.Introduction,
                OnPlatform = source.OnPlatform,
                OriginBPFile = source.OriginBPFile,
                Province = source.Province,
                ProvineceId = source.ProvineceId,
                ProjectVisible = source.ProjectVisible == null ? null : new ProjectVisibleRule()
                {
                    ProjectId = this.ProjectVisible.ProjectId,
                    Tags = this.ProjectVisible.Tags,
                    Visible = this.ProjectVisible.Visible
                },
                Valuation = source.Valuation,
                ShowSecurityInfo = source.ShowSecurityInfo,
                RegiseterTime = source.RegiseterTime,
                Revenue = source.Revenue,
            };
            newProject.Properties = new List<ProjectProperties>();
            foreach(var item in source.Properties)
            {
                newProject.Properties.Add(new ProjectProperties()
                {
                    Key=item.Key,
                    Text=item.Text,
                    Value=item.Value,
                });
            }
            return newProject;
        }
        private Project ContributorFork(int contributorId,Project source= null)
        {
            if (source == null)
            {
                source = this;
            }
            var newProject = this.CloneProject(source);
            newProject.UserId = contributorId;
            newProject.SourceId = source.SourceId == 0 ? source.Id : source.SourceId;
            newProject.ReferenceId = source.ReferenceId == 0 ? source.Id : source.ReferenceId;
            newProject.UpdateTime = DateTime.Now;
            return newProject;
        }
        public void AddViewer(int userId,string userName,string avatar)
        {
            var viewer = new ProjectViewer()
            {
                UserId = userId,
                UserName = userName,
                Avatar = avatar,
                CreateTime=DateTime.Now
            };
            if (!this.Viewer.Any(x => x.UserId == userId))
            {
                this.Viewer.Add(viewer);
                this.AddDomainEvent(new ProjectViewedEvent() { Viewer = viewer });
            }
          
        }
        public void AddContributor(ProjectContributor contributor)
        {
            if (!this.Contributors.Any(x => x.UserId == contributor.UserId))
            {
                this.Contributors.Add(contributor);
                this.AddDomainEvent(new ProjectJoinedEvent() { Contributor = contributor });
            }
           
        }
    }
}
