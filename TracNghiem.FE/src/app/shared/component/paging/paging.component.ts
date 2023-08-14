import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-paging',
  templateUrl: './paging.component.html',
  styleUrls: ['./paging.component.scss']
})
export class PagingComponent implements OnInit {

  constructor() { }
  @Input() totalPage : number;
  @Input() currentPage: number;
  @Output() changePage  = new EventEmitter<number>();

  ngOnInit(): void {
  }
  // changePageHandler(event: any){
  //   this.changePage.emit(event);
  // }
  caculatePage() : Array<number> {
    let max = Math.min(this.totalPage, this.currentPage+2);
    let min = Math.max(1, this.currentPage - 2);
    let arr = new Array<number>();
    for(let i=min; i < max+1 ; i++){
      arr.push(i);
    }
    return arr;
  }

}
