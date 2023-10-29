export interface IComment {
  id: number;
  value: number;
  comment: string;
  productId: number;
  createDate: any;
  repliedId: number | null;
  parentId: number | null;
  htmlPosition: string | null;
  userId: number;
  userRepliedId: number | null;
  idsToDelete: string;
  replies: any[];
  [key: string]: any;
}

export interface IProduct {
  id: number;
  code: string;
  ppc: string;
  name: string;
  description: string;
  size: string;
  delivery: string;
  repay: string;
  insurance: string;
  isLegit: boolean;
  brand: {
    id: number;
    name: string;
    description: string;
    descriptionTitle: string;
    imagePath: string;
  };
  shop: {
    id: number;
    name: string;
  };
  options: {
    id: number;
    name: string;
    values: {
      id: number;
      name: string;
    }[];
  }[];
  attributes: {
    id: number;
    value: string;
    name: string;
  }[];
  importDate: Date;
  // price
  discountPercent: number;
  priceAvailable: number;
  pricePreOrder: number;
  discountAvailable: number;
  discountPreOrder: number;
  // images
  imagePaths: string[];
  userImagePaths: string[];
  // rating
  review: {
    avgValue: number;
    totalRating: number;
    rates: IComment[];
  };
}
