import { SessionModel } from "../auth/session.model";
import { UserModel } from "./user.model";

// this class is used to request a specific voucher to be suspended
export class SuspendUserRequestModel {
    constructor(
    public session?: SessionModel,
    public user?: UserModel
    )   {
    }
}