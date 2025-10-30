BEGIN TRANSACTION;
CREATE TABLE [MembershipAcceptedDatas] (
    [IssueId] bigint NOT NULL IDENTITY,
    [RegistrationId] bigint NULL,
    [ReferanceNo] nvarchar(max) NULL,
    [Name] nvarchar(max) NULL,
    [CivilId] nvarchar(max) NULL,
    [PassportNo] nvarchar(max) NULL,
    [DateofBirth] datetime2 NULL,
    [GenderId] bigint NULL,
    [BloodGroupId] bigint NULL,
    [ProfessionId] bigint NULL,
    [WorkPlaceId] bigint NULL,
    [CountryCodeId] bigint NULL,
    [ContactNo] bigint NULL,
    [Email] nvarchar(max) NULL,
    [DistrictId] bigint NULL,
    [AreaId] bigint NULL,
    [ZoneId] bigint NULL,
    [UnitId] bigint NULL,
    [CampaignId] bigint NULL,
    [CampaignAmount] bigint NULL,
    [AmountRecieved] nvarchar(max) NULL,
    [PaymentTypeId] bigint NULL,
    [PaymentRemarks] nvarchar(max) NULL,
    [HearAboutUsId] bigint NULL,
    [Active] bit NOT NULL,
    [CreatedBy] bigint NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedBy] bigint NULL,
    [UpdatedDate] datetime2 NULL,
    CONSTRAINT [PK_MembershipAcceptedDatas] PRIMARY KEY ([IssueId]),
    CONSTRAINT [FK_MembershipAcceptedDatas_AreaDatas_AreaId] FOREIGN KEY ([AreaId]) REFERENCES [AreaDatas] ([AreaId]),
    CONSTRAINT [FK_MembershipAcceptedDatas_MembershipDetails_RegistrationId] FOREIGN KEY ([RegistrationId]) REFERENCES [MembershipDetails] ([MembershipId]),
    CONSTRAINT [FK_MembershipAcceptedDatas_Professions_ProfessionId] FOREIGN KEY ([ProfessionId]) REFERENCES [Professions] ([ProfessionId]),
    CONSTRAINT [FK_MembershipAcceptedDatas_Units_UnitId] FOREIGN KEY ([UnitId]) REFERENCES [Units] ([UnitId]),
    CONSTRAINT [FK_MembershipAcceptedDatas_WorkPlace_ProfessionId] FOREIGN KEY ([ProfessionId]) REFERENCES [WorkPlace] ([WorkPlaceId]),
    CONSTRAINT [FK_MembershipAcceptedDatas_Zones_ZoneId] FOREIGN KEY ([ZoneId]) REFERENCES [Zones] ([ZoneId])
);

