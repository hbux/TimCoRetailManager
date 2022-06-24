CREATE PROCEDURE [dbo].[spSale_SaleReport]
	@param1 int = 0,
	@param2 int
AS
BEGIN
	SET NOCOUNT ON

	SELECT [dbo].[Sale].[SaleDate], [dbo].[Sale].[SubTotal], [dbo].[Sale].[Tax], [dbo].[Sale].[Total], 
	[dbo].[User].[FirstName], [dbo].[User].[LastName], [dbo].[User].[EmailAddress]
	FROM dbo.Sale
	INNER JOIN dbo.[User]
	ON Sale.CashierId = [User].Id
END