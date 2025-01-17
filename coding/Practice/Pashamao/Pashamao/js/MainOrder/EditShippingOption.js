document.addEventListener("DOMContentLoaded", function () {
    axios.post("/MainOrder/GetShippingOption")
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

            //成功的話
            let shippingOptions = response.data.shippingOptions;
            if (shippingOptions == null) return;
            populateTable(shippingOptions);
        })

        .catch(error => {
            console.error("fail", error);
        });
});

function populateTable(shippingOptions) {
    let cardContainer = document.querySelector(".cardContainer");
    shippingOptions.forEach((shippingOption) => {
        //運輸方式的card
        let shippingOptionCard = document.createElement("div");
        shippingOptionCard.className = "card col-3 p-1";
        shippingOptionCard.style = "display: flex; justify-content: center; align-items: center;  text-align: center; ";
        var timestamp = shippingOption.UpdateTime.match(/\d+/)[0];  // 提取數字部分
        var date = new Date(parseInt(timestamp));
        console.log(date);  // 顯示日期時間
        //標題
        let shippingOptionCardTitle = document.createElement("label");
        shippingOptionCardTitle.className = "card-header mb-2 w-100";
        shippingOptionCardTitle.textContent = shippingOption.Option + "(" + formatDateToYYYYMMDDHHMMSS(shippingOption.UpdateTime) + ")";
        shippingOptionCard.appendChild(shippingOptionCardTitle);

        //運輸方式名
        let optionNameBox = document.createElement("div");
        optionNameBox.className = "mb-1";
        let optionNameTitle = document.createElement("span");
        optionNameTitle.innerHTML = "運輸方式：";
        let optionNameInput = document.createElement("input");
        optionNameInput.value = shippingOption.Option;
        optionNameInput.disabled = true;
        optionNameBox.appendChild(optionNameTitle);
        optionNameBox.appendChild(optionNameInput);
        shippingOptionCard.appendChild(optionNameBox);

        //運費
        let shippingFeeBox = document.createElement("div");
        shippingFeeBox.className = "mb-1";
        let shippingFeeTitle = document.createElement("span");
        shippingFeeTitle.innerHTML = "運費：";
        let shippingFeeInput = document.createElement("input");
        shippingFeeInput.value = shippingOption.ShippingFee;
        shippingFeeInput.disabled = true;
        shippingFeeBox.appendChild(shippingFeeTitle);
        shippingFeeBox.appendChild(shippingFeeInput);
        shippingOptionCard.appendChild(shippingFeeBox);

        //免運標準
        let freeShippingBox = document.createElement("div");
        freeShippingBox.className = "mb-1";
        let freeShippingTitle = document.createElement("span");
        freeShippingTitle.innerHTML = "免運標準：";
        let freeShippingInput = document.createElement("input");
        freeShippingInput.value = shippingOption.FreeShipping;
        freeShippingInput.disabled = true;
        freeShippingBox.appendChild(freeShippingTitle);
        freeShippingBox.appendChild(freeShippingInput);
        shippingOptionCard.appendChild(freeShippingBox);

        //編輯按鈕
        let btnEdit = document.createElement("button");
        btnEdit.className = "btn btnEdit";
        let btnSubmitEdit = document.createElement("button");
        btnSubmitEdit.innerHTML = "確認";
        btnSubmitEdit.className = "btn btn-outline-success";
        btnSubmitEdit.style.display = 'none';

        btnEdit.addEventListener("click", function () {
            btnEdit.style.display = 'none';
            btnSubmitEdit.style.display = 'inline-block';
            shippingFeeInput.disabled = false;
            freeShippingInput.disabled = false;
        });

        //送出修改運費
        btnSubmitEdit.addEventListener("click", function () {
            btnEdit.style.display = 'inline-block';
            btnSubmitEdit.style.display = 'none';
            shippingFeeInput.disabled = true;
            freeShippingInput.disabled = true;

            const priceRegex = /^[0-9]{1,9}$/;
            if (!priceRegex.test(shippingFeeInput.value) || !priceRegex.test(freeShippingInput.value)) {
                Swal.fire('請輸入有效的價格（EX：123、123.45）')
                    .then(result => {
                        if (result.isConfirmed) {
                            setRedirectPage('MainOrder', 'GetRedirectShippingOptionView');
                        }
                    });
                return;
            }

            //如果有修改才提交請求
            if (!(shippingFeeInput.value == shippingOption.ShippingFee
                && freeShippingInput.value == shippingOption.FreeShipping)) {
                postEditShippingOption(shippingOption.ShippingOptionId, shippingFeeInput.value, freeShippingInput.value, shippingOption.UpdateTime);
            }
        });

        shippingOptionCard.appendChild(btnEdit);
        shippingOptionCard.appendChild(btnSubmitEdit);

        cardContainer.appendChild(shippingOptionCard);
    });
}
function postEditShippingOption(id, shippingFee, freeShipping, updateTime) {

    let editShippingOptionDto = {
        ShippingOptionId: id,
        ShippingFee: shippingFee,
        FreeShipping: freeShipping,
        UpdateTime: formatDateToYYYYMMDDHHMMSS(updateTime)
    };
    axios.post("/MainOrder/EditShippingOption", editShippingOptionDto)
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
                                window.location.href = "/Login/Index";
                            }
                            //跳轉頁面(情況是未登入的使用者輸入登入後的URL)
                            if (errorCode == errorCodeDefine.UserNotLogged) {
                                window.location.href = "/Login/Index";
                            }

                            setRedirectPage('MainOrder', 'GetRedirectShippingOptionView');
                        }
                    });
                return;
            }

            //成功的話
            setRedirectPage('MainOrder', 'GetRedirectShippingOptionView');
        })

        .catch(error => {
            console.error("fail", error);
        });
}
function formatDateToYYYYMMDDHHMMSS(dateString) {
    // 使用正則表達式提取時間戳部分
    var timestamp = dateString.match(/\/Date\((\d+)\)\//);

    if (timestamp && timestamp[1]) {
        var date = new Date(parseInt(timestamp[1], 10));  // 轉換為毫秒並創建 Date 物件

        // 取得年份、月份、日期、時、分、秒
        var year = date.getFullYear();
        var month = date.getMonth() + 1; // 月份從 0 開始，需加 1
        var day = date.getDate();
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var seconds = date.getSeconds();

        // 格式化為兩位數
        month = month < 10 ? '0' + month : month;
        day = day < 10 ? '0' + day : day;
        hours = hours < 10 ? '0' + hours : hours;
        minutes = minutes < 10 ? '0' + minutes : minutes;
        seconds = seconds < 10 ? '0' + seconds : seconds;

        // 返回格式化後的字串：YYYY-MM-DD HH:mm:ss
        return year + '-' + month + '-' + day + ' ' + hours + ':' + minutes + ':' + seconds;
    }
    return null;  // 如果無法匹配，返回 null
}
