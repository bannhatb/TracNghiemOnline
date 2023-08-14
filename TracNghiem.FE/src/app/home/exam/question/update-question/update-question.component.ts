import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ExamService } from 'src/app/shared/services/exam.service';
import { QuestionService } from 'src/app/shared/services/question.service';
@Component({
  selector: 'app-update-question',
  templateUrl: './update-question.component.html',
  styleUrls: ['./update-question.component.scss']
})
export class UpdateQuestionComponent implements OnInit {
  formQuestion : FormGroup;
  listCategory : any;
  listCategoryChoose = new Array<number>();
  listCateDisplay = new Array<string>();
  questionID : number;
  collectedQuestion: any;
  listLevels : any;
  isAdmin : any;
  constructor(private questionService: QuestionService, private activeRoute : ActivatedRoute,
    private examService: ExamService,
    private fb : FormBuilder) { }
    formUpdateQuestion : FormGroup;
    ngOnInit(): void {  
    this.formUpdateQuestion = this.fb.group({
      question : this.fb.group({
        questionId : [0],
        questionContent: [''],
        explaint: [''],
        typeId: [1],
        levelId : [1],
        categories : this.fb.array([3]),
        // Categories: [0]
      }),
      answers: this.fb.array([
        this.fb.group({
          answerId: [0],
          answerContent: [''],
          questionId: [''],
          rightAnswer: [false], 
        }),
      ])
    });
    
    this.getQuestion();
    this.GetAllLevels();
    this.GetAllCategory()
    
  }
  get answers(): FormArray {
    return this.formUpdateQuestion.get('answers') as FormArray;
  }


  addInput(): void {
    this.answers.push(this.fb.group({
      answerContent: [''],
      rightAnswer: [false]
    }));
  }
  updateQuestion(): void {
    const requestModel = {
      question: this.formUpdateQuestion.value.question,
      answers: this.formUpdateQuestion.value.answers
    };
    
    this.questionService.UpdateQuestion(requestModel).subscribe(
      (res) => {
        console.log(res)
        console.log(requestModel);
      },
      (err) => {
        console.log(requestModel);

        console.log(err);
      }
    );
  }
  GetAllLevels(){
    this.questionService.GetAllLevel().subscribe((res)=>{
      this.listLevels = res.result;
      console.log(this.listLevels);
    }, (err)=>{
      console.log(err.error.message);
    })
  }
  newCategory(event: any){
    let value = event.target.value as number;
    if(this.listCategoryChoose.indexOf(value) !==-1){
      let index = this.listCategoryChoose.indexOf(value);
      this.listCategoryChoose.splice(index,1);
    }
    else{
      this.listCategoryChoose.push(value);
    }
    console.log(this.listCategoryChoose);
  }
 
  getQuestion(){
    
    this.activeRoute.params.subscribe((id)=>{
      this.questionID = Number(id.id);
      this.isAdmin = id.admin;
      console.log(this.isAdmin);
      
      console.log(this.questionID);
    })
    // this.activeRoute.parent?.queryParams.subscribe(params => {
    //   this.isAdmin = params['admin'];
    //   console.log(this.isAdmin);
    // });
    this.questionService.GetQuestionDetail(String(this.questionID)).subscribe((res)=>{
      this.collectedQuestion = res.result;
      console.log(this.collectedQuestion);
      console.log( this.collectedQuestion.levelId);
      
      this.formUpdateQuestion.patchValue({
        question:{
          questionId : this.questionID,
          questionContent: this.collectedQuestion.questionContent,
          explaint: this.collectedQuestion.explaint,
          typeId: this.collectedQuestion.typeId,
          levelId: this.collectedQuestion.levelId,
          categories: this.collectedQuestion.Categories,
        }
      })
      
      this.answers.removeAt(0);
      this.collectedQuestion.listAnswers.forEach((answer:any) => {
        console.log(answer);
        
        this.answers.push(this.fb.group({
          answerId: [answer.id],
          answerContent: [answer.answerContent],
          questionId: [answer.questionId],
          rightAnswer: [answer.rightAnswer]
        }));
      })
    },
    (err)=>{
      console.log(err);
    }
    )
    
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
