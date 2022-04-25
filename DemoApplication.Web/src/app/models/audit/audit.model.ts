export class AuditLogModel {
    constructor(
        public id?: string, // actually Guid
        public date?: Date,
        public userId?: string,
        public user?: string,
        public displayContext?: string,
        public model?: string,
        public contents?: string
    ) {
    }
}