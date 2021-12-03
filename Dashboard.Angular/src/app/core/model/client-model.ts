
export class ClientModel {
    uId: string;
    maskedName: string;
    isPleased: number;
    tweet: string;
    dateTime: Date;
}

export class ClientsRequestModel {
    message: string;
    success: boolean;
    data: ClientModel[]
}
