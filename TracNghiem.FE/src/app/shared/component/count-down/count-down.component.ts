import { Component, Input, OnInit } from '@angular/core';
import { interval, Subscription } from 'rxjs';

@Component({
  selector: 'app-count-down',
  templateUrl: './count-down.component.html',
  styleUrls: ['./count-down.component.scss']
})
export class CountDownComponent implements OnInit {

  constructor() { }

    @Input() time : number;
    seconds : number =0 ;
    minutes : number=0 ;
    hours : number =0;
    days : number =0;
    MiliSeconds : number;
    subscription: Subscription;
  ngOnInit(): void {
    this.MiliSeconds = this.time * 60 * 1000;
    this.subscription = interval(1000)
          .subscribe(x => { this.allocateTimeUnits(); });
  }
  allocateTimeUnits() {
    this.MiliSeconds = this.MiliSeconds -1000;
    this.seconds = Math.floor((this.MiliSeconds / 1000)%60);
    this.minutes = Math.floor((this.MiliSeconds / (1000*60)) % 60);
    this.hours = Math.floor((this.MiliSeconds / (1000*60 * 60)) % 24);
    this.days = Math.floor((this.MiliSeconds / (1000*60 * 60 * 24)));
}

}
