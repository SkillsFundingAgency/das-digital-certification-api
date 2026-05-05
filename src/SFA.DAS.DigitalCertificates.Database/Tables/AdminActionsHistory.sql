CREATE TABLE [dbo].[AdminActionsHistory]
(
    [Id] UNIQUEIDENTIFIER NOT NULL,
    [Username] VARCHAR(255) NOT NULL,
    [ActionTime] DATETIME2 NOT NULL,
    [Action] VARCHAR(50) NOT NULL,
    [UserActionId] BIGINT NOT NULL,
    [ValidFrom] DATETIME2 (0) NOT NULL,
    [ValidTo] DATETIME2 (0) NOT NULL
)
