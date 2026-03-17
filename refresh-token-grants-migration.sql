START TRANSACTION;

CREATE TABLE refresh_token_grants (
    "Id" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "TokenHash" character varying(256) NOT NULL,
    "ExpiresAtUtc" timestamp with time zone NOT NULL,
    "CreatedAtUtc" timestamp with time zone NOT NULL,
    "RevokedAtUtc" timestamp with time zone NULL,
    CONSTRAINT "PK_refresh_token_grants" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_refresh_token_grants_app_users_UserId" FOREIGN KEY ("UserId") REFERENCES app_users ("Id") ON DELETE CASCADE
);

CREATE UNIQUE INDEX "IX_refresh_token_grants_TokenHash" ON refresh_token_grants ("TokenHash");

CREATE INDEX "IX_refresh_token_grants_UserId_RevokedAtUtc" ON refresh_token_grants ("UserId", "RevokedAtUtc");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260317190000_AddRefreshTokenGrants', '8.0.11');

COMMIT;
