console.log(productData.productDetail);
let product = productData.productDetail.SelectProductDetailDto;
let styles = productData.productDetail.SelectProductStyleDto;
let images = productData.productDetail.SelectProductImages;

//計算展示圖片的代數
let currentImageIndex = 0;
let showNavImageCnt = 4;
let imageTotal = 0;

//刪除的圖片
let delImageList = [];

populateImage();
populateProduct();
populateStyle();

function populateImage() {
    images.forEach(image => {
        let imageUrl = image.ImageUrl;
        let lastIndex = imageUrl.lastIndexOf("\\");
        let imageName = imageUrl.substring(lastIndex + 1);

        let displayImageContainer = document.getElementById("displayImageContainer");
        let ProductImageBox = document.createElement("div");
        ProductImageBox.className = "displayImageBox";
        let ProductImage = document.createElement("img");
        ProductImage.id = imageName;
        ProductImage.setAttribute("data-id", image.ProductImageId);
        ProductImage.className = "img-fluid displayImage";
        ProductImage.src = imageUrl;
        ProductImageBox.appendChild(ProductImage);
        displayImageContainer.appendChild(ProductImageBox);
        imageTotal++;
    });
    updateImageDisplay();
}

function populateProduct() {
    let productStatus = product.Status;
    document.getElementById("txbName").value = product.Name;
    document.getElementById("txbDescription").value = product.Description;
    document.getElementById("dropdownCategory").value = product.CategoryId;
    document.getElementById("productIntroduce").value = product.Introduction;
    if (productStatus == true) {
        document.getElementById("btnProductStatusOn").className = "opacity-100 btn btn-dark";
        document.getElementById("btnProductStatusOff").className = "btn btn-outline-dark opacity-50";
    } else {
        document.getElementById("btnProductStatusOn").className = "btn btn-outline-dark opacity-50";
        document.getElementById("btnProductStatusOff").className = "opacity-100 btn btn-dark";
    }
}

function populateStyle() {
    styles.forEach(style => {
        let imageSrc = "";
        let imageName = "";

        if (style.ImageUrl == " " || style.ImageUrl == "") {
            imageSrc = "/images/productImage/noImage.jpg";
            imageName = ` `;
        } else {
            let imageUrl = style.ImageUrl;
            let lastIndex = imageUrl.lastIndexOf("\\");
            imageName = imageUrl.substring(lastIndex + 1);
            imageSrc = style.ImageUrl;
        }

        addStyleRow(style, imageSrc, imageName);
    });
}

