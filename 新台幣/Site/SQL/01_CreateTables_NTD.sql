/*****************************************************************************
 新臺幣定期存款專案利率申請表 - 資料表建置腳本
 ----------------------------------------------------------------------------
 目標資料庫: chb_iom  (公司正式區沿用既有 DB)
 本機測試時: 若無 chb_iom 會自動建立
 ----------------------------------------------------------------------------
 分段說明:
   第 1 段 - NTD_Main         主檔 (一張申請單)
   第 2 段 - NTD_Detail       明細 (多檔期/多金額/多利率)
   第 3 段 - NTD_Log          簽核紀錄
   第 4 段 - NTD_RateType     利率別代號 (B0002 決行時填)
   第 5 段 - BranchOnly       分行清單  (公司已有 -> 帶回前略過)
   第 6 段 - EMPLOYEE         員工資料  (公司已有 -> 帶回前略過)
   第 7 段 - SC_USER          系統使用者 (公司已有 -> 帶回前略過)
*****************************************************************************/

SET NOCOUNT ON;
GO

-- 若無 chb_iom 則建立 (公司正式區已存在, 跑這段會跳過)
IF DB_ID('chb_iom') IS NULL
BEGIN
    PRINT '[INFO] Database chb_iom not found, creating...';
    CREATE DATABASE chb_iom;
END
GO

USE chb_iom;
GO

/*============================================================================
  第 1 段 - NTD_Main  主檔
============================================================================*/
IF OBJECT_ID('dbo.NTD_Main', 'U') IS NOT NULL DROP TABLE dbo.NTD_Main;
GO
CREATE TABLE dbo.NTD_Main (
    ApplyNo        VARCHAR(20)   NOT NULL PRIMARY KEY,          -- 申請單號 NTD+yyyyMMdd+流水4碼
    BranchCode     VARCHAR(10)   NOT NULL,                      -- 申請單位代號
    ApplyDate      DATETIME      NOT NULL,                      -- 申請日期
    CustomerName   NVARCHAR(100) NULL,                          -- 客戶名稱
    CustomerID     VARCHAR(20)   NULL,                          -- 客戶統編/身分證
    TotalAmount    DECIMAL(18,2) NULL,                          -- 申請總金額
    Reason         NVARCHAR(3000) NULL,                         -- 申請理由 (限 3000 字)
    RateTypeCode   VARCHAR(20)   NULL,                          -- 利率別代號 (科長決行才填)
    Status         INT           NOT NULL DEFAULT(0),           -- 0=草稿/退回 1=等襄理 2=等經理 4=等總行經辦 6=等總行科長 9=結案
    CreateUser     VARCHAR(20)   NOT NULL,
    CreateTime     DATETIME      NOT NULL DEFAULT(GETDATE()),
    UpdateUser     VARCHAR(20)   NULL,
    UpdateTime     DATETIME      NULL
);
GO
CREATE INDEX IX_NTD_Main_Status   ON dbo.NTD_Main(Status);
CREATE INDEX IX_NTD_Main_Branch   ON dbo.NTD_Main(BranchCode);
CREATE INDEX IX_NTD_Main_ApplyDt  ON dbo.NTD_Main(ApplyDate);
GO

/*============================================================================
  第 2 段 - NTD_Detail  明細
============================================================================*/
IF OBJECT_ID('dbo.NTD_Detail', 'U') IS NOT NULL DROP TABLE dbo.NTD_Detail;
GO
CREATE TABLE dbo.NTD_Detail (
    DetailID       INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ApplyNo        VARCHAR(20)   NOT NULL,
    SeqNo          INT           NOT NULL,                       -- 顯示順序
    PeriodMonth    INT           NOT NULL,                       -- 期間 (月)
    Amount         DECIMAL(18,2) NOT NULL,                       -- 金額
    ProposedRate   DECIMAL(8,5)  NOT NULL,                       -- 申請利率 (%)
    Memo           NVARCHAR(500) NULL
);
GO
CREATE INDEX IX_NTD_Detail_ApplyNo ON dbo.NTD_Detail(ApplyNo);
GO

/*============================================================================
  第 3 段 - NTD_Log  簽核紀錄
============================================================================*/
IF OBJECT_ID('dbo.NTD_Log', 'U') IS NOT NULL DROP TABLE dbo.NTD_Log;
GO
CREATE TABLE dbo.NTD_Log (
    LogID          INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ApplyNo        VARCHAR(20)   NOT NULL,
    ActionType     VARCHAR(20)   NOT NULL,                       -- CREATE/EDIT/APPROVE/REJECT/DECIDE
    FromStatus     INT           NULL,
    ToStatus       INT           NULL,
    ActionUser     VARCHAR(20)   NOT NULL,
    ActionUserName NVARCHAR(50)  NULL,
    ActionTime     DATETIME      NOT NULL DEFAULT(GETDATE()),
    Comment        NVARCHAR(500) NULL
);
GO
CREATE INDEX IX_NTD_Log_ApplyNo ON dbo.NTD_Log(ApplyNo);
GO

