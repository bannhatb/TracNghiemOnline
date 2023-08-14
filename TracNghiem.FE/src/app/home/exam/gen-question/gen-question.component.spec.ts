import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GenQuestionComponent } from './gen-question.component';

describe('GenQuestionComponent', () => {
  let component: GenQuestionComponent;
  let fixture: ComponentFixture<GenQuestionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GenQuestionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GenQuestionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
