ShoppingWeb應用程式的使用步驟

資料庫建立

1．拉取ShoppingWeb-DB repository的Main分支的資料
	=>現在會有四個資料夾, 分別是DataTable. SP. UserDefinedFunction. UserDefinedTableType
	=>每個資料夾各有一隻.SQL檔案可以建立對應的資料庫物件
2．進入SSMS, 右鍵點擊資料庫, 新增資料庫
	=>取名"Pashamao"
3．對剛建立的資料庫右鍵新增查詢, 案順序執行剛剛下載下來的.SQL檔案 (總共需要執行4支程式)
	=>順序為：建立 "資料表" (DataTablesScript.sql)
	=>建立"使用者自定義方法" (ScriptUserDefinedFunctionScript.sql)
	=>建立"使用者定義資料表類型" (UserDefinedTableTypeScript.sql)
	=>建立"預存程序" (StoredProcedureScript.sql)

以上流程結束, 就完成了資料庫的建立

應用程式建立