function getImageAndEdit() {
    let addImageList = [];
    let delOldImageList = [];
    let imagesElement = document.querySelectorAll(".displayImage");
    let imagesHtmlStrings = [];
    let imageCount = 0;

    if (imagesElement.length > 0) {
        imagesElement.forEach((image, index) => {
            let html = `
                    <div id="" class="imageContainer me-2 col-6" style=" position: relative; ">
                        <img id="${image.id}" src="${image.src}" class="img-fluid" />
                        <p class="imageName">${image.id}</p>
                        <button class = "btn btnDeleteImage" style=""  data-id="${image.id}">
                    </div>`;
            imagesHtmlStrings.push(html);
            imageCount++;
        });
    }

    Swal.fire({
        title: '修改圖片',
        width: '80%',
        html: `
                <p class="text-danger">最多放10張圖片, 每張圖片最大1mb</p>
                <div id= "addImageBox">
                    <div id= "imagePreviews" style = "display: flex; align-items: flex-start; justify-content: space-between; position: relative; ">
                           ${imagesHtmlStrings}
                    </div>
                </div>
                `
        ,
        background: '#f0f0f0',
        showCancelButton: true,
        confirmButtonText: '確定',
        cancelButtonText: '取消',
        heightAuto: true,
        didOpen: () => {
            var addImageBox = document.getElementById('addImageBox');
            var imgUploadBox = document.createElement('div');
            var inputElement = document.createElement('input');
            var btnAddImage = document.createElement('button');
            imgUploadBox.id = "imgUploadBox";
            inputElement.type = "file";
            inputElement.id = "txbImage";
            inputElement.accept = "image/*";
            inputElement.style.display = "none";
            btnAddImage.id = "btnUploadImage";
            btnAddImage.className = "btn btnAdd";

            imgUploadBox.appendChild(btnAddImage);
            imgUploadBox.appendChild(inputElement);
            addImageBox.appendChild(imgUploadBox);

            const imagePreviews = document.getElementById('imagePreviews');
            const txbImage = document.getElementById('txbImage');
            const btnUploadImage = document.getElementById('btnUploadImage');
            btnUploadImage.addEventListener('click', () => {
                //呼叫input更換圖片
                txbImage.click();
            });

            txbImage.addEventListener('change', (e) => {
                if (imageCount > 9) {
                    alert("超過十張");
                    return;
                }

                const file = e.target.files[0];

                if (!file) return;

                if (file.name.length > 20) {
                    alert("圖片名過長");
                    return;
                }

                //驗證檔案最大1mb
                const maxSize = 1024 * 1024;
                if (file.size > maxSize) {
                    alert("圖片檔案過大");
                    return;
                }

                const mimeType = file.type.toLowerCase();
                switch (mimeType) {
                    case 'image/jpeg':
                        imageType = '.jpg';
                        break;
                    case 'image/png':
                        imageType = '.png';
                        break;
                    case 'image/webp':
                        imageType = '.webp';
                        break;
                    default:
                        alert('請上傳 JPEG(JPG)、PNG 或 WebP 格式的圖片');
                        return;
                        break;
                }

                const reader = new FileReader();
                reader.onload = function (event) {
                    const imgNewBox = document.createElement('div');
                    const imgElement = document.createElement('img');
                    let imgNameElement = document.createElement('p');
                    const btnDeleteImage = document.createElement('button');
                    const timestamp = new Date().getTime();
                    imgNameElement.innerHTML = `${file.name}`;
                    imgElement.id = `${timestamp}-${file.name}`;
                    imgNewBox.className = "imageContainer me-2 col-6";
                    imgNewBox.style.position = "relative";
                    btnDeleteImage.className = "btn btnDeleteImage";

                    imgElement.src = event.target.result;
                    imgElement.className = "img-fluid newAddImages";

                    btnDeleteImage.addEventListener('click', (event) => {
                        const parentDiv = event.target.parentElement;

                        const index = addImageList.indexOf(imgElement.id);

                        if (index > -1) {
                            addImageList.splice(index, 1);
                        }

                        imageCount--;
                        parentDiv.remove();
                    });

                    imgNewBox.appendChild(imgElement);
                    imgNewBox.appendChild(imgNameElement);
                    imgNewBox.appendChild(btnDeleteImage);
                    imagePreviews.appendChild(imgNewBox);
                    addImageList.push(imgElement.id);
                    imageCount++;
                };
                reader.readAsDataURL(file);
            });

            const buttons = document.querySelectorAll('.btnDeleteImage');
            buttons.forEach(button => {
                button.addEventListener('click', (event) => {
                    const btn = event.target;
                    delOldImageList.push(btn.dataset.id);
                    const parentDiv = btn.parentElement;

                    imageCount--;
                    parentDiv.remove();
                });
            });

        },
        preConfirm: () => {
            return { delOldImageList: delOldImageList, addImageList: addImageList };
        }
    }).then((result) => {
        if (result.isConfirmed) {
            if (result.value.addImageList.length > 0) {
                result.value.addImageList.forEach(imgId => {
                    const image = document.getElementById(imgId);
                    let displayImageContainer = document.getElementById("displayImageContainer");
                    let ProductImageBox = document.createElement("div");
                    ProductImageBox.className = "displayImageBox";
                    let ProductImage = document.createElement("img");
                    ProductImage.setAttribute("data-id", " ");
                    ProductImage.id = imgId;
                    ProductImage.className = "img-fluid displayImage";
                    ProductImage.src = image.src;
                    ProductImageBox.appendChild(ProductImage);
                    displayImageContainer.appendChild(ProductImageBox);
                    imageTotal++;
                });
            }

            if (result.value.delOldImageList.length > 0) {
                result.value.delOldImageList.forEach(imgId => {
                    const image = document.getElementById(imgId);
                    var machingImage = images.filter(img => img.ProductImageId == image.dataset.id);
                    if (machingImage.length > 0) {
                        delImageList.push(machingImage[0].ProductImageId);
                    }

                    const parentDiv = image.parentElement;
                    parentDiv.remove();
                    imageTotal--;
                });
            }
            currentImageIndex = 0;
            updateImageDisplay();
        }
    });
}

