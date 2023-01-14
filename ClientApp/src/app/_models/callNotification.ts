export interface CallNotification{
    connectionId : string;
    notificationType : string;
    callerUsername: string;
    data: any;
}