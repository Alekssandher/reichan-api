 using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

[Route("api/[controller]")]
[ApiController]

public class CaptchaController : ControllerBase {
    // private readonly ICaptchaService _captchaService;

    // public CaptchaController(ICaptchaService captchaService)
    // {
    //     _captchaService = captchaService;
    // }

    [HttpGet("get")]
    public async Task<IActionResult> GetCaptcha() {

        if (HttpContext.Session == null)
        {
            throw new InvalidOperationException("Session is not available.");
        }

        string captchaText = GenerateRandomText(5);
        Console.WriteLine(captchaText);
        byte[] captchaImage = GenerateCaptchaImage(captchaText);
        
       
        HttpContext.Session.SetString("CaptchaCode", captchaText);

        return File(captchaImage, "image/png");
    }
    
    public static byte[] GenerateCaptchaImage(string captchaText)
    {
        using Image<Rgba32> image = new(180, 60);
        image.Mutate(ctx =>
        {
            ctx.Fill(Color.Black);

            AddNoise(ctx, image.Width, image.Height);

            AddRandomLines(ctx, image.Width, image.Height);

            FontCollection fontCollection = new();
            
            
            string fontPath = "wwwroot/fonts/";
            if (Directory.Exists(fontPath))
            {
                foreach (string file in Directory.GetFiles(fontPath, "*.ttf"))
                {
                    fontCollection.Add(file);
                }

            } else fontCollection.AddSystemFonts();

            List<FontFamily> fonts = fontCollection.Families.ToList();
            Random random = new();
            FontFamily selectedFont = fonts[random.Next(fonts.Count)];
            Font font = new(selectedFont, 28, FontStyle.Bold);

            for (int i = 0; i < captchaText.Length; i++)
            {
                float x = 20 + (i * 30);
                float y = random.Next(10, 30); 
                float rotation = random.Next(-20, 20); 

                ctx.DrawText(captchaText[i].ToString(), font, Color.DarkBlue, new PointF(x, y));
            }
        });

        using MemoryStream memoryStream = new();
        image.SaveAsPng(memoryStream);
        return memoryStream.ToArray();
    }

    private static void AddNoise(IImageProcessingContext ctx, int width, int height)
    {
        Random random = new();
        for (int i = 0; i < 500; i++)
        {
            int x = random.Next(width);
            int y = random.Next(height);
            ctx.Fill(Color.Gray, new Rectangle(x, y, 1, 1));
        }
    }

    private static void AddRandomLines(IImageProcessingContext ctx, int width, int height)
    {
        Random random = new();
        for (int i = 0; i < 5; i++)
        {
            PointF[] points =
            [
                new PointF(random.Next(width), random.Next(height)),
                new PointF(random.Next(width), random.Next(height))
            ];

            ctx.Draw(Color.Gray, 2, new PathBuilder().AddLines(points).Build());
        }
    }

    private static string GenerateRandomText(int length)
    {
        const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz23456789";
        char[] result = new char[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = chars[RandomNumberGenerator.GetInt32(chars.Length)];
        }
        return new string(result);
    }
}