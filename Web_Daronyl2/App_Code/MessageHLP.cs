using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.UI;

/// <summary>
/// Summary description for MessageHLP
/// </summary>
public class MessageHLP
{
    public MessageHLP()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static void ShowPopUpMsg(string msg, Page objPage)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("alert('");
        sb.Append(msg.Replace("\n", "\\n").Replace("\r", "").Replace("'", "\\'"));
        sb.Append("');");
        System.Web.UI.ScriptManager.RegisterStartupScript(objPage, objPage.GetType(), "showalert", sb.ToString(), true);
    }
}