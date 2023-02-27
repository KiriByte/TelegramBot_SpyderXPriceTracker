// See https://aka.ms/new-console-template for more information

using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;


string url = "https://spyderx.datacolor.com/shop-products/display-calibration/";
string? token = Environment.GetEnvironmentVariable("TELEGRAM_TOKEN");

var document = new HtmlDocument();
document.LoadHtml(GetHtmlString());

float proPrice = ParseProPrice(document);
float elitePrice = ParseElitePrice(document);

var sb = new StringBuilder();
sb.AppendLine("🔥SpyderX🔥")
    .AppendLine(DateTime.Now.Date.ToString())
    .AppendLine()
    .AppendLine($"SpyderX Pro = {proPrice}€")
    .AppendLine($"SpyderX Elite = {elitePrice}€");
Console.WriteLine(sb);

TelegramBotClient? botClient = null;
if (token != "")
{
    botClient = new TelegramBotClient(token);
}
else
{
    Console.WriteLine("Token is invalid");
    Environment.Exit(0);
}


await botClient.SendTextMessageAsync(
    chatId: "@Kiribyte_channel",
    text: sb.ToString(),
    replyMarkup: new InlineKeyboardMarkup(
        InlineKeyboardButton.WithUrl(
            text: "Check sendMessage method",
            url: url)));


string GetHtmlString()
{
    var chromeOptions = new ChromeOptions();
    chromeOptions.AddArguments("--no-sandbox","headless");
    var driver = new ChromeDriver(chromeOptions);
    driver.Navigate().GoToUrl(url);
    string htmlString = driver.PageSource;
    driver.Close();
    return htmlString;
}

float ParseProPrice(HtmlDocument doc)
{
    Regex regex = new Regex(@"\d+.\d+");

    var collection = doc.DocumentNode.SelectNodes("//div[@class='product-price product-price-sxp100']//p");
    MatchCollection matches = regex.Matches(collection[0].InnerText);

    return matches.Count > 0 ? float.Parse(matches[0].Value, CultureInfo.InvariantCulture) : 0;
}

float ParseElitePrice(HtmlDocument doc)
{
    Regex regex = new Regex(@"\d+.\d+");

    var collection = doc.DocumentNode.SelectNodes("//div[@class='product-price product-price-sxe100']//p");
    MatchCollection matches = regex.Matches(collection[0].InnerText);

    return matches.Count > 0 ? float.Parse(matches[0].Value, CultureInfo.InvariantCulture) : 0;
}