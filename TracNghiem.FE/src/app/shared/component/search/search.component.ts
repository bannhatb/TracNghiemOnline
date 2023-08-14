import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit {
  @Input() textSearch : string;
  @Output() search = new EventEmitter<string>();
  constructor() { }

  ngOnInit(): void {
  }
}
