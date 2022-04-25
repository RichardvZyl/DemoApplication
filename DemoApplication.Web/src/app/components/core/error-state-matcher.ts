import { ErrorStateMatcher } from "@angular/material/core";
import { FormControl, FormGroupDirective, NgForm } from "@angular/forms";

export class MyErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null) : boolean {
    const invalidCtrl = !!(control && control.invalid && control.parent.dirty);
    const controlTouched = !!(control && (control.dirty || control.touched)); // this ensures the error is only displayed once the controll has been accessed
    const invalidParent = !!(control && control.parent && control.parent.invalid && control.parent.dirty);

    console.log(form); // this is to get the warning to go away that this value is never used

    return controlTouched && (invalidCtrl || invalidParent);
    // return control.parent.errors && control.parent.errors['notSame']
    // return control.parent.errors && control.parent.errors && control.touched && (invalidCtrl || invalidParent);
  }
}