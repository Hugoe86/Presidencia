<%@ WebHandler Language="C#" Class="Mantenedor_Session" %>

using System;
using System.Web;

public class Mantenedor_Session : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.Cache.SetNoStore();
        context.Response.ContentType = "application/x-javascript";
        context.Response.Write("//");
    }
 
    public bool IsReusable {
        get {
            return true;
        }
    }

}