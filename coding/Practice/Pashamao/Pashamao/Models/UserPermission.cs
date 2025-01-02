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
        EditProduct = 32768,
        DelProduct = 65536,//此為第16個 最大64
    }
}