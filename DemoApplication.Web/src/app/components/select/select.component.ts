import { Input, OnInit } from "@angular/core";
import { Observable } from "rxjs";
import { AppBaseComponent } from "../base/base.component";
import { OptionModel } from "./option.model";

export abstract class AppSelectComponent extends AppBaseComponent<any> implements OnInit {
    @Input() child!: AppSelectComponent;

    options = new Array<OptionModel>();

    abstract getOptions(parameter?: any): Observable<OptionModel[]>;

    ngOnInit(): void {
        this.load();
    }

    change($event: any) {
        this.value = $event.target.value;
        this.changeChildren(true);
    }

    clearOptions() {
        this.options = new Array<OptionModel>();
    }

    load(parameter?: any) {
        this.getOptions(parameter).subscribe((options: OptionModel[]) => { this.options = options; });
    }

    writeValue(value: any) {
        this.value = value;
        this.changeChildren();
    }

    private changeChildren(clearValue: boolean = false) {
        if (!this.child) { return; }

        let child = this.child;

        while (child) {
            child.clearOptions();
            if (clearValue) { child.value = undefined; }
            child = child.child;
        }

        this.child.load(this.value);
    }
}
