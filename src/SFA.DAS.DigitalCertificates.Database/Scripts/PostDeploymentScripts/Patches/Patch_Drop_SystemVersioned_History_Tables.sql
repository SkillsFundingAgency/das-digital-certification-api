/*
    Pre-deployment patch
*/

SET XACT_ABORT ON;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[PatchLog]
    WHERE [PatchName] = 'Patch_Drop_SystemVersioned_History_Tables'
)
BEGIN
    BEGIN TRANSACTION;

    DROP TABLE IF EXISTS AdminActionsHistory;
    DROP TABLE IF EXISTS SharingHistory;
    DROP TABLE IF EXISTS SharingEmailHistory;
    DROP TABLE IF EXISTS SharingAccessHistory;
    DROP TABLE IF EXISTS SharingEmailAccessHistory;
    DROP TABLE IF EXISTS UserAuthorisationHistory;
    DROP TABLE IF EXISTS UserHistory;

    ------------------------------------------------------------
    -- Mark patch as applied
    ------------------------------------------------------------

    INSERT INTO [dbo].[PatchLog] ([PatchName])
    VALUES ('Patch_Drop_SystemVersioned_History_Tables');

    COMMIT TRANSACTION;
END;
GO