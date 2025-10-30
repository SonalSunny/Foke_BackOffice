BEGIN TRANSACTION;
ALTER TABLE [NotificationDatas] ADD [RemovedNumbers] nvarchar(max) NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250729053321_RemovedNumbersData', N'9.0.4');

COMMIT;
GO

