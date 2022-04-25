import { Injectable } from "@angular/core";

@Injectable({ providedIn: "root" })
export class AppTokenService {
    private storage = sessionStorage;
    private token = "token";

    clear(): void {
        this.storage.removeItem(this.token);
    }

    any(): boolean {
        return this.get() !== null;
    }

    get(): string | null {
        return this.storage.getItem(this.token);
    }

    set(token: string): void {
        this.storage.setItem(this.token, token);
    }
}
