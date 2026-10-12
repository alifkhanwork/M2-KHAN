
-- Platform from the stock code prefix
UPDATE dbo.Items SET Platform = 'SNES'        WHERE Code LIKE 'SNES-%';
GO

UPDATE dbo.Items SET Platform = 'Genesis'     WHERE Code LIKE 'GEN-%';
GO

UPDATE dbo.Items SET Platform = 'PlayStation' WHERE Code LIKE 'PS1-%';
GO

UPDATE dbo.Items SET Platform = 'N64'         WHERE Code LIKE 'N64-%';
GO

UPDATE dbo.Items SET Platform = 'NES'         WHERE Code LIKE 'NES-%';
GO

UPDATE dbo.Items SET Platform = 'Game Boy'    WHERE Code LIKE 'GB-%';
GO

UPDATE dbo.Items SET Platform = 'PS4'         WHERE Code = 'JPL-001';
GO

-- Quantities (a few at or below ReorderLevel = 2, so low-stock features have something to show)
UPDATE dbo.Items SET Quantity = 5  WHERE Code = 'SNES-001';
GO

UPDATE dbo.Items SET Quantity = 3  WHERE Code = 'SNES-002';
GO

UPDATE dbo.Items SET Quantity = 1  WHERE Code = 'SNES-003';
GO

UPDATE dbo.Items SET Quantity = 4  WHERE Code = 'SNES-004';
GO

UPDATE dbo.Items SET Quantity = 6  WHERE Code = 'GEN-001';
GO

UPDATE dbo.Items SET Quantity = 2  WHERE Code = 'GEN-005';
GO

UPDATE dbo.Items SET Quantity = 3  WHERE Code = 'PS1-001';
GO

UPDATE dbo.Items SET Quantity = 7  WHERE Code = 'PS1-002';
GO

UPDATE dbo.Items SET Quantity = 8  WHERE Code = 'PS1-012';
GO

UPDATE dbo.Items SET Quantity = 10 WHERE Code = 'N64-001';
GO

UPDATE dbo.Items SET Quantity = 0  WHERE Code = 'N64-005';
GO

UPDATE dbo.Items SET Quantity = 5  WHERE Code = 'N64-010';
GO

UPDATE dbo.Items SET Quantity = 9  WHERE Code = 'NES-001';
GO

UPDATE dbo.Items SET Quantity = 2  WHERE Code = 'NES-008';
GO

UPDATE dbo.Items SET Quantity = 6  WHERE Code = 'GB-001';
GO

UPDATE dbo.Items SET Quantity = 12 WHERE Code = 'GB-002';
GO

UPDATE dbo.Items SET Quantity = 4  WHERE Code = 'JPL-001';
GO

-- Mix up the conditions a bit
UPDATE dbo.Items SET [Condition] = 'Mint' WHERE Code IN ('SNES-003', 'N64-005', 'GB-001', 'PS1-012');
GO

UPDATE dbo.Items SET [Condition] = 'Fair' WHERE Code IN ('NES-008', 'GEN-005', 'GB-002');
GO

SELECT * FROM dbo.Items;
GO
