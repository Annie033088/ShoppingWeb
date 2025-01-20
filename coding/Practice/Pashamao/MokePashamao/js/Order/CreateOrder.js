function submitCreateOrder1() {
    let element = document.getElementById("mokeOne");
    submitCreateOrder(element);
}
function submitCreateOrder2() {
    let element = document.getElementById("mokeTwo");
    submitCreateOrder(element);
}

function submitCreateOrder(parentElement) {
    let txbAccount = parentElement.querySelector("#txbAccount");
    let memberId = txbAccount.dataset.id;

    let txbName = parentElement.querySelector("#txbName").value;
    let txbPostalCode = parentElement.querySelector("#txbPostalCode").value;
    let txbAddress = parentElement.querySelector("#txbAddress").value;
    let txbPhone = parentElement.querySelector("#txbPhone").value;
    let shippingOptionId = parentElement.querySelector("#txbShippingMethod").dataset.id;

    let nameRegex = /^.{1,50}$/; //1到50字名字
    if (!nameRegex.test(txbName)) {
        Swal.fire('請輸入正確的名字');
        return;
    }

    let postalCodeRegex = /^([0-9]{3})$/;
    if (!postalCodeRegex.test(txbPostalCode)) {
        Swal.fire('請輸入正確的郵遞區號');
        return;
    }

    let addressRegex = /^.{1,326}$/;
    if (!addressRegex.test(txbAddress)) {
        Swal.fire('請輸入正確的名字');
        return;
    }

    let phoneRegex = /^([0-9]{10})$/;
    if (!phoneRegex.test(txbPhone)) {
        Swal.fire('請輸入正確的電話');
        return;
    }

    let productList = [];

    const productTableBody = parentElement.getElementsByTagName('tbody')[0];
    const productRows = productTableBody.querySelectorAll("tr");

    productRows.forEach(row => {
        let datas = row.querySelectorAll("td");
        let product = {
            ProductId: row.dataset.productid,
            ProductStyleId: row.dataset.productstyleid,
            Quantity: datas[2].innerText
        };
        productList.push(product);
    });

    const createOrderDto = {
        MemberId: memberId,
        Name: txbName,
        PostalCode: txbPostalCode,
        Address: txbAddress,
        Phone: txbPhone,
        ShippingOptionId: shippingOptionId,
        createOrderProductDtos: productList
    };

    axios.post("/Order/CreateOrder", createOrderDto)
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