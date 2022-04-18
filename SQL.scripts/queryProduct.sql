SELECT product.*, brand.BrandName, shop.ShopName, subcategory.SubCategoryName, price.PriceOnSell, producttype.ProductType
FROM Product product, Brand brand, Shop shop, SubCategory subcategory, ProductPrice price, ProductType producttype
WHERE brand.BrandId = 1 and
	  brand.BrandId = product.BrandId and
	  shop.ShopId = product.ShopId and
	  subcategory.SubCategoryId = product.SubCategoryId and 
	  producttype.ProductTypeId = price.ProductTypeId and price.ProductId = Product.ProductId and
	  producttype.ProductTypeId = price.ProductTypeId and price.ProductId = product.ProductId order by product.SubCategoryId

SELECT producttype.ProductType
FROM Product product, ProductPrice price, ProductType producttype
WHERE product.ProductId = 5 and
	  price.ProductTypeId = productType.ProductTypeId and price.ProductId = product.ProductId
	  