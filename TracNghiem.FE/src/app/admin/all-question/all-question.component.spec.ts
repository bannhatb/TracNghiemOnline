import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllQuestionComponent } from './all-question.component';

describe('AllQuestionComponent', () => {
  let component: AllQuestionComponent;
  let fixture: ComponentFixture<AllQuestionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AllQuestionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AllQuestionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
