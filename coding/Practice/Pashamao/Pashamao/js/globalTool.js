//設定errorCode對應資料
function errorCodeToInfo(errorCode) {
    let successMessage;
    let kickOutMessage;
    let errorMessage;
    let redirectMessage;
    let redirectUrl;

    switch (errorCode) {
        case 1:
            successMessage = "請求成功";
            return { successMessage: successMessage };
            break;
        case 2:
            kickOutMessage = "您的帳號已被其他使用者踢出";
            return { kickOutMessage: kickOutMessage };
            break;
        case 3:
            kickOutMessage = "您的帳號已被禁用";
            return { kickOutMessage: kickOutMessage };
            break;
        case 4:
            kickOutMessage = "您的權限已被更動，請重新登入";
            return { kickOutMessage: kickOutMessage };
            break;
        case 5:
            errorMessage = "請求格式錯誤或無效數據";
            return { errorMessage: errorMessage };
            break;
        case 6:
            errorMessage = "伺服器錯誤";
            return { errorMessage: errorMessage };
            break;
        case 7:
            errorMessage = "沒有此權限";
            return { errorMessage: errorMessage };
            break;
        case 8:
            redirectMessage = "使用者未登入";
            redirectUrl = "/Login/Index";
            return { redirectMessage: redirectMessage, redirectUrl: redirectUrl };
            break;
        case 9:
            errorMessage = "登入失敗";
            return { errorMessage: errorMessage };
            break;
        case 10:
            errorMessage = "密碼輸入錯誤";
            return { errorMessage: errorMessage };
            break;
        case 11:
            errorMessage = "此帳號已經存在";
            return { errorMessage: errorMessage };
            break;
        case 12:
            errorMessage = "修改失敗";
            return { errorMessage: errorMessage };
            break;
        case 13:
            errorMessage = "刪除失敗";
            return { errorMessage: errorMessage };
            break;
        default:
    }
}

//設定通用的errorCode回傳的responseInfo物件的處理方法(不包括可能回傳頁面的post的response)
function responseInfoHandler(responseInfo) {
    //失敗
    if (responseInfo.errorMessage) {
        Swal.fire(responseInfo.errorMessage)
            .then(result => {
                if (result.isConfirmed) {
                    return;
                }
            });
    }
    //被kickOut
    else if (responseInfo.kickOutMessage) {
        swal.fire(responseInfo.kickOutMessage)
            .then(result => {
                if (result.isConfirmed) {
                    window.location.href = "/Login/Index";
                }
            });
    }
    //轉導頁面
    else if (responseInfo.redirectMessage) {
        window.location.href = responseInfo.redirectUrl;
    }
}

//設定跳轉頁面
function setRedirectPage(controller, action) {
    axios.post(`/${controller}/${action}`)
        .then(response => {
            //如果回傳的是View而不是errorCode
            if (!response.data.errorCode) {
                window.location.href = `/${controller}/${action}`;
            }

            //取得errorCode的信息
            responseInfo = errorCodeToInfo(response.data.errorCode);
            //失敗的話
            if (responseInfo.errorMessage) {
                Swal.fire(responseInfo.errorMessage);
            }
            //被kickOut的話
            else if (responseInfo.kickOutMessage) {
                swal.fire(responseInfo.kickOutMessage)
                    .then(result => {
                        if (result.isConfirmed) {
                            window.location.href = "/Login/Index";
                        }
                    });
            }
            //轉導頁面
            else if (responseInfo.redirectMessage) {
                window.location.href = responseInfo.redirectUrl;
            }
        })
        .catch(error => {
            console.error("fail", error);
        });
}