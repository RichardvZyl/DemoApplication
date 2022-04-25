import { SignInModel } from "../auth/signIn.model";

// this class defines a user and it's properties
export class UserModel {
    constructor(
    public id?: number,
    public name?: string,
    public surname?: string,
    public email?: string,
    public active?: boolean,
    public auth?: SignInModel
    )   {
    }
}