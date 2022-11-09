using SkiaSharp;
using ZXing;
using ZXing.Common;
using ZXing.SkiaSharp;

Console.WriteLine("ZXing.Net CODE 128 faulty ResultPoints");

if (!File.Exists("header.jpeg"))
{
    throw new FileNotFoundException();
}

var barcode = SKBitmap.Decode("header.jpeg");

using var rectangleColor = new SKPaint()
{
    Color = new SKColor(255, 0, 0),
    StrokeWidth = 10,
    Style = SKPaintStyle.Stroke,
};
using var textColor = new SKPaint()
{
    Color = new SKColor(255, 0, 0),
    TextSize = 64,
};

var reader = new BarcodeReader()
{
    Options = new DecodingOptions()
    {
        TryHarder = true,
        PossibleFormats = new List<BarcodeFormat>
        {
            BarcodeFormat.CODE_128
        }
    },
    AutoRotate = true,
};

var result = reader.Decode(barcode);
if (result != null)
{
    Console.WriteLine("Found a CODE 128 1D barcode.");

    using var outputImage = barcode.Copy();
    using var canvas = new SKCanvas(outputImage);

    var coords = string.Join(", ", result.ResultPoints.Select(x => $"({x.X}, {x.Y})"));

    var firstPoint = new SKPoint(result.ResultPoints.First().X, result.ResultPoints.First().Y - 100);
    var lastPoint = new SKPoint(result.ResultPoints.Last().X, result.ResultPoints.Last().Y + 100);

    canvas.DrawRect(firstPoint.X, firstPoint.Y, (lastPoint.X - firstPoint.X) + 50, 100, rectangleColor);
    canvas.DrawText(result.Text, new SKPoint(firstPoint.X, firstPoint.Y - 25), textColor);
    
    var data = outputImage.Encode(SKEncodedImageFormat.Jpeg, 100);
    File.WriteAllBytes("output.jpeg", data.AsSpan().ToArray());   
}
