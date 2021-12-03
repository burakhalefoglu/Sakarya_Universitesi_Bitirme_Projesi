using System;
using Core.Entities;

namespace Entities.Concrete
{
    public class ClientModel : DocumentDbEntity
    {
        public string UId { get; set; }
        public string MaskedName { get; set; }
        public int IsPleased { get; set; }
        public string Tweet { get; set; }
        public DateTime DateTime { get; set; }
    }
}