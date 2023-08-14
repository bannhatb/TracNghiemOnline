import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ExamService } from 'src/app/shared/services/exam.service';
import { QuestionService } from 'src/app/shared/services/question.service';

@Component({
  selector: 'app-add-question',
  templateUrl: './add-question.component.html',
  styleUrls: ['./add-question.component.scss']
})
export class AddQuestionComponent implements OnInit {
  formQuestion : FormGroup;
  listCategory : any;
  listCategoryChoose = new Array<number>();
  listCateDisplay = new Array<string>();
  listLevels : any;
  constructor(private questionService: QuestionService,
    private examService: ExamService,
    private fb : FormBuilder) { }
    formAddQuestion : FormGroup;
  ngOnInit(): void {
    this.formAddQuestion = this.fb.group({
      question : this.fb.group({
        questionContent: [''],
        explaint: [''],
        levelID: [1],
        typeId: [1],
        Categories : this.fb.array([1]),
      }),
      answers: this.fb.array([
        this.fb.group({
          answerContent: [''],
          rightAnswer: [false],
        }),
      ])
    });
    this.GetAllLevels();
    this.GetAllCategory();
  }
  get answers(): FormArray {
    return this.formAddQuestion.get('answers') as FormArray;
  }
  get question(): FormGroup {
    return this.formAddQuestion.get('question') as FormGroup;
  }
  inputs: string[] = [];

  addInput(): void {
    this.answers.push(this.fb.group({
      answerContent: [''],
      rightAnswer: [false]
    }));
  }
  addQuestion(): void {
    const requestModel = {
      question:  this.formAddQuestion.value.question,
      answers: this.formAddQuestion.value.answers
    };

    this.questionService.AddNewQuestion(requestModel).subscribe(
      (res) => {
        console.log(requestModel);
      },
      (err) => {
        console.log(requestModel);

        console.log(err);
      }
    );
  }
  newCategory(event: any){
    let value = event.target.value;
    if(this.listCategoryChoose.indexOf(value) !==-1){
      let index = this.listCategoryChoose.indexOf(value);
      this.listCategoryChoose.splice(index,1);
      this.listCateDisplay.splice(index,1);
    }
    else{
      this.listCategoryChoose.push(Number(value));
      this.listCateDisplay.push(this.GetNameCate(value));
    }
    console.log(this.listCategoryChoose);
    console.log(this.listCateDisplay);
  }
  GetNameCate(id : number)  {
    let name = ''
    this.listCategory.result.items.forEach(
      (item : any) => {
        if (item.id == id) {
          name = item.categoryName;
        }
      }
    )
    return name;
  }
  GetAllLevels(){
    this.questionService.GetAllLevel().subscribe((res)=>{
      this.listLevels = res.result;
      console.log(this.listLevels);
    }, (err)=>{
      console.log(err.error.message);
    })
  }
  GetAllCategory(){
    this.examService.GetCategory().subscribe((res)=>{
      this.listCategory = res;
      console.log(this.listCategory);
    }, (err)=>{
      console.log(err.error.message);
    })
  }
}
