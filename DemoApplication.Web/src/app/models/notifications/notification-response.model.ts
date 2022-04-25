import { SessionModel } from "../auth/session.model";
import { ResultModel } from "../auth/result.model";
import { NotificationModel } from "./notification.model";

// This class is the expected response from backend when requesting notifications
export class NotificationResponseModel {
    constructor(
    public session?: SessionModel,
    public result?: ResultModel,
    public notifications?: NotificationModel[]
    )   {
    }
}

// TODO ensure this is the format you will be receiving from backend