// this class is used to filter the requested report to ensure it falls within the parameters provided
// Current Assumption says that if no filters are selected the complete report is requested
export class FilterModel {
    constructor(
    public report?: string, // TODO this value exists in the ReportRequestModel as well //check where the value is supposed to be located and remove the duplicate
    public toDate?: Date,
    public fromDate?: Date,
    public voucherType?: string, // this should allign with the available voucher types
    public industryType?: string, // this should allign with the available industry types
    public users?: string[]
    )   {
    }
}

// TODO this model should include more properties
// TODO properties need to be populated per requirement (each report will have different filters available)