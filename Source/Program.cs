using Microsoft.Extensions.Logging.Abstractions;

using VkNet;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VKViewsBot;
using Timer = System.Timers.Timer;

Console.WriteLine("[VKViewsBot] - Author: Minjalidze");
Console.WriteLine();
Console.WriteLine("|---------------------------------|");
Console.WriteLine();

const int UpdateInterval = 60; //интервал обновления поста в секундах

const string AuthToken = "***YOUR TOKEN***"; //Токен авторизации ВК
const string WallText = $"⭐ Пост волшебства!\n\n🔥 Обновляй страницу и смотри на магию каждую минуту!"; //Итоговый текст поста, который должен вставляться после его обновления
const string CodePhrase = "Просмотров:"; //Кодовая фраза, для поиска поста, который необходимо обновлять

var timer = new Timer { Interval = UpdateInterval * 1000 };
timer.Elapsed += (_, _) =>
{
    var vkApi = new VkApi(new NullLogger<VkApi>(), new RuCaptcha());

    Console.WriteLine("[VKViewsBot] - Authorization...");
    vkApi.Authorize(new ApiAuthParams { AccessToken = AuthToken });

    Console.WriteLine("[VKViewsBot] - Get 5 last Wall posts...");
    var wallApi = vkApi.Wall.Get(new WallGetParams { Count = 5 });
    var wallPost = wallApi.WallPosts.FirstOrDefault(wallPost => wallPost.Text.ToLower().Contains(CodePhrase));

    Console.WriteLine("[VKViewsBot] - Editing the post...");
    vkApi.Wall.Edit(new WallEditParams
    {
        PostId = (long)wallPost!.Id!,
        Message = $"{WallText}\n\n⚡ Просмотров: {wallPost.Views.Count}"
    });

    Console.WriteLine("[VKViewsBot] - LogOut...");
    vkApi.LogOut();
};
timer.Start();

Console.ReadLine();