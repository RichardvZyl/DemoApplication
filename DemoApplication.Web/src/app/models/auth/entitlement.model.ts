// this class contains errors or success codes to be used throughout for responses from backend could possibly use below
import { EntitilementExceptionsModel } from "./entitlement-exceptions.model";

export class EntitlementModel {
    constructor(
        public login?: string,
        public role?: number[],
        public claimExceptions?: EntitilementExceptionsModel
    ) {
    }
}

// TODO should I create a new model?
// Should I do the conversion in backend?
// Possibly leave model as is for exceptions.