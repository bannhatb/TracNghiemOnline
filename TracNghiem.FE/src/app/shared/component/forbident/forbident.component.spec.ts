import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ForbidentComponent } from './forbident.component';

describe('ForbidentComponent', () => {
  let component: ForbidentComponent;
  let fixture: ComponentFixture<ForbidentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ForbidentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ForbidentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
