using System;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using System.Globalization;


// Телеграмм бот калькулятор.
// Панель управления > свойства браузера > подключения > настройка сети > использовать прокси сервер.

namespace ConsoleApp2
{
    class Program
    {   
        // Создаём бота с токеном.
        private static readonly TelegramBotClient Bot = new TelegramBotClient("626096259:AAFJNjKD74o5Tx8n-0-X3zj_mVZYk7NL4oY");

        static void Main(string[] args)
        {
            Bot.OnMessage += Bot_OnMessage;
            Bot.OnMessageEdited += Bot_OnMessage;

            // Имхо, чтобы "Выберите операцию" и результат отправлялись лишь раз, надо подписаться на события.
            

            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();

            

        }

        private static void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            string history = "";
            history += e.Message.Text;

            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                
                


                var keyboard = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(new[]
                {
                      new [] 
                      // Строка кнопок.
                      {
                                      InlineKeyboardButton.WithCallbackData("Калькулятор", "calculator"),
                                      InlineKeyboardButton.WithCallbackData("История", "history"),                                     
                      }                     
                });               

                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Выберите действие", replyMarkup: keyboard);
            }

            // При нажатии на кнопку.
            Bot.OnCallbackQuery += async (object sc, Telegram.Bot.Args.CallbackQueryEventArgs ev) => 
            {
                var message = ev.CallbackQuery.Message;
                if (ev.CallbackQuery.Data == "calculator")
                // При нажатии на левую кнопку меню (калькулятор).
                {
                    var keyboard = new Telegram.Bot.Types.ReplyMarkups.InlineKeyboardMarkup(new[]
                    {
                            new [] 
                            // Первая строка кнопок с операциями.
                            {
                                 InlineKeyboardButton.WithCallbackData("+", "+"),
                                 InlineKeyboardButton.WithCallbackData("-", "-"),
                            },

                            new [] 
                            // Вторая строка кнопок с операциями.
                            {
                                InlineKeyboardButton.WithCallbackData("*", "*"),
                                InlineKeyboardButton.WithCallbackData("/", "/")
                            }
                    });


                    await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Выберите операцию", replyMarkup: keyboard);
                    


                    // При нажатии на кнопку в подменю.
                    Bot.OnCallbackQuery += async (object op, Telegram.Bot.Args.CallbackQueryEventArgs sec) => 
                    {
                        var message2 = sec.CallbackQuery.Message;

                        // При нажатии на кнопку сложения.
                        if (sec.CallbackQuery.Data == "+")                      
                        {

                            try
                            {
                                string s = e.Message.Text.Replace(',', '.');

                                string[] strNumbers;

                                double sum = 0;

                                // Разделяем на подстроки.
                                strNumbers = s.Split(new char[] { ' ' });

                                // Суммируем подстроки, преобразованные в double.
                                for (int i = 0; i < strNumbers.Length; i++)
                                {
                                    sum += Double.Parse(strNumbers[i], CultureInfo.InvariantCulture);
                                }

                                history += " + ";
                                // Отправляем результат собеседнику.
                                await Bot.SendTextMessageAsync(e.Message.Chat.Id, Convert.ToString(sum));
                            }
                            catch
                            {
                                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Неверный ввод");
                            }
                            
                        }
                        else

                        // При нажатии на кнопку вычитания.
                        if (sec.CallbackQuery.Data == "-")
                        {
                            try
                            {
                                string s = e.Message.Text.Replace(',', '.');

                                string[] strNumbers;

                                double difer = 0;

                                // Разделяем на подстроки.
                                strNumbers = s.Split(new char[] { ' ' });

                                // Вычитаем поочерёдно подстроки, преобразованные в double.
                                for (int i = 0; i < strNumbers.Length; i++)
                                {
                                    difer -= Double.Parse(strNumbers[i], CultureInfo.InvariantCulture);
                                }

                                // Отправляем результат собеседнику.
                                await Bot.SendTextMessageAsync(e.Message.Chat.Id, Convert.ToString(difer));

                                history += " - ";
                            }
                            catch
                            {
                                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Неверный ввод");
                            }
                            
                        }
                        else

                        // При нажатии на кнопку умножения.
                        if (sec.CallbackQuery.Data == "*")
                        {
                            try
                            {
                                string s = e.Message.Text.Replace(',', '.');

                                string[] strNumbers;

                                double prod = 1;

                                // Разделяем на подстроки.
                                strNumbers = s.Split(new char[] { ' ' });

                                // Перемножаем подстроки, преобразованные в double.
                                for (int i = 0; i < strNumbers.Length; i++)
                                {
                                    prod *= Double.Parse(strNumbers[i], CultureInfo.InvariantCulture);
                                }

                                // Отправляем результат собеседнику.
                                await Bot.SendTextMessageAsync(e.Message.Chat.Id, Convert.ToString(prod));

                                history += " * ";
                            }
                            catch
                            {
                                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Неверный ввод");
                            }
                            
                        }
                        else
                        if (sec.CallbackQuery.Data == "/") // при нажатии на кнопку деления
                        {
                            try
                            {
                                string s = e.Message.Text.Replace(',', '.');

                                string[] strNumbers;

                                double div;

                                // Разделяем на подстроки.
                                strNumbers = s.Split(new char[] { ' ' });

                                div = Double.Parse(strNumbers[0], CultureInfo.InvariantCulture);
                                
                                // Разделяем поочерёдно подстроки, преобразованные в double.
                                for (int i = 1; i < strNumbers.Length; i++)
                                {
                                    div /= Double.Parse(strNumbers[i], CultureInfo.InvariantCulture);
                                }

                                // Отправляем результат собеседнику.
                                await Bot.SendTextMessageAsync(e.Message.Chat.Id, Convert.ToString(div));

                                history += " / ";
                            }
                            catch
                            {
                                await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Неверный ввод");
                            }
                            
                        }
                    };
                }
                else
                if (ev.CallbackQuery.Data == "history")
                {
                    await Bot.SendTextMessageAsync(e.Message.Chat.Id, history);
                }
            };
        }
    }
}
    

