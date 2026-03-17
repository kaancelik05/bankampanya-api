CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260316142834_InitialContentSchema') THEN
    CREATE TABLE assistant_prompt_templates (
        "Id" uuid NOT NULL,
        "Text" character varying(500) NOT NULL,
        "Tone" character varying(120) NOT NULL,
        "CreatedAtUtc" timestamp with time zone NOT NULL,
        "UpdatedAtUtc" timestamp with time zone NOT NULL,
        "Status" integer NOT NULL,
        "PublishedAtUtc" timestamp with time zone,
        CONSTRAINT "PK_assistant_prompt_templates" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260316142834_InitialContentSchema') THEN
    CREATE TABLE campaigns (
        "Id" uuid NOT NULL,
        "BankName" character varying(120) NOT NULL,
        "Category" character varying(120) NOT NULL,
        "Title" character varying(200) NOT NULL,
        "ShortDescription" character varying(500) NOT NULL,
        "RewardText" character varying(120) NOT NULL,
        "RewardType" integer NOT NULL,
        "DeadlineText" character varying(120) NOT NULL,
        "ValidFromUtc" timestamp with time zone,
        "ValidToUtc" timestamp with time zone,
        "ValidDateRangeLabel" character varying(120) NOT NULL,
        "IsProgressive" boolean NOT NULL,
        "ProgressTarget" integer,
        "NextActionText" character varying(500),
        "CreatedAtUtc" timestamp with time zone NOT NULL,
        "UpdatedAtUtc" timestamp with time zone NOT NULL,
        "Status" integer NOT NULL,
        "PublishedAtUtc" timestamp with time zone,
        CONSTRAINT "PK_campaigns" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260316142834_InitialContentSchema') THEN
    CREATE TABLE credit_offers (
        "Id" uuid NOT NULL,
        "BankName" character varying(120) NOT NULL,
        "Title" character varying(200) NOT NULL,
        "Type" integer NOT NULL,
        "Subtype" integer,
        "Rate" character varying(120) NOT NULL,
        "AmountRange" character varying(120) NOT NULL,
        "DetailSummary" character varying(600) NOT NULL,
        "Terms" jsonb NOT NULL,
        "CreatedAtUtc" timestamp with time zone NOT NULL,
        "UpdatedAtUtc" timestamp with time zone NOT NULL,
        "Status" integer NOT NULL,
        "PublishedAtUtc" timestamp with time zone,
        CONSTRAINT "PK_credit_offers" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260316142834_InitialContentSchema') THEN
    CREATE TABLE notification_templates (
        "Id" uuid NOT NULL,
        "Type" integer NOT NULL,
        "Title" character varying(200) NOT NULL,
        "Body" character varying(600) NOT NULL,
        "CtaLabel" character varying(120) NOT NULL,
        "Route" character varying(300) NOT NULL,
        "Tone" integer NOT NULL,
        "CreatedAtUtc" timestamp with time zone NOT NULL,
        "UpdatedAtUtc" timestamp with time zone NOT NULL,
        "Status" integer NOT NULL,
        "PublishedAtUtc" timestamp with time zone,
        CONSTRAINT "PK_notification_templates" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260316142834_InitialContentSchema') THEN
    CREATE TABLE campaign_terms (
        "Id" uuid NOT NULL,
        "CampaignId" uuid NOT NULL,
        "SortOrder" integer NOT NULL,
        "Text" character varying(500) NOT NULL,
        "CreatedAtUtc" timestamp with time zone NOT NULL,
        "UpdatedAtUtc" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_campaign_terms" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_campaign_terms_campaigns_CampaignId" FOREIGN KEY ("CampaignId") REFERENCES campaigns ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260316142834_InitialContentSchema') THEN
    CREATE TABLE tracking_templates (
        "Id" uuid NOT NULL,
        "CampaignId" uuid NOT NULL,
        "BankName" character varying(120) NOT NULL,
        "Title" character varying(200) NOT NULL,
        "Description" character varying(500) NOT NULL,
        "RequirementText" character varying(500) NOT NULL,
        "NextActionText" character varying(500) NOT NULL,
        "RewardText" character varying(120) NOT NULL,
        "DefaultProgressTarget" integer,
        "ProgressStatus" integer NOT NULL,
        "CreatedAtUtc" timestamp with time zone NOT NULL,
        "UpdatedAtUtc" timestamp with time zone NOT NULL,
        "Status" integer NOT NULL,
        "PublishedAtUtc" timestamp with time zone,
        CONSTRAINT "PK_tracking_templates" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_tracking_templates_campaigns_CampaignId" FOREIGN KEY ("CampaignId") REFERENCES campaigns ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260316142834_InitialContentSchema') THEN
    CREATE INDEX "IX_campaign_terms_CampaignId" ON campaign_terms ("CampaignId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260316142834_InitialContentSchema') THEN
    CREATE INDEX "IX_tracking_templates_CampaignId" ON tracking_templates ("CampaignId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260316142834_InitialContentSchema') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260316142834_InitialContentSchema', '8.0.11');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260316184702_AddWalletCards') THEN
    CREATE TABLE wallet_cards (
        "Id" uuid NOT NULL,
        "UserId" uuid NOT NULL,
        "BankName" character varying(120) NOT NULL,
        "CardType" character varying(120) NOT NULL,
        "CustomName" character varying(160) NOT NULL,
        "Status" integer NOT NULL,
        "CreatedAtUtc" timestamp with time zone NOT NULL,
        "UpdatedAtUtc" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_wallet_cards" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260316184702_AddWalletCards') THEN
    CREATE INDEX "IX_wallet_cards_UserId_BankName" ON wallet_cards ("UserId", "BankName");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260316184702_AddWalletCards') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260316184702_AddWalletCards', '8.0.11');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;


DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260316185512_AddUserCampaignsAndTrackingEvents') THEN
    CREATE TABLE user_campaigns (
        "Id" uuid NOT NULL,
        "UserId" uuid NOT NULL,
        "CampaignId" uuid NOT NULL,
        "TrackingTemplateId" uuid,
        "Status" integer NOT NULL,
        "ProgressCurrent" integer NOT NULL,
        "ProgressTarget" integer NOT NULL,
        "JoinedAtUtc" timestamp with time zone NOT NULL,
        "CompletedAtUtc" timestamp with time zone,
        "RewardedAtUtc" timestamp with time zone,
        "CreatedAtUtc" timestamp with time zone NOT NULL,
        "UpdatedAtUtc" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_user_campaigns" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_user_campaigns_campaigns_CampaignId" FOREIGN KEY ("CampaignId") REFERENCES campaigns ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_user_campaigns_tracking_templates_TrackingTemplateId" FOREIGN KEY ("TrackingTemplateId") REFERENCES tracking_templates ("Id") ON DELETE SET NULL
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260316185512_AddUserCampaignsAndTrackingEvents') THEN
    CREATE TABLE tracking_events (
        "Id" uuid NOT NULL,
        "UserCampaignId" uuid NOT NULL,
        "OccurredAtUtc" timestamp with time zone NOT NULL,
        "MerchantName" character varying(160) NOT NULL,
        "Amount" numeric(18,2),
        "AmountText" character varying(120) NOT NULL,
        "Qualified" boolean NOT NULL,
        "Note" character varying(500),
        "CreatedAtUtc" timestamp with time zone NOT NULL,
        "UpdatedAtUtc" timestamp with time zone NOT NULL,
        CONSTRAINT "PK_tracking_events" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_tracking_events_user_campaigns_UserCampaignId" FOREIGN KEY ("UserCampaignId") REFERENCES user_campaigns ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260316185512_AddUserCampaignsAndTrackingEvents') THEN
    CREATE INDEX "IX_tracking_events_UserCampaignId_OccurredAtUtc" ON tracking_events ("UserCampaignId", "OccurredAtUtc");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260316185512_AddUserCampaignsAndTrackingEvents') THEN
    CREATE INDEX "IX_user_campaigns_CampaignId" ON user_campaigns ("CampaignId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260316185512_AddUserCampaignsAndTrackingEvents') THEN
    CREATE INDEX "IX_user_campaigns_TrackingTemplateId" ON user_campaigns ("TrackingTemplateId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260316185512_AddUserCampaignsAndTrackingEvents') THEN
    CREATE UNIQUE INDEX "IX_user_campaigns_UserId_CampaignId" ON user_campaigns ("UserId", "CampaignId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260316185512_AddUserCampaignsAndTrackingEvents') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260316185512_AddUserCampaignsAndTrackingEvents', '8.0.11');
    END IF;
END $EF$;
COMMIT;

