
export class ApiInfoModel {
    totalRequestLimit: number;
    remainingRequest: number;
    mlResultAccuracyRate: number;
    mlResultF1Rate: number;
    mlResultROCRate: number;
    truePositive: number;
    trueNegative: number;
    falseNegative: number;
    falsePositive: number;
    errorRate: number;
    sensivityRate: number;
    specifityRate: number;
    precisionRate: number;
    prevalenceRate: number;
    recalRate: number;
    totalData: number;
    machineLearningModelName: number;
}

export class ApiInfoRequestModel {
    message: string;
    success: boolean;
    data: ApiInfoModel
}
