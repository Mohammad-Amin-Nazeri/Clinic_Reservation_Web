using System.Drawing;

namespace Clinic.Mvc.Extensions
{
    public static class CaptchaService
    {
        public static byte[] GenerateCaptchaImage(out string captchaText)
        {
            // Create random text
            const string chars = "abcdefghijklmnopqrstuvwxyz23456789";
            var rand = new Random();
            captchaText = new string(Enumerable
                .Range(0, 5)
                .Select(x => chars[rand.Next(chars.Length)])
                .ToArray());

            // Create image
            using var bmp = new Bitmap(130, 50);
            using var graphics = Graphics.FromImage(bmp);
            graphics.Clear(Color.White);

            // Draw text
            using (var font = new Font("Arial", 24, FontStyle.Bold))
            using (var brush = new SolidBrush(Color.Black))
            {
                graphics.DrawString(captchaText, font, brush, 10, 5);
            }

            // Add noise
            for (int i = 0; i < 10; i++)
            {
                graphics.DrawLine(Pens.Gray, rand.Next(130), rand.Next(50), rand.Next(130), rand.Next(50));
            }

            using var ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }
    }
}
