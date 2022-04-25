// this class defines a user and it's properties
export class UserModelNoAuth {
    constructor(
    public id?: number,
    public name?: string,
    public surname?: string,
    public email?: string,
    public active?: boolean
    )   {
    }
}