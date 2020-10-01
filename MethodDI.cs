using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MethodDependencyInjection
{

    class Program
    {

        static void Main(string[] args)
        {

            Warrior warrior = Generator.MakeWarrior();
            Preparation p = Generator.MakePreperations();
            p.BattlePreperation();
            //Method DI
            warrior.EquipWeapon(new Weapon());
            warrior.EquipShield(new Shield());


            IDragon dragon = Generator.MakeDragon(p.dragonType);
            IBattle Battle = Generator.Battle(p.sword, p.shield);
            Battle.Clash(warrior, dragon);

            Battle.BattleOutcome(warrior, dragon);

            Console.ReadKey();
        }

    }


    public class Preparation
    {
        public string sword;
        public string shield;
        public int dragonType;
        public void BattlePreperation()
        {
            Console.WriteLine("Enter sword you want to equip: ");
            sword = Console.ReadLine();
            Console.WriteLine("Enter shield you want to equip: ");
            shield = Console.ReadLine();
            Console.WriteLine("Enter type of dragon you wish to face: \n 1 - Fire dragon \n 2 - Ice dragon ");
            dragonType = int.Parse(Console.ReadLine());
        }

    }

    public interface IBattle
    {
        void Clash(Warrior warrior, IDragon dragon);

        void BattleStatus(Warrior warrior, IDragon dragon);
        void BattleOutcome(Warrior warrior, IDragon dragon);
        string sword { get; set; }
        string shield { get; set; }

    }

    public class Battle : IBattle
    {
        public string sword { get; set; }
        public string shield { get; set; }


        public Battle(string sword, string shield)
        {
            this.sword = sword;
            this.shield = shield;

        }

        public void Clash(Warrior warrior, IDragon dragon)
        {

            while (warrior.health > 0 && dragon.health > 0)
            {
                Console.WriteLine("Choose from options: \n 1 - Sword Attack \n 2 - Shield Block  \n 3 - Sword Parry \n 4 - Shield Attack");
                string choice = Console.ReadLine();


                if (choice == "")
                {
                    Console.WriteLine("Enter options from 1-4");
                }
                else
                {
                    switch (choice)
                    {

                        case "1":
                            warrior.SwordAttack(sword, dragon.DragonType);
                            dragon.health -= 15;
                            BattleStatus(warrior, dragon);
                            break;
                        case "2":
                            dragon.DragonsBreath();
                            warrior.ShieldBlock(shield, dragon.DragonType);
                            warrior.health -= 15;
                            BattleStatus(warrior, dragon);
                            break;
                        case "3":
                            dragon.MeleeAttack();
                            warrior.SwordParry(sword, dragon.DragonType);
                            warrior.health -= 5;
                            BattleStatus(warrior, dragon);
                            break;
                        case "4":
                            warrior.ShieldAttack(sword, dragon.DragonType);
                            dragon.health -= 5;
                            dragon.MeleeAttack();
                            warrior.health -= 15;
                            BattleStatus(warrior, dragon);
                            break;
                        default:
                            Console.WriteLine("Choose options from 1-4");
                            break;

                    }
                }
            }
        }

        public void DragonWins(Warrior warrior)
        {
            if (warrior.health < 0)
            {
                Console.WriteLine("Overkill!");
            }
            Console.WriteLine($"DEFEAT, Dragon slained {warrior.GetType().Name}");
        }

        public void WarriorWins(IDragon dragon)
        {
            if (dragon.health < 0)
            {
                Console.WriteLine("Overkill!");
            }
            Console.WriteLine($"VICTORY, Warrior slained {dragon.DragonType}");
        }


        public void BattleStatus(Warrior warrior, IDragon dragon)
        {
            Console.WriteLine($"Warrior hp: {warrior.health} | Dragon health: {dragon.health}");
        }


        public void BattleOutcome(Warrior warrior, IDragon dragon)
        {


            if (warrior.health <= 0 && dragon.health <= 0)
            {
                Console.WriteLine("Both warrior and dragon have fallen.");
            }
            else if (warrior.health <= 0)
            {
                DragonWins(warrior);
            }
            else
            {
                WarriorWins(dragon);
            }


        }

    }


    public interface ISword
    {
        void SwordHit(string sword, string target);
        void SwordParry(string sword, string target);

    }

    public class Weapon : ISword
    {

        public void SwordHit(string sword, string target)
        {

            Console.WriteLine($"Heroic strike with {sword} hits {target}");
        }

        public void SwordParry(string sword, string target)
        {
            Console.WriteLine($"{sword} parries {target}'s attack");
        }
    }

    public interface IShield
    {
        void ShieldBlock(string shield, string target);
        void ShieldAttack(string shield, string target);

    }


    public class Shield : IShield
    {
        public void ShieldAttack(string shield, string target)
        {
            Console.WriteLine($"{target} attacked with {shield}");
        }

        public void ShieldBlock(string shield, string target)
        {
            Console.WriteLine($"{shield} blocked {target}'s damage");
        }
    }

    public class Warrior
    {

        public int health;

        private ISword _sword;
        private IShield _shield;

        public Warrior(int health)
        {
            this.health = health;

        }

        public void EquipWeapon(ISword sword)
        {
            _sword = sword;
        }

        public void EquipShield(IShield shield)
        {
            _shield = shield;
        }

        public void SwordAttack(string sword, string target)
        {
            _sword.SwordHit(sword, target);

        }

        public void SwordParry(string sword, string target)
        {
            _sword.SwordParry(sword, target);

        }

        public void ShieldAttack(string shield, string target)
        {
            _shield.ShieldAttack(shield, target);
        }

        public void ShieldBlock(string shield, string target)
        {
            _shield.ShieldBlock(shield, target);
        }

    }




    public interface IDragon
    {
        void DragonsBreath();
        void MeleeAttack();
        int health { get; set; }
        string DragonType { get; set; }
    }


    public class FireDragon : IDragon
    {
        public int health { get; set; }

        public string DragonType { get; set; }
        public FireDragon(int health, string dragonType)
        {
            this.health = health;
            DragonType = dragonType;
        }

        public FireDragon()
        {

        }

        public void DragonsBreath()
        {
            Console.WriteLine($"{DragonType} breathes fire");
        }

        public void MeleeAttack()
        {
            Console.WriteLine($"{DragonType} swipes battlefield");

        }
    }


    public class IceDragon : IDragon
    {
        public int health { get; set; }

        public string DragonType { get; set; }

        public IceDragon(int health, string dragonType)
        {
            this.health = health;
            DragonType = dragonType;

        }
        public void DragonsBreath()
        {
            Console.WriteLine($"{DragonType} breathes ice");
        }

        public void MeleeAttack()
        {
            Console.WriteLine($"{DragonType} swipes battlefield");

        }
    }
    public static class Generator
    {
        

        public static Warrior MakeWarrior()
        {
            return new Warrior(100);
        }

        public static IDragon MakeDragon(int dragonType)
        {
            IDragon dragon = new FireDragon();
            //Implementation of Inversion of Control
            //based on input making different objects
            if (dragonType == 1)
            {
                dragon.DragonType = "Fire dragon";
                return new FireDragon(150, dragon.DragonType);
            }
            else
            {
                dragon.DragonType = "Ice dragon";
                return new IceDragon(150, dragon.DragonType);
            }

        }

        public static Preparation MakePreperations()
        {
            return new Preparation();
        }

        public static Battle Battle(string sword, string shield)
        {
            return new Battle(sword, shield);
        }
    }


}