function imagePrevious() {
    if (currentImageIndex > 0) {
        currentImageIndex--;
        updateImageDisplay();
    }
}

function imageNext() {
    if (currentImageIndex < imageTotal - showNavImageCnt) {
        currentImageIndex++;
        updateImageDisplay();
    }
}

function updateImageDisplay() {

    let displayImages = document.querySelectorAll('.displayImage');
    const prevBtn = document.getElementById('btnPrevious');
    const nextBtn = document.getElementById('btnNext');

    displayImages.forEach((img, index) => {
        if (index < showNavImageCnt + currentImageIndex && index >= currentImageIndex) {
            img.style.display = 'block';
        } else {
            img.style.display = 'none';
        }
    });

    if (currentImageIndex <= 0) {
        prevBtn.style.opacity = "0";
        prevBtn.disabled = true;
    } else {
        prevBtn.style.opacity = "0.8";
        prevBtn.disabled = false;
    }
    if (currentImageIndex >= imageTotal - showNavImageCnt) {
        nextBtn.style.opacity = "0";
        nextBtn.disabled = true;
    } else {
        nextBtn.style.opacity = "0.8";
        nextBtn.disabled = false;
    }
}

function addStyle() {
    let addImage = false;
    let imageType = "";
    let htmlStatus = `
                        <div id="addStyleStatusBox" class="input-group mt-3">
                            <span id="txtStatus" class="input-group-text">上/下架</span>
                            <button id="btnStatusOn" class="btn btn-outline-dark opacity-50">上架</button>
                            <button id="btnStatusOff" class="opacity-100 btn btn-dark" >下架</button>
                        </div>`;

    Swal.fire({
        title: '新增細項',
        html: `
       <div class="addStyleBox">
            <div id="addStyleImageBox" class="position-relative" style="border:solid">
                <img id="addStyleImage" class="img-fluid styleImage" src="/images/productImage/noImage.jpg" alt="" data-name=" ">
                <input type="file" id="txbAddImage" accept="image/*" style="display: none;">
                <p id="textEditImage">點擊圖片修改</p>
            </div>
            <div id="addStyleNameBox" class="input-group mt-3">
                <span class="input-group-text">細項名</span>
                <input id="txbAddStyleName" type="text" class="form-control" value="">
            </div>

            <div id="addStylePriceBox" class="input-group mt-3">
                <span class="input-group-text">價格</span>
                <input id="txbAddStylePrice" type="text" class="form-control" value="">
            </div>

            <div id="addStyleQuantityBox" class="input-group mt-3">
                <span class="input-group-text">數量</span>
                <input id="txbAddStyleQuantity" type="text" class="form-control" value="">
            </div>
            ${htmlStatus}
        </div>
        `,
        showCancelButton: true,
        confirmButtonText: '確定',
        cancelButtonText: '取消',
        didOpen: () => {
            let addStyleImage = document.getElementById("addStyleImage");
            let txbAddImage = document.getElementById("txbAddImage");
            let btnStatusOn = document.getElementById("btnStatusOn");
            let btnStatusOff = document.getElementById("btnStatusOff");
            let textEditImage = document.getElementById("textEditImage");
            addStyleImage.addEventListener('click', () => { txbAddImage.click(); });
            textEditImage.addEventListener('click', () => { txbAddImage.click(); });

            txbAddImage.addEventListener('change', (e) => {
                const file = e.target.files[0];
                const reader = new FileReader();

                if (!file) {
                    return;
                }
                else {
                    if (file.name.length > 20) {
                        alert("圖片名過長");
                        return;
                    }

                    const maxSize = 1024 * 1024;//最大1mb

                    if (file.size > maxSize) {
                        alert("圖片檔案過大");
                        return;
                    }
                }

                const mimeType = file.type.toLowerCase();

                switch (mimeType) {
                    case 'image/jpeg':
                        imageType = '.jpg';
                        break;
                    case 'image/png':
                        imageType = '.png';
                        break;
                    case 'image/webp':
                        imageType = '.webp';
                        break;
                    default:
                        alert('請上傳 JPEG(JPG)、PNG 或 WebP 格式的圖片');
                        return;
                        break;
                }

                reader.onload = function (event) {
                    addStyleImage.src = event.target.result;
                    addImage = true;
                };
                reader.readAsDataURL(file);
            });

            btnStatusOn.addEventListener("click", function () {
                btnStatusOn.className = "opacity-100 btn btn-dark";
                btnStatusOff.className = "btn btn-outline-dark opacity-50";
            });
            btnStatusOff.addEventListener("click", function () {
                btnStatusOn.className = "btn btn-outline-dark opacity-50";
                btnStatusOff.className = "opacity-100 btn btn-dark";
            });

        },
        preConfirm: () => {
            let styleStatus = "";
            let styleName = document.getElementById("txbAddStyleName").value.trim();
            let stylePrice = document.getElementById("txbAddStylePrice").value;
            let styleQuantity = document.getElementById("txbAddStyleQuantity").value;
            let btnStatusOn = document.getElementById("btnStatusOn").className;
            if (btnStatusOn == "opacity-100 btn btn-dark") { styleStatus = true; }
            else { styleStatus = false; }

            let addStyle = {
                ProductId: product.ProductId,
                Style: styleName,
                Price: stylePrice,
                StockQuantity: styleQuantity,
                Status: styleStatus,
            };

            //驗證輸入符合訊息
            const nameRegex = /^.{1,25}$/;

            if (!nameRegex.test(styleName)) {
                Swal.showValidationMessage('請輸入項目名');
                return false;
            }

            const priceRegex = /^[0-9]{1,9}(\.[0-9]{1,2})?$/;
            if (!priceRegex.test(stylePrice)) {
                Swal.showValidationMessage('請輸入有效的價格（EX：123、123.45）');
                return false;
            }

            const QuantityRegex = /^\d{1,9}$/;
            if (!QuantityRegex.test(styleQuantity)) {
                Swal.showValidationMessage('請輸入有效的數量');
                return false;
            }

            return { addStyle: addStyle };
        }
    }).then((result) => {
        if (result.isConfirmed) {
            const formData = new FormData();
            formData.append('LastEditTime', product.LastEditTime);
            formData.append('AddStyle', JSON.stringify(result.value.addStyle));

            const imgFile = document.getElementById("addStyleImage");

            if (addImage) {
                formData.append("ImageType", imageType);
                const imgBlob = dataURItoBlob(imgFile.src);
                formData.append('images[]', imgBlob);
            }

            axios.post("/MainProduct/AddProductStyle", formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            })
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
                    window.location.href = `/MainProduct/ProductDetail?productId=${product.ProductId}`;
                })
                .catch(error => {
                    console.error("fail", error);
                });
        }
    });
}

