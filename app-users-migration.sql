START TRANSACTION;

CREATE TABLE app_users (
    "Id" uuid NOT NULL,
    "FullName" character varying(160) NOT NULL,
    "Email" character varying(160) NOT NULL,
    "Phone" character varying(32) NOT NULL,
    "PasswordHash" character varying(512) NOT NULL,
    "CreatedAtUtc" timestamp with time zone NOT NULL,
    "UpdatedAtUtc" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_app_users" PRIMARY KEY ("Id")
);

CREATE UNIQUE INDEX "IX_app_users_Email" ON app_users ("Email");

CREATE UNIQUE INDEX "IX_app_users_Phone" ON app_users ("Phone");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260316223659_AddAppUsers', '8.0.11');

COMMIT;

