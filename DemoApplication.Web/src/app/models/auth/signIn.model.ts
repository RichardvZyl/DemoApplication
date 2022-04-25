import { RolesEnum } from "./../enums.model";

export class SignInModel {
    constructor(
    public login?: string,
    public password?: string,
    public roles: RolesEnum = RolesEnum.Clerk
    )  {
    }
}

// this was included in template so no changes required at this time
