using System;
using System.Collections.Generic;
using System.Text;
using Core.Entities;

namespace Entities.Concrete
{
    public class ApiInfoModel : DocumentDbEntity
    {
        public string TypeKey { get; set; }
        public long TotalRequestLimit { get; set; }
        public long RemainingRequest { get; set; }
        public float MlResultRate { get; set; }
    }

}
