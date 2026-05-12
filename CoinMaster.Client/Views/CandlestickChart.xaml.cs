using CoinMaster.Core.Entities;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CoinMaster.Client.Views;

public partial class CandlestickChart : UserControl
{

    public static readonly DependencyProperty CandlesProperty =
        DependencyProperty.Register(
            nameof(Candles),
            typeof(IEnumerable<Candle>),
            typeof(CandlestickChart),
            new FrameworkPropertyMetadata(null, OnCandlesChanged));

    public IEnumerable<Candle> Candles
    {
        get => (IEnumerable<Candle>)GetValue(CandlesProperty);
        set => SetValue(CandlesProperty, value);
    }

    private static void OnCandlesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        => ((CandlestickChart)d).Redraw();

    private const double PadTop = 8;

    private const double PadBottom = 8;

    private const double PadLeft = 4;

    private const double PadRight = 4;

    private const int GridLines = 4;

    private const int MaxXLabels = 6;

    public CandlestickChart()
    {
        InitializeComponent();
        SizeChanged += (_, _) => Redraw();
    }

    private void Redraw()
    {
        ChartCanvas.Children.Clear();
        YAxisCanvas.Children.Clear();
        XAxisCanvas.Children.Clear();

        var candles = Candles?.ToList();
        if (candles is not { Count: > 0 }) return;

        double w = ChartCanvas.ActualWidth;
        double h = ChartCanvas.ActualHeight;
        if (w <= 0 || h <= 0) return;

        var bullish = TryFindResource("BullishBrush") as Brush ?? Brushes.Green;
        var bearish = TryFindResource("BearishBrush") as Brush ?? Brushes.Red;
        var gridBrush = TryFindResource("ChartGridBrush") as Brush ?? Brushes.LightGray;
        var textBrush = TryFindResource("SecondaryTextBrush") as Brush ?? Brushes.Gray;

        decimal minLow = candles.Min(c => c.Low);
        decimal maxHigh = candles.Max(c => c.High);
        decimal range = maxHigh - minLow;
        if (range == 0) range = 1;

        decimal margin = range * 0.05m;
        decimal yMin = minLow - margin;
        decimal yMax = maxHigh + margin;
        decimal yRange = yMax - yMin;

        double ToY(decimal price) =>
            PadTop + (double)((yMax - price) / yRange) * (h - PadTop - PadBottom);

        for (int i = 0; i <= GridLines; i++)
        {
            decimal price = yMin + yRange * i / GridLines;
            double y = ToY(price);

            var line = new Line
            {
                X1 = 0,
                X2 = w,
                Y1 = y,
                Y2 = y,
                Stroke = gridBrush,
                StrokeThickness = 1,
                StrokeDashArray = new DoubleCollection([4, 4])
            };
            ChartCanvas.Children.Add(line);

            var tb = MakeLabel(FormatPrice(price), textBrush, 10);
            tb.Width = 54;
            tb.TextAlignment = TextAlignment.Right;
            Canvas.SetRight(tb, 4);
            Canvas.SetTop(tb, y - 8);
            YAxisCanvas.Children.Add(tb);
        }

        int count = candles.Count;
        double slotW = (w - PadLeft - PadRight) / count;
        double bodyW = Math.Max(1.5, slotW * 0.55);
        double wickW = Math.Max(0.5, slotW * 0.12);

        for (int i = 0; i < count; i++)
        {
            var c = candles[i];
            Brush brush = c.IsBullish ? bullish : bearish;
            double xCenter = PadLeft + i * slotW + slotW / 2;

            ChartCanvas.Children.Add(new Line
            {
                X1 = xCenter,
                X2 = xCenter,
                Y1 = ToY(c.High),
                Y2 = ToY(c.Low),
                Stroke = brush,
                StrokeThickness = wickW
            });

            double bodyTop = ToY(Math.Max(c.Open, c.Close));
            double bodyBottom = ToY(Math.Min(c.Open, c.Close));
            double bodyHeight = Math.Max(1, bodyBottom - bodyTop);

            var rect = new Rectangle
            {
                Width = bodyW,
                Height = bodyHeight,
                Fill = brush
            };
            Canvas.SetLeft(rect, xCenter - bodyW / 2);
            Canvas.SetTop(rect, bodyTop);
            ChartCanvas.Children.Add(rect);
        }

        DrawXLabels(candles, slotW, textBrush);
    }

    private void DrawXLabels(List<Candle> candles, double slotW, Brush textBrush)
    {
        int count = candles.Count;
        int step = Math.Max(1, count / MaxXLabels);

        for (int i = 0; i < count; i += step)
        {
            double x = i * slotW + slotW / 2 + PadLeft;
            var tb = MakeLabel(candles[i].Timestamp.ToString("dd.MM"), textBrush, 10);
            tb.Width = 40;
            tb.TextAlignment = TextAlignment.Center;
            Canvas.SetLeft(tb, x - 20);
            Canvas.SetTop(tb, 2);
            XAxisCanvas.Children.Add(tb);
        }
    }

    private static TextBlock MakeLabel(string text, Brush foreground, double fontSize) =>
        new()
        {
            Text = text,
            Foreground = foreground,
            FontSize = fontSize
        };

    private static string FormatPrice(decimal price) =>
        price >= 1000 ? price.ToString("N0", CultureInfo.InvariantCulture) :
        price >= 1 ? price.ToString("N2", CultureInfo.InvariantCulture) :
        price >= 0.01m ? price.ToString("N4", CultureInfo.InvariantCulture) :
                         price.ToString("N6", CultureInfo.InvariantCulture);
}