function editStyle(styleData, imageSrc, id) {
    let htmlStatus = "";
    let editImage = false;
    let imageType = "";

    if (styleData.Status == true) {
        htmlStatus = `
                      <div id="addStyleStatusBox" class="input-group mt-3">
                          <span id="txtStatus" class="input-group-text">上/下架</span>
                          <button id="btnStatusOn" class="opacity-100 btn btn-dark">上架</button>
                          <button id="btnStatusOff" class="btn btn-outline-dark opacity-50" >下架</button>
                      </div>`;
    }
    else {
        htmlStatus = `
                      <div id="addStyleStatusBox" class="input-group mt-3">
                          <span id="txtStatus" class="input-group-text">上/下架</span>
                          <button id="btnStatusOn" class="btn btn-outline-dark opacity-50">上架</button>
                          <button id="btnStatusOff" class="opacity-100 btn btn-dark" >下架</button>
                      </div>`;
    }

    Swal.fire({
        title: '新增細項',
        html: `
       <div class="addStyleBox">
            <div id="addStyleImageBox" class="position-relative" style="border:solid">
                <img id="addStyleImage" class="img-fluid styleImage" src="${imageSrc}" alt="" >
                <input type="file" id="txbAddImage" accept="image/*" style="display: none;">
                <p id = "textEditImage">點擊圖片修改</p>
            </div>
            <div id="addStyleNameBox" class="input-group mt-3">
                <span class="input-group-text">細項名</span>
                <input id="txbAddStyleName" type="text" class="form-control" value="${styleData.Style}">
            </div>

            <div id="addStylePriceBox" class="input-group mt-3">
                <span class="input-group-text">價格</span>
                <input id="txbAddStylePrice" type="text" class="form-control" value="${styleData.Price}">
            </div>

            <div id="addStyleQuantityBox" class="input-group mt-3">
                <span class="input-group-text">數量</span>
                <input id="txbAddStyleQuantity" type="text" class="form-control" value="${styleData.StockQuantity}">
            </div>
            ${htmlStatus}
        </div>
        `,
        showCancelButton: true,
        confirmButtonText: '確定',
        cancelButtonText: '取消',
        didOpen: () => {
            let addStyleImage = document.getElementById("addStyleImage");
            let txbAddImage = document.getElementById("txbAddImage");
            let btnStatusOn = document.getElementById("btnStatusOn");
            let btnStatusOff = document.getElementById("btnStatusOff");
            let textEditImage = document.getElementById("textEditImage");
            textEditImage.addEventListener('click', () => { txbAddImage.click(); });
            addStyleImage.addEventListener('click', () => { txbAddImage.click(); });

            txbAddImage.addEventListener('change', (e) => {
                const file = e.target.files[0];
                const reader = new FileReader();

                if (!file) {
                    return;
                }
                else {
                    if (file.name.length > 20) {
                        alert("圖片名過長");
                        return;
                    }

                    const maxSize = 1024 * 1024;//最大1mb

                    if (file.size > maxSize) {
                        alert("圖片檔案過大");
                        return;
                    }
                }

                const mimeType = file.type.toLowerCase();

                switch (mimeType) {
                    case 'image/jpeg':
                        imageType = '.jpg';
                        break;
                    case 'image/png':
                        imageType = '.png';
                        break;
                    case 'image/webp':
                        imageType = '.webp';
                        break;
                    default:
                        alert('請上傳 JPEG(JPG)、PNG 或 WebP 格式的圖片');
                        return;
                        break;
                }

                reader.onload = function (event) {
                    addStyleImage.src = event.target.result;
                    editImage = true;
                };
                reader.readAsDataURL(file);
            });

            btnStatusOn.addEventListener("click", function () {
                btnStatusOn.className = "opacity-100 btn btn-dark";
                btnStatusOff.className = "btn btn-outline-dark opacity-50";
            });
            btnStatusOff.addEventListener("click", function () {
                btnStatusOn.className = "btn btn-outline-dark opacity-50";
                btnStatusOff.className = "opacity-100 btn btn-dark";
            });
        },
        preConfirm: () => {
            let styleStatus = "";
            let styleName = document.getElementById("txbAddStyleName").value.trim();
            let stylePrice = document.getElementById("txbAddStylePrice").value;
            let styleQuantity = document.getElementById("txbAddStyleQuantity").value;
            let btnStatusOn = document.getElementById("btnStatusOn").className;

            if (btnStatusOn == "opacity-100 btn btn-dark") { styleStatus = true; }
            else { styleStatus = false; }

            if (styleName == styleData.Style && stylePrice == styleData.Price && styleQuantity == styleData.StockQuantity && styleStatus == styleData.Status && !editImage) {
                Swal.showValidationMessage('請修改資料');
                return false;
            }

            let editStyle = {
                ProductId: product.ProductId,
                ProductStyleId: styleData.ProductStyleId,
                Style: styleName,
                Price: stylePrice,
                StockQuantity: styleQuantity,
                Status: styleStatus,
            };


            //驗證輸入符合訊息
            const nameRegex = /^.{1,25}$/;
            if (!nameRegex.test(styleName)) {
                Swal.showValidationMessage('請輸入項目名');
                return false;
            }

            const priceRegex = /^[0-9]{1,9}(\.[0-9]{1,2})?$/;
            if (!priceRegex.test(stylePrice)) {
                Swal.showValidationMessage('請輸入有效的價格（EX：123、123.45）');
                return false;
            }

            const QuantityRegex = /^\d{1,9}$/;
            if (!QuantityRegex.test(styleQuantity)) {
                Swal.showValidationMessage('請輸入有效的數量');
                return false;
            }

            return { editStyle: editStyle };
        }
    }).then((result) => {
        if (result.isConfirmed) {
            const formData = new FormData();
            formData.append('LastEditTime', product.LastEditTime);
            formData.append('EditStyle', JSON.stringify(result.value.editStyle));

            const imgFile = document.getElementById("addStyleImage");

            if (editImage) {
                formData.append("ImageType", imageType);
                const imgBlob = dataURItoBlob(imgFile.src);
                formData.append('images[]', imgBlob);
            }

            axios.post("/MainProduct/EditProductStyle", formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            })
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
                    window.location.href = `/MainProduct/ProductDetail?productId=${product.ProductId}`;
                })
                .catch(error => {
                    console.error("fail", error);
                });
        }
    });

}

