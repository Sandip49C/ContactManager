CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

CREATE UNIQUE INDEX `IX_Contacts_Phone` ON `Contacts` (`Phone`);

CREATE UNIQUE INDEX `IX_Contacts_Email` ON `Contacts` (`Email`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20250306181614_AddUniqueConstraintsToPhoneAndEmail', '8.0.13');

COMMIT;

