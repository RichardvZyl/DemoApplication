export enum SeverityEnum {
  Info = 1,
  General,
  Serious,
  Error,
}

export enum EntityEnum {
  User = 1,
  Entitlement,
  MakerChecker,
  Report,
}

export enum RolesEnum {
  Clerk = 1,
  Financial,
  Interface,
  Supervisor,
  Administrator,
}

export enum MakerCheckerActionsEnum {
  CreateUser = 1,
  Entitlement,
  SuspendUser,
  ChangePassword,
}
