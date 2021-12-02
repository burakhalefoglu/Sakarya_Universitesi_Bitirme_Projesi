using System;
using Core.Entities;

namespace Entities.Concrete
{
    public class MostUsedWordsModel : DocumentDbEntity
    {
        public string[] Words { get; set; }
        public DateTime DateTime { get; set; }
    }
}