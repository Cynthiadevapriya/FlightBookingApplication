IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220622090259_init')
BEGIN
    CREATE TABLE [BookingDetails] (
        [BookingId] int NOT NULL IDENTITY,
        [UserName] nvarchar(max) NULL,
        [Email] nvarchar(max) NULL,
        [PNRNumber] bigint NOT NULL,
        [From] nvarchar(max) NULL,
        [To] nvarchar(max) NULL,
        [StartTime] datetime2 NOT NULL,
        [EndTime] datetime2 NOT NULL,
        [NoOfSeatsBooked] int NOT NULL,
        [Meal] nvarchar(max) NULL,
        [SeatNumbers] nvarchar(max) NULL,
        CONSTRAINT [PK_BookingDetails] PRIMARY KEY ([BookingId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220622090259_init')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220622090259_init', N'5.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220626090249_booking')
BEGIN
    ALTER TABLE [BookingDetails] ADD [FlightNumber] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220626090249_booking')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220626090249_booking', N'5.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220627142634_initited')
BEGIN
    CREATE TABLE [Coupon] (
        [Id] int NOT NULL IDENTITY,
        [Code] nvarchar(max) NULL,
        [Amount] float NOT NULL,
        CONSTRAINT [PK_Coupon] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220627142634_initited')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220627142634_initited', N'5.0.0');
END;
GO

COMMIT;
GO

