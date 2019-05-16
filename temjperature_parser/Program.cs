  using System;
 using System.Collections.Generic;
 using System.Linq;
  using System.Net;
  using System.Runtime.InteropServices;
  using System.Text;
  using System.Text.RegularExpressions;
  using System.Threading;
  using System.Threading.Tasks;
  using HtmlAgilityPack;
  using MySql.Data.MySqlClient;

namespace temjperature_parser
{
    class Program
    {
        static void Main(string[] args)
        {
         
            while (true)
            {
                try
                {
                    string connStr = "server=localhost;user=root;database=xesko;password=;"; // строка подключения к БД
                    MySqlConnection conn = new MySqlConnection(connStr); // создаём объект для подключения к БД
                    // MySQl
                    var html = @"http://meteo.gov.ua/ua/33902/forecast/ukraine/33345";
                    HtmlWeb web = new HtmlWeb();
                    var htmlDoc = web.Load(html);
                    var title = htmlDoc.DocumentNode.SelectSingleNode("//head/title"); //получим тайтл
                    HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'hdr_fr_bl2')]"); //выцепим класс
                    if (nodes != null && title != null)
                    {
                        var input = nodes[0].InnerHtml;
                        string pattern ="curWeatherT\">(.*?)</span"; // <span id=\"curWeatherT\">13.1</span>    отлавливаем
                        MatchCollection matches = Regex.Matches(input, pattern);
                        string sqlDateFormat = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        string city = "Херсон";
                        if (title.OuterHtml != "<title> Український Гідрометцентр. Прогноз погоди Херсон</title>")
                        {
                            city = title.OuterHtml;
                        }
                        conn.Open();     // устанавливаем соединение с БД
                        string sql = "INSERT INTO `wather` (`date`,`title`,`value`) VALUE ('"+ sqlDateFormat + "','"+ city + "','"+ matches[0].Groups[1].Value + "');";  // запрос
                        MySqlCommand command = new MySqlCommand(sql, conn);  // объект для выполнения SQL-запроса
                        command.ExecuteNonQuery();            // выполняем запрос 
                        conn.Clone(); // закрываем соединение с БД
                        Console.WriteLine("Успешно занесли в базу показания температуры " + sqlDateFormat + " : " + city + " : " + matches[0].Groups[1].Value + " Градусов.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                //    throw;
                }
                Console.WriteLine("Ждем 3 часа  до сделуещей проверки теммпературы");
                Thread.Sleep(1000*60*60*3);// 1 час
            }
        }
    }
}
