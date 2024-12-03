namespace Pashamao.Models
{
    public enum UserPermission : long
    {
        None = 0,
        SelectUser = 1,
        CreateUser = 2,
        EditUser = 4,
        DelUser = 8,
        SelectMember = 16,
        CreateMember = 32,
        EditMemberAddress = 64,
        EditMemberPhone = 128,
        EditMemberEmail = 256,
        EditMemberLevel = 512,
        SelectOrder = 1024,
        EditOrderPrice = 2048,
        EditOrderStatus = 4096,
        SelectProduct = 8192,
        CreateProduct = 16384,
        EditProductName = 32768,
        EditProductDescription = 65536,
        EditProductPrice = 131072,
        EditProductQuantity = 262144,
        EditProductCategory = 524288,
        EditProductStatus = 1048576,
        DelProduct = 2097152,
        SelectCategory = 4194304,
        EditCategory = 8388608 //此為第24個 最大64
    }
}