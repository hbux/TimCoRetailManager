﻿CREATE PROCEDURE [dbo].[spSaleDetail_Insert]
	@SaleId int,
	@ProductId int,
	@Quantity int,
	@PurchasePrice MONEY,
	@Tax MONEY
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO dbo.SaleDetail(SaleId, ProductId, Quantity, PurchasePrice, Tax)
	VALUES (@SaleId, @ProductId, @Quantity, @PurchasePrice, @Tax);
END