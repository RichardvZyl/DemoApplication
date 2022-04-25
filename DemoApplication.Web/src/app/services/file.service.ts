import { HttpClient, HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { FileModel } from "../models/file/file.model";
import { environment } from "../../environments/environment";

@Injectable({ providedIn: "root" })


// this class is used to define the interactions with the backend pertaining to Files
export class AppFileService {
    documentUrl = environment.apiUrl + "Documents";
    reader: FileReader = new FileReader();
    doc: FileModel = new FileModel();

    constructor(private readonly http: HttpClient) {
        console.log("AppFileService Constructor called")
     }


    async uploadFile(file: File): Promise<any> {
        // Upload a file to the backend server from client
        console.log("upload function called within FileService");

        const formData = new FormData();

        formData.append("File", file);

        formData.forEach((value, key) => {
            console.log("key %s: value %s", key, value);
            });

        const id = await this.http.post<string>(`${this.documentUrl}`, formData).toPromise();

        return id;
    }

    removeFile(id: string){
        this.http.post(`${this.documentUrl}/Remove/${id}`, null).subscribe(
            data => console.log(data),
            error  => alert(error.error ?? error.message)
        );
    }

    async downloadFile(id: string) : Promise<HttpResponse<Blob>> { // Promise<HttpResponse<Blob>>
        // const headers = new HttpHeaders().append("Content-Disposition", "multipart/form-data");

        const result = await this.http.get(`${this.documentUrl}/${id}`, { responseType: "blob", observe: "response" }).toPromise();

        return result;
    }
}
