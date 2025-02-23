using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace practice_8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // характеристики персонажа
            Dictionary<string, int> specifications = InitializeGame();
            StartGame(specifications);
        }

        public static Dictionary<string, int> InitializeGame()
            /* Устанавливаем начальные характеристики персонажа*/ 
        {
            Dictionary<string, int> specifications = new Dictionary<string, int>
            {
                {"здоровье", 100},
                {"максимальное здоровье", 100},
                {"золото", 10},
                {"зелья", 2},
                {"стрелы", 5},
                {"бонус урон меча", 0}
            };

            return specifications;
        }

        public static void StartGame(Dictionary<string, int> specifications)
            /* Основной игровой процесс */
        {
            Console.WriteLine("Добро пожаловать в захватывающую текстовую RPG-игру \"Числовой квест ULTIMATE\"!\nТы мне кого-то напоминаешь, хммм, ладно\nКак только ты нажмёшь на любую клавишу, ты отправишься в путь..");
            Console.ReadKey();
            for (int i = 1; i < 15; i++)
            {
                Console.WriteLine($"-------------------------------------------------Вы в {i} комнате-------------------------------------------------------");
                Random rnd = new Random();
                int room = rnd.Next(1, 7);
                ProcessRoom(room, specifications);
            }
            FightBoss(specifications);
        }

        public static void ProcessRoom(int roomNumber, Dictionary<string, int> specifications)
            /* обработка событий в комнате */
        {
            Random rnd = new Random();
            switch (roomNumber)
            {
                case 1:
                    FightMonster(rnd.Next(20, 51), rnd.Next(5, 16), specifications);
                    break;
                case 2:
                    OpenChest(specifications);
                    break;
                case 3:
                    VisitMerchant(specifications);
                    break;
                case 4:
                    VisitAltar(specifications);
                    break;
                case 5:
                    MeetDarkMage(specifications);
                    break;
                case 6:
                    ComeToTrap(specifications);
                    break;
            }
        }

        public static void FightMonster(int monsterHP, int monsterAttack, Dictionary<string, int> specifications)
            /* битва с монстром */
        {
            Random rnd = new Random();

            Console.WriteLine("О нет!! Здесь монстр!! Чтож, придётся сражаться..");
            while (monsterHP > 0 && specifications["здоровье"] > 0)
            {
                Console.WriteLine("\nТвои статы:");
                ShowStats(specifications);
                Console.WriteLine($"\nСтаты этого простофили:\nздоровье - {monsterHP}\nатака - {monsterAttack}");
                Console.WriteLine($"\nВыбери оружие для атаки (нажми 1, 2 или 3):\n1. Меч - наносит 10-20 урона\n2. Лук – наносит 5-15 урона, но требует стрелы (у тебя их {specifications["стрелы"]})\n3. Зелье - восстановит 30 здоровья\nВ случае ошибки понимания запроса, будет атака мечом!");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "2":
                        if (specifications["стрелы"] > 0)
                        {
                            int hit = rnd.Next(5, 16);
                            monsterHP -= hit;
                            specifications["стрелы"]--;
                            Console.WriteLine($"Ты ударяешь луком на {hit} урона");
                        }
                        else
                        {
                            Console.WriteLine("Стрел нет, поэтому выбирай что-то другое:\n1. Меч - наносит 10-20 урона\n2. Зелье - восстановит 30 здоровья\nПри ошибке понимания запроса, будет атака мечом!");
                            string choice2 = Console.ReadLine();
                            switch ( choice2 )
                            {
                                case "2":
                                    bool use = UsePotion(specifications);
                                    if (!use)
                                    {
                                        Console.WriteLine("Остаётся только атака мечом!");
                                        int hit = rnd.Next(10, 21) + specifications["бонус урон меча"];
                                        monsterHP -= hit;
                                        Console.WriteLine($"Ты ударяешь мечом на {hit} урона");
                                    }
                                    break;
                                default:
                                    int hit2 = rnd.Next(10, 21) + specifications["бонус урон меча"];
                                    monsterHP -= hit2;
                                    Console.WriteLine($"Ты ударяешь мечом на {hit2} урона");
                                    break;
                            }
                        }
                        break;
                    case "3":
                        bool use2 = UsePotion(specifications);
                        if (!use2)
                        {
                            Console.WriteLine("Остаётся только атака мечом!");
                            int hit = rnd.Next(10, 21) + specifications["бонус урон меча"];
                            monsterHP -= hit;
                            Console.WriteLine($"Ты ударяешь мечом на {hit} урона");
                        }
                        break;
                    default:
                        int hit3 = rnd.Next(10, 21) + specifications["бонус урон меча"];
                        monsterHP -= hit3;
                        Console.WriteLine($"Ты ударяешь мечом на {hit3} урона");
                        break;
                }

                Console.ReadKey();
                Console.WriteLine("\nТеперь черёд монстра!\n");
                specifications["здоровье"] -= monsterAttack;
                Console.WriteLine($"Монстр ударил на {monsterAttack} урона");

                Console.ReadKey();
            }

            if (specifications["здоровье"] < 1)
            {
                Console.WriteLine("\nТы погибаешь от рук этого придурка, поздравляем! Ты точно умерла, прям окончательно я буду скучать по тебе..");
                Console.ReadKey();
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine($"\nМонстр умер! Он умер на твоих глазах! Ты просто умничка!!!! Пойдём дальше");
                Console.ReadKey();
            }


        }

        public static void ShowStats(Dictionary<string, int> specifications)
            /* вывод характеристик */
        {
            foreach (var pair  in specifications)
            {
                Console.WriteLine($"{pair.Key} - {pair.Value}");
            }
        }

        public static bool UsePotion(Dictionary<string, int> specifications)
            /* использование зелий */
        {
            if (specifications["зелья"] > 0)
            {
                if ((specifications["здоровье"] + 30) > specifications["максимальное здоровье"])
                {
                    specifications["здоровье"] = specifications["максимальное здоровье"];
                }
                else
                {
                    specifications["здоровье"] += 30;
                }
                specifications["зелья"]--;
                Console.WriteLine($"ты юзаешь зельку! -1 зелька у тебя. Ты воссполняешь себе 30 здоровья. Щас у тебя {specifications["здоровье"]} здоровья");
                return true;
            }
            else
            {
                Console.WriteLine("К сожалению, зелий у тебя нет");
                return false;
            }
        }

        public static void OpenChest(Dictionary<string, int> specifications)
            /* сундук */
        {
            Random rnd = new Random();
            int chest = rnd.Next(1, 3);
            int number1 = rnd.Next(1, 1000);
            int number2 = rnd.Next(1, 1000);

            int correctAnswer = number1 + number2;
            Console.WriteLine($"Ты нашла сундук! Какое везение! Правда чтобы его открыть, реши математическую задачку, которая написана на листочке на сундуке Чурь калькулятор не использовать!! Я всё вижу!!!!. Текст на ней гласит:\nСколько будет: {number1} + {number2}?");
            int answer = Convert.ToInt32(Console.ReadLine());

            // угадан правильный ответ
            if (answer == correctAnswer)
            {
                Console.WriteLine("Правильно! Сундук открылся и:");
                // обычный сундук
                if (chest == 1)
                {
                    int reward = rnd.Next(1, 4);
                    switch (reward)
                    {
                        // в сундуке золото
                        case 1:
                            int chest_gold = rnd.Next(20, 41);
                            specifications["золото"] += chest_gold;
                            Console.WriteLine($"\nНичего себе! В сундуке лежало золото! +{chest_gold} золота в твои кармашки! Пойдём дальше");
                            Console.ReadKey();
                            break;
                        // в сундуке зелье
                        case 2:
                            Console.WriteLine("Ты нашла в нём зелье! +1 зелька в твой инвентарь. Крутяк. Пойдём дальше");
                            Console.ReadKey();
                            break;
                        // в сундуке стрелы
                        case 3:
                            int chest_arrows = rnd.Next(3, 7);
                            specifications["стрелы"] += chest_arrows;
                            Console.WriteLine($"\nНичего себе! В сундуке лежали стрелы! +{chest_arrows} стрел в твой колчан! Пойдём дальше");
                            Console.ReadKey();
                            break;
                    }
                }
                // сундук проклятый
                else
                {
                    int chest_gold = rnd.Next(20, 41);
                    specifications["максимальное здоровье"] -= 10;
                    Console.WriteLine($"Этот сундук не такой как все, ты сразу это заметил.. Это ПРОКЛЯТЫЙ сундук!! Ты взял {chest_gold} золота из него, но этот сундук высосал у тебя немного жизненных сил, и теперь у тебя -10 максимального здоровья..  Чтож, пойдём дальше");
                    Console.ReadKey();
                }
            }
            // ответ неправильный
            else
            {
                Console.WriteLine($"К сожалению это не правильный ответ.. Сундук закрылся намертво, и его больше не открыть.. Тебе явно нужно больше заниматься математикой! Может повезёт в следующий раз. Пойдём дальше");
                Console.ReadKey();
            }
        }

        public static void VisitMerchant(Dictionary<string, int> specifications)
            /* торговец */
        {
            Console.WriteLine($"Пурум пум пум! Тут торговец оказывается сидит! Можешь у него купить зельку или же стрелы\nУ тебя щас {specifications["золото"]} золота\n\nВот его предложение:\nзелье - 10 золота\nстрелы(3 шт) - 5 золота\n\nВыбери с умом и только одно! (1 или 2):");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    if (specifications["золото"] > 9)
                    {
                        specifications["золото"] -= 10;
                        specifications["зелья"]++;
                        Console.WriteLine("Ухтыж.. Покупаешь всё-таки. +1 зелька к тебе и -10 золота из твоих кармашек :P:P Пойдём дальше");
                    }
                    else
                    {
                        Console.WriteLine("\nСлышь! У тебя золота вообще не хватит, проваливай давай, нищенка! >:( Пойдём дальше");
                    }
                    break;
                case "2":
                    if (specifications["золото"] > 9)
                    {
                        specifications["золото"] -= 5;
                        specifications["стрелы"] += 3;
                        Console.WriteLine($"Ухтыж.. Покупаешь всё-таки. +3 стрел к тебе в колчан и -5 золота из твоих кармашек :P:P Пойдём дальше");                     
                    }
                    else
                    {
                        Console.WriteLine("\nСлышь! У тебя золота вообще не хватит, проваливай давай, нищенка! >:( Пойдём дальше");
                    }

                    break;
                default:
                    Console.WriteLine("Торговец вообще не понял что ты там промямлил, посчитал тебя за дурачка и прогнал тебя.. Нужно говорить чётче!! Пойдём дальше");
                    break;
            }

            Console.ReadKey();
        }

        public static void VisitAltar(Dictionary<string, int> specifications)
            /* алтарь */
        {
            Console.WriteLine("В середине комнаты ты замечаешь странный алтарь из камня и обсидиана. Ты подходишь ближе, чтобы рассмотреть его.");
            if (specifications["золото"] < 10)
            {
                Console.WriteLine(" И он потух сразу же. Хмм, странно, возможно не было слышно звона монет в твоих карманах.. Пойдём дальше");
            }
            else
            {
                Console.WriteLine(" И тут он начинает светиться и источать силу, неведомую тебе. В твоих мыслях ты слышишь отголоски древних духов: \"золото...взамен...на....урон...здоровье...пустота...\".\nЧто же ты выберешь?\n1 - урон\n2 - здоровье\n3 - пустота");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        // плюс урон к мечу
                        specifications["золото"] -= 10;
                        specifications["бонус урон меча"] = 5;

                        Console.WriteLine("Ты чувствуешь, что твой меч становится тяжелее и мощнее, теперь ты бьёшь больнее, а золота в кармашках стало меньше, а именно -10.. Алтарь потух, а голоса затихли. Пойдём дальше");
                        break;
                    case "2":
                        // плюс здоровье
                        specifications["золото"] -= 10;
                        if ((specifications["здоровье"] + 20) > specifications["максимальное здоровье"])
                        {
                            specifications["здоровье"] = specifications["максимальное здоровье"];
                        }
                        else
                        {
                            specifications["здоровье"] += 20;
                        }
                        Console.WriteLine("Ты чувствуешь как жизненная сила начинает приливать к тебе, ты вылечился на 20 здоровья, а золота в кармашках стало меньше, а именно -10.. Алтарь потух, а голоса затихли. Пойдём дальше");
                        break;
                    case "3":
                        Console.WriteLine("Ты начал прогонять голоса из головы и тебе это удалось, и одновременно с этим алтарь потух.. Пойдём дальше");
                        break;
                    default:
                        Console.WriteLine("Голоса не поняли поток твоих мыслей и поскорее свалили от тебя, а алтарь потух. Пойдём дальше");
                        break;
                }
            }

            Console.ReadKey();
        }

        public static void MeetDarkMage(Dictionary<string, int> specifications)
            /* тёмный маг */
        {
            Console.WriteLine("Ты входишь в комнату и замечаешь таинственную фигуру, которая окутана тьмой. Похоже на одного из тёмных магов. ");
            if (specifications["здоровье"] < 11)
            {
                Console.WriteLine("Он увидел, что ты изрядно потрёпан и понял, тебе мало что поможет и он исчез. Пойдём дальше");
            }
            else
            {
                Console.WriteLine("Он видет, ты имеешь достаточно жизненных сил, чтобы совершить с ним сделку, пожертвовать 10 твоего здоровья на 2 зелья и 5 стрел. Что скажешь?\n1 - да, давай, я готов\n2 - ты чего, ни в коем случае");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        specifications["здоровье"] -= 10;
                        specifications["зелья"] += 2;
                        specifications["стрелы"] += 5;
                        Console.WriteLine("Маг направляет в твою сторону руку и забирает 10 твоего здоровья, ты прикрываешь глаза, и как только открываешь, видишь, что мага уже нет, а около тебя валяется 2 зелья и 5 стрел. Ты их забираешь. Пойдём дальше");
                        break;
                    case "2":
                        Console.WriteLine("Нет так нет, маг понял твои намерения и покинул тебя. Пойдём дальше");
                        break;
                    default:
                        Console.WriteLine("Тёмный маг вообще не понял, что ты там пробормотал и понял, с тобой ничего не выйдет, и оставил тебя в полном одиночестве. Пойдём дальше");
                        break;
                }
            }
            Console.ReadKey();
        }

        public static void FightBoss(Dictionary<string, int> specifications)
        /* Босс */
        {
            int monsterHP = 100;
            int recover_hp = 0;
            int special_attack = 0;

            Console.WriteLine("-------------------------------------------------Вы в комнате с БОССОМ-------------------------------------------------------");
            Console.WriteLine("Ты проходишь достаточно комнат, и тут перед тобой сам босс!!!! Придётся сразиться с этим чертополохом!!! А вот собственно и он... Погнали");
            Random rnd = new Random();

            while (monsterHP > 0 && specifications["здоровье"] > 0)
            {
                int monsterAttack = rnd.Next(15, 26);
                Console.WriteLine("\nТвои статы:");
                ShowStats(specifications);
                Console.WriteLine($"\nСтаты этого простофили:\nздоровье - {monsterHP}\nатака - {monsterAttack}");
                Console.WriteLine($"\nВыбери оружие для атаки (нажми 1, 2 или 3):\n1. Меч - наносит 10-20 урона\n2. Лук – наносит 5-15 урона, но требует стрелы (у тебя их {specifications["стрелы"]})\n3. Зелье - восстановит 30 здоровья\nВ случае ошибки понимания запроса, будет атака мечом!");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "2":
                        if (specifications["стрелы"] > 0)
                        {
                            int hit = rnd.Next(5, 16);
                            monsterHP -= hit;
                            specifications["стрелы"]--;
                            Console.WriteLine($"Ты ударяешь луком на {hit} урона");
                        }
                        else
                        {
                            Console.WriteLine("Стрел нет, поэтому выбирай что-то другое:\n1. Меч - наносит 10-20 урона\n2. Зелье - восстановит 30 здоровья\nПри ошибке понимания запроса, будет атака мечом!");
                            string choice2 = Console.ReadLine();
                            switch (choice2)
                            {
                                case "2":
                                    bool use = UsePotion(specifications);
                                    if (!use)
                                    {
                                        Console.WriteLine("Остаётся только атака мечом!");
                                        int hit = rnd.Next(10, 21) + specifications["бонус урон меча"];
                                        monsterHP -= hit;
                                        Console.WriteLine($"Ты ударяешь мечом на {hit} урона");
                                    }
                                    break;
                                default:
                                    int hit2 = rnd.Next(10, 21) + specifications["бонус урон меча"];
                                    monsterHP -= hit2;
                                    Console.WriteLine($"Ты ударяешь мечом на {hit2} урона");
                                    break;
                            }
                        }
                        break;
                    case "3":
                        bool use2 = UsePotion(specifications);
                        if (!use2)
                        {
                            Console.WriteLine("Остаётся только атака мечом!");
                            int hit = rnd.Next(10, 21) + specifications["бонус урон меча"];
                            monsterHP -= hit;
                            Console.WriteLine($"Ты ударяешь мечом на {hit} урона");
                        }
                        break;
                    default:
                        int hit3 = rnd.Next(10, 21) + specifications["бонус урон меча"];
                        monsterHP -= hit3;
                        Console.WriteLine($"Ты ударяешь мечом на {hit3} урона");
                        break;
                }

                Console.ReadKey();
                Console.WriteLine("\nТеперь черёд БОССА!\n");
                specifications["здоровье"] -= monsterAttack;
                Console.WriteLine($"БОСС ударил на {monsterAttack} урона");
                recover_hp++;
                special_attack++;

                // босс восстанавливает здоровье
                if (recover_hp ==  3)
                {
                    int chance_recover = rnd.Next(1, 4);

                    if (chance_recover == 2)
                    {
                        monsterHP += 10;
                        Console.WriteLine("Этот чертюга смог накопить достаточно мощи, чтобы восстановить себе 10 здоровья!! Но нам это не помешает..");
                    }
                    recover_hp = 0;
                }
                // босс бьёт дважды
                if (special_attack == 3)
                {
                    int chance_double = rnd.Next(1, 4);

                    if (chance_double == 2)
                    {
                        Console.WriteLine("Этот жухлый гумус настолько быстрый, что успевает ударить ещё раз! Берегись!!");
                        specifications["здоровье"] -= monsterAttack;
                        Console.WriteLine($"БОСС ударил на {monsterAttack} урона");
                    }
                    special_attack = 0;
                }

                Console.ReadKey();
            }

            if (specifications["здоровье"] < 1)
            {
                EndGame(false, specifications);
            }
            else
            {
                EndGame(true, specifications);
            }
        }

        public static void EndGame(bool isWin, Dictionary<string, int> specifications)
        {
            if (isWin == false)
            {
                Console.WriteLine($"\nНу чтож ты умираешь и не успеваешь ничего уже сделать. Всё когда-либо заканчивается, мне было с тобой интересно проходить этот квест, увидимся позже.. прощай");
                Console.ReadKey();
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine($"\nБОСС, он.. он умер!!!!! Ты одолел его. Прям горжусь тобой, ты просто что-то с чем-то! Ты можешь с уверенностью уходить отсюда. За меня не беспокойся. Мы встретимся ещё когда-либо, надеюсь.. ты можешь уйти отсюда с гордо поднятой головой");
                Console.ReadKey();
            }
        }

        public static void ComeToTrap(Dictionary<string, int> specifications)
            /* ловушка */
        {
            Random rnd = new Random();
            int hit = rnd.Next(5, 21);
            specifications["здоровье"] -= hit;
            Console.WriteLine($"Какое невезение! Ты угождаешь в ловушку с очень острыми шипами из стен!! Ты теряешь {hit} здоровья");

            if (specifications["здоровье"] < 1)
            {
                Console.WriteLine("\nЭти шипи настолько острые, что ты не выдерживаешь и погибаешь, поздравляем! Ты точно умерла, прям окончательно я буду скучать по тебе..");
                Console.ReadKey();
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine($"Несмотря на дырки в твоём теле, ты всё ещё можешь идти с {specifications["здоровье"]} здоровья! Пойдём дальше");
                Console.ReadKey();
            }
        }
    }
}
