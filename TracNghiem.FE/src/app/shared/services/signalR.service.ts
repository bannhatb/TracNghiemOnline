import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import * as signalR from "@microsoft/signalr";
import { environment } from "src/environments/environment";
import { TestService } from "./test.service";


@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  public connectionId : string;
  result : any;
  private thenable: Promise<void>
public hubConnection: signalR.HubConnection
  constructor(private testService : TestService,
    private router : Router){
  }
  private start() {
    this.thenable = this.hubConnection.start();
    this.thenable
      .then(() => console.log('Connection started!'))
      .catch(err => console.log('Error while establishing connection :('));
  }
  public startConect() {
    this.hubConnection = new signalR.HubConnectionBuilder()
                            .withUrl(environment.API_ENDPOINT + '/signalr')
                            .build();
    this.start();
    // this.hubConnection
    //   .start()
    //   .then(() => console.log('Connection started'))
    //   .catch(err => console.log('Error while starting connection: ' + err));
    //   console.log(this.hubConnection);
  }
  public GetAlo = () => {
    this.hubConnection.on('alo', (data) => {
      this.result = data;
      console.log(data);
    });
  }
  public getConnectionId = () => {
    this.hubConnection.invoke('getconnectionid').then(
      (data) => {
        console.log(data);
          this.connectionId = data;
        }
    );
  }
  public GetOneQuestionUserTestClient(testId : number, page : number){
    this.hubConnection.invoke('getOneQuestionUserTestServer', testId, page)
    .catch(err => console.error(err))
    .then(data => {
      console.log(data);
    });
  }
  public addSendDataListener = () => {
    this.hubConnection.on('sendata', (data) => {
      this.result = data;
    })
  }
  public TimeRun(time : number, testId : number){
    this.thenable.then(()=>{
      this.hubConnection.invoke('presstime', time)
      .catch(err => console.log(err));
      console.log("goi press time");
      this.hubConnection.on("dung", () =>{
        console.log("go");
        this.testService.CacularPoint(testId).subscribe((res)=>{
          console.log(res);
        })
        this.hubConnection.stop().then(() =>{
          console.log("disconnected")
        });
        this.router.navigateByUrl(`/test/user-point/${testId}`);
    })})
  }
}
