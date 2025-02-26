using System;

class Program
{
    static Random rand = new Random();
    static int playerHP = 100;
    static int potions = 3;
    static int gold = 0;
    static int arrows = 5;
    static int difficulty = 1;
    static int[] dungeonMap = new int[10];
    static int bossHP;

    static void Main()
    {
        Console.WriteLine("Выберите сложность: 1 - Легко, 2 - Средне, 3 - Сложно");
        try
        {
            difficulty = int.Parse(Console.ReadLine());
            if (difficulty < 1 || difficulty > 3)
            {
                throw new Exception("Неправаильно написали число. Установлена средняя сложность.");
            }
        }
        catch
        {
            Console.WriteLine("Ошибка, установлена средняя сложность.");
            difficulty = 2;
        }

        bossHP = difficulty == 1 ? 75 : difficulty == 2 ? 100 : 125;

        for (int i = 0; i < dungeonMap.Length - 1; i++)
        {
            dungeonMap[i] = rand.Next(1, 6);
        }
        dungeonMap[9] = 6;

        for (int i = 0; i < dungeonMap.Length; i++)
        {
            if (playerHP <= 0)
            {
                Console.WriteLine("Вы погибли...");
                break;
            }

            Console.WriteLine($"\n==== Комната {i + 1} ====");

            switch (dungeonMap[i])
            {
                case 1: Fight(); break;
                case 2: Trap(); break;
                case 3: Chest(); break;
                case 4: Shop(); break;
                case 5: Console.WriteLine("Комната пуста."); break;
                case 6: BossFight(); break;
            }

            Console.WriteLine("\nНажмите Enter, чтобы продолжить...");
            Console.ReadLine();
        }

        Console.WriteLine(playerHP > 0 ? "Вы успешно прошли подземелье!" : "Конец игры.");
    }

    static void Fight()
    {
        int monsterHP = rand.Next(20, 51) * difficulty;
        Console.WriteLine($"Вы встретили монстра с {monsterHP} HP!");

        while (monsterHP > 0 && playerHP > 0)
        {
            Console.WriteLine("1 - Удар мечом, 2 - Использование лука, 3 - Использовать зелье, 4 - Убежать");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Attack(ref monsterHP, 10, 20);
                    break;
                case "2":
                    if (arrows > 0)
                    {
                        Attack(ref monsterHP, 5, 15);
                        arrows--;
                        Console.WriteLine($"Осталось стрел: {arrows}");
                    }
                    else
                    {
                        Console.WriteLine("У вас нету стрел 😢😢😢");
                    }
                    break;
                case "3":
                    UsePotion();
                    break;
                case "4":
                    Console.WriteLine("Вы сбежали!");
                    return;
                default:
                    Console.WriteLine("Некорректный ввод!");
                    break;
            }

            if (monsterHP > 0)
            {
                int monsterDamage = rand.Next(5, 16) * difficulty;
                playerHP -= monsterDamage;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Монстр атакует! -{monsterDamage} HP. Осталось {Math.Max(playerHP, 0)} HP.");
                Console.ResetColor();
            }
        }

        if (playerHP > 0)
        {
            int loot = rand.Next(10, 51);
            gold += loot;
            Console.WriteLine($"Монстр побежден! Вы получили {loot} золота.");
        }
    }

    static void Attack(ref int enemyHP, int minDmg, int maxDmg)
    {
        int damage = rand.Next(minDmg, maxDmg + 1);
        enemyHP -= damage;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Вы нанесли {damage} урона. Осталось {Math.Max(enemyHP, 0)} HP.");
        Console.ResetColor();
    }

    static void Trap()
    {
        int damage = rand.Next(10, 31);
        playerHP -= damage;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Вы попали в ловушку! Потеряно {damage} HP. Осталось {Math.Max(playerHP, 0)} HP.");
        Console.ResetColor();
    }

    static void Chest()
    {
        int goldFound = rand.Next(20, 101);
        gold += goldFound;
        Console.WriteLine($"Вы нашли сундук! Внутри {goldFound} золота!");
    }

    static void Shop()
    {
        Console.WriteLine("Вы встретили торговца Хотите купить зелье за 30 золота? (да/нет)");
        string choice = Console.ReadLine().ToLower();
        if (choice == "да" && gold >= 30)
        {
            gold -= 30;
            potions++;
            Console.WriteLine("Вы купили зелье");
        }
        else
        {
            Console.WriteLine("Недостаточно золота 😢😢😢");
        }
    }

    static void BossFight()
    {
        Console.WriteLine($"🔥🔥🔥 БОСС! HP: {bossHP} 🔥🔥🔥");
        while (bossHP > 0 && playerHP > 0)
        {
            Console.WriteLine("1 - Удар мечом, 2 - Выстрел из лука, 3 - Использовать зелье");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Attack(ref bossHP, 10, 20);
                    break;
                case "2":
                    if (arrows > 0)
                    {
                        Attack(ref bossHP, 5, 15);
                        arrows--;
                    }
                    else Console.WriteLine("Нет стрел 😢😢😢");
                    break;
                case "3":
                    UsePotion();
                    break;
            }

            if (bossHP > 0)
            {
                int bossDamage = rand.Next(2, 10) * difficulty;
                playerHP -= bossDamage;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Босс атакует! -{bossDamage} HP 🔥🔥🔥 Осталось {Math.Max(playerHP, 0)} HP.");
                Console.ResetColor();
            }
        }
        Console.WriteLine(playerHP > 0 ? "Вы победили босса!" : "😢 Босс вас победил...");
    }

    static void UsePotion()
    {
        if (potions > 0)
        {
            playerHP += 20;
            potions--;
            Console.WriteLine($"Выпито зелье и пополнено здоровье. HP: {playerHP}");
        }
        else Console.WriteLine("Нет зелий((( 😢😢😢");
    }
}