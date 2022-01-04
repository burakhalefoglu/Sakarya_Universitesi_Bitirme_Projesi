
export class ClientModel {
    name: string;
    likeCount: number;
    quoteCount: number;
    replyCount: number;
    retweetCount: number;
    original_text: string;
    user_sentiment: string;
    createdAt: Number;
}

export class ClientsRequestModel {
    message: string;
    success: boolean;
    data: ClientModel[]
}

export class ClientsRequestCountModel {
    message: string;
    success: boolean;
    data: number
}