/*============================================================================
  第 4 段 - NTD_RateType  利率別代號
============================================================================*/
IF OBJECT_ID('dbo.NTD_RateType', 'U') IS NOT NULL DROP TABLE dbo.NTD_RateType;
GO
CREATE TABLE dbo.NTD_RateType (
    RateCode       VARCHAR(20)   NOT NULL PRIMARY KEY,
    RateName       NVARCHAR(100) NOT NULL,
    IsActive       BIT           NOT NULL DEFAULT(1)
);
GO

INSERT INTO dbo.NTD_RateType (RateCode, RateName) VALUES
    ('R001', N'一般定存優惠利率'),
    ('R002', N'大額存款特別利率'),
    ('R003', N'VIP 客戶專案利率'),
    ('R004', N'法人客戶專案利率'),
    ('R005', N'外幣連動利率');
GO

/*============================================================================
  第 5 段 - BranchOnly  (公司既有 - 帶回時略過)
============================================================================*/
IF OBJECT_ID('dbo.BranchOnly', 'U') IS NOT NULL DROP TABLE dbo.BranchOnly;
GO
CREATE TABLE dbo.BranchOnly (
    BranchCode     VARCHAR(10)   NOT NULL PRIMARY KEY,
    BranchName     NVARCHAR(50)  NOT NULL,
    BranchType     INT           NOT NULL DEFAULT(1)             -- 1=分行 2=總行
);
GO

INSERT INTO dbo.BranchOnly (BranchCode, BranchName, BranchType) VALUES
    ('5234', N'民生分行',     1),
    ('5101', N'總行營業部',   1),
    ('5102', N'信義分行',     1),
    ('5103', N'忠孝分行',     1),
    ('5104', N'中山分行',     1),
    ('5105', N'松山分行',     1),
    ('5106', N'內湖分行',     1),
    ('5107', N'南港分行',     1),
    ('5108', N'士林分行',     1),
    ('5185', N'總部分行',     2),
    ('5186', N'總行存匯部',   2);
GO

/*============================================================================
  第 6 段 - EMPLOYEE  員工 (公司既有 - 帶回時略過)
  TitleLevel:  1=經辦  2=襄理  3=經理  4=總行經辦  5=總行科長
============================================================================*/
IF OBJECT_ID('dbo.EMPLOYEE', 'U') IS NOT NULL DROP TABLE dbo.EMPLOYEE;
GO
CREATE TABLE dbo.EMPLOYEE (
    EmpID          VARCHAR(20)   NOT NULL PRIMARY KEY,
    EmpName        NVARCHAR(50)  NOT NULL,
    BranchCode     VARCHAR(10)   NOT NULL,
    Title          NVARCHAR(50)  NULL,
    TitleLevel     INT           NOT NULL DEFAULT(1),
    Email          VARCHAR(100)  NULL,
    IsActive       BIT           NOT NULL DEFAULT(1)
);
GO

INSERT INTO dbo.EMPLOYEE (EmpID, EmpName, BranchCode, Title, TitleLevel, Email) VALUES
    ('A0001', N'王經辦',     '5234', N'經辦',     1, 'a0001@chb.test'),
    ('A0002', N'李襄理',     '5234', N'存匯襄理', 2, 'a0002@chb.test'),
    ('A0003', N'陳經理',     '5234', N'單位主管', 3, 'a0003@chb.test'),
    ('B0001', N'金總行經辦', '5185', N'總行經辦', 4, 'b0001@chb.test'),
    ('B0002', N'李總行科長', '5185', N'總行科長', 5, 'b0002@chb.test');
GO

/*============================================================================
  第 7 段 - SC_USER  系統使用者 (公司既有 - 帶回時略過)
============================================================================*/
IF OBJECT_ID('dbo.SC_USER', 'U') IS NOT NULL DROP TABLE dbo.SC_USER;
GO
CREATE TABLE dbo.SC_USER (
    UserID         VARCHAR(20)   NOT NULL PRIMARY KEY,
    UserName       NVARCHAR(50)  NOT NULL,
    BranchCode     VARCHAR(10)   NOT NULL,
    RoleCode       VARCHAR(20)   NULL,
    IsActive       BIT           NOT NULL DEFAULT(1)
);
GO

INSERT INTO dbo.SC_USER (UserID, UserName, BranchCode, RoleCode) VALUES
    ('A0001', N'王經辦',     '5234', 'NTD_EDIT'),
    ('A0002', N'李襄理',     '5234', 'NTD_APPR1'),
    ('A0003', N'陳經理',     '5234', 'NTD_APPR2'),
    ('B0001', N'金總行經辦', '5185', 'NTD_APPR3'),
    ('B0002', N'李總行科長', '5185', 'NTD_DECIDE');
GO

PRINT '[OK] NTD 資料表建置完成';
GO
