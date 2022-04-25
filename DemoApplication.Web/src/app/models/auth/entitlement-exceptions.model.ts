export class EntitilementExceptionsModel {
  constructor(
    public userId?: string, // actually Guid
    public expiresOn?: Date,
    public viewNotifications?: boolean,
    public viewUsers?: boolean,
    public suspendUsers?: boolean,
    public statisticalReport?: boolean,
    public auditReport?: boolean,
    public authorizeMakerChecker?: boolean,
    public entitlementChange?: boolean,
    public auditLogs?: boolean
  ) {}
}