function addStyleRow(styleData, imageSrc, imageName) {
    const tableBody = document.getElementById("styleTable").getElementsByTagName('tbody')[0];

    const row = document.createElement('tr');

    if (styleData.ProductStyleId) {
        row.setAttribute('data-id', styleData.ProductStyleId);
    } else {
        row.setAttribute('data-id', ' ');
    }

    const timestamp = new Date().getTime();
    row.id = `rowStyle${timestamp}${styleData.ProductStyleId}`;
    const cellStyleName = document.createElement('td');
    cellStyleName.textContent = styleData.Style;
    cellStyleName.className = "styleName";
    row.appendChild(cellStyleName);

    const cellStyleImage = document.createElement('td');
    let styleImage = document.createElement('img');
    styleImage.setAttribute('data-name', imageName);
    styleImage.src = imageSrc;
    styleImage.style = " height:100%";
    styleImage.className = "me-2 sryleImage";
    cellStyleImage.appendChild(styleImage);
    row.appendChild(cellStyleImage);

    const cellStockQuantity = document.createElement('td');
    cellStockQuantity.textContent = styleData.StockQuantity;
    cellStockQuantity.className = "styleQuantity";
    if (styleData.StockQuantity <= 10) {
        const cellStockWarning = document.createElement('img');
        cellStockWarning.src = "/images/warn-removebg-preview.png";
        cellStockWarning.className = "stockWarning";
        cellStockWarning.style = " position: relative; background-size: contain;background-position: center; background-repeat: no-repeat;width: 25px;  height: 25px; ";
        cellStockQuantity.appendChild(cellStockWarning);
        row.appendChild(cellStockQuantity);
    } else {
        row.appendChild(cellStockQuantity);
    }

    const cellStylePrice = document.createElement('td');
    cellStylePrice.textContent = styleData.Price;
    cellStylePrice.className = "stylePrice";
    row.appendChild(cellStylePrice);

    const cellStatus = document.createElement('td');
    cellStatus.textContent = styleData.Status == true ? "上架" : "下架";
    cellStatus.className = "styleStatus";
    row.appendChild(cellStatus);

    const cellEditData = document.createElement('td');

    const cellEditDataBtn = document.createElement("button");
    cellEditDataBtn.className = "btnEditStyleData btnEdit";
    cellEditDataBtn.addEventListener("click", function () {
        editStyle(styleData, styleImage.src, row.id);
    });
    const cellDelDataBtn = document.createElement("button");
    cellDelDataBtn.className = "btnDelStyleData btnDel";
    cellDelDataBtn.addEventListener("click", function () {
        delStyle(row.dataset.id);
    });
    cellEditData.appendChild(cellEditDataBtn);
    cellEditData.appendChild(cellDelDataBtn);
    row.appendChild(cellEditData);
    // 把這一行加到表格中
    tableBody.appendChild(row);
}

