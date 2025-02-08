# ShoppingWeb-CMS 方案說明 #

## 目錄 ##

1. 簡介
2. 還原專案
3. 登入超級管理員

## 簡介 ##

ShoppingWeb-CMS方案包含三個專案
分別是 MockPashamao、Pashamao (.Net Framework MVC) 及 UnitTestPashamao (單元測試)
補充：單元測試未完成，未來有機會完成此專案

Pashamao 的主要功能為：運行一個網頁版購物網站的後台管理應用程式

MockPashamao 的主要功能為：模擬須由前端 API 或者外部 API 實現的功能  (Ex：註冊購物網站的會員)

## 還原專案 ##

1. 複製 ShoppingWeb-CMS Repo `https://github.com/Annie033088/ShoppingWeb-CMS.git`
2. 進入 Pashamao 資料夾運行方案 Pashamao.sln

## 登入超級管理員 ##

1. 將此應用程式的管理員資料匯入資料庫
    1. 打開 Pashamao 資料夾底下的 AddAdmin.sql ，於專案的資料庫執行指令
2. 執行 Pashamao 應用程式，並登入帳號密碼
    acc：BG6nNMVFp34q
    pwd：yXb8S9azKg7VPfTY
