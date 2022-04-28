SELECT product.ProductName
FROM Product product, Shop shop, Brand brand, ProductOptionValue optionVal
WHERE	product.ShopId = shop.ShopId and 
		product.BrandId = brand.BrandId and 
		product.ProductId = optionVal.ProductId
