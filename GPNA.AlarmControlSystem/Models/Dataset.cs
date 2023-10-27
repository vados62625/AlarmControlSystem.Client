namespace GPNA.AlarmControlSystem.Models;

public class Dataset
{
    public PointChart[]? Data { get; set; }
    public string BackgroundColor { get; set; } = "#0078D2";
    public string BorderColor { get; set; } = "#0078D2";
    public string BorderWidth { get; set; } = "1";
    public string PointRadius { get; set; } = "0";
    public string? Label { get; set; }
    public bool Normalized { get; set; } = true;
    public bool Stepped { get; set; } = false;
    // public string? YAxisID { get; set; }
    public int[] BorderDash = new[] { 0, 0 };

}