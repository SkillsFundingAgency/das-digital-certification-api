/*
    Pre-deployment patch:
    Delete ActionType (0) Reprint which is a duplicate
*/

SET XACT_ABORT ON;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[PatchLog]
    WHERE [PatchName] = 'Delete_Duplicate_Reprint_ActionType'
)
BEGIN
    BEGIN TRANSACTION;

    DELETE FROM [ActionType] WHERE [ActionType].Id = 0

    ------------------------------------------------------------
    -- Mark patch as applied
    ------------------------------------------------------------

    INSERT INTO [dbo].[PatchLog] ([PatchName])
    VALUES ('Delete_Duplicate_Reprint_ActionType');

    COMMIT TRANSACTION;
END;
GO