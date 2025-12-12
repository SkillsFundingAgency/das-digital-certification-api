/* 
   All system versioning tables must be explictly created and modified as the DACPAC does not correctly automatically sychronize added columns.
   
   The system versioning is turned off then the DACPAC is allowed to add columns and then the system versioning is turned back on.

   Ensure that any added columns have an explicitly named constraint and that the same constraint is added to both the main table and the system versioning table.
*/

IF EXISTS (
    SELECT 1
    FROM sys.tables t
    JOIN sys.schemas s ON t.schema_id = s.schema_id
    WHERE t.name = 'User'
    AND s.name = 'dbo'
    AND t.temporal_type = 2
)
BEGIN
    ALTER TABLE [dbo].[User]
    SET (
    SYSTEM_VERSIONING = OFF
    );
END

IF EXISTS (
    SELECT 1
    FROM sys.tables t
    JOIN sys.schemas s ON t.schema_id = s.schema_id
    WHERE t.name = 'UserAuthorisation'
    AND s.name = 'dbo'
    AND t.temporal_type = 2
)
BEGIN
    ALTER TABLE [dbo].[UserAuthorisation]
    SET (
      SYSTEM_VERSIONING = OFF
    );
END

IF EXISTS (
    SELECT 1
    FROM sys.tables t
    JOIN sys.schemas s ON t.schema_id = s.schema_id
    WHERE t.name = 'Sharing'
    AND s.name = 'dbo'
    AND t.temporal_type = 2
)
BEGIN
    ALTER TABLE [dbo].[Sharing]
    SET (
    SYSTEM_VERSIONING = OFF
    );
END

IF EXISTS (
    SELECT 1
    FROM sys.tables t
    JOIN sys.schemas s ON t.schema_id = s.schema_id
    WHERE t.name = 'SharingAccess'
    AND s.name = 'dbo'
    AND t.temporal_type = 2
)
BEGIN
    ALTER TABLE [dbo].[SharingAccess]
    SET (
        SYSTEM_VERSIONING = OFF
    );
END

IF EXISTS (
    SELECT 1
    FROM sys.tables t
    JOIN sys.schemas s ON t.schema_id = s.schema_id
    WHERE t.name = 'SharingEmail'
    AND s.name = 'dbo'
 AND t.temporal_type = 2
)
BEGIN
    ALTER TABLE [dbo].[SharingEmail]
    SET (
        SYSTEM_VERSIONING = OFF
    );
END

IF EXISTS (
    SELECT 1
    FROM sys.tables t
    JOIN sys.schemas s ON t.schema_id = s.schema_id
    WHERE t.name = 'SharingEmailAccess'
    AND s.name = 'dbo'
    AND t.temporal_type = 2
)
BEGIN
    ALTER TABLE [dbo].[SharingEmailAccess]
    SET (
        SYSTEM_VERSIONING = OFF
    );
END

