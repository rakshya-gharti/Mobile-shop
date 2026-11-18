using System;
using System.Configuration;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>Data-access and helper methods shared by every page.</summary>
public static class Db
{
    public static string ConnStr
    {
        get
        {
            string dir = HttpContext.Current != null ? HttpContext.Current.Server.MapPath("~/App_Data") : AppDomain.CurrentDomain.BaseDirectory;
            return "Data Source=" + Path.Combine(dir, "ShopDB.sqlite") + ";Version=3;foreign keys=True";
        }
    }

    /// <summary>Converts the few T-SQL constructs used in this project into SQLite syntax.</summary>
    public static string Translate(string sql)
    {
        sql = Regex.Replace(sql, @"\bISNULL\s*\(", "IFNULL(", RegexOptions.IgnoreCase);
        sql = Regex.Replace(sql, @"\bNEWID\(\)", "RANDOM()", RegexOptions.IgnoreCase);
        sql = Regex.Replace(sql, @"\bSCOPE_IDENTITY\(\)", "last_insert_rowid()", RegexOptions.IgnoreCase);
        sql = Regex.Replace(sql, @"\bGETDATE\(\)", "CURRENT_TIMESTAMP", RegexOptions.IgnoreCase);
        Match m = Regex.Match(sql, @"^(\s*SELECT)\s+TOP\s+(\d+)\s", RegexOptions.IgnoreCase);
        if (m.Success)
            sql = m.Groups[1].Value + " " + sql.Substring(m.Length) + " LIMIT " + m.Groups[2].Value;
        return sql;
    }

    public static SqliteParameter P(string name, object value)
    {
        return new SqliteParameter(name, value ?? DBNull.Value);
    }

    public static SqliteConnection Open()
    {
        SqliteConnection c = new SqliteConnection(ConnStr);
        c.Open();
        using (SqliteCommand pr = new SqliteCommand("PRAGMA foreign_keys=ON", c)) pr.ExecuteNonQuery();
        return c;
    }

    private static SqliteCommand Cmd(SqliteConnection c, string sql, SqliteParameter[] ps)
    {
        SqliteCommand cmd = new SqliteCommand(Translate(sql), c);
        cmd.Parameters.AddRange(ps);
        return cmd;
    }

    // SQLite returns Int64 for COUNT/SUM; the pages expect Int32.
    private static object Norm(object o)
    {
        if (o is long && (long)o >= int.MinValue && (long)o <= int.MaxValue) return (int)(long)o;
        return o;
    }

    public static DataTable Table(string sql, params SqliteParameter[] ps)
    {
        using (SqliteConnection c = Open())
        using (SqliteCommand cmd = Cmd(c, sql, ps))
        using (SqliteDataReader rd = cmd.ExecuteReader())
        {
            DataTable dt = new DataTable();
            object[][] rows = null;
            System.Collections.Generic.List<object[]> list = new System.Collections.Generic.List<object[]>();
            int n = rd.FieldCount;
            while (rd.Read())
            {
                object[] row = new object[n];
                for (int i = 0; i < n; i++) row[i] = Norm(rd.IsDBNull(i) ? DBNull.Value : rd.GetValue(i));
                list.Add(row);
            }
            rows = list.ToArray();
            for (int i = 0; i < n; i++)
            {
                Type t = null;
                foreach (object[] r in rows)
                    if (!(r[i] is DBNull)) { t = r[i].GetType(); break; }
                if (t == null) t = typeof(string);
                // keep one consistent type per column
                foreach (object[] r in rows)
                    if (!(r[i] is DBNull) && r[i].GetType() != t)
                    {
                        if (t == typeof(int) && r[i] is double) t = typeof(double);
                        else if (t == typeof(double) || t == typeof(int)) { if (r[i] is decimal) t = typeof(decimal); }
                    }
                dt.Columns.Add(rd.GetName(i), t);
            }
            foreach (object[] r in rows)
            {
                for (int i = 0; i < n; i++)
                    if (!(r[i] is DBNull) && r[i].GetType() != dt.Columns[i].DataType)
                        r[i] = Convert.ChangeType(r[i], dt.Columns[i].DataType);
                dt.Rows.Add(r);
            }
            return dt;
        }
    }

