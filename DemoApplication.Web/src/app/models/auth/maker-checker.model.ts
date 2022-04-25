// import { FileModel } from "./../file/file.model";
import { MakerCheckerActionsEnum } from "./../enums.model";

export class MakerCheckerModel {
    constructor(
        public id?: string, // actually guid
        public action?: MakerCheckerActionsEnum,
        public makerUser?: string, // actually guid
        public makerUserString?: string,
        public checkerUser?: string, // actually guid
        public checkerUserString?: string,
        public accepted?: boolean,
        public makerDate?: any,
        public checkerDate?: any,
        public motivation?: string,
        public files?: string[],
        public fileNames?: string[],
        public model?: string,
        public context?: string
    ) {
    }
}

