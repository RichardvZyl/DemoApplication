// this class contains errors or success codes to be used throughout for responses from backend
export class ResultModel {
    constructor(
    public description?: string,
    public resultCode?: number
    ) {
    }
}

// export class ResultModel {
//     constructor(
//     public Succeeded?: boolean,
//     public Message?: string
//     ) {
//     }
// }

// TODO should this include an enumerable with the result codes? // or will that only be handled by the back end?