CREATE TABLE [MemberShipCancelledDatas] (
    [IssueId] bigint NOT NULL IDENTITY,
    [RegistrationId] bigint NOT NULL,
    [ReferanceNo] nvarchar(max) NULL,
    [Name] nvarchar(max) NULL,
    [CivilId] nvarchar(max) NULL,
    [PassportNo] nvarchar(max) NULL,
    [DateofBirth] datetime2 NULL,
    [GenderId] bigint NULL,
    [BloodGroupId] bigint NULL,
    [ProfessionId] bigint NULL,
    [WorkPlaceId] bigint NULL,
    [CountryCode] bigint NULL,
    [ContactNo] bigint NULL,
    [Email] nvarchar(max) NULL,
    [DistrictId] bigint NULL,
    [AreaId] bigint NULL,
    [ZoneId] bigint NULL,
    [UnitId] bigint NULL,
    [CampaignId] bigint NULL,
    [CampaignAmount] bigint NULL,
    [AmountRecieved] bigint NULL,
    [PaymentTypeId] bigint NULL,
    [PaymentRemarks] nvarchar(max) NULL,
    [HearAboutUsId] bigint NULL,
    [Active] bit NOT NULL,
    [CreatedBy] bigint NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedBy] bigint NULL,
    [UpdatedDate] datetime2 NULL,
    CONSTRAINT [PK_MemberShipCancelledDatas] PRIMARY KEY ([IssueId]),
    CONSTRAINT [FK_MemberShipCancelledDatas_AreaDatas_AreaId] FOREIGN KEY ([AreaId]) REFERENCES [AreaDatas] ([AreaId]),
    CONSTRAINT [FK_MemberShipCancelledDatas_MembershipDetails_RegistrationId] FOREIGN KEY ([RegistrationId]) REFERENCES [MembershipDetails] ([MembershipId]) ON DELETE CASCADE,
    CONSTRAINT [FK_MemberShipCancelledDatas_Professions_ProfessionId] FOREIGN KEY ([ProfessionId]) REFERENCES [Professions] ([ProfessionId]),
    CONSTRAINT [FK_MemberShipCancelledDatas_Units_UnitId] FOREIGN KEY ([UnitId]) REFERENCES [Units] ([UnitId]),
    CONSTRAINT [FK_MemberShipCancelledDatas_WorkPlace_ProfessionId] FOREIGN KEY ([ProfessionId]) REFERENCES [WorkPlace] ([WorkPlaceId]),
    CONSTRAINT [FK_MemberShipCancelledDatas_Zones_ZoneId] FOREIGN KEY ([ZoneId]) REFERENCES [Zones] ([ZoneId])
);

CREATE TABLE [MembershipRejectedDatas] (
    [IssueId] bigint NOT NULL IDENTITY,
    [RegistrationId] bigint NOT NULL,
    [ReferanceNo] nvarchar(max) NULL,
    [Name] nvarchar(max) NULL,
    [CivilId] nvarchar(max) NULL,
    [PassportNo] nvarchar(max) NULL,
    [DateofBirth] datetime2 NULL,
    [GenderId] bigint NULL,
    [BloodGroupId] bigint NULL,
    [ProfessionId] bigint NULL,
    [WorkPlaceId] bigint NULL,
    [CountryCodeId] bigint NULL,
    [ContactNo] bigint NULL,
    [Email] nvarchar(max) NULL,
    [DistrictId] bigint NULL,
    [AreaId] bigint NULL,
    [ZoneId] bigint NULL,
    [UnitId] bigint NULL,
    [CampaignId] bigint NULL,
    [CampaignAmount] bigint NULL,
    [AmountRecieved] nvarchar(max) NULL,
    [PaymentTypeId] bigint NULL,
    [PaymentRemarks] nvarchar(max) NULL,
    [HearAboutUsId] bigint NULL,
    [Active] bit NOT NULL,
    [CreatedBy] bigint NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedBy] bigint NULL,
    [UpdatedDate] datetime2 NULL,
    CONSTRAINT [PK_MembershipRejectedDatas] PRIMARY KEY ([IssueId]),
    CONSTRAINT [FK_MembershipRejectedDatas_AreaDatas_AreaId] FOREIGN KEY ([AreaId]) REFERENCES [AreaDatas] ([AreaId]),
    CONSTRAINT [FK_MembershipRejectedDatas_MembershipDetails_RegistrationId] FOREIGN KEY ([RegistrationId]) REFERENCES [MembershipDetails] ([MembershipId]) ON DELETE CASCADE,
    CONSTRAINT [FK_MembershipRejectedDatas_Professions_ProfessionId] FOREIGN KEY ([ProfessionId]) REFERENCES [Professions] ([ProfessionId]),
    CONSTRAINT [FK_MembershipRejectedDatas_Units_UnitId] FOREIGN KEY ([UnitId]) REFERENCES [Units] ([UnitId]),
    CONSTRAINT [FK_MembershipRejectedDatas_WorkPlace_ProfessionId] FOREIGN KEY ([ProfessionId]) REFERENCES [WorkPlace] ([WorkPlaceId]),
    CONSTRAINT [FK_MembershipRejectedDatas_Zones_ZoneId] FOREIGN KEY ([ZoneId]) REFERENCES [Zones] ([ZoneId])
);

