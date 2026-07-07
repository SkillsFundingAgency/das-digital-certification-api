/*
    Post-deployment patch:
    Backfill CreatedAt on dbo.[User] and dbo.[UserHistory].

    Rules:
    - Use the earliest ValidFrom available for each user.
    - If the user has history rows, that will come from UserHistory.
    - If the user has no history rows, it will come from the current User.ValidFrom.
    - Run once only.
*/

SET XACT_ABORT ON;
GO

IF NOT EXISTS
(
    SELECT 1
    FROM [dbo].[PatchLog]
    WHERE [PatchName] = 'Backfill_User_CreatedAt_From_ValidFrom'
)
BEGIN
    BEGIN TRANSACTION;

    ------------------------------------------------------------
    -- Backfill dbo.[User].CreatedAt
    ------------------------------------------------------------

    ;WITH AllVersions AS
    (
        SELECT
            Id,
            ValidFrom
        FROM [dbo].[User]

        UNION ALL

        SELECT
            Id,
            ValidFrom
        FROM [dbo].[UserHistory]
    ),
    CreatedDates AS
    (
        SELECT
            Id,
            MIN(ValidFrom) AS CreatedAt
        FROM AllVersions
        GROUP BY Id
    )
    UPDATE u
    SET u.CreatedAt = cd.CreatedAt
    FROM [dbo].[User] u
    INNER JOIN CreatedDates cd
        ON cd.Id = u.Id
    WHERE u.CreatedAt IS NULL
       OR u.CreatedAt <> cd.CreatedAt;

    ------------------------------------------------------------
    -- Backfill dbo.[UserHistory].CreatedAt
    ------------------------------------------------------------

    ;WITH AllVersions AS
    (
        SELECT
            Id,
            ValidFrom
        FROM [dbo].[User]

        UNION ALL

        SELECT
            Id,
            ValidFrom
        FROM [dbo].[UserHistory]
    ),
    CreatedDates AS
    (
        SELECT
            Id,
            MIN(ValidFrom) AS CreatedAt
        FROM AllVersions
        GROUP BY Id
    )
    UPDATE uh
    SET uh.CreatedAt = cd.CreatedAt
    FROM [dbo].[UserHistory] uh
    INNER JOIN CreatedDates cd
        ON cd.Id = uh.Id
    WHERE uh.CreatedAt IS NULL
       OR uh.CreatedAt <> cd.CreatedAt;

    ------------------------------------------------------------
    -- Mark patch as applied
    ------------------------------------------------------------

    INSERT INTO [dbo].[PatchLog] ([PatchName])
    VALUES ('Backfill_User_CreatedAt_From_ValidFrom');

    COMMIT TRANSACTION;
END;
GO