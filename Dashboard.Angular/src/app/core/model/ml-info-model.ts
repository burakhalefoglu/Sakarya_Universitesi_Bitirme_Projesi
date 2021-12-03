
export class MlInfoModel {
    totalRequestLimit: number;
    remainingRequest: number;
    mlResultRate: number;
}

export class MlInfoRequestModel {
    message: string;
    success: boolean;
    data: MlInfoModel
}
