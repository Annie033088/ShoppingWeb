using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;



public partial class ConnectionDB : BasePage //這邊固定要繼承BasePage來取得公用的方法屬性以及事件
{
    //頁面載入事件


    //DataTable
    //通常用於傳回一個資料表的處理，速度比DataReader慢，但因為是讀回來存放在網站記憶體，所以使用上可以較靈活
    private void DataTableExample()
    {
        SqlCommand cmd = new SqlCommand(); //宣告SqlCommand物件
        cmd.Connection = new SqlConnection(this.ConnStr); //設定連線字串
        SqlDataAdapter da = new SqlDataAdapter(); //宣告一個配接器(DataTable與DataSet必須)
        DataTable dt = new DataTable(); //宣告DataTable物件

        //以下try catch finally為固定寫法，且釋放資源必定要寫在finally裡
        //偵錯
        try
        {
            //使用SqlCommand物件的CommandText屬性設置SQL命令
            cmd.CommandText = "SELECT xxx FROM xxx WHERE xxx = @xxx .....";
            //這是呼叫SP的方式，與上面的其實大同小異，可以當做是直接在SQL的查詢工具那邊下命令
            cmd.CommandText = "EXEC pro_XXXXX_XXX @xxx";

            //參數化查詢條件，可以有效防止SQL injection(資料隱碼攻擊)
            cmd.Parameters.Add("@xxx", SqlDbType.Int).Value = Request["xxx"];

            cmd.Connection.Open(); //開啟資料庫連線

            da.SelectCommand = cmd; //執行
            da.Fill(dt); //結果存放至DataTable

            cmd.Connection.Close(); //關閉連線

            //開始讀取
            for (int i = 0; i < dt.Rows.Count; i++) 
            {
                //這邊可以開始處理取回來的操作
                string aa = dt.Rows[i]["xxx"].ToString();
            }
        }
        //錯誤捕獲區塊
        catch (Exception ex) //ex用於除錯時方便查看用 ，正式上線時請務必不要直接顯示ex於畫面上
        {
            Response.Write(ex.Message);
        }
        //最終無論如何 必定會執行的區塊　用來放置釋放或關閉資源
        finally
        {
            cmd.Parameters.Clear();
            //判斷是否已關閉
            if (cmd.Connection.State != ConnectionState.Closed)
                cmd.Connection.Close();
        }
    }

    //DataSet
    //通常用於傳回多個資料表的處理，其實就是DataTable的集合
    private void DataSetExample()
    {
        SqlCommand cmd = new SqlCommand(); //宣告SqlCommand物件
        cmd.Connection = new SqlConnection(this.ConnStr); //設定連線字串
        SqlDataAdapter da = new SqlDataAdapter(); //宣告一個配接器(DataTable與DataSet必須)
        DataSet ds = new DataSet(); //宣告DataSet物件

        //以下try catch finally為固定寫法，且釋放資源必定要寫在finally裡
        //偵錯
        try
        {
            //使用SqlCommand物件的CommandText屬性設置SQL命令
            cmd.CommandText = "SELECT xxx FROM xxx WHERE xxx = @xxx .....";
            //這是呼叫SP的方式，與上面的其實大同小異，可以當做是直接在SQL的查詢工具那邊下命令
            cmd.CommandText = "EXEC pro_XXXXX_XXX @xxx";

            //參數化查詢條件，可以有效防止SQL injection(資料隱碼攻擊)
            cmd.Parameters.Add("@xxx", SqlDbType.Int).Value = Request["xxx"];

            cmd.Connection.Open(); //開啟資料庫連線

            da.SelectCommand = cmd; //執行
            da.Fill(ds); //結果存放至DataTable

            cmd.Connection.Close(); //關閉連線

            //開始讀取
            //這裡只針對回傳的第一個表做操作
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string aa = ds.Tables[0].Rows[i]["xxx"].ToString();
            }
        }
        //錯誤捕獲區塊
        catch (Exception ex) //ex用於除錯時方便查看用 ，正式上線時請務必不要直接顯示ex於畫面上
        {
            Response.Write(ex.Message);
        }
        //最終無論如何 必定會執行的區塊　用來放置釋放或關閉資源
        finally
        {
            cmd.Parameters.Clear();
            //判斷是否已關閉
            if (cmd.Connection.State != ConnectionState.Closed)
                cmd.Connection.Close();
        }
    }

    //Execute
    //通常用於執行命令，不需要回傳資料行的做法，但會回傳受影響的筆數
    private void ExecuteExample()
    {
        SqlCommand cmd = new SqlCommand(); //宣告SqlCommand物件
        cmd.Connection = new SqlConnection(this.ConnStr); //設定連線字串

        //以下try catch finally為固定寫法，且釋放資源必定要寫在finally裡
        //偵錯
        try
        {
            //使用SqlCommand物件的CommandText屬性設置SQL命令
            cmd.CommandText = "INSERT INTO t_xxx (f_xxx) VALUES (@xxx) "+
                              "UPDATE t_xxx SET f_xxx = @xxx";
            //這是呼叫SP的方式，與上面的其實大同小異，可以當做是直接在SQL的查詢工具那邊下命令
            cmd.CommandText = "EXEC pro_XXXXX_XXX @xxx";

            //參數化查詢條件，可以有效防止SQL injection(資料隱碼攻擊)
            cmd.Parameters.Add("@xxx", SqlDbType.Int).Value = Request["xxx"];

            cmd.Connection.Open(); //開啟資料庫連線

            int iExecuteCount = cmd.ExecuteNonQuery(); //執行並回傳受影響筆數
        }
        //錯誤捕獲區塊
        catch (Exception ex) //ex用於除錯時方便查看用 ，正式上線時請務必不要直接顯示ex於畫面上
        {
            Response.Write(ex.Message);
        }
        //最終無論如何 必定會執行的區塊　用來放置釋放或關閉資源
        finally
        {
            cmd.Parameters.Clear();
            cmd.Connection.Close();
        }
    }

    //ExecuteScalar
    //通常用於執行只會回傳一個欄位的用法
    private void ScalarExample()
    {
        SqlCommand cmd = new SqlCommand(); //宣告SqlCommand物件
        cmd.Connection = new SqlConnection(this.ConnStr); //設定連線字串

        //以下try catch finally為固定寫法，且釋放資源必定要寫在finally裡
        //偵錯
        try
        {
            //使用SqlCommand物件的CommandText屬性設置SQL命令
            cmd.CommandText = "SELECT f_xxx FROM t_xxx WHERE f_xxx = @xxx";
            //這是呼叫SP的方式，與上面的其實大同小異，可以當做是直接在SQL的查詢工具那邊下命令
            cmd.CommandText = "EXEC pro_XXXXX_XXX @xxx";

            //參數化查詢條件，可以有效防止SQL injection(資料隱碼攻擊)
            cmd.Parameters.Add("@xxx", SqlDbType.Int).Value = Request["xxx"];

            cmd.Connection.Open(); //開啟資料庫連線

            string aa = cmd.ExecuteScalar().ToString(); //執行並取回資料
        }
        //錯誤捕獲區塊
        catch (Exception ex) //ex用於除錯時方便查看用 ，正式上線時請務必不要直接顯示ex於畫面上
        {
            Response.Write(ex.Message);
        }
        //最終無論如何 必定會執行的區塊　用來放置釋放或關閉資源
        finally
        {
            cmd.Parameters.Clear();
            cmd.Connection.Close();
        }
    }
}