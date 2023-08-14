export class UrlQuery{
  pageNumber:number = 1;
  pageSize:number = 10;
  keyword:string = "";
  constructor(page?: number, pageSize?: number, keyword?: string){
    this.pageNumber = page || 1;
    this.pageSize = pageSize || 10;
    this.keyword = keyword || '';
  }
}