function delStyle(styleId) {
    const tableBody = document.getElementById("styleTable").getElementsByTagName('tbody')[0];
    const styleCnt = tableBody.querySelectorAll('tr').length;

    if (styleCnt < 2) {
        Swal.fire({
            title: '商品至少有一個細項'
        });
        return;
    }

    Swal.fire({
        title: '確定要刪除這個項目嗎？',
        text: "這個操作無法恢復！",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: '刪除',
        cancelButtonText: '取消'
    }).then((result) => {
        if (result.isConfirmed) {
            axios.post("/MainProduct/DeleteProductStyle", {
                ProductStyleId: styleId,
                ProductId: product.ProductId,
                LastEditTime: product.LastEditTime
            })
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
                    window.location.href = `/MainProduct/ProductDetail?productId=${product.ProductId}`;
                })
                .catch(error => {
                    console.error("fail", error);
                });
        }
    });
}

function setProductStatusOn() {
    document.getElementById("btnProductStatusOn").className = "opacity-100 btn btn-dark";
    document.getElementById("btnProductStatusOff").className = "btn btn-outline-dark opacity-50";
}

function setProductStatusOff() {
    document.getElementById("btnProductStatusOn").className = "btn btn-outline-dark opacity-50";
    document.getElementById("btnProductStatusOff").className = "opacity-100 btn btn-dark";
}

