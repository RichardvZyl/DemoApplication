import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { AuditLogModel } from "../models/audit/audit.model";
import { environment } from "../../environments/environment";

@Injectable({ providedIn: "root" })

export class AppAuditService {
    auditUrl = environment.apiUrl + "Audits";
    angularListUrl = environment.apiUrl + "AngularList";

    constructor(
        private readonly http: HttpClient
        ) {
            console.log("Audit Service Constructor");
    }

    async getAuditLogs() : Promise<AuditLogModel[]> {
        console.log("get Audit trail called within Audit service");
        // get users current entitlement
        const entitlement = await this.http.get<AuditLogModel[]>(`${this.angularListUrl}/Audits`).toPromise();
        return entitlement;
    }
}