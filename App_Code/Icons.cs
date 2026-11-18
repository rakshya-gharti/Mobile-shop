public static class Icons
{
    // Lucide "star" icons (symbols are defined in Site.master). Filled = rated, outline = empty.
    public static string Stars(int full)
    {
        if (full < 0) full = 0; if (full > 5) full = 5;
        var sb = new System.Text.StringBuilder();
        for (int i = 0; i < 5; i++)
            sb.Append("<svg class=\"ic" + (i < full ? " fill" : "") + "\" aria-hidden=\"true\"><use href=\"#i-star\"/></svg>");
        return sb.ToString();
    }
}
