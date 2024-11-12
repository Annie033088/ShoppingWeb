let userLoginControl = document.getElementById("UserLogin");
let userForm = userLoginControl.closest("form");//向上搜尋找到最近的 form元素 通常是唯一的 不需要特別指定

//進行userform監聽
userForm.addEventListener("submit", function (event) {
    let username = userForm.querySelector('input[name$="UserName"]').value;
    let pwd = userForm.querySelector('input[name$="Password"]').value;
    let regex = /^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z\d]{8,20}$/; //?=.*[a-zA-Z])至少包含一英文, {8,20}8~20位數


    //進行密碼驗證

    if (!(regex.test(username) && regex.test(pwd))) {
        alert("請輸入有效帳號..");
        event.preventDefault();  // 阻止表單提交
    }

})