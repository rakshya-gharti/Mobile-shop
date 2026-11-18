using System;

public partial class Controls_StarRating : System.Web.UI.UserControl
{
    public double Value { get; set; }
    public int Count { get; set; }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        int full = (int)Math.Round(Value);
        litStars.Text = Icons.Stars(full);
        litText.Text = Count > 0 ? Value.ToString("0.0") + " (" + Count + ")" : "No ratings";
    }
}
