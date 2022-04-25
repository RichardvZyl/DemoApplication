import { AppBaseComponent } from "../base/base.component";

export abstract class AppInputComponent extends AppBaseComponent<any> {
    constructor(type: string) {
        super();
        this.type = type;
    }

    type!: string;

    input($event: any): void {
        this.value = $event.target.value;
    }
}
