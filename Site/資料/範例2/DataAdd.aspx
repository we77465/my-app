<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataAdd.aspx.cs" Inherits="Program_LargeMoney_DataAdd" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>大額資金通報(G5310)-填報</title>
    <link rel="stylesheet" type="text/css" href="../Common/css/Common.css" />
    <style>
    /* ===== 重置可能造成問題的 Common. css 樣式 ===== */
    table, tr, td, th, div, form {
        overflow:  visible ! important;
    }

    /* ===== 表格樣式 ===== */
    table.tablestyle {
        width:  90%;
        margin: 20px auto;
        border-collapse: collapse;
        border:  1px solid #ccc;
    }

    .tdhstyle {
        background-color: #f5f5f5;
        font-weight: bold;
        padding: 12px 15px;
        border-bottom: 1px solid #ddd;
        vertical-align: middle;
    }

    .tdbstyle {
        padding: 12px 15px;
        border-bottom: 1px solid #ddd;
        vertical-align: middle;
    }

    tr.trstyle:nth-child(even) {
        background-color: #f9f9f9;
    }

    tr.trstyle:hover {
        background-color: #f0f0f0;
    }

    /* ===== 下拉選單 (最重要) ===== */
    select,
    .DropDownList {
        height:100%;
        width: 100%;
        padding: 8px 10px;
        border: 1px solid #ddd;
        border-radius: 4px;
        font-size: 14px;
        background-color: white;
        cursor:  pointer;
        box-sizing: border-box;
        /* 確保原生下拉選單樣式 */
        -webkit-appearance: menulist;
        -moz-appearance: menulist;
        appearance: menulist;
    }

    select:focus {
        outline: none;
        border-color: #4CAF50;
        box-shadow: 0 0 5px rgba(76, 175, 80, 0.5);
    }

    /* ===== 輸入框 ===== */
    input[type="text"],
    input[type="date"],
    textarea {
        width: 100%;
        padding: 8px;
        border: 1px solid #ddd;
        border-radius: 4px;
        font-size: 14px;
        box-sizing: border-box;
    }

    input[type="text"]:focus,
    input[type="date"]:focus,
    textarea:focus {
        outline: none;
        border-color: #4CAF50;
        box-shadow: 0 0 5px rgba(76, 175, 80, 0.5);
    }

    . readonly-field {
        background-color: #f5f5f5;
        color: #666;
    }

    /* ===== 必填與提示 ===== */
    .required {
        color: #ff0066;
        font-weight: bold;
    }

    .error {
        color: #ff0066;
        font-size: 12px;
        margin-top: 5px;
        display: block;
    }

    .field-hint {
        color: #888;
        font-size: 11px;
        margin-top: 3px;
    }

    /* ===== 區塊標題 ===== */
    .section-header {
        background-color: #2196F3 !important;
        color: white ! important;
        font-weight: bold;
        text-align: center;
        padding: 10px;
    }

    /* ===== 按鈕 ===== */
    .btn-container {
        text-align: center;
        padding: 20px;
    }

    .btn-submit, .btn-reset, .btn-back {
        padding: 12px 35px;
        margin: 0 10px;
        font-size: 16px;
        cursor: pointer;
        border: none;
        border-radius:  4px;
    }

    .btn-submit {
        background-color: #4CAF50;
        color: white;
    }

    .btn-submit:hover {
        background-color: #45a049;
    }

    .btn-reset {
        background-color:  #f44336;
        color: white;
    }
    /* ===== 金額千位格式 ===== */
    #txtAmtDisplay {
        width: 100%;
        padding: 8px;
        border: 1px solid #ddd;
        border-radius: 4px;
        font-size: 14px;
        box-sizing: border-box;
        text-align: right;
    }

    .btn-reset:hover {
        background-color: #da190b;
    }

    .btn-back {
        background-color: #2196F3;
        color: white;
    }

    .btn-back:hover {
        background-color: #1976D2;
    }
    
        /* 新增查詢按鈕樣式 */
        .btn-query {
            padding: 8px 15px;
            font-size: 14px;
            cursor: pointer;
            border: none;
            border-radius: 4px;
            background-color: #FF9800;
            color: white;
            white-space: nowrap;
        }

        .btn-query:hover {
            background-color: #F57C00;
        }

        .btn-query:disabled {
            background-color: #ccc;
            cursor: not-allowed;
        }
