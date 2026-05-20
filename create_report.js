const {
  Document, Packer, Paragraph, TextRun, ImageRun,
  HeadingLevel, AlignmentType, PageBreak, BorderStyle,
  Table, TableRow, TableCell, WidthType
} = require('docx');
const fs = require('fs');
const path = require('path');

const IMG_DIR = 'D:\\work\\上傳到公司\\screenshots_ops';
const OUT_FILE = 'D:\\work\\上傳到公司\\CHBApp_功能操作測試報告.docx';

function img(filename, widthPx) {
  const p = path.join(IMG_DIR, filename);
  if (!fs.existsSync(p)) {
    console.warn('Missing image:', p);
    return null;
  }
  const buf = fs.readFileSync(p);
  const w = widthPx || 600;
  const h = Math.round(w * 0.5625); // 16:9 approx
  return new ImageRun({ data: buf, transformation: { width: w, height: h } });
}

function heading1(text) {
  return new Paragraph({
    text,
    heading: HeadingLevel.HEADING_1,
    spacing: { before: 400, after: 200 }
  });
}

function heading2(text) {
  return new Paragraph({
    text,
    heading: HeadingLevel.HEADING_2,
    spacing: { before: 300, after: 100 }
  });
}

function body(text) {
  return new Paragraph({
    children: [new TextRun({ text, size: 24 })],
    spacing: { before: 100, after: 100 }
  });
}

function imgPara(filename, widthPx) {
  const image = img(filename, widthPx);
  if (!image) return body('[圖片缺失: ' + filename + ']');
  return new Paragraph({
    children: [image],
    alignment: AlignmentType.CENTER,
    spacing: { before: 100, after: 200 }
  });
}

function caption(text) {
  return new Paragraph({
    children: [new TextRun({ text, italics: true, size: 20, color: '666666' })],
    alignment: AlignmentType.CENTER,
    spacing: { before: 0, after: 200 }
  });
}

