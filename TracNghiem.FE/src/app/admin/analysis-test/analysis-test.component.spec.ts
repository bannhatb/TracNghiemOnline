import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AnalysisTestComponent } from './analysis-test.component';

describe('AnalysisTestComponent', () => {
  let component: AnalysisTestComponent;
  let fixture: ComponentFixture<AnalysisTestComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AnalysisTestComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AnalysisTestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
