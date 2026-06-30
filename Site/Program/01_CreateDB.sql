-- =====================================================================
-- NTD 本機開發環境 - 建立資料庫與資料表
-- 在 SQL Server Management Studio (SSMS) 或 LocalDB 中執行
-- =====================================================================

-- 建立本地測試資料庫（模擬公司 chb_iom）
IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'chb_iom_dev')
    CREATE DATABASE chb_iom_dev;
GO
USE chb_iom_dev;
GO

-- =====================================================================
-- NTD 主檔
-- 異動說明：CustomerID、CustomerName 已移至 NTD_Detail（每筆明細獨立帶客戶）
--           新增 Related3M、Related6M（關係戶存款實績：最近三/六個月，仟元）
-- =====================================================================
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'NTD_Main')
CREATE TABLE dbo.NTD_Main (
    DocNo       NVARCHAR(20)   NOT NULL PRIMARY KEY,
    BranchCode    NVARCHAR(10)   NOT NULL,
    ApplyDate     DATE           NOT NULL,
    GroupName     NVARCHAR(100)  NULL,
    TotalAmount   DECIMAL(18,4)  NOT NULL DEFAULT 0,  -- 由明細 Amount 加總，系統自動計算
    IsRelated     NCHAR(1)       NULL,                -- 'Y'=是 / 'N'=否
    Contrib3M     DECIMAL(18,4)  NULL,                -- 客戶存款實績：最近三個月（仟元）
    Contrib6M     DECIMAL(18,4)  NULL,                -- 客戶存款實績：最近六個月（仟元）
    Contrib1Y     DECIMAL(18,4)  NULL,                -- 最近一年貢獻度（仟元）
    Related3M     DECIMAL(18,4)  NULL,                -- 關係戶存款實績：最近三個月（仟元）
    Related6M     DECIMAL(18,4)  NULL,                -- 關係戶存款實績：最近六個月（仟元）
    RelatedAmt    DECIMAL(18,4)  NULL,                -- 關係戶存款實績：最近一年（仟元）
    BranchNote    NVARCHAR(3000) NULL,
    RateTypeCode  NVARCHAR(10)   NULL,                -- 總行科長決行後填入
    Status        INT            NOT NULL DEFAULT 0,
    -- 0=草稿/退回 1=待襄理 2=待經理 4=待總行經辦 6=待總行科長 9=結案
    CreateUser    NVARCHAR(20)   NOT NULL,
    CreateTime    DATETIME       NOT NULL DEFAULT GETDATE(),
    UpdateUser    NVARCHAR(20)   NULL,
    UpdateTime    DATETIME       NULL
);
GO

-- =====================================================================
-- NTD 明細（每筆明細獨立帶客戶統編 + 名稱，支援同申請多客戶）
-- =====================================================================
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'NTD_Detail')
CREATE TABLE dbo.NTD_Detail (
    DetailID      INT            IDENTITY(1,1) PRIMARY KEY,
    DocNo       NVARCHAR(20)   NOT NULL REFERENCES dbo.NTD_Main(DocNo),
    SeqNo         INT            NOT NULL,
    CustomerID    NVARCHAR(20)   NULL,                -- 統一編號 / 身分證（由 EAI 查詢帶入）
    CustomerName  NVARCHAR(200)  NULL,                -- 客戶名稱（由 EAI 查詢帶入）
    DepositType   NVARCHAR(30)   NULL,                -- 定期存款 / 定期儲蓄存款
    PeriodMonth   INT            NOT NULL DEFAULT 12,
    Amount        DECIMAL(18,4)  NOT NULL DEFAULT 0,
    ProposedRate  DECIMAL(10,4)  NOT NULL DEFAULT 0,
    StartDate     DATE           NULL,
    EndDate       DATE           NULL,
    NewAmount     DECIMAL(18,4)  NULL,
    RenewAmount   DECIMAL(18,4)  NULL,
    Memo          NVARCHAR(500)  NULL
);
GO

-- =====================================================================
-- NTD 簽核紀錄
-- =====================================================================
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'NTD_Log')
CREATE TABLE dbo.NTD_Log (
    LogID          INT            IDENTITY(1,1) PRIMARY KEY,
    DocNo        NVARCHAR(20)   NOT NULL,
    ActionType     NVARCHAR(20)   NOT NULL,  -- CREATE / EDIT / SUBMIT / APPROVE / DECIDE / REJECT
    FromStatus     INT            NOT NULL,
    ToStatus       INT            NOT NULL,
    ActionUser     NVARCHAR(20)   NOT NULL,
    ActionUserName NVARCHAR(50)   NULL,
    Comment        NVARCHAR(500)  NULL,
    ActionTime     DATETIME       NOT NULL DEFAULT GETDATE()
);
GO

-- =====================================================================
-- NTD 利率別代號（總行科長決行時選擇）
-- =====================================================================
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'NTD_RateType')
CREATE TABLE dbo.NTD_RateType (
    RateCode  NVARCHAR(10)  NOT NULL PRIMARY KEY,
    RateName  NVARCHAR(50)  NOT NULL,
    IsActive  BIT           NOT NULL DEFAULT 1
);
GO

INSERT INTO dbo.NTD_RateType (RateCode, RateName) VALUES
('A01', N'一般定期存款利率'),
('A02', N'專案定期存款利率'),
('B01', N'大額定儲利率'),
('B02', N'專案定儲利率');
GO

-- =====================================================================
-- 如已存在舊版資料表，執行下列 migration 腳本
-- （舊版 NTD_Main 有 CustomerID / CustomerName，新版移至 NTD_Detail）
-- =====================================================================
/*
-- Step 1: 為 NTD_Detail 加欄位（如尚未有）
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.NTD_Detail') AND name = 'CustomerID')
    ALTER TABLE dbo.NTD_Detail ADD CustomerID NVARCHAR(20) NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.NTD_Detail') AND name = 'CustomerName')
    ALTER TABLE dbo.NTD_Detail ADD CustomerName NVARCHAR(200) NULL;

-- Step 2: 將 NTD_Main 的客戶資料複製到所有對應明細列
UPDATE d
SET    d.CustomerID   = m.CustomerID,
       d.CustomerName = m.CustomerName
FROM   dbo.NTD_Detail d
JOIN   dbo.NTD_Main   m ON d.DocNo = m.DocNo
WHERE  m.CustomerID IS NOT NULL;

-- Step 3: 在 NTD_Main 新增 Related3M / Related6M（如尚未有）
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.NTD_Main') AND name = 'Related3M')
    ALTER TABLE dbo.NTD_Main ADD Related3M DECIMAL(18,4) NULL;
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.NTD_Main') AND name = 'Related6M')
    ALTER TABLE dbo.NTD_Main ADD Related6M DECIMAL(18,4) NULL;

-- Step 4: 確認無問題後，從 NTD_Main 移除舊欄位
-- ALTER TABLE dbo.NTD_Main DROP COLUMN CustomerID;
-- ALTER TABLE dbo.NTD_Main DROP COLUMN CustomerName;
-- ALTER TABLE dbo.NTD_Main DROP COLUMN Reason;
*/

PRINT '>> NTD 業務資料表建立完成 (chb_iom_dev)';
GO
