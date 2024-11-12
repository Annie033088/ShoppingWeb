using System;
using System.Configuration;
using System.Web.UI;

/// <summary>
/// BasePage 用於基本的共用項目配置
/// 例如【連線字串】或是【公用方法與屬性】或是【事件】
/// 這邊會有兩個固定的【屬性】與一個固定的【事件】 一個是取得連線字串 另一個是取得使用者資訊物件
/// </summary>
public class BasePage : Page
{
    /// <summary>
    /// 取得連線字串
    /// </summary>
    protected string ConnStr
    {
        get { return ConfigurationManager.ConnectionStrings["ConnStr"].ConnectionString; }
    }

    private UserInfo userInfo;
    /// <summary>
    /// 取得使用者資訊物件
    /// </summary>
    protected UserInfo UserInfo
    {
        get { return this.userInfo; }
    }

    /// <summary>
    /// 建構子
    /// </summary>
	public BasePage()
	{
        //添加初始事件
        this.Init += new EventHandler(BasePage_Init);
	}

    //頁面初始事件
    private void BasePage_Init(object sender, EventArgs e)
    {
        //這邊通常會有會員登入狀態的判斷

        //或者是語系檔判斷

        //或是其它的判斷
    }
}