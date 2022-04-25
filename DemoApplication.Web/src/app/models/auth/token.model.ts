// this class is used to define the logged in user as well as when his connection expires
export class TokenModel {
    constructor(
    public token?: string
    )   {
    }
}

// TODO Consider removing model definition as it only contains a string
// TODO this or session? or both? include token in session?