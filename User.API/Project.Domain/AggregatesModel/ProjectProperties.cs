using System;
using System.Collections.Generic;
using System.Text;
using Project.Domain.SeedWork;
   
namespace Project.Domain.AggregatesModel
{
    public class ProjectProperties : ValueObject
    {   
        public string Key { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
