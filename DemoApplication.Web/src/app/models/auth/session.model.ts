// this class defines a specific connection
// should be able to identify the logged in user

export class SessionModel {
    constructor(
    public originator?: string,
    public requestNumber?: number
    ) {
    }
}

// TODO certainly session model should contain more properties?
// Does this work in tangent with token? or should I remove session model and only use token?