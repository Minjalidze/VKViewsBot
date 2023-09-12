using System.Net;
using TwoCaptcha.Captcha;

namespace VKViewsBot
{
    internal class RuCaptcha : VkNet.Utils.AntiCaptcha.ICaptchaSolver
    {
		const string CaptchaCode = "***YOUR RuCaptcha CODE***"
		
        public string Solve(string url)
        {
            var solver = new TwoCaptcha.TwoCaptcha(CaptchaCode);

            var web = new WebClient();
            var bytes = web.DownloadData(url);
			
            if (!Directory.Exists("captcha")) Directory.CreateDirectory("captcha");
            File.WriteAllBytes("captcha\\img.jpg", bytes);

            var captcha = new Normal("captcha\\img.jpg");
            try
            {
                solver.Solve(captcha).Wait();
                Console.WriteLine("[VKViewsBot] - Captcha successfully solved!\n");
                return captcha.Code;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return string.Empty;
            }
        }

        public void CaptchaIsFalse()
        {
            throw new NotImplementedException();
        }
    }
}