</style>
</head>
<body>
<form id="form1" runat="server" onsubmit="return beforeSubmit();">
        <!-- 隱藏欄位：功能、案件種類、案件編號 (保留資料但不顯示) -->
        <asp:DropDownList ID="ddlKind" runat="server" CssClass="DropDownList" style="display:none;">
            <asp:ListItem Text="1. 新增" Value="1"></asp:ListItem>
        </asp:DropDownList>
        <asp:TextBox ID="txtApType" runat="server" Text="NE" ReadOnly="true" style="display:none;"></asp:TextBox>
        <asp:TextBox ID="txtSrno" runat="server" ReadOnly="true" style="display:none;"></asp:TextBox>
        
        <div>
        <table class="tablestyle">
            <!-- 標題列 -->
            <tr class="trstyle">
                <td class="tdhstyle" colspan="4" style="background-color: #4CAF50; color: white; text-align: center; font-size: 18px;">
                    <asp:Literal ID="litTitle" runat="server" Text="大額資金通報(G5310)-填報"></asp:Literal>
                </td>
            </tr>
            
            <!-- 基本資訊區塊 -->
            <tr class="trstyle">
                <td class="section-header" colspan="4">基本資訊</td>
            </tr>
            
            <tr class="trstyle">
                <td class="tdhstyle" style="width: 15%;">
                   <%-- <span class="required">*</span>--%>
                    <asp:Literal ID="litReportDate" runat="server" Text="通報日期"></asp:Literal>
                    <div class="field-hint">負責人/主管放行後會自動填上通報日期</div>
                </td>
                <td class="tdbstyle" style="width:35%;">
                    <asp:TextBox ID="txtReportDate" runat="server" ReadOnly="true" TextMode="Date"></asp:TextBox>
                    <%--<asp:RequiredFieldValidator ID="rfvReportDate" runat="server" 
                        ControlToValidate="txtReportDate" ErrorMessage="通報日期為必填項" 
                        Display="Dynamic" CssClass="error"></asp:RequiredFieldValidator>--%>
                </td>
                <td class="tdhstyle" style="width:15%;"></td>
                <td class="tdbstyle" style="width:35%;"></td>
            </tr>
            
            <!-- 客戶資訊區塊 -->
            <tr class="trstyle">
                <td class="section-header" colspan="4">客戶資訊</td>
            </tr>
            
            <!-- 客戶資訊區塊 - 修改統一編號和客戶名稱的部分 -->
            <tr class="trstyle">
                <td class="tdhstyle">
                    <span class="required">*</span>
                    <asp:Literal ID="litUnino" runat="server" Text="統一編號"></asp:Literal>
                </td>
                <td class="tdbstyle">
                    <div style="display: flex; gap: 10px; align-items: flex-start;">
                        <asp:TextBox ID="txtUnino" runat="server" MaxLength="10" style="flex: 1;"></asp:TextBox>
                        <asp:Button ID="btnQueryCustomer" runat="server" Text="查詢客戶" 
                            OnClick="BtnQueryCustomer_Click" CssClass="btn-query" CausesValidation="false" />
                    </div>
                    <asp:RequiredFieldValidator ID="rfvUnino" runat="server" 
                        ControlToValidate="txtUnino" ErrorMessage="統一編號為必填項" 
                        Display="Dynamic" CssClass="error"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revUnino" runat="server"
                        ControlToValidate="txtUnino" 
                        ValidationExpression="^[A-Za-z0-9\*]{8,10}$"
                        ErrorMessage="統一編號格式錯誤(8-10碼英數字)" 
                        Display="Dynamic" CssClass="error"></asp:RegularExpressionValidator>
                    <div class="field-hint">最多10碼，輸入完成後按「查詢客戶」帶入客戶名稱</div>
                </td>
                <td class="tdhstyle">
                    <asp:Literal ID="litCustomerName" runat="server" Text="客戶名稱"></asp:Literal>
                </td>
                <td class="tdbstyle">
                    <asp:TextBox ID="txtCustomerName" runat="server" MaxLength="50" ReadOnly="true" CssClass="readonly-field"></asp:TextBox>
                    <div class="field-hint">由統一編號自動帶入</div>
                </td>
            </tr>
            
            <!-- 交易資訊區塊 -->
            <tr class="trstyle">
                <td class="section-header" colspan="4">交易資訊</td>
            </tr>
            
            <tr class="trstyle">
                <td class="tdhstyle">
                    <span class="required">*</span>
                    <asp:Literal ID="litStday" runat="server" Text="交割日期"></asp:Literal>
                </td>
                <td class="tdbstyle">
                    <asp:TextBox ID="txtStday" runat="server" TextMode="Date"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvStday" runat="server" 
                        ControlToValidate="txtStday" ErrorMessage="交割日期為必填項" 
                        Display="Dynamic" CssClass="error"></asp:RequiredFieldValidator>
                    <asp:CustomValidator ID="cvStday" runat="server"
                        ControlToValidate="txtStday"
                        ErrorMessage="交割日期不能小於今天"
                        Display="Dynamic" CssClass="error"
                        ClientValidationFunction="validateStday"
                        OnServerValidate="cvStday_ServerValidate">
                    </asp:CustomValidator>
                </td>
                <td class="tdhstyle">
                    <span class="required">*</span>
                    <asp:Literal ID="litIocd" runat="server" Text="匯出/匯入"></asp:Literal>
                </td>
                <td class="tdbstyle">
                    <asp:DropDownList ID="ddlIocd" runat="server" CssClass="DropDownList">
                        <asp:ListItem Text="---請選擇---" Value=""></asp:ListItem>
                        <asp:ListItem Text="1.匯入" Value="1"></asp:ListItem>
                        <asp:ListItem Text="2.匯出" Value="2"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvIocd" runat="server" 
                        ControlToValidate="ddlIocd" ErrorMessage="匯出/匯入為必填項" 
                        Display="Dynamic" CssClass="error"></asp:RequiredFieldValidator>
                </td>
            </tr>
            
            <tr class="trstyle">
                <td class="tdhstyle">
                    <span class="required">*</span>
                    <asp:Literal ID="litIbrno" runat="server" Text="通報行"></asp:Literal>
                </td>
                <%--<td class="tdbstyle">
                    <asp:TextBox ID="txtIbrno" runat="server" MaxLength="4"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvIbrno" runat="server" 
                        ControlToValidate="txtIbrno" ErrorMessage="通報行為必填項" 
                        Display="Dynamic" CssClass="error"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revIbrno" runat="server"
                        ControlToValidate="txtIbrno" 
                        ValidationExpression="^\d{4}$"
                        ErrorMessage="通報行必須為4位數字" 
                        Display="Dynamic" CssClass="error"></asp:RegularExpressionValidator>
                    <div class="field-hint">4位數字</div>
                </td>--%>
                <td class="tdbstyle">
                    <asp:DropDownList ID="ddlIbrno" runat="server" CssClass="DropDownList">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvIbrno" runat="server" 
                        ControlToValidate="ddlIbrno" ErrorMessage="通報行為必填項" 
                        Display="Dynamic" CssClass="error" InitialValue="" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                    <div class="field-hint">預設為填報人員所屬單位</div>
                </td>
                <td class="tdhstyle">
                    <span class="required">*</span>
                    <asp:Literal ID="litAcbrno" runat="server" Text="設帳行"></asp:Literal>
                </td>
                <td class="tdbstyle">
                    <asp:DropDownList ID="ddlAcbrno" runat="server" CssClass="DropDownList">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvAcbrno" runat="server" 
                        ControlToValidate="ddlAcbrno" ErrorMessage="設帳行為必填項" 
                        Display="Dynamic" CssClass="error" InitialValue="" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                </td>
            </tr>
            
            <tr class="trstyle">
                <td class="tdhstyle">
                    <span class="required">*</span>
                    <asp:Literal ID="litCurcd" runat="server" Text="幣別"></asp:Literal>
                </td>
                <td class="tdbstyle">
                    <asp:DropDownList ID="ddlCurcd" runat="server" CssClass="DropDownList"
                        AutoPostBack="true" OnSelectedIndexChanged="ddlCurcd_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvCurcd" runat="server" 
                        ControlToValidate="ddlCurcd" ErrorMessage="幣別為必填項" 
                        Display="Dynamic" CssClass="error"></asp:RequiredFieldValidator>
                </td>
                <td class="tdhstyle" style="display:none">
                    <asp:Literal ID="litBkcur" runat="server"  Text="存匯行幣別"></asp:Literal>
                </td>
                <td class="tdbstyle" style="display:none">
                    <asp:DropDownList ID="ddlBkcur" runat="server" CssClass="DropDownList">
                    </asp:DropDownList>
                </td>
            </tr>
            
            <tr class="trstyle">
                <td class="tdhstyle" id="tdBankCodeLabel" runat="server">
                    <span class="required" id="spanBankCodeRequired" runat="server">*</span>
                    <asp:Literal ID="litBksrno" runat="server" Text="存匯行代號"></asp:Literal>
                </td>
                <td class="tdbstyle" id="tdBankCodeField" runat="server">
                    <asp:DropDownList ID="ddlBankCode" runat="server" CssClass="DropDownList">
                    </asp:DropDownList>
                    <div class="field-hint">依幣別自動篩選可選項目</div>
                </td>
                <td class="tdhstyle">
                    <span class="required">*</span>
                    <asp:Literal ID="litAmt" runat="server" Text="金額"></asp:Literal>
                </td>
                <td class="tdbstyle">
                <%-- 顯示用：千位格式 --%>
                <input type="text" id="txtAmtDisplay" maxlength="18"
            style="width:100%; padding:8px; border:1px solid #ddd; border-radius:4px;
           font-size:14px; box-sizing:border-box; text-align:right;" />

                <%-- 後端驗證 + 送出用：隱藏純數字 --%>
                <asp:TextBox ID="txtAmt" runat="server" MaxLength="15" style="display:none;"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvAmt" runat="server" 
                    ControlToValidate="txtAmt" ErrorMessage="金額為必填項" 
                    Display="Dynamic" CssClass="error"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revAmt" runat="server"
                    ControlToValidate="txtAmt" 
                    ValidationExpression="^\d{1,12}(\.\d{1,2})?$"
                    ErrorMessage="金額格式錯誤(最多12位整數,2位小數)" 
                    Display="Dynamic" CssClass="error"></asp:RegularExpressionValidator>
                <div class="field-hint">格式: 9(12)V9(02)</div>
            </td>
            </tr>
            
            <!-- 資金說明區塊 -->
            <tr class="trstyle">
                <td class="section-header" colspan="4">資金說明</td>
            </tr>
            
            <tr class="trstyle">
                <td class="tdhstyle">
                    <%--<span class="required">*</span>--%>
                    <asp:Literal ID="litAmtwhere" runat="server" Text="資金來源/去處"></asp:Literal>
                </td>
                <td class="tdbstyle" colspan="3">
                    <asp:TextBox ID="txtAmtwhere" runat="server" MaxLength="40" Width="100%"></asp:TextBox>
                    <%--<asp:RequiredFieldValidator ID="rfvAmtwhere" runat="server" 
                        ControlToValidate="txtAmtwhere" ErrorMessage="資金來源/去處為必填項" 
                        Display="Dynamic" CssClass="error"></asp:RequiredFieldValidator>--%>
                    <div class="field-hint">最多40字元</div>
                </td>
            </tr>
            
            <tr class="trstyle">
                <td class="tdhstyle">
                    <%--<span class="required">*</span>--%>
                    <asp:Literal ID="litReason" runat="server" Text="變動原因內容"></asp:Literal>
                </td>
                <td class="tdbstyle" colspan="3">
                    <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Rows="3" MaxLength="80" Width="100%"></asp:TextBox>
                   <%-- <asp:RequiredFieldValidator ID="rfvReason" runat="server" 
                        ControlToValidate="txtReason" ErrorMessage="變動原因內容為必填項" 
                        Display="Dynamic" CssClass="error"></asp:RequiredFieldValidator>--%>
                    <div class="field-hint">最多80字元</div>
                </td>
            </tr>
            
            <!-- 經辦人員區塊 -->
            <tr class="trstyle">
                <td class="section-header" colspan="4">經辦人員</td>
            </tr>
            
            <tr class="trstyle">
                <td class="tdhstyle">
                    <asp:Literal ID="litEmpno" runat="server" Text="員工編號"></asp:Literal>
                </td>
                <td class="tdbstyle">
                    <asp:TextBox ID="txtEmpno" runat="server" ReadOnly="true" CssClass="readonly-field"></asp:TextBox>
                </td>
                <td class="tdhstyle">
                    <asp:Literal ID="litEmpnm" runat="server" Text="員工姓名"></asp:Literal>
                </td>
                <td class="tdbstyle">
                    <asp:TextBox ID="txtEmpnm" runat="server" ReadOnly="true" CssClass="readonly-field"></asp:TextBox>
                </td>
            </tr>
            
            <!-- 按鈕列 -->
            <tr class="trstyle">
                <td colspan="4">
                    <div class="btn-container">
                        <asp:Button ID="btnSubmit" runat="server" Text="新增" OnClick="BtnSubmit_Click" CssClass="btn-submit" />
                        <asp:Button ID="btnReset" runat="server" Text="清空" OnClick="BtnReset_Click" CssClass="btn-reset" CausesValidation="false" />
                        <asp:Button ID="btnBack" runat="server" Text="返回查詢" OnClick="BtnBack_Click" CssClass="btn-back" CausesValidation="false" />
                    </div>
                </td>
            </tr>
            
        </table>
        </div>
    </form>
    
