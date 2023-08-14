import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TestExamComponent } from './test-exam.component';

describe('TestExamComponent', () => {
  let component: TestExamComponent;
  let fixture: ComponentFixture<TestExamComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TestExamComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TestExamComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
