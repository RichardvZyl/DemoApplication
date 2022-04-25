import { SessionModel } from "../auth/session.model";

// this class is used to request notifications and should identify the user as notifications are specific to entitlement
export class NotificationRequestModel {
    constructor(
    public session?: SessionModel
    )   {
    }
}

// TODO Does this model need to exist? will it gain properties as currently it only has one property