const children = [
  // Cover
  new Paragraph({
    children: [new TextRun({ text: 'CHBApp.BK', bold: true, size: 52 })],
    alignment: AlignmentType.CENTER,
    spacing: { before: 2000, after: 400 }
  }),
  new Paragraph({
    children: [new TextRun({ text: '員工薪資系統 功能操作測試報告', bold: true, size: 40 })],
    alignment: AlignmentType.CENTER,
    spacing: { after: 200 }
  }),
  new Paragraph({
    children: [new TextRun({ text: '測試日期：2026/05/17', size: 28 })],
    alignment: AlignmentType.CENTER,
    spacing: { after: 200 }
  }),
  new Paragraph({
    children: [new TextRun({ text: '測試帳號：1111 / 1111', size: 28 })],
    alignment: AlignmentType.CENTER,
    spacing: { after: 2000 }
  }),
  new Paragraph({ children: [new PageBreak()] }),

  // 1. Login
  heading1('1. 登入功能'),
  body('測試登入畫面，輸入帳號 1111 / 密碼 1111，驗證成功後進入主選單。'),
  imgPara('LOGIN_01_登入畫面.jpg'),
  caption('圖 1-1：登入畫面'),
  imgPara('LOGIN_02_主選單.jpg'),
  caption('圖 1-2：登入成功，主選單'),
  new Paragraph({ children: [new PageBreak()] }),

  // 2. BK101
  heading1('2. BK101 員工基本資料建檔'),
  body('測試新增員工資料功能。輸入員工編號、姓名、身份字號、帳號等欄位，儲存後查詢確認，並測試刪除功能。'),
  heading2('2.1 空白畫面'),
  imgPara('BK101_01_空白畫面.jpg'),
  caption('圖 2-1：BK101 空白建檔畫面'),
  heading2('2.2 填入資料'),
  imgPara('BK101_02_填資料.jpg'),
  caption('圖 2-2：填入測試員工資料'),
  heading2('2.3 儲存成功'),
  imgPara('BK101_03_儲存成功.jpg'),
  caption('圖 2-3：儲存成功訊息'),
  heading2('2.4 查詢結果'),
  imgPara('BK101_04_查詢結果.jpg'),
  caption('圖 2-4：查詢已建立之員工'),
  heading2('2.5 刪除確認'),
  imgPara('BK101_05_刪除確認.jpg'),
  caption('圖 2-5：刪除確認對話框'),
  heading2('2.6 刪除成功'),
  imgPara('BK101_06_刪除成功.jpg'),
  caption('圖 2-6：員工資料刪除完成'),
  new Paragraph({ children: [new PageBreak()] }),

  // 3. BK102
  heading1('3. BK102 員工資料查詢'),
  body('測試員工資料維護查詢功能，包含空白查詢（全部列表）及關鍵字查詢。'),
  heading2('3.1 查詢畫面（無資料）'),
  imgPara('BK102_01_查詢畫面.jpg'),
  caption('圖 3-1：BK102 查詢畫面，初始 0 筆'),
  heading2('3.2 查詢結果（有資料）'),
  imgPara('BK102_02_查詢結果.jpg'),
  caption('圖 3-2：BK102 查詢結果，共 1 筆 TEST01'),
  new Paragraph({ children: [new PageBreak()] }),

  // 4. BK201
  heading1('4. BK201 薪資輸入－個人'),
  body('測試個別員工薪資輸入功能。找到 TEST01，輸入本月薪資 50,000，儲存確認。'),
  heading2('4.1 空白薪資畫面'),
  imgPara('BK201_01_空白.jpg'),
  caption('圖 4-1：BK201 個人薪資輸入，初始畫面'),
  heading2('4.2 儲存成功'),
  imgPara('BK201_02_儲存成功.jpg'),
  caption('圖 4-2：薪資儲存成功，本月薪資 50,000'),
  new Paragraph({ children: [new PageBreak()] }),

  // 5. BK202
  heading1('5. BK202 薪資輸入－全公司'),
  body('測試全公司薪資統一輸入功能，可批次設定所有員工的本月薪資及存提區分。'),
  imgPara('BK202_01_全公司.jpg'),
  caption('圖 5-1：BK202 全公司薪資輸入畫面'),
  new Paragraph({ children: [new PageBreak()] }),

  // 6. BK301
  heading1('6. BK301 列印作業'),
  body('測試本月薪資清冊列印功能，包含列印設定及預覽。'),
  heading2('6.1 列印設定'),
  imgPara('BK301_01_列印設定.jpg'),
  caption('圖 6-1：BK301 列印設定畫面'),
  heading2('6.2 列印預覽'),
  imgPara('BK301_02_預覽.jpg'),
  caption('圖 6-2：本月薪資清冊列印預覽，TEST01 共 1 筆，合計 50,000'),
  new Paragraph({ children: [new PageBreak()] }),

  // 7. BK401
  heading1('7. BK401 轉出磁片作業－全體（百年規格）'),
  body('測試全體員工薪資轉出磁片功能（百年規格），設定撥帳日期、檔案路徑等參數。'),
  imgPara('BK401_01_全體磁片.jpg'),
  caption('圖 7-1：BK401 轉出磁片全體畫面'),
  new Paragraph({ children: [new PageBreak()] }),

  // 8. BK4011
  heading1('8. BK4011 轉出磁片作業－個別（百年規格）'),
  body('測試個別員工薪資轉出磁片功能（百年規格），可指定特定員工帳號輸出。'),
  imgPara('BK4011_01_個別磁片.jpg'),
  caption('圖 8-1：BK4011 轉出磁片個別畫面'),
  new Paragraph({ children: [new PageBreak()] }),

  // 9. BK501
  heading1('9. BK501 二次轉帳輸入（結果磁片匯入）'),
  body('測試二次轉扣帳作業，匯入銀行退件結果磁片，重新處理退件帳號。'),
  imgPara('BK501_01_二次轉帳.jpg'),
  caption('圖 9-1：BK501 薪資磁片匯入（二扣作業）畫面'),
  new Paragraph({ children: [new PageBreak()] }),

  // 10. 密碼變更
  heading1('10. 密碼變更'),
  body('測試密碼作業功能。點選密碼作業選單觸發密碼變更提示對話框，確認功能正常運作。'),
  imgPara('密碼變更_01_提示.jpg'),
  caption('圖 10-1：密碼變更提示畫面'),
  new Paragraph({ children: [new PageBreak()] }),

  // Summary
  heading1('測試總結'),
  body('本次對 CHBApp.BK 新版員工薪資管理系統（.NET 8）進行完整功能操作測試，共涵蓋 10 項功能模組，測試結果如下：'),
  new Paragraph({ spacing: { before: 100, after: 100 } }),

  new Table({
    width: { size: 100, type: WidthType.PERCENTAGE },
    rows: [
      new TableRow({
        children: [
          new TableCell({ children: [new Paragraph({ children: [new TextRun({ text: '功能代號', bold: true })] })] }),
          new TableCell({ children: [new Paragraph({ children: [new TextRun({ text: '功能名稱', bold: true })] })] }),
          new TableCell({ children: [new Paragraph({ children: [new TextRun({ text: '測試結果', bold: true })] })] }),
        ]
      }),
      ...([
        ['Login', '登入', '✓ 通過'],
        ['BK101', '員工基本資料建檔', '✓ 通過'],
        ['BK102', '員工資料查詢', '✓ 通過'],
        ['BK201', '薪資輸入－個人', '✓ 通過'],
        ['BK202', '薪資輸入－全公司', '✓ 通過'],
        ['BK301', '列印作業', '✓ 通過'],
        ['BK401', '轉出磁片－全體（百年規格）', '✓ 通過'],
        ['BK4011', '轉出磁片－個別（百年規格）', '✓ 通過'],
        ['BK501', '二次轉帳輸入', '✓ 通過'],
        ['密碼變更', '密碼作業', '✓ 通過'],
      ].map(([code, name, result]) =>
        new TableRow({
          children: [
            new TableCell({ children: [new Paragraph(code)] }),
            new TableCell({ children: [new Paragraph(name)] }),
            new TableCell({ children: [new Paragraph({ children: [new TextRun({ text: result, color: '007700' })] })] }),
          ]
        })
      ))
    ]
  }),

  new Paragraph({ spacing: { before: 400 } }),
  body('結論：CHBApp.BK 新版系統所有主要功能均運作正常，操作流程符合預期。'),
];

const doc = new Document({
  sections: [{
    properties: {
      page: {
        size: { width: 12240, height: 15840 },
        margin: { top: 1440, right: 1440, bottom: 1440, left: 1440 }
      }
    },
    children
  }]
});

Packer.toBuffer(doc).then(buf => {
  fs.writeFileSync(OUT_FILE, buf);
  console.log('Report saved to:', OUT_FILE);
}).catch(err => {
  console.error('Error:', err.message);
  process.exit(1);
});
