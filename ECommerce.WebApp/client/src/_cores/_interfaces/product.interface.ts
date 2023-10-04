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
    rates: {
      id: number;
      value: number; // 1 - 5
      comment: string;
      htmlPosition: string;
      repliedId: number;
      parentId: number;
      productId: number;
      idsToDelete: string;

      userId: number;
      userRepliedId: number;
      createDate: number;
    }[];
  };
}
