// Minimal ASP.NET host for Mono (no xsp needed). Build: see run.sh
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Hosting;

public class AppHost : MarshalByRefObject
{
    HttpListener listener;
    public override object InitializeLifetimeService() { return null; }

    public bool Ping() { return listener != null && listener.IsListening; }

    public void Start(int port)
    {
        listener = new HttpListener();
        listener.Prefixes.Add("http://*:" + port + "/");
        listener.Start();
        new Thread(Loop) { IsBackground = false }.Start();
    }

    void Loop()
    {
        while (true)
        {
            HttpListenerContext ctx;
            try { ctx = listener.GetContext(); } catch (Exception) { return; }
            ThreadPool.QueueUserWorkItem(delegate { Handle(ctx); });
        }
    }

    void Handle(HttpListenerContext ctx)
    {
        try { HttpRuntime.ProcessRequest(new Worker(ctx)); }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
            try { ctx.Response.StatusCode = 500; ctx.Response.Close(); } catch { }
        }
    }
}

public class Worker : HttpWorkerRequest
{
    HttpListenerContext ctx;
    HttpListenerRequest req;
    HttpListenerResponse res;
    MemoryStream body = new MemoryStream();
    byte[] entity;
    bool done;

    public Worker(HttpListenerContext c)
    {
        ctx = c; req = c.Request; res = c.Response;
        if (req.HasEntityBody)
        {
            MemoryStream ms = new MemoryStream();
            req.InputStream.CopyTo(ms);
            entity = ms.ToArray();
        }
    }

    public override string GetUriPath() { string p = req.Url.AbsolutePath; return p.EndsWith("/") ? p + "Default.aspx" : p; }
    public override string GetQueryString() { string q = req.Url.Query; return q.StartsWith("?") ? q.Substring(1) : q; }
    public override string GetRawUrl() { return req.RawUrl; }
    public override string GetHttpVerbName() { return req.HttpMethod; }
    public override string GetHttpVersion() { return "HTTP/" + req.ProtocolVersion; }
    public override string GetRemoteAddress() { return req.RemoteEndPoint.Address.ToString(); }
    public override int GetRemotePort() { return req.RemoteEndPoint.Port; }
    public override string GetLocalAddress() { return req.LocalEndPoint.Address.ToString(); }
    public override int GetLocalPort() { return req.LocalEndPoint.Port; }
    public override string GetServerName() { return req.Url.Host; }
    public override string GetAppPath() { return HttpRuntime.AppDomainAppVirtualPath; }
    public override string GetAppPathTranslated() { return HttpRuntime.AppDomainAppPath; }
    public override string GetFilePath() { return GetUriPath(); }
    public override string GetFilePathTranslated() { return MapPath(GetUriPath()); }
    public override string GetPathInfo() { return ""; }
    public override bool IsSecure() { return false; }
    public override string GetProtocol() { return "http"; }
    public override string MapPath(string virtualPath)
    {
        string root = HttpRuntime.AppDomainAppPath;
        if (string.IsNullOrEmpty(root)) root = AppDomain.CurrentDomain.BaseDirectory;
        string p = virtualPath ?? "";
        if (p.StartsWith("~")) p = p.Substring(1);
        p = p.TrimStart('/');
        return Path.Combine(root, p.Replace('/', Path.DirectorySeparatorChar));
    }

    public override string GetKnownRequestHeader(int index)
    {
        return req.Headers[GetKnownRequestHeaderName(index)];
    }
    public override string[][] GetUnknownRequestHeaders()
    {
        List<string[]> l = new List<string[]>();
        foreach (string k in req.Headers.AllKeys)
            if (GetKnownRequestHeaderIndex(k) < 0) l.Add(new string[] { k, req.Headers[k] });
        return l.ToArray();
    }
    public override byte[] GetPreloadedEntityBody() { return entity; }
    public override bool IsEntireEntityBodyIsPreloaded() { return true; }
    public override int ReadEntityBody(byte[] buffer, int size) { return 0; }

    public override void SendStatus(int statusCode, string statusDescription)
    {
        res.StatusCode = statusCode;
        res.StatusDescription = statusDescription;
    }
    public override void SendKnownResponseHeader(int index, string value)
    {
        string name = GetKnownResponseHeaderName(index);
        SendUnknownResponseHeader(name, value);
    }
    public override void SendUnknownResponseHeader(string name, string value)
    {
        switch (name.ToLowerInvariant())
        {
            case "content-length": return; // set at the end
            case "content-type": res.ContentType = value; return;
            case "location": res.RedirectLocation = value; return;
            case "transfer-encoding": case "connection": case "keep-alive": case "server": case "date": return;
            default: res.AppendHeader(name, value); return;
        }
    }
    public override void SendResponseFromMemory(byte[] data, int length) { body.Write(data, 0, length); }
    public override void SendResponseFromMemory(IntPtr data, int length)
    {
        byte[] b = new byte[length];
        System.Runtime.InteropServices.Marshal.Copy(data, b, 0, length);
        body.Write(b, 0, length);
    }
    public override void SendResponseFromFile(string filename, long offset, long length)
    {
        using (FileStream fs = File.OpenRead(filename))
        {
            if (length < 0) length = fs.Length - offset;
            fs.Seek(offset, SeekOrigin.Begin);
            byte[] buf = new byte[length];
            int read = 0;
            while (read < length) { int n = fs.Read(buf, read, (int)length - read); if (n <= 0) break; read += n; }
            body.Write(buf, 0, read);
        }
    }
    public override void SendResponseFromFile(IntPtr handle, long offset, long length) { }
    public override void FlushResponse(bool finalFlush) { }
    public override void EndOfRequest()
    {
        if (done) return;
        done = true;
        try
        {
            res.ContentLength64 = body.Length;
            if (req.HttpMethod != "HEAD") res.OutputStream.Write(body.GetBuffer(), 0, (int)body.Length);
        }
        catch { }
        try { res.Close(); } catch { }
    }
}

public class Program
{
    public static int Main(string[] args)
    {
        int port = args.Length > 1 ? int.Parse(args[1]) : 8080;
        string dir = Path.GetFullPath(args.Length > 0 ? args[0] : ".");
        bool first = true;
        while (true)
        {
            try
            {
                AppHost h = (AppHost)ApplicationHost.CreateApplicationHost(typeof(AppHost), "/", dir);
                h.Start(port);
                if (first) Console.WriteLine("GlowUp running at http://localhost:" + port + "/  (Ctrl+C to stop)");
                first = false;
                while (true) { Thread.Sleep(1000); h.Ping(); }   // throws when the app domain was unloaded (config change)
            }
            catch (Exception) { Console.WriteLine("Restarting application..."); Thread.Sleep(1500); }
        }
    }
}
