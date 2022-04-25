import { SeverityEnum, RolesEnum, EntityEnum } from "./../enums.model"

export class NotificationModel {
    constructor(
    public id?: number,
    public originator?: string,
    public originatorString?: string,
    public severity?: SeverityEnum,
    public description?: string,
    public date?: Date,
    public read?: boolean,
    public seenBy?: string,
    public seenByUser?: string,
    public seenAt?: Date,
    public forRole?: RolesEnum,
    public relatedId?: string,
    public relatedDescription?: string,
    public entity?: EntityEnum
    )   {
    }
}



// export interface Inotifications {
//   // interface to define a displayable notification
//     originator?: string,
//     severity?: string,
//     description?: string,
//     severityCode?: number,
//     date?: any
//   }