    public static int Exec(string sql, params SqliteParameter[] ps)
    {
        using (SqliteConnection c = Open())
        using (SqliteCommand cmd = Cmd(c, sql, ps))
            return cmd.ExecuteNonQuery();
    }

    public static object Scalar(string sql, params SqliteParameter[] ps)
    {
        using (SqliteConnection c = Open())
        using (SqliteCommand cmd = Cmd(c, sql, ps))
            return Norm(cmd.ExecuteScalar());
    }

    public static string Hash(string password)
    {
        using (SHA256 sha = SHA256.Create())
        {
            byte[] b = sha.ComputeHash(Encoding.UTF8.GetBytes("MWM2025$" + password));
            StringBuilder sb = new StringBuilder();
            foreach (byte x in b) sb.Append(x.ToString("x2"));
            return sb.ToString();
        }
    }

    /// <summary>Returns the logged-in user's id (0 if anonymous).</summary>
    public static int CurrentUserId()
    {
        if (!HttpContext.Current.Request.IsAuthenticated) return 0;
        object o = Scalar("SELECT UserId FROM Users WHERE Username=@u", P("@u", HttpContext.Current.User.Identity.Name));
        return o == null ? 0 : Convert.ToInt32(o);
    }

    public static void Notify(int userId, string message)
    {
        Exec("INSERT INTO Notifications (UserId, Message) VALUES (@u,@m)", P("@u", userId), P("@m", message));
    }

    /// <summary>"Sends" an email by saving it as an .eml file in App_Data/Mail (no SMTP server needed). Failures are ignored.</summary>
    public static void SendMail(string to, string subject, string body)
    {
        try
        {
            string dir = HttpContext.Current.Server.MapPath("~/App_Data/Mail");
            Directory.CreateDirectory(dir);
            string from = ConfigurationManager.AppSettings["MailFrom"];
            string file = Path.Combine(dir, DateTime.Now.ToString("yyyyMMdd_HHmmss_fff") + "_" + Guid.NewGuid().ToString("N").Substring(0, 6) + ".eml");
            File.WriteAllText(file, "From: " + from + "\r\nTo: " + to + "\r\nSubject: " + subject.Replace("\r", " ").Replace("\n", " ") +
                "\r\nDate: " + DateTime.Now.ToString("r") + "\r\nContent-Type: text/plain; charset=utf-8\r\n\r\n" + body + "\r\n");
        }
        catch { }
    }

    /// <summary>Creates ShopDB.sqlite in App_Data (if missing) and runs Schema.sql + seed users.</summary>
    public static void EnsureDatabase()
    {
        string dataDir = HttpContext.Current != null ? HttpContext.Current.Server.MapPath("~/App_Data") : AppDomain.CurrentDomain.BaseDirectory;
        string db = Path.Combine(dataDir, "ShopDB.sqlite");
        if (File.Exists(db) && new FileInfo(db).Length > 0)
        {
            object t = Scalar("SELECT COUNT(*) FROM sqlite_master WHERE name='Users'");
            if (Convert.ToInt32(t) > 0) return;
        }

        string script = File.ReadAllText(Path.Combine(dataDir, "Schema.sql"));
        string[] batches = Regex.Split(script, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase);
        foreach (string b in batches)
            if (b.Trim().Length > 0) Exec(b);

        Exec("INSERT INTO Users (Username,PasswordHash,FullName,Email,Phone,Address,Role) VALUES (@u,@p,@n,@e,'0123456789','APU, Kuala Lumpur','Admin')",
             P("@u", "admin"), P("@p", Hash("Admin@123")), P("@n", "Site Administrator"), P("@e", "admin@shopease.local"));
        Exec("INSERT INTO Users (Username,PasswordHash,FullName,Email,Phone,Address,Role) VALUES (@u,@p,@n,@e,'0129876543','Bukit Jalil, Kuala Lumpur','Member')",
             P("@u", "member"), P("@p", Hash("Member@123")), P("@n", "Demo Member"), P("@e", "member@shopease.local"));
    }
}
