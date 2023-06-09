﻿//create 2d array for the game map 5x5
using CIS129FinalProject;
using System.Diagnostics;

bool start = true;
while (start)
{
    //create a new wizert
    Wizert wizert = new Wizert(100, 200);

    //random starting point for Wizert
    Random wizertStart = new Random();
    wizert.wizertLocationX = wizertStart.Next(0, 6);
    wizert.wizertLocationY = wizertStart.Next(0, 6);
//    wizert.PrintWizertLocation();

    //create a list enemies
    List<Enemy> enemies = new List<Enemy>();

    enemies.Add(new Enemy("Goblin", 3, 2, 2));
    enemies.Add(new Enemy("Goblin", 3, 2, 4));
    enemies.Add(new Enemy("Orc", 5, 2, 3));
    enemies.Add(new Enemy("Orc", 5, 2, 5));

    enemies.Add(new Enemy("Banshee", 8, 3, 5));
    enemies.Add(new Enemy("Goblin", 3, 3, 1));

    enemies.Add(new Enemy("Orc", 5, 5, 5));

    enemies.Add(new Enemy("Orc", 5, 1, 5));
    enemies.Add(new Enemy("Orc", 5, 1, 3));

    enemies.Add(new Enemy("Banshee", 8, 4, 4));
    enemies.Add(new Enemy("Banshee", 8, 4, 3));

    enemies.Add(new Enemy("Banshee", 8, 0, 0));
    enemies.Add(new Enemy("Banshee", 8, 0, 3));
    enemies.Add(new Enemy("Goblin", 3, 0, 1));

    //create list of powerups
    List<Powerup> powerups = new List<Powerup>();

    powerups.Add(new Powerup("Health Potion", 5, 1));
    powerups.Add(new Powerup("Health Potion", 5, 0));
    powerups.Add(new Powerup("Magicka Potion", 5, 3));

    powerups.Add(new Powerup("Health Potion", 0, 2));

    powerups.Add(new Powerup("Magicka Potion", 4, 2));
    powerups.Add(new Powerup("Health Potion", 4, 5));



    //get random exit location
    Random exitRand = new Random();
    int ExitX = exitRand.Next(0, 6);
    int ExitY = exitRand.Next(0, 6);
    //location of exit
    (int, int) ExitLocation = (ExitX, ExitY);
//    Console.WriteLine(ExitLocation);

    bool run = true;

    //Start of each game print out beginning Game
    Console.WriteLine("\r\n\r\n\r\n***Game Starting!***\r\n\r\n");
    Console.WriteLine("There is a metallic clang as you awake with a start. You quickly get to your feet, having no " +
        "memory of how you got to this place. The air is damp and stale. You must find the dungeon exit. " +
        "You can hear screams beyond the corridor...\r\n");

    while (run)
    {
        //random descriptions of rooms to be displayed
        List<string> roomDescriptionsList = new List<string> {
        "You are in a room illuminated by torches. It reeks of filthy orcs, though you do not see any nearby. Press...",
        "Your eyes strain to see in the dark of the room. You hope it's only bones crunching beneath your boots. Press...",
        "The door creaks open slowly. The smell of rotting corpses fills your nose as you try not to gag. Press...",
        "Silently, you advance, you can hear faint grunts and snarling coming from nearby. Press...",
        "The coldness of the dungeon begins to seep into your bones, attempting to drain the life out of you. You must find the exit. Press..."};

        Random rnd = new Random();
        int roomDescriptionIndex = rnd.Next(0, roomDescriptionsList.Count);

        bool replay = false;

        //have wizert move through map with prompts from user
        Console.WriteLine(roomDescriptionsList[roomDescriptionIndex]);
        Console.WriteLine(  "1. To go north\r\n" +
                            "2. To go south\r\n" +
                            "3. To go east\r\n" +
                            "4. To go west");

        var input = Console.ReadLine();

        //check if wizert found powerup
        Powerup powerup = powerups.FirstOrDefault(x => x.locationX == wizert.wizertLocationX
                                                && x.locationY == wizert.wizertLocationY);

        //check if wizert in enemy location
        Enemy enemy = enemies.FirstOrDefault(x => x.locationX == wizert.wizertLocationX
                                                && x.locationY == wizert.wizertLocationY);

        switch (input)
        {
            case "1":
                Console.WriteLine("\r\nYou travel North...");
                if (wizert.MoveNorth())
                {

                    if (powerup != null)
                    {
                        if (powerup.powerupName == "Health Potion")
                        {
                            Console.WriteLine($"\r\nYou have found a {powerup.powerupName}! Restore {powerup.PointsRestored()} Health Points!\r\n");
                            wizert.ReceivePowerup(powerup.PointsRestored());
                            powerups.Remove(powerup);
                            Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth());
                            Console.WriteLine("Wizert Magicka: " + wizert.GetMagickaPoints() + "\r\n");
                        }
                        else
                        {
                            Console.WriteLine($"\r\nYou have found a {powerup.powerupName}! Restore {powerup.PointsRestored()} Magicka Points!\r\n");
                            wizert.ReceivePowerup(powerup.PointsRestored());
                            powerups.Remove(powerup);
                            Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth());
                            Console.WriteLine("Wizert Magicka: " + wizert.GetMagickaPoints() + "\r\n");
                        }
                    }

                    //if enemy is not null, intitate a battle
                    if (enemy != null)
                    {
                        //bool for battle loop
                        bool battle = true;
                        while (battle)
                        {

                            Console.WriteLine($"\r\nYou have encountered a {enemy.enemyName}.\r\n" +
                            $"Its current HP is {enemy.GetEnemyHealth()}.\r\n" +
                            "Press...\r\n" +
                            "1. To Attack\r\n" +
                            "2. To Heal\r\n" +
                            "3. To Attempt to Flee\r\n");
                            //List wizert health and magicka after each round
                            Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth());
                            Console.WriteLine("Wizert Magicka: " + wizert.GetMagickaPoints());

                            var choice = Console.ReadLine();
                            //swicth statement for battles
                            switch (choice)
                            {
                                case "1":
                                    //wizert attack
                                    if (wizert.GetMagickaPoints() >= 3)
                                    {
                                        //wizert attacks for 5, if enemy is still alive, enemy attacks at end of case
                                        Console.WriteLine($"\r\nYou use {wizert.MPCost(choice)} MP to attack with Fireball! It does 5 Damage to the {enemy.enemyName}.\r\n");
                                        wizert.UseFireBall();
                                        //enemy taking damage - always 5
                                        enemy.TakeDamage();
                                        //check if enemy is killed
                                        if (enemy.GetEnemyHealth() <= 0)
                                        {
                                            //remove enemy from list
                                            enemies.Remove(enemy);
                                            Console.WriteLine("\r\nEnemy Destroyed!\r\n");
                                            Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth());
                                            Console.WriteLine("Wizert Magicka: " + wizert.GetMagickaPoints() + "\r\n");
                                            battle = false;
                                            break;
                                        }

                                        Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth());
                                        Console.WriteLine("Wizert Magicka: " + wizert.GetMagickaPoints());

                                        //enemy attack if enemy is alive
                                        Console.WriteLine($"\r\n{enemy.enemyName} attacks with {enemy.EnemyAttackType()}, doing {enemy.EnemyAttack()} damage.\r\n");
                                        wizert.TakeDamage(enemy.EnemyAttack());

                                        //check if wizert is killed
                                        if (wizert.GetCurrentHealth() <= 0)
                                        {
                                            Console.WriteLine("\r\nThe Wizert has been mortally wounded. Game Over.\r\n");
                                            battle = false;
                                            run = false;
                                            replay = true;
                                            break;
                                        }
                                        Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth() + "\r\n");
                                    }
                                    else
                                    {
                                        Console.WriteLine("\r\nNot enough Magicka Points!\r\n");
                                        battle = true;
                                        break;
                                    }

                                    break;

                                case "2":
                                    if (wizert.GetMagickaPoints() >= 5)
                                    {
                                        Console.WriteLine($"Using {wizert.MPCost(choice)} MP, you successfully heal for 3 HP!");
                                        //wizert heals for 3
                                        wizert.Heal();
                                        Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth() + "\r\n");
                                        //enemy attack if enemy is alive
                                        Console.WriteLine($"{enemy.enemyName} attacks with {enemy.EnemyAttackType()}, doing {enemy.EnemyAttack()} damage.\r\n");
                                        wizert.TakeDamage(enemy.EnemyAttack());
                                    }
                                    else
                                    {
                                        Console.WriteLine("Heal failed! Not enough Magicka!");
                                        Console.WriteLine($"\r\n{enemy.enemyName} attacks with {enemy.EnemyAttackType()}, doing {enemy.EnemyAttack()} damage.\r\n");
                                        wizert.TakeDamage(enemy.EnemyAttack());
                                    }


                                    //check if wizert is killed
                                    if (wizert.GetCurrentHealth() <= 0)
                                    {
                                        Console.WriteLine("\r\nThe Wizert has been mortally wounded! Game Over.");
                                        battle = false;
                                        run = false;
                                        replay = true;
                                        break;
                                    }
                                    Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth());
                                    Console.WriteLine("Wizert Magicka: " + wizert.GetMagickaPoints() + "\r\n");

                                    break;

                                case "3":
                                    Console.WriteLine("\r\nYou attempt to flee!");

                                    Random random = new Random();

                                    if (random.Next(0, 2) == 0)
                                    {
                                        Console.WriteLine("\r\nFlee attempt successful!\r\n");
                                        battle = false;
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("\r\nFlee unsuccessful!\r\n");
                                        Console.WriteLine($"\r\n{enemy.enemyName} attacks with {enemy.EnemyAttackType()}, doing {enemy.EnemyAttack()} damage.\r\n");
                                        wizert.TakeDamage(enemy.EnemyAttack());
                                        battle = true;
                                        break;
                                    }

                                default:
                                    Console.WriteLine("Not a valid Entry. Try Again.");
                                    break;
                            }
                        }

                    }

                    //check if wizert location is equal to exit location
                    if (wizert.WizertLocation() == ExitLocation && wizert.GetCurrentHealth() > 0)
                    {
                        wizert.PrintExitFoundMessage();
                        start = false;
                        run = false;
                        replay = true;
                        break;
                    }
                }
                //breaks out of main switch statement
                break;

            case "2":
                Console.WriteLine("\r\nYou travel South...\r\n");
                if (wizert.MoveSouth())
                {

                    if (powerup != null)
                    {
                        if (powerup.powerupName == "Health Potion")
                        {
                            Console.WriteLine($"\r\nYou have found a {powerup.powerupName}! Restore {powerup.PointsRestored()} Health Points!\r\n");
                            wizert.ReceivePowerup(powerup.PointsRestored());
                            powerups.Remove(powerup);
                            Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth());
                            Console.WriteLine("Wizert Magicka: " + wizert.GetMagickaPoints() + "\r\n");
                        }
                        else
                        {
                            Console.WriteLine($"\r\nYou have found a {powerup.powerupName}! Restore {powerup.PointsRestored()} Magicka Points!\r\n");
                            wizert.ReceivePowerup(powerup.PointsRestored());
                            powerups.Remove(powerup);
                            Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth());
                            Console.WriteLine("Wizert Magicka: " + wizert.GetMagickaPoints() + "\r\n");
                        }
                    }

                    //if enemy is not null, intitate a battle
                    if (enemy != null)
                    {
                        //bool for battle loop
                        bool battle = true;
                        while (battle)
                        {

                            Console.WriteLine($"\r\nYou have encountered a {enemy.enemyName}.\r\n" +
                            $"Its current HP is {enemy.GetEnemyHealth()}.\r\n" +
                            "Press...\r\n" +
                            "1. To Attack\r\n" +
                            "2. To Heal\r\n" +
                            "3. To Attempt to Flee\r\n");
                            //List wizert health and magicka after each round
                            Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth());
                            Console.WriteLine("Wizert Magicka: " + wizert.GetMagickaPoints());

                            var choice = Console.ReadLine();
                            //swicth statement for battles
                            switch (choice)
                            {
                                case "1":
                                    //wizert attack
                                    if (wizert.GetMagickaPoints() >= 3)
                                    {
                                        //wizert attacks for 5, if enemy is still alive, enemy attacks at end of case
                                        Console.WriteLine($"\r\nYou use {wizert.MPCost(choice)} MP to attack with Fireball! It does 5 Damage to the {enemy.enemyName}.\r\n");
                                        wizert.UseFireBall();
                                        //enemy taking damage - always 5
                                        enemy.TakeDamage();
                                        //check if enemy is killed
                                        if (enemy.GetEnemyHealth() <= 0)
                                        {
                                            //remove enemy from list
                                            enemies.Remove(enemy);
                                            Console.WriteLine("\r\nEnemy Destroyed!\r\n");
                                            Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth());
                                            Console.WriteLine("Wizert Magicka: " + wizert.GetMagickaPoints() + "\r\n");
                                            battle = false;
                                            break;
                                        }

                                        Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth());
                                        Console.WriteLine("Wizert Magicka: " + wizert.GetMagickaPoints());

                                        //enemy attack if enemy is alive
                                        Console.WriteLine($"\r\n{enemy.enemyName} attacks with {enemy.EnemyAttackType()}, doing {enemy.EnemyAttack()} damage.\r\n");
                                        wizert.TakeDamage(enemy.EnemyAttack());

                                        //check if wizert is killed
                                        if (wizert.GetCurrentHealth() <= 0)
                                        {
                                            Console.WriteLine("\r\nThe Wizert has been mortally wounded. Game Over.\r\n");
                                            battle = false;
                                            run = false;
                                            replay = true;
                                            break;
                                        }
                                        Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth() + "\r\n");
                                    }
                                    else
                                    {
                                        Console.WriteLine("\r\nNot enough Magicka Points!\r\n");
                                        battle = true;
                                        break;
                                    }

                                    break;

                                case "2":
                                    if (wizert.GetMagickaPoints() >= 5)
                                    {
                                        Console.WriteLine($"Using {wizert.MPCost(choice)} MP, you successfully heal for 3 HP!");
                                        //wizert heals for 3
                                        wizert.Heal();
                                        Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth() + "\r\n");
                                        //enemy attack if enemy is alive
                                        Console.WriteLine($"{enemy.enemyName} attacks with {enemy.EnemyAttackType()}, doing {enemy.EnemyAttack()} damage.\r\n");
                                        wizert.TakeDamage(enemy.EnemyAttack());
                                    }
                                    else
                                    {
                                        Console.WriteLine("Heal failed! Not enough Magicka!");
                                        Console.WriteLine($"\r\n{enemy.enemyName} attacks with {enemy.EnemyAttackType()}, doing {enemy.EnemyAttack()} damage.\r\n");
                                        wizert.TakeDamage(enemy.EnemyAttack());
                                    }


                                    //check if wizert is killed
                                    if (wizert.GetCurrentHealth() <= 0)
                                    {
                                        Console.WriteLine("\r\nThe Wizert has been mortally wounded! Game Over.");
                                        battle = false;
                                        run = false;
                                        replay = true;
                                        break;
                                    }
                                    Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth());
                                    Console.WriteLine("Wizert Magicka: " + wizert.GetMagickaPoints() + "\r\n");

                                    break;

                                case "3":
                                    Console.WriteLine("\r\nYou attempt to flee!");

                                    Random random = new Random();

                                    if (random.Next(0, 2) == 0)
                                    {
                                        Console.WriteLine("\r\nFlee attempt successful!\r\n");
                                        battle = false;
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("\r\nFlee unsuccessful\r\n");
                                        Console.WriteLine($"\r\n{enemy.enemyName} attacks with {enemy.EnemyAttackType()}, doing {enemy.EnemyAttack()} damage.\r\n");
                                        wizert.TakeDamage(enemy.EnemyAttack());
                                        battle = true;
                                        break;
                                    }

                                default:
                                    Console.WriteLine("Not a valid Entry. Try Again.");
                                    break;
                            }
                        }

                    }

                    if (wizert.WizertLocation() == ExitLocation && wizert.GetCurrentHealth() > 0)
                    {
                        wizert.PrintExitFoundMessage();
                        start = false;
                        run = false;
                        replay = true;
                    }
                }
                break;

            case "3":
                Console.WriteLine("\r\nYou travel East...\r\n");
                if (wizert.MoveEast())
                {

                    if (powerup != null)
                    {
                        if (powerup.powerupName == "Health Potion")
                        {
                            Console.WriteLine($"\r\nYou have found a {powerup.powerupName}! Restore {powerup.PointsRestored()} Health Points!\r\n");
                            wizert.ReceivePowerup(powerup.PointsRestored());
                            powerups.Remove(powerup);
                            Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth());
                            Console.WriteLine("Wizert Magicka: " + wizert.GetMagickaPoints() + "\r\n");
                        }
                        else
                        {
                            Console.WriteLine($"\r\nYou have found a {powerup.powerupName}! Restore {powerup.PointsRestored()} Magicka Points!\r\n");
                            wizert.ReceivePowerup(powerup.PointsRestored());
                            powerups.Remove(powerup);
                            Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth());
                            Console.WriteLine("Wizert Magicka: " + wizert.GetMagickaPoints() + "\r\n");
                        }
                    }

                    //if enemy is not null, intitate a battle
                    if (enemy != null)
                    {
                        //bool for battle loop
                        bool battle = true;
                        while (battle)
                        {

                            Console.WriteLine($"\r\nYou have encountered a {enemy.enemyName}.\r\n" +
                            $"Its current HP is {enemy.GetEnemyHealth()}.\r\n" +
                            "Press...\r\n" +
                            "1. To Attack\r\n" +
                            "2. To Heal\r\n" +
                            "3. To Attempt to Flee\r\n");
                            //List wizert health and magicka after each round
                            Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth());
                            Console.WriteLine("Wizert Magicka: " + wizert.GetMagickaPoints());

                            var choice = Console.ReadLine();
                            //swicth statement for battles
                            switch (choice)
                            {
                                case "1":
                                    //wizert attack
                                    if (wizert.GetMagickaPoints() >= 3)
                                    {
                                        //wizert attacks for 5, if enemy is still alive, enemy attacks at end of case
                                        Console.WriteLine($"\r\nYou use {wizert.MPCost(choice)} MP to attack with Fireball! It does 5 Damage to the {enemy.enemyName}.\r\n");
                                        wizert.UseFireBall();
                                        //enemy taking damage - always 5
                                        enemy.TakeDamage();
                                        //check if enemy is killed
                                        if (enemy.GetEnemyHealth() <= 0)
                                        {
                                            //remove enemy from list
                                            enemies.Remove(enemy);
                                            Console.WriteLine("\r\nEnemy Destroyed!\r\n");
                                            Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth());
                                            Console.WriteLine("Wizert Magicka: " + wizert.GetMagickaPoints() + "\r\n");
                                            battle = false;
                                            break;
                                        }

                                        Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth());
                                        Console.WriteLine("Wizert Magicka: " + wizert.GetMagickaPoints());

                                        //enemy attack if enemy is alive
                                        Console.WriteLine($"\r\n{enemy.enemyName} attacks with {enemy.EnemyAttackType()}, doing {enemy.EnemyAttack()} damage.\r\n");
                                        wizert.TakeDamage(enemy.EnemyAttack());

                                        //check if wizert is killed
                                        if (wizert.GetCurrentHealth() <= 0)
                                        {
                                            Console.WriteLine("\r\nThe Wizert has been mortally wounded! Game Over.");
                                            battle = false;
                                            run = false;
                                            replay = true;
                                            break;
                                        }
                                        Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth() + "\r\n");
                                    }
                                    else
                                    {
                                        Console.WriteLine("\r\nNot enough Magicka Points!\r\n");
                                        battle = true;
                                        break;
                                    }

                                    break;

                                case "2":
                                    if (wizert.GetMagickaPoints() >= 5)
                                    {
                                        Console.WriteLine($"Using {wizert.MPCost(choice)} MP, you successfully heal for 3 HP!");
                                        //wizert heals for 3
                                        wizert.Heal();
                                        Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth() + "\r\n");
                                        //enemy attack if enemy is alive
                                        Console.WriteLine($"{enemy.enemyName} attacks with {enemy.EnemyAttackType()}, doing {enemy.EnemyAttack()} damage.\r\n");
                                        wizert.TakeDamage(enemy.EnemyAttack());
                                    }
                                    else
                                    {
                                        Console.WriteLine("Heal failed! Not enough Magicka!");
                                        Console.WriteLine($"\r\n{enemy.enemyName} attacks with {enemy.EnemyAttackType()}, doing {enemy.EnemyAttack()} damage.\r\n");
                                        wizert.TakeDamage(enemy.EnemyAttack());
                                    }


                                    //check if wizert is killed
                                    if (wizert.GetCurrentHealth() <= 0)
                                    {
                                        Console.WriteLine("\r\nThe Wizert has been mortally wounded! Game Over.");
                                        battle = false;
                                        run = false;
                                        replay = true;
                                        break;
                                    }
                                    Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth());
                                    Console.WriteLine("Wizert Magicka: " + wizert.GetMagickaPoints() + "\r\n");

                                    break;

                                case "3":
                                    Console.WriteLine("\r\nYou attempt to flee!");

                                    Random random = new Random();

                                    if (random.Next(0, 2) == 0)
                                    {
                                        Console.WriteLine("\r\nFlee attempt successful!\r\n");
                                        battle = false;
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("\r\nFlee unsuccessful\r\n");
                                        Console.WriteLine($"\r\n{enemy.enemyName} attacks with {enemy.EnemyAttackType()}, doing {enemy.EnemyAttack()} damage.\r\n");
                                        wizert.TakeDamage(enemy.EnemyAttack());
                                        battle = true;
                                        break;
                                    }

                                default:
                                    Console.WriteLine("Not a valid Entry. Try Again.");
                                    break;
                            }
                        }

                    }

                    if (wizert.WizertLocation() == ExitLocation && wizert.GetCurrentHealth() > 0)
                    {
                        wizert.PrintExitFoundMessage();
                        start = false;
                        run = false;
                        replay = true;
                    }
                }
                break;

            case "4":
                Console.WriteLine("\r\nYou travel West...\r\n");
                if (wizert.MoveWest())
                    {

                        if (powerup != null)
                        {
                            if (powerup.powerupName == "Health Potion")
                            {
                                Console.WriteLine($"\r\nYou have found a {powerup.powerupName}! Restore {powerup.PointsRestored()} Health Points!\r\n");
                                wizert.ReceivePowerup(powerup.PointsRestored());
                                powerups.Remove(powerup);
                                Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth());
                                Console.WriteLine("Wizert Magicka: " + wizert.GetMagickaPoints() + "\r\n");
                            }
                            else
                            {
                                Console.WriteLine($"\r\nYou have found a {powerup.powerupName}! Restore {powerup.PointsRestored()} Magicka Points!\r\n");
                                wizert.ReceivePowerup(powerup.PointsRestored());
                                powerups.Remove(powerup);
                                Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth());
                                Console.WriteLine("Wizert Magicka: " + wizert.GetMagickaPoints() + "\r\n");
                            }
                        }

                        //if enemy is not null, intitate a battle
                        if (enemy != null)
                        {
                            //bool for battle loop
                            bool battle = true;
                            while (battle)
                            {

                                Console.WriteLine($"\r\nYou have encountered a {enemy.enemyName}.\r\n" +
                                $"Its current HP is {enemy.GetEnemyHealth()}.\r\n" +
                                "Press...\r\n" +
                                "1. To Attack\r\n" +
                                "2. To Heal\r\n" +
                                "3. To Attempt to Flee\r\n");
                                //List wizert health and magicka after each round
                                Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth());
                                Console.WriteLine("Wizert Magicka: " + wizert.GetMagickaPoints());

                                var choice = Console.ReadLine();
                                //swicth statement for battles
                                switch (choice)
                                {
                                    case "1":
                                        //wizert attack
                                        if (wizert.GetMagickaPoints() >= 3)
                                        {
                                            //wizert attacks for 5, if enemy is still alive, enemy attacks at end of case
                                            Console.WriteLine($"\r\nYou use {wizert.MPCost(choice)} MP to attack with Fireball! It does 5 Damage to the {enemy.enemyName}.\r\n");
                                            wizert.UseFireBall();
                                            //enemy taking damage - always 5
                                            enemy.TakeDamage();
                                            //check if enemy is killed
                                            if (enemy.GetEnemyHealth() <= 0)
                                            {
                                                //remove enemy from list
                                                enemies.Remove(enemy);
                                                Console.WriteLine("\r\nEnemy Destroyed!\r\n");
                                                Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth());
                                                Console.WriteLine("Wizert Magicka: " + wizert.GetMagickaPoints() + "\r\n");
                                                battle = false;
                                                break;
                                            }

                                            Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth());
                                            Console.WriteLine("Wizert Magicka: " + wizert.GetMagickaPoints());

                                            //enemy attack if enemy is alive
                                            Console.WriteLine($"\r\n{enemy.enemyName} attacks with {enemy.EnemyAttackType()}, doing {enemy.EnemyAttack()} damage.\r\n");
                                            wizert.TakeDamage(enemy.EnemyAttack());

                                            //check if wizert is killed
                                            if (wizert.GetCurrentHealth() <= 0)
                                            {
                                                Console.WriteLine("\r\nThe Wizert has been mortally wounded! Game Over.");
                                                battle = false;
                                                run = false;
                                                replay = true;
                                                break;
                                            }
                                            Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth() + "\r\n");
                                        }
                                        else
                                        {
                                            Console.WriteLine("\r\nNot enough Magicka Points!\r\n");
                                            battle = true;
                                            break;
                                        }

                                        break;

                                    case "2":
                                        if (wizert.GetMagickaPoints() >= 5)
                                        {
                                            Console.WriteLine($"Using {wizert.MPCost(choice)} MP, you successfully heal for 3 HP!");
                                            //wizert heals for 3
                                            wizert.Heal();
                                            Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth() + "\r\n");
                                            //enemy attack if enemy is alive
                                            Console.WriteLine($"{enemy.enemyName} attacks with {enemy.EnemyAttackType()}, doing {enemy.EnemyAttack()} damage.\r\n");
                                            wizert.TakeDamage(enemy.EnemyAttack());
                                        }
                                        else
                                        {
                                            Console.WriteLine("Heal failed! Not enough Magicka!");
                                            Console.WriteLine($"\r\n{enemy.enemyName} attacks with {enemy.EnemyAttackType()}, doing {enemy.EnemyAttack()} damage.\r\n");
                                            wizert.TakeDamage(enemy.EnemyAttack());
                                        }


                                        //check if wizert is killed
                                        if (wizert.GetCurrentHealth() <= 0)
                                        {
                                            Console.WriteLine("The Wizert has been mortally wounded. Game Over.");
                                            battle = false;
                                            run = false;
                                            replay = true;
                                            break;
                                        }
                                        Console.WriteLine("Wizert Health: " + wizert.GetCurrentHealth());
                                        Console.WriteLine("Wizert Magicka: " + wizert.GetMagickaPoints() + "\r\n");

                                        break;

                                    case "3":
                                        Console.WriteLine("\r\nYou attempt to flee!");

                                        Random random = new Random();

                                        if (random.Next(0, 2) == 0)
                                        {
                                            Console.WriteLine("\r\nFlee attempt successful!\r\n");
                                            battle = false;
                                            break;
                                        }
                                        else
                                        {
                                            Console.WriteLine("\r\nFlee unsuccessful\r\n");
                                            Console.WriteLine($"\r\n{enemy.enemyName} attacks with {enemy.EnemyAttackType()}, doing {enemy.EnemyAttack()} damage.\r\n");
                                            wizert.TakeDamage(enemy.EnemyAttack());
                                            battle = true;
                                            break;
                                        }

                                    default:
                                        Console.WriteLine("Not a valid Entry. Try Again.");
                                        break;
                                }
                            }

                        }

                        if (wizert.WizertLocation() == ExitLocation && wizert.GetCurrentHealth() > 0)
                        {
                            wizert.PrintExitFoundMessage();
                            start = false;
                            run = false;
                            replay = true;
                        }
                    }
                break;
                

            default:
                Console.WriteLine("Not a valid Entry. Try Again.\r\n");
                break;
        }

        while (replay)
        {
            Console.WriteLine("\r\nWould you like to play again? (y/n)");
            string again = Console.ReadLine();

            again = again.ToLower();

            if (again == "n")
            {
                replay = false;
                run = false;
                start = false;
            }
            else if (again == "y")
            {
                replay = false;
                run = false;
                start = true;
            }
            else
            {
                Console.WriteLine("Not valid input. Try again.");
                replay = true;
            }
        }

    }
}