CREATE INDEX [IX_MembershipAcceptedDatas_AreaId] ON [MembershipAcceptedDatas] ([AreaId]);

CREATE INDEX [IX_MembershipAcceptedDatas_ProfessionId] ON [MembershipAcceptedDatas] ([ProfessionId]);

CREATE INDEX [IX_MembershipAcceptedDatas_RegistrationId] ON [MembershipAcceptedDatas] ([RegistrationId]);

CREATE INDEX [IX_MembershipAcceptedDatas_UnitId] ON [MembershipAcceptedDatas] ([UnitId]);

CREATE INDEX [IX_MembershipAcceptedDatas_ZoneId] ON [MembershipAcceptedDatas] ([ZoneId]);

CREATE INDEX [IX_MemberShipCancelledDatas_AreaId] ON [MemberShipCancelledDatas] ([AreaId]);

CREATE INDEX [IX_MemberShipCancelledDatas_ProfessionId] ON [MemberShipCancelledDatas] ([ProfessionId]);

CREATE INDEX [IX_MemberShipCancelledDatas_RegistrationId] ON [MemberShipCancelledDatas] ([RegistrationId]);

CREATE INDEX [IX_MemberShipCancelledDatas_UnitId] ON [MemberShipCancelledDatas] ([UnitId]);

CREATE INDEX [IX_MemberShipCancelledDatas_ZoneId] ON [MemberShipCancelledDatas] ([ZoneId]);

CREATE INDEX [IX_MembershipRejectedDatas_AreaId] ON [MembershipRejectedDatas] ([AreaId]);

CREATE INDEX [IX_MembershipRejectedDatas_ProfessionId] ON [MembershipRejectedDatas] ([ProfessionId]);

CREATE INDEX [IX_MembershipRejectedDatas_RegistrationId] ON [MembershipRejectedDatas] ([RegistrationId]);

CREATE INDEX [IX_MembershipRejectedDatas_UnitId] ON [MembershipRejectedDatas] ([UnitId]);

CREATE INDEX [IX_MembershipRejectedDatas_ZoneId] ON [MembershipRejectedDatas] ([ZoneId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250519093702_Issue_Membership', N'9.0.4');

CREATE TABLE [Campaigns] (
    [CampaignId] bigint NOT NULL IDENTITY,
    [CampaignName] nvarchar(max) NULL,
    [StartDate] datetime2 NULL,
    [EndDate] datetime2 NULL,
    [MemberShipFee] nvarchar(max) NULL,
    [Active] bit NOT NULL,
    [CreatedBy] bigint NULL,
    [CreatedDate] datetime2 NULL,
    [UpdatedBy] bigint NULL,
    [UpdatedDate] datetime2 NULL,
    CONSTRAINT [PK_Campaigns] PRIMARY KEY ([CampaignId])
);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250519111353_Campaign_Table', N'9.0.4');

ALTER TABLE [MembershipDetails] ADD [HearAboutus] bigint NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250519113815_HearAboutusadded', N'9.0.4');

ALTER TABLE [MembershipRejectedDatas] ADD [RejectionReason] nvarchar(max) NULL;

ALTER TABLE [MembershipRejectedDatas] ADD [RejectionRemarks] nvarchar(max) NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250519131854_Reject_Member_Data', N'9.0.4');

ALTER TABLE [MembershipRejectedDatas] DROP CONSTRAINT [FK_MembershipRejectedDatas_MembershipDetails_RegistrationId];

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[MembershipRejectedDatas]') AND [c].[name] = N'RegistrationId');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [MembershipRejectedDatas] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [MembershipRejectedDatas] ALTER COLUMN [RegistrationId] bigint NULL;

ALTER TABLE [MembershipRejectedDatas] ADD CONSTRAINT [FK_MembershipRejectedDatas_MembershipDetails_RegistrationId] FOREIGN KEY ([RegistrationId]) REFERENCES [MembershipDetails] ([MembershipId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250519132520_Reject_Member_Data_fix', N'9.0.4');

COMMIT;
GO

