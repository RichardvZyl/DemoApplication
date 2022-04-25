
export enum SeverityEnum {
  Info = 1,
  General,
  Serious,
  Error
};

export enum EntityEnum
{
    Voucher = 1,
    User,
    Entitlement,
    MakerChecker,
    IndustryType,
    VoucherType,
    Merchant,
    Report
};

export enum RolesEnum
{
    Clerk = 1,
    Financial,
    Interface,
    Supervisor,
    Administrator
};

export enum MakerCheckerActionsEnum
{
    RefundVoucher = 1,
    TransferVoucher,
    CreateUser,
    Entitlement,
    InactivateUser,
    ChangePassword
    // TODO other actions?
};

export enum VoucherStatusEnum
{
    Unactivated = 1,
    Active,
    Redeemed,
    Refunded,
    Expired
};