/// <summary>
/// UserInfo 的摘要说明
/// 用於記錄使用者資訊的地方，例如[使用者代號][使用者帳號][使用者性名][使用者SessionID][登入IP]等等...
/// </summary>
public class UserInfo
{
	public UserInfo(){}

    #region 全域變數

    private int iUid;           //使用者代號
    private string sAccount;    //使用者帳號
    private string sName;       //使用者姓名 
    private string sSessionID;  //使用者登入的SessionID
    private string sLoginIp;    //使用者登入IP

    #endregion

    #region 屬性

    /// <summary>
    /// 取得或設定使用者代號
    /// </summary>
    public int UID { set { this.iUid = value; } get { return this.iUid; } }

    /// <summary>
    /// 取得或設定使用者帳號
    /// </summary>
    public string Account { set { this.sAccount = value; } get { return this.sAccount; } }

    /// <summary>
    /// 取得或設定使用者名稱
    /// </summary>
    public string Name { set { this.sName = value; } get { return this.sName; } }

    /// <summary>
    /// 取得或設定SessionID
    /// </summary>
    public string SessionID { set { this.sSessionID = value; } get { return this.sSessionID; } }

    /// <summary>
    /// 取得或設定使用者登入IP
    /// </summary>
    public string LoginIp { set { this.sLoginIp = value; } get { return this.sLoginIp; } }

    #endregion
}