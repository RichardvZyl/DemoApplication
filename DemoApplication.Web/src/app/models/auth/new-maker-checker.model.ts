import { MakerCheckerActionsEnum } from "./../enums.model";
// import { FileModel } from "../file/file.model";

export class NewMakerCheckerModel {
    constructor(
        public action?: MakerCheckerActionsEnum,
        public motivation?: string,
        public files?: string[],
        public model?: string
    ) {
    }
}