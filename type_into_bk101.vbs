Set ws = CreateObject("WScript.Shell")
WScript.Sleep 3000
ws.AppActivate "員工基本資料建檔"
WScript.Sleep 500
ws.SendKeys "E0099"
WScript.Sleep 200
ws.SendKeys "{TAB}"
WScript.Sleep 200
ws.SendKeys "測試員工"
WScript.Sleep 200
ws.SendKeys "{TAB}"
WScript.Sleep 200
ws.SendKeys "A123456789"
WScript.Sleep 200
ws.SendKeys "{TAB}"
WScript.Sleep 200
ws.SendKeys "00951850100099"
MsgBox "Done typing!", 64, "Auto Type"
