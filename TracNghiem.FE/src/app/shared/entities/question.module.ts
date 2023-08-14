import { AnswerModel } from "./answer.module";

export interface QuestionModel{
  id: number;
  questionContent: string;
  explaint : string;
  rightCount: number;
  typeId: number;
}
export interface QuestionFullModel{
  id: number;
  questionContent: string;
  explaint : string;
  rightCount: number;
  typeId: number;
  listAnswers : AnswerModel[];
  categories: number[];
}
