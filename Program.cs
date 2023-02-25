// See https://aka.ms/new-console-template for more information

using System.Globalization;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;

Console.WriteLine("Hello, World!");

string url = "https://spyderx.datacolor.com/shop-products/display-calibration/";


var document = new HtmlDocument();
document.LoadHtml(GetHtmlString());

float proPrice = ParseProPrice(document);
float elitePrice = ParseElitePrice(document);

var dt = DateTime.Now;
Console.WriteLine(dt);
Console.WriteLine(proPrice);
Console.WriteLine(elitePrice);


string GetHtmlString()
{
    var chromeOptions = new ChromeOptions();
    chromeOptions.AddArgument("headless");
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