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
        EditMemberPersonalData = 64,
        EditMemberLevelAndStatus = 128,
        SelectOrder = 256,
        EditOrder = 512,
        DelOrder = 1024,
        SelectProduct = 2048,
        CreateProduct = 4096,
        EditProduct = 8192,
        DelProduct = 16384,
        ResetMemberPoints = 32768,
        EditShippingFee = 65536//再不包含None的情況下, 此為第17個, 最多可儲存63個權限
    }
}