using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Yolov5Net.Scorer;
using Yolov5Net.Scorer.Models;

namespace Yolov5Net.App
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] files = Directory.GetFiles(@"C:\Users\estagio.sst17\source\repos\yolov5-net\src\Yolov5Net.App\bin\x64\Debug\net5.0\Assets\images");

            foreach(string file in files)
            {
                using var image = Image.FromFile(file);

                using var scorer = new YoloScorer<YoloEPIModel>("Assets/Weights/best.onnx");

                List<YoloPrediction> predictions = scorer.Predict(image);

                using var graphics = Graphics.FromImage(image);

                foreach (var prediction in predictions) // iterate predictions to draw results
                {
                    double score = Math.Round(prediction.Score, 2);

                    graphics.DrawRectangles(new Pen(prediction.Label.Color, 1),
                        new[] { prediction.Rectangle });

                    var (x, y) = (prediction.Rectangle.X - 3, prediction.Rectangle.Y - 23);

                    graphics.DrawString($"{prediction.Label.Name} ({score})",
                        new Font("Consolas", 16, GraphicsUnit.Pixel), new SolidBrush(prediction.Label.Color),
                        new PointF(x, y));
                }
                string[] name = file.Split('\\');

                image.Save(@"C:\Users\estagio.sst17\source\repos\yolov5-net\src\Yolov5Net.App\bin\x64\Debug\net5.0\Assets\results\" + name[^1]);
            }
        }
    }
}
