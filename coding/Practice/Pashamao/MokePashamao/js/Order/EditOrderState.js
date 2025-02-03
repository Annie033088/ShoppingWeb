function memberEditOrderState() {
    let orderId = document.getElementById("orderId").value;
    let orderState = document.getElementById("memberOrderState").value;

    let EditOrderStateDto = {
        OrderId: orderId,
        LogisticsNumber: null,
        State: orderState
    };

    axios.post("/Order/EditOrderState", EditOrderStateDto)
        .then(response => {
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
                                window.location.href = "/Home/Index";
                            }
                            //跳轉頁面(情況是未登入的使用者輸入登入後的URL)
                            if (errorCode == errorCodeDefine.UserNotLogged) {
                                window.location.href = "/Home/Index";
                            }
                        }
                    });
                return;
            }

            //成功的話
            Swal.fire("成功!");
        })
        .catch(error => {
            console.error("fail", error);
        });
}

function logisticsEditOrderState() {
    let logisticsNumber = document.getElementById("logisticsNumber").value;
    let orderState = document.getElementById("logisticsOrderState").value;

    let EditOrderStateDto = {
        OrderId: null,
        LogisticsNumber: logisticsNumber,
        State: orderState
    };

    axios.post("/Order/EditOrderState", EditOrderStateDto)
        .then(response => {
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
                                window.location.href = "/Home/Index";
                            }
                            //跳轉頁面(情況是未登入的使用者輸入登入後的URL)
                            if (errorCode == errorCodeDefine.UserNotLogged) {
                                window.location.href = "/Home/Index";
                            }
                        }
                    });
                return;
            }

            //成功的話
            Swal.fire("成功!");
        })
        .catch(error => {
            console.error("fail", error);
        });
}