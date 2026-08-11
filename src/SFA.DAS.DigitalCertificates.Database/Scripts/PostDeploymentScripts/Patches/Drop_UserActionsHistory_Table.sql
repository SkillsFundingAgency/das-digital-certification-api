/*
    Pre-deployment patch:
    Drop UserActionHistoryTable
*/

SET XACT_ABORT ON;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[PatchLog]
    WHERE [PatchName] = 'Drop_UserActionsHistory_Table'
)
BEGIN
    BEGIN TRANSACTION;

    DROP TABLE IF EXISTS UserActionsHistory;

    ------------------------------------------------------------
    -- Mark patch as applied
    ------------------------------------------------------------

    INSERT INTO [dbo].[PatchLog] ([PatchName])
    VALUES ('Drop_UserActionsHistory_Table');

    COMMIT TRANSACTION;
END;
GO