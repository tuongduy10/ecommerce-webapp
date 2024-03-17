export interface IBrand {
  id: number;
  name: string;
  imagePath: string;
  status: boolean;
  createdDate: "2023-02-19T00:00:00";
  highlights: boolean;
  new: any;
  description: string;
  descriptionTitle: string;
  category: string;
  categoryIds: number[];
  shops: any;
  categoryNames: string[]
}

export interface ICategory {
  categoryId: number;
  categoryName: string;
}

export interface IOptionValue {
  id?: number;
  name: string;
  isBase?: boolean
  totalRecord?: number; // total product records
}
export interface IOption {
  id: number;
  name: string;
  values: IOptionValue[];
}
export interface ISubCategory {
  id: number;
  name: string;
  categoryId?: number;
  category?: ICategory;
  optionList?: IOption[];
  attributes?: IAttribute[];
}

export interface IAttribute {
  id: number;
  name: string;
  value: string | null;
}
