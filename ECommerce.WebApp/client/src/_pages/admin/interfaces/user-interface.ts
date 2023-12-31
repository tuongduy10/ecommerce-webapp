export interface IPagingGetRequest {
  keyword: string;
  pageIndex: number;
  pageSize: number;
}

export interface IUserGetParam extends IPagingGetRequest {
  userId: number;
}