<script type="text/javascript">
    // ===== 交割日期驗證 =====
    function validateStday(sender, args) {
        var parts = args.Value.split('-');
        var inputDate = new Date(parts[0], parts[1] - 1, parts[2]);
        var today = new Date();
        today.setHours(0, 0, 0, 0);
        args.IsValid = inputDate >= today;
    }

    // ===== 金額千位格式化 =====

    function addThousandSeparator(val) {
        val = val.replace(/[^\d.]/g, '');
        if (val === '' || val === '.') return val;
        var parts = val.split('.');
        if (parts[0].length > 1) {
            parts[0] = parts[0].replace(/^0+/, '') || '0';
        }
        parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ',');
        if (parts.length > 1) {
            parts[1] = parts[1].substring(0, 2);
            return parts[0] + '.' + parts[1];
        }
        return parts[0];
    }

    function removeThousandSeparator(val) {
        return val.replace(/,/g, '');
    }

    function syncToHiddenAmt(raw) {
        var hiddenTxt = document.getElementById('<%= txtAmt.ClientID %>');
        if (hiddenTxt) hiddenTxt.value = raw;
    }

    function initAmtDisplay() {
        var el = document.getElementById('txtAmtDisplay');
        if (!el) return;

        var isComposing = false;  // ★ 每個 input 獨立的 flag，放在 closure 內

        // 監聽 IME 組字開始／結束
        el.addEventListener('compositionstart', function () {
            isComposing = true;
        });

        el.addEventListener('compositionend', function () {
            isComposing = false;
            // composition 結束後強制做一次格式化
            doFormat(el);
        });

        el.addEventListener('input', function () {
            if (isComposing) return;  // ★ IME 組字中，不格式化
            doFormat(el);
        });

        el.addEventListener('blur', function () {
            var raw = removeThousandSeparator(el.value);
            if (raw === '' || isNaN(parseFloat(raw))) {
                el.value = '';
                syncToHiddenAmt('');
                return;
            }
            var num = parseFloat(raw);
            el.value = num.toLocaleString('zh-TW', {
                minimumFractionDigits: 0,
                maximumFractionDigits: 2
            });
            syncToHiddenAmt(raw);
        });

        el.addEventListener('focus', function () {
            el.value = removeThousandSeparator(el.value);
        });

        // 回填時格式化
        var hiddenTxt = document.getElementById('<%= txtAmt.ClientID %>');
        if (hiddenTxt && hiddenTxt.value !== '') {
            var num = parseFloat(hiddenTxt.value);
            if (!isNaN(num)) {
                el.value = num.toLocaleString('zh-TW', {
                    minimumFractionDigits: 0,
                    maximumFractionDigits: 2
                });
            }
        }
    }

    function doFormat(el) {
        var raw = removeThousandSeparator(el.value);

        // 計算游標前有幾個「非逗號」字元
        var cursorPos = el.selectionStart;
        var rawBeforeCursor = el.value.substring(0, cursorPos).replace(/,/g, '').length;

        var formatted = addThousandSeparator(raw);

        // ★ 用 execCommand 或直接賦值（execCommand 可觸發 undo 堆疊，但部分瀏覽器已棄用）
        // 這裡直接賦值，然後立即還原游標
        el.value = formatted;

        // 還原游標位置
        var newCursor = 0;
        var count = 0;
        for (var i = 0; i < formatted.length; i++) {
            if (formatted[i] !== ',') count++;
            if (count === rawBeforeCursor) {
                newCursor = i + 1;
                break;
            }
        }
        // rawBeforeCursor 為 0 時，newCursor 保持 0
        el.setSelectionRange(newCursor, newCursor);

        syncToHiddenAmt(raw);
    }

    function beforeSubmit() {
        var display = document.getElementById('txtAmtDisplay');
        if (display) {
            syncToHiddenAmt(removeThousandSeparator(display.value));
        }
        return true;
    }

    // DOM 載入完成後初始化
    document.addEventListener('DOMContentLoaded', function () {
        initAmtDisplay();
    });
</script>
</body>
</html>
