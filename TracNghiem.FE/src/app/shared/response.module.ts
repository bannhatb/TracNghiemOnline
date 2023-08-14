export interface Response<T> {
  result? : T;
  status? : boolean;
  message? : string;
}
export interface ResponseDefault{
  data : any;
}
export interface Pagination<T>{
  total?: number;
  items : T[]
}
export interface ResponseToken {
  token : string;
}
export enum ErrorCode
  {
  Success= "00000000",
  BadRequest ="00000001",
  ValidateError = "00000010",
  NotFound   = "00000011",
  NotEmpty   = "00000100",
  ShortLengthData   = "00000101",
  OverLengthData   = "00000111",
  InvalidFormat   = "00001000",
  InvalidVerifyPassword   = "00001001",
  InvalidCurrentPassword   = "00001011",
  LockoutUser   = "00001111",
  ExistUserOrEmail   = "00010000",
  ExistStore   = "00010001",
  ExcuteDB   = "00010011",
  Forbidden   = "00010111",
  NullList   = "00011111",
  InvalidData   = "00100000",
  AnswerLess   = "00100001",
  MissingRightAnswer   = "00100011",
  FalseTypeQuestion   = "00100111",
  QuestionNotExist   = "00101111",
  ExamNotExist   = "00111111",
  GreaterThanZero   = "01000000",
  DataExist   = "00101111",
  SameData   = "00111111",
  NothingChange = "01000000",
  TestUserDone = "01000001",
  UserDoingTest = "01000010",
  NullFileUpload = "01000011",
  InvalidQuestionCount = "01000100",
  BlockAdmin = "01000101",
  WrongPasss = "01000111",
  BlockUser = "01001000"
  }
