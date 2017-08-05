using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Cactb.Bot.Models;
using NetTelegramBotApi;
using NetTelegramBotApi.Requests;
using NetTelegramBotApi.Types;

namespace Cactb.Bot.Controllers
{
    public class DefaultController : Controller
    {

        private static string ApiToken = "408662723:AAEKlergv-R33OWl9dcNVU78M2lTghA2byo";

        private static ReplyKeyboardMarkup fieldsKeyboard;
        private static ReplyKeyboardMarkup serviceKeyboard;
        //private static ReplyKeyboardMarkup foodMenuKeys;
        //private static ReplyKeyboardMarkup actMenuKeys;
        private static int InsertLogedIn(string username, string firstName, string lastName)
        {
            return DA.LogedInDA.Insert(new LogedIn
            {
                FirstName = firstName ?? "",
                LastName = lastName ?? "",
                PhoneNumber = "",
                Username = username ?? ""
            });
        }

        public DefaultController()
        {
            serviceKeyboard = new ReplyKeyboardMarkup()
            {
                Keyboard = new KeyboardButton[][] {
                    new KeyboardButton[] {
                        new KeyboardButton("دریافت چارت دروس رشته") } }
            };
            fieldsKeyboard = new ReplyKeyboardMarkup()
            {
                Keyboard = new KeyboardButton[][] {
                    new KeyboardButton[] {
                        new KeyboardButton("نرم افزار"),
                        new KeyboardButton("معماری سیستمهای کامپیوتری"),
                        new KeyboardButton("فناوری اطلاعات") } }
            };
        }
        public static async Task RunBot()
        {
            try
            {
                int lev = 0;

                var bot = new TelegramBot(ApiToken);
                var me = await bot.MakeRequestAsync(new GetMe());
                Console.WriteLine($"UserName : {me.Username}");
                long offset = 0;
                int whileCount = 0;
                while (true)
                {
                    var updates = await bot.MakeRequestAsync(new GetUpdates() { Offset = offset });
                    try
                    {
                        foreach (var update in updates)
                        {
                            offset = update.UpdateId + 1;
                            var text = update.Message.Text;
                            if (text.ToLower() == "/start" || text.ToLower() == "/new")
                            {
                                var chat = update.Message.Chat;
                                InsertLogedIn(chat.Username, chat.FirstName, chat.LastName);
                                var resp = new SendMessage(0, "");
                                resp = new SendMessage(update.Message.Chat.Id, $"درودبر شما به ربات انجمن علمی کامپیوتر خوش آمدید.{Environment.NewLine}لطفا یکی ار سرویس های زیر را انتخاب کنید")
                                { ReplyMarkup = serviceKeyboard };
                                await bot.MakeRequestAsync(resp);
                            }
                            else if (text != null)
                            {
                                if (text.ToLower().Contains("دریافت چارت دروس رشته".Trim()))
                                {
                                    var resp = new SendMessage(update.Message.Chat.Id, "در کدام گرایش مشغول به تحصیل هستید؟")
                                    {
                                        ReplyMarkup = fieldsKeyboard
                                    };
                                    await bot.MakeRequestAsync(resp);
                                    lev = lev + 1;
                                    continue;
                                }
                                if (text.ToLower().Contains("نرم افزار".TrimEnd()))
                                {
                                    var resp1 = new SendMessage(update.Message.Chat.Id,"صبور باشید...");
                                    await bot.MakeRequestAsync(resp1);
                                    string path = AppDomain.CurrentDomain.BaseDirectory + @"\Images\Soft.jpg";
                                    path = path.Replace(@"\bin\Debug", "");
                                    using (var stream = System.IO.File.Open(path, FileMode.Open))
                                    {
                                        var resp = new SendPhoto(update.Message.Chat.Id, new FileToSend(stream, "Soft.jpg"));
                                        await bot.MakeRequestAsync(resp);
                                    continue;
                                    }
                                }
                                else if (text.ToLower().Contains("معماری سیستمهای کامپیوتری".TrimEnd()))
                                {
                                    var resp1 = new SendMessage(update.Message.Chat.Id, "صبور باشید...");
                                    await bot.MakeRequestAsync(resp1);
                                    string path = AppDomain.CurrentDomain.BaseDirectory + @"Images\Arch.jpg";
                                    path = path.Replace(@"\bin\Debug", "");
                                    using (var stream = System.IO.File.Open(path, FileMode.Open))
                                    {
                                        var resp = new SendPhoto(update.Message.Chat.Id, new FileToSend(stream, "Arch.jpg"));
                                        await bot.MakeRequestAsync(resp);
                                    continue;
                                    }
                                }
                                else if (text.ToLower().Contains("فناوری اطلاعات".ToLower()))
                                {
                                    var resp1 = new SendMessage(update.Message.Chat.Id, "صبور باشید...");
                                    await bot.MakeRequestAsync(resp1);
                                    string path = AppDomain.CurrentDomain.BaseDirectory + @"Images\IT.jpg";
                                    path = path.Replace(@"\bin\Debug", "");
                                    using (var stream = System.IO.File.Open(path, FileMode.Open))
                                    {
                                        var resp = new SendPhoto(update.Message.Chat.Id, new FileToSend(stream, "IT.jpg"));
                                        await bot.MakeRequestAsync(resp);
                                    continue;
                                    }
                                }
                                else
                                {
                                    var resp = new SendMessage(update.Message.Chat.Id, "دستور نامفهوم بود!");
                                    await bot.MakeRequestAsync(resp);
                                    continue;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error : " + ex.Message);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }
        }


        // GET: Default
        public ActionResult Index()
        {
            try
            {
                Task.Run(() => RunBot());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : " + ex.Message);
            }
            return View();
        }
    }
}