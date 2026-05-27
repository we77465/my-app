-- 新臺幣定期存款專案利率申請 - 資料庫重建腳本
-- 執行環境：chb_iom  ⚠ 會刪除舊資料

USE chb_iom;
GO

IF OBJECT_ID('dbo.NTD_Log',      'U') IS NOT NULL DROP TABLE dbo.NTD_Log;
IF OBJECT_ID('dbo.NTD_Detail',   'U') IS NOT NULL DROP TABLE dbo.NTD_Detail;
IF OBJECT_ID('dbo.NTD_Main',     'U') IS NOT NULL DROP TABLE dbo.NTD_Main;
IF OBJECT_ID('dbo.NTD_RateType', 'U') IS NOT NULL DROP TABLE dbo.NTD_RateType;
GO

CREATE TABLE dbo.NTD_RateType (
    RateCode  NVARCHAR(10) NOT NULL,
    RateName  NVARCHAR(50) NOT NULL,
    CONSTRAINT PK_NTD_RateType PRIMARY KEY (RateCode)
);
GO

CREATE TABLE dbo.NTD_Main (
    ApplyNo      NVARCHAR(20)   NOT NULL,
    BranchCode   NVARCHAR(10)   NOT NULL,
    ApplyDate    DATE           NOT NULL,
    CustomerName NVARCHAR(100)  NOT NULL,
    CustomerID   NVARCHAR(20)   NULL,
    GroupName    NVARCHAR(100)  NULL,
    TotalAmount  DECIMAL(18,2)  NULL,
    Reason       NVARCHAR(3000) NULL,
    IsRelated    NVARCHAR(2)    NULL,
    Contrib3M    DECIMAL(18,2)  NULL,
    Contrib6M    DECIMAL(18,2)  NULL,
    Contrib1Y    DECIMAL(5,2)   NULL,
    RelatedAmt   DECIMAL(18,2)  NULL,
    BranchNote   NVARCHAR(3000) NULL,
    RateTypeCode NVARCHAR(10)   NULL,
    Status       INT            NOT NULL DEFAULT 0,
    CreateUser   NVARCHAR(20)   NOT NULL,
    CreateTime   DATETIME       NOT NULL DEFAULT GETDATE(),
    UpdateUser   NVARCHAR(20)   NULL,
    UpdateTime   DATETIME       NULL,
    CONSTRAINT PK_NTD_Main PRIMARY KEY (ApplyNo),
    CONSTRAINT FK_NTD_Main_RateType FOREIGN KEY (RateTypeCode) REFERENCES dbo.NTD_RateType (RateCode)
);
GO

CREATE TABLE dbo.NTD_Detail (
    DetailID     INT            NOT NULL IDENTITY(1,1),
    ApplyNo      NVARCHAR(20)   NOT NULL,
    SeqNo        INT            NOT NULL,
    DepositType  NVARCHAR(20)   NULL,
    PeriodMonth  INT            NULL,
    Amount       DECIMAL(18,4)  NULL,
    ProposedRate DECIMAL(8,4)   NULL,
    StartDate    DATE           NULL,
    EndDate      DATE           NULL,
    NewAmount    DECIMAL(18,4)  NULL,
    RenewAmount  DECIMAL(18,4)  NULL,
    Memo         NVARCHAR(500)  NULL,
    CONSTRAINT PK_NTD_Detail PRIMARY KEY (DetailID),
    CONSTRAINT UQ_NTD_Detail  UNIQUE (ApplyNo, SeqNo),
    CONSTRAINT FK_NTD_Detail_Main FOREIGN KEY (ApplyNo) REFERENCES dbo.NTD_Main (ApplyNo)
);
GO

CREATE TABLE dbo.NTD_Log (
    LogID          INT            NOT NULL IDENTITY(1,1),
    ApplyNo        NVARCHAR(20)   NOT NULL,
    ActionType     NVARCHAR(20)   NOT NULL,
    FromStatus     INT            NOT NULL DEFAULT 0,
    ToStatus       INT            NOT NULL DEFAULT 0,
    ActionUser     NVARCHAR(20)   NOT NULL,
    ActionUserName NVARCHAR(50)   NULL,
    ActionTime     DATETIME       NOT NULL DEFAULT GETDATE(),
    Comment        NVARCHAR(2000) NULL,
    CONSTRAINT PK_NTD_Log PRIMARY KEY (LogID),
    CONSTRAINT FK_NTD_Log_Main FOREIGN KEY (ApplyNo) REFERENCES dbo.NTD_Main (ApplyNo)
);
GO

PRINT '完成：NTD 資料庫重建';