function submitEditProduct() {
    let productStatus = false;
    let productName = document.getElementById("txbName").value.trim();
    let productDescription = document.getElementById("txbDescription").value.trim();
    let productCategory = document.getElementById("dropdownCategory").value.trim();
    let productIntroduce = document.getElementById("productIntroduce").value.trim();
    const regexName = /^[^\s].{0,29}$/;
    const regexDescription = /^[\s\S]{0,40}$/;
    const regexIntroduction = /^[\s\S]{0,1500}$/;

    if (!regexName.test(productName)) {
        Swal.fire("請輸入30字以內商品名(第一個字不得為空)");
        return;
    }

    if (!regexDescription.test(productDescription)) {
        Swal.fire("請輸入40字以內的描述內容");
        return;
    }


    if (!regexIntroduction.test(productIntroduce)) {
        Swal.fire("請輸入1500字以內的介紹");
        return;
    }

    if (productDescription == "") productDescription = " ";
    if (productIntroduce == "") productIntroduce = " ";

    if (document.getElementById("btnProductStatusOn").className == "opacity-100 btn btn-dark") {
        productStatus = true;
    }

    let newProduct = {
        ProductId: product.ProductId,
        Name: productName,
        Description: productDescription,
        CategoryId: productCategory,
        Introduction: productIntroduce,
        Status: productStatus,
        LastEditTime: product.LastEditTime
    };

    if (newProduct.Name == product.Name && newProduct.CategoryId == product.CategoryId && newProduct.Description == product.Description
        && newProduct.Introduction == product.Introduction && newProduct.Status == product.Status) {
        Swal.fire("請修改商品內容");
        return;
    }

    axios.post("/MainProduct/EditProduct", { product: newProduct })
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
            window.location.href = `/MainProduct/ProductDetail?productId=${product.ProductId}`;
        })
        .catch(error => {
            console.error("fail", error);
        });

}

function submitEditImage() {
    const displayImageContainer = document.getElementById("displayImageContainer");
    const imageElements = displayImageContainer.querySelectorAll("img");
    let imageList = [];

    if (imageElements.length != 0) {
        for (var i = 0; i < imageElements.length; i++) {
            if (imageElements[i].dataset.id == " ") {
                let image = {
                    ImageName: imageElements[i].id,
                    ImageUrl: imageElements[i].src
                };
                imageList.push(image);
            }
        }
    }

    if (imageList.length == 0 && delImageList.length == 0) {
        Swal.fire("請修改圖片內容");
        return;
    }

    const formData = new FormData();

    formData.append('ProductId', product.ProductId);
    formData.append('LastEditTime', product.LastEditTime);
    formData.append('DelImageList', JSON.stringify(delImageList));

    imageList.forEach(img => {
        const imgBlob = dataURItoBlob(img.ImageUrl);
        formData.append('images[]', imgBlob, `${img.ImageName}`);
    });

    axios.post("/MainProduct/EditProductImage", formData, {
        headers: {
            'Content-Type': 'multipart/form-data'
        }
    })
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
            window.location.href = `/MainProduct/ProductDetail?productId=${product.ProductId}`;
        })
        .catch(error => {
            console.error("fail", error);
        });
}

function dataURItoBlob(dataURI) {
    var byteString = atob(dataURI.split(',')[1]);
    var arrayBuffer = new ArrayBuffer(byteString.length);
    var uintArray = new Uint8Array(arrayBuffer);

    for (var i = 0; i < byteString.length; i++) {
        uintArray[i] = byteString.charCodeAt(i);
    }

    return new Blob([uintArray], { type: 'image/jpeg' });
}

