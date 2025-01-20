
function submitCreateMember() {
    const account = document.getElementById("txbAccount").value.trim();
    const pwd = document.getElementById("txbPwd").value.trim();
    const email = document.getElementById("txbEmail").value.trim();
    const phone = document.getElementById("txbPhone").value.trim();
    const memberName = document.getElementById("txbMemberName").value.trim();
    const nickname = document.getElementById("txbNickname").value.trim();

    //驗證資料
    let accountAndPwdRegex = /^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z\d]{8,20}$/;//至少包含1個數字及英文, 長度為8~20
    if (!accountAndPwdRegex.test(account)) {
        Swal.fire('請輸入至少包含1個數字及英文, 長度為8~20的帳號');
        return;
    }

    if (!accountAndPwdRegex.test(pwd)) {
        Swal.fire('請輸入至少包含1個數字及英文, 長度為8~20的密碼');
        return;
    }

    if (pwd == account) {
        Swal.fire('帳密不可相同');
        return;
    }

    // 帳號:表示字母、數字、下劃線(長度64以內) + @ + 匹配域名 包含字母、數字、點和破折號(最多253) + . + 頂級域名 至少包含 2 個的字母或數字
    let emailRegex = /^([a-zA-Z0-9.-]{1,64})@([a-zA-Z0-9.-]{1,253})\.[a-zA-Z0-9]{2,}$/ //不一定要有
    if (!emailRegex.test(email)) {
        Swal.fire('請輸入正確的信箱');
        return;
    }

    //10位電話
    let phoneRegex = /^([0-9]{10})?$/; //不一定要有
    if (!phoneRegex.test(phone)) {
        Swal.fire('請輸入正確的電話');
        return;
    }

    let memberNameRegex = /^.{1,50}$/; //1到50字名字
    if (!memberNameRegex.test(memberName)) {
        Swal.fire('請輸入正確的名字');
        return;
    }

    let nickNameRegex = /^.{0,25}$/; //0到20字名字
    if (!nickNameRegex.test(nickname)) {
        Swal.fire('請輸入25字以內暱稱');
        return;
    }

    let createMemberDto = {
        Account: account,
        Pwd: pwd,
        Email: email,
        Phone: phone,
        MemberName: memberName,
        Nickname: nickname
    };

    axios.post("/Member/CreateMember", { createMemberDto })
        .then(response => {
            let errorCode = response.data.errorCode;
            console.log(response);
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
            window.location.href = "/Home/Index";
        })
        .catch(error => {
            console.error("fail", error);
        });
}