// This object is sent to backend to Request Reports
export class ReportRequest {
    constructor(
    public report?: string,
    public toDate?: Date,
    public fromDate?: Date,
    public voucherType?: string,
    public industryType?: string,
    public users?: string[],
    )   {
    }
}
