using Core.Entities;

namespace Entities.Concrete
{
    public class ApiInfoModel : DocumentDbEntity
    {
        public string TypeKey { get; set; }
        public long TotalRequestLimit { get; set; }
        public long RemainingRequest { get; set; }
        public double MlResultAccuracyRate { get; set; }
        public double MlResultF1Rate { get; set; }
        public double MlResultROCRate { get; set; }

        public int TruePositive { get; set; }
        public int TrueNegative { get; set; }
        public int FalseNegative { get; set; }
        public int FalsePositive { get; set; }
        public int ErrorRate { get; set; }
        public int SensivityRate { get; set; }
        public int SpecifityRate { get; set; }
        public int PrecisionRate { get; set; }
        public int PrevalenceRate { get; set; }
        public int RecalRate { get; set; }
        public int TotalData { get; set; }
        public string MachineLearningModelName { get; set; }
    }
}