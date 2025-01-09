
const errorCodeDefine = {
    //預設
    Default : 0,

    //成功
    Success: 1,

    //被他人踢出
    KickOut: 2,

    //被Ban掉
    Baned: 3,

    //權限已被修改
    PermissionModified: 4,

    //無效輸入
    InvalidFormatOrEntry : 5,

    //伺服器錯誤
    ServerError : 6,

    //無權限
    NoPermission : 7,

    //使用者未登入
    UserNotLogged : 8,

    //登入失敗
    LoginFailed :9,

    //密碼輸入錯誤
    PasswordEnterIncorrectly : 10,

    //創建失敗
    CreateFailed : 11,

    //修改失敗
    ModifiedFailed : 12,

    //刪除失敗
    DeleteFailed : 13
};

//設定errorCode對應資料
function errorCodeToMessage(errorCode) {
    let message;

    switch (errorCode) {
        case 1:
            message = "請求成功";
            return  message ;
            break;
        case 2:
            message = "您的帳號已被其他使用者踢出";
            return  message ;
            break;
        case 3:
            message = "您的帳號已被禁用";
            return  message ;
            break;
        case 4:
            message = "您的權限已被更動，請重新登入";
            return message;
            break;
        case 5:
            message = "請求格式錯誤或無效數據";
            return  message ;
            break;
        case 6:
            message = "伺服器錯誤";
            return  message ;
            break;
        case 7:
            message = "沒有此權限";
            return  message;
            break;
        case 8:
            message = "使用者未登入";
            return message;
            break;
        case 9:
            message = "登入失敗";
            return message;
            break;
        case 10:
            message = "密碼輸入錯誤";
            return  message;
            break;
        case 11:
            message = "新增失敗";
            return  message;
            break;
        case 12:
            message = "修改失敗";
            return  message;
            break;
        case 13:
            message = "刪除失敗";
            return message;
            break;
        default:
    }
}

//設定跳轉頁面
function setRedirectPage(controller, action) {
    axios.post(`/${controller}/${action}`)
        .then(response => {
            //如果回傳的是View而不是errorCode
            if (!response.data.errorCode) {
                window.location.href = `/${controller}/${action}`;
                return;
            }

            let errorCode = response.data.errorCode;

            //沒有成功
            if (errorCode != errorCodeDefine.Success) {
                let message = errorCodeToMessage(errorCode);

                //顯示訊息
                Swal.fire(message)
                    .then(result => {
                        //確認後處理
                        if (result.isConfirmed) {
                            //被踢出去
                            if (errorCode == errorCodeDefine.KickOut || errorCode == errorCodeDefine.Baned
                                || errorCode == errorCodeDefine.PermissionModified) {
                                window.location.href = "/Login/Index";
                            }
                            //跳轉頁面(情況是未登入的使用者輸入登入後的URL)
                            if (errorCode == errorCodeDefine.UserNotLogged) {
                                window.location.href = "/Login/Index";
                            }
                        }
                    });
                return;
            }
        })
        .catch(error => {
            console.error("fail", error);
        });
}