using Project.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Domain.AggregatesModel
{  /// <summary>
/// 项目可见范围
/// </summary>
   public class ProjectVisibleRule: Entity
    {   /// <summary>
    ///  项目有ID
    /// </summary>
        public  int ProjectId { get; set; }

        /// <summary>
        /// 是否可见
        /// </summary>
       public   bool Visible { get; set; }

        public  string Tags { get; set; }
    }
}
