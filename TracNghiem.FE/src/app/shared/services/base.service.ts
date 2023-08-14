import { HttpClient, HttpErrorResponse, HttpHeaders, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, throwError } from "rxjs";
import { catchError, map } from "rxjs/operators";
import { environment } from "src/environments/environment";
import { Response } from "../response.module";

@Injectable({
  providedIn: 'root'
})
export class BaseService{
  constructor(public httpClient: HttpClient){

  }
  get ApiEndpoint(): string {
    return environment.API_ENDPOINT;
  }
  getToken() {
    let accessToken = localStorage.getItem('PBL7') ? localStorage.getItem('PBL7') : sessionStorage.getItem('PBL7') || '';
    if (accessToken !== '') {
      return `Bearer ${accessToken}`;
    } else {
      return '';
    }
  }
  clearLocalStorage(){
    localStorage.removeItem('PBL7');
  }
  getRequestOptions() {
    var token = this.getToken();

    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'api-key' : 'f754287e4dd49f7ad5b1',
        Authorization: token
      })
    };
    return httpOptions;
  }

  /**
   * Get method
   * @param url
   */
   get(url : string) {
    return this.httpClient.get(this.ApiEndpoint + url, this.getRequestOptions()).pipe(catchError(this.handleError));
  }

  /**
   * Get method
   * @param url
   */
   getnew(url : string) {
    return this.httpClient.get('https://eventsonmetaverse.com' + url, this.getRequestOptions()).pipe(catchError(this.handleError));
  }

  /**
   * Post method
   * @param url
   * @param data
   */
  post(url : string, data : any) {
    data = JSON.stringify(data);
    return this.httpClient.post(this.ApiEndpoint + url, data, this.getRequestOptions()).pipe(catchError(this.handleError));
  }
  put(url : string, data: any){
    data = JSON.stringify(data);
    return this.httpClient.put(this.ApiEndpoint + url, data, this.getRequestOptions()).pipe(catchError(this.handleError));
  }
  delete(url: string){
    return this.httpClient.delete(this.ApiEndpoint + url, this.getRequestOptions()).pipe(catchError(this.handleError));
  }
  accessToken = localStorage.getItem('PBL7') ? localStorage.getItem('PBL7') : sessionStorage.getItem('PBL7') || '';
  getHeaders(): any{
    const httpOptions = new HttpHeaders()
    .set( 'Authorization', `Bearer ${this.accessToken}`);
    return httpOptions;
  }
  public postFile<T>(url: string, data?: any, headers?: any): Observable<any> {
    return this.httpClient
      .post(this.ApiEndpoint + url, data, { headers })
      .pipe(map((result: Response<T>) => result.result as T));
  }
  /**
   * Upload File
   * @param url
   * @param file
   */
   uploadFile(url: string, file: FormData) {
    const req = new HttpRequest('POST', this.ApiEndpoint + url, file, this.getOptionsProcess());
    return this.httpClient.request(req);
  }

  getOptionsProcess() {
    var token = this.getToken();

    const httpOptions = {
      headers: new HttpHeaders({
        Authorization: token
      }),
      reportProgress: true
      // observe: 'events'
    };

    return httpOptions;
  }

  private handleError(error : HttpErrorResponse){
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      // The response body may contain clues as to what went wrong,
      console.error(`Backend returned code ${error.status}, ` + `body was: ${error.message}`);
    }
    // return an observable with a user-facing error message
    return throwError(error);
    // 'Something bad happened; please try again later.');
  }
}
