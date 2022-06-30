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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220615134530_initial1')
BEGIN
    CREATE TABLE [Airline] (
        [Id] int NOT NULL IDENTITY,
        [AirlineName] nvarchar(max) NULL,
        [IsEnable] bit NOT NULL,
        CONSTRAINT [PK_Airline] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220615134530_initial1')
BEGIN
    CREATE TABLE [FlightSchedule] (
        [Id] int NOT NULL IDENTITY,
        [FlightNumber] int NOT NULL,
        [AirlineId] int NOT NULL,
        [From] nvarchar(max) NULL,
        [To] nvarchar(max) NULL,
        [StartTime] datetime2 NOT NULL,
        [EndTime] datetime2 NOT NULL,
        [Instrument] nvarchar(max) NULL,
        [NoOfBusinessSeats] int NOT NULL,
        [NoOfNonBusinessSeats] int NOT NULL,
        [TicketCost] real NOT NULL,
        [Meal] nvarchar(max) NULL,
        CONSTRAINT [PK_FlightSchedule] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_FlightSchedule_Airline_AirlineId] FOREIGN KEY ([AirlineId]) REFERENCES [Airline] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220615134530_initial1')
BEGIN
    CREATE TABLE [Flight] (
        [Id] int NOT NULL IDENTITY,
        [FlightId] int NOT NULL,
        [AirlineName] nvarchar(max) NULL,
        [From] nvarchar(max) NULL,
        [To] nvarchar(max) NULL,
        [StartTime] datetime2 NOT NULL,
        [EndTime] datetime2 NOT NULL,
        [TicketCost] real NOT NULL,
        CONSTRAINT [PK_Flight] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Flight_FlightSchedule_FlightId] FOREIGN KEY ([FlightId]) REFERENCES [FlightSchedule] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220615134530_initial1')
BEGIN
    CREATE INDEX [IX_Flight_FlightId] ON [Flight] ([FlightId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220615134530_initial1')
BEGIN
    CREATE INDEX [IX_FlightSchedule_AirlineId] ON [FlightSchedule] ([AirlineId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220615134530_initial1')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220615134530_initial1', N'5.0.0');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220627142133_coupon')
BEGIN
    DROP TABLE [Flight];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220627142133_coupon')
BEGIN
    CREATE TABLE [Discount] (
        [Id] int NOT NULL IDENTITY,
        [Code] nvarchar(max) NULL,
        [Amount] float NOT NULL,
        CONSTRAINT [PK_Discount] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20220627142133_coupon')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20220627142133_coupon', N'5.0.0');
END;
GO

COMMIT;
GO

