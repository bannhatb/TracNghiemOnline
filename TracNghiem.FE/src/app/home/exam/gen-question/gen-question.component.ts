import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ConsoleLogger } from '@microsoft/signalr/dist/esm/Utils';
import { ExamService } from 'src/app/shared/services/exam.service';
import { NotificationService } from 'src/app/shared/services/notification.service';

@Component({
  selector: 'app-gen-question',
  templateUrl: './gen-question.component.html',
  styleUrls: ['./gen-question.component.scss']
})
export class GenQuestionComponent implements OnInit {
  formGen :  FormGroup;
  examId : number;
  selectedFile : string;
  result : any;
  constructor(private examService: ExamService,
    private fb : FormBuilder,
    private activeRoute: ActivatedRoute,
    private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.formGen = this.fb.group({
      file : [''],
      splitQuestion : [''],
      rightMark : ['']
    });
  }
  onSelectFile(event : any){
    //this.selectedFile= <File>event.target.files[0];
    const fileList: FileList = event.target.files;
    if (fileList.length > 0) {
      const file: File = fileList[0];
      this.examService.UpFileWord(file).subscribe((res) => {
        console.log(res);
        this.selectedFile = res.data;
      }, (err)=>{
        console.log(err.error.message);
      });
    }
  }
  Submit(){
    this.activeRoute.params.subscribe((id)=>{
      this.examId = id.id;
    });
    let requestModel ={
      fileUp : this.selectedFile,
      splitNumberAndContent : this.formGen.value.splitQuestion,
      rightMark : this.formGen.value.rightMark,
      examId : this.examId
    };
    this.examService.GenQuestionAuto(requestModel).subscribe((res)=>{
      this.result = res;
      console.log(this.result);
      if(this.result.message== "00000000"){
        this.notificationService.success("Tạo đề thi thành công");
      }
    }, (err)=>{
      console.log(err.error.message);
      this.notificationService.error("Đã có lỗi xảy ra!");
    });
  }



}
