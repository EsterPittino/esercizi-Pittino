using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArenaClasses;

namespace E05_exercise_13_arena
{
    class Program
    {
        #region consegna
        // ## 13.1 - ARENA - simpler version
        // We have an arena with different characters fighting one against the other.
        // 
        // Every character has an epic name, an amount of health-points, and a speed.When he is attacked, he loses a certain amount of health-point.
        // 
        // There are 3 types of character:
        // 1) `Warrior`: has a property `DamagePercentage`, that tells which percentage of health-points is removed from the adversary: if the adversary has 80 hp and the percentage is 10%, the attack will subtract 8 hp.Is the hp to remove are less than 5, the attack removes exactly 5 hp.
        // 2) `Wizard`: has a property `MaximumPower`, that tells the max magical power available, ma magic is a mistery, so the hp to remove are calculated every time multiplying the max power with a random number between 0 and 1.
        // 3) `Ork`: has a property `Strength`, that tells how many fixed hp are removed from the adversary.
        // 
        // The game is divided in turns.At every turn, every character has the possibility to fight.
        // 
        // The characters attack in order of speed: the faster obviously attacks first.The game select a target randomly between the other fighters(a character cannot attack himself or a dead character).
        // 
        // When a character finishes its health - points, he dies.
        // 
        // The last living character wins.
        // 
        // The Arena class receives a list of characters, and has a method `StartFight` in which it handles the turns and calculates the winner.
        // 
        // Create a list of characters, with different types, names, strength, etc.
        // 
        // Create an instance of the Arena, passing the list of characters, and start the game.
        // 
        // Every time something happens(a character strikes, a character dies, a character wins, ...) a message is printed on the Console(ex: `"The Ork Gurg attacked Gandalf il Grigio taking 20 hp"`).
        // 
        // The characters can print on the Console, and so can the Arena.
        // 
        // 
        // ## 13.2 ARENA - more object oriented *
        // As 13.1, but we want to decouple the operations from the printing on the Console.
        // 
        // Every class (the classes for the fighters and the Arena) receive in the constructor an instance of an `INotifier`, an interface with a method `Notify(string message)`. Every time an object has something to notify(ex: a warrior died), the `Notify()` method is called.
        // 
        // Since it's just an interface, the other objects don't know where these messages are going.
        // 
        // Create a class `ConsoleNotifier` that implements `INotifier` and prints on the Console the messages.
        // 
        // Instantiate this class in the `Main()`, and pass it to all the other objects.
        // 
        // Then start the game.
        // 
        #endregion

        // Ho scelto questo come terzo esercizio perché ha un po' tutto quello che abbiamo visto finora: ereditarietà,
        // polimorfismo, classi astratte, interfacce... 
        // E anche perché sono nerd nell'animo.

        // Ho fatto un uso molto empirico dei random, ho capito in teoria come funzionano ma non sono molto sicura nell'applicazione

        static void Main(string[] args)
        {
            var myConsole = new ConsoleNotifier();  // creo il mio notificatore
            myConsole.SetName("Arena");             // assegno il nome al notificatore
            var myArena = new Arena(myConsole);     // creo l'arena (vuota), indicandole a chi deve inviare le notifiche

            while (true)   // non ho implementato metodi per accettare un input, per ora lo lascio in ciclo infinito
            {
                myConsole.Refresh();   // svuoto il notificatore per un nuovo combattimento

                List<Hero> myHorde = GenerateHorde(myConsole); // Creo una lista casuale di eroi; tutti faranno riferimento al notificatore myConsole
                myArena.Horde = myHorde;   // assegno l'orda all'arena

                myArena.StartToFight();   // inizio il cobattimento
                myConsole.Wait();         // alla fine, metto il notificatore in wait prima di iniziare un nuovo combattimento
            }
        }

        private static List<Hero> GenerateHorde(ConsoleNotifier myConsole)
        {
            var myRandom = new Random();
            var myHorde = new List<Hero>();

            var numberOfHeroes = myRandom.Next(2, 11); // genero un numero casuale di eroi tra 2 e 10
            for (int i = 1; i <= numberOfHeroes; i++)
            {
                Hero hero = GenerateHero(myConsole, myRandom);
                myHorde.Add(hero);
            }

            return myHorde;
        }

        private static Hero GenerateHero(ConsoleNotifier myConsole, Random myRandom)
        {
            Hero hero;
            var heroName = GenerateName(myRandom);
            var heroHealth = myRandom.Next(50, 151);
            var heroSpeed = myRandom.Next(0, 101);

            var heroType = myRandom.Next(1, 4);
            switch (heroType)
            {
                case 1:
                    var heroPrecision = myRandom.Next(10, 51);
                    hero = new Warrior(myConsole, heroName, heroHealth, heroSpeed, heroPrecision);
                    break;
                case 2:
                    var heroPower = myRandom.Next(25, 76);
                    hero = new Wizard(myConsole, heroName, heroHealth, heroSpeed, heroPower);
                    break;
                case 3:
                    var heroStrength = myRandom.Next(5, 26);
                    hero = new Ork(myConsole, heroName, heroHealth, heroSpeed, heroStrength);
                    break;
                default:
                    throw new NotImplementedException();
            }

            return hero;
        }

        private static string GenerateName(Random myRandom)
        {
            var NameCollection = new List<string> { "Ahura", "Alejandro", "Baltazar", "Beowulf", "Gawain", "Hector", "Leandro", "Tristan", "Tyr", "Wieland", "Acanta", "Adrastrea", "Aphrodite", "Amalthea", "Calliope", "Cassandra", "Demetra", "Freyja", "Hersilia", "Morgana", "Olwen", "Persefone", "Shakti" };
            return NameCollection.ElementAt(myRandom.Next(0, NameCollection.Count));
            // si potrebbe aggiungere anche delle liste di titoli e aggettivi per comporre nomi più epici, tipo "Beowulf the merciless Prince"
            // per ora lascio solo il nome
        }
    }
}

namespace ArenaClasses
{
    #region gestione notifiche
    // Every class (the classes for the fighters and the Arena) receive in the constructor an instance of an `INotifier`,
    // an interface with a method `Notify(string message)`. Every time an object has something to notify(ex: a warrior died), 
    // the `Notify()` method is called.

    interface INotifier
    {
        // `INotifier`, an interface with a method `Notify(string message)`
        void Notify(string message);

        // creo dei metodi per notifiche ad alta o bassa importanza
        void HighNotify(string message);
        void LowNotify(string message);

        // creo dei metodi per rendere più leggibili le notifiche, con degli 'stacchi'
        void LittleBreak();
        void MediumBreak();
        void LargeBreak();

        // creo un metodo che mi permetta di mettere in pausa le notifiche (con o senza messaggio personalizzato)
        void Wait(string message);
        void Wait();

        // creo un metodo per assegnare un nome al notificatore
        void SetName(string name);

        // creo un metodo che mi permetta di ripulire il notificatore
        void Refresh();
    }

    class ConsoleNotifier : INotifier
    {
        // Ho riportato nel notificatore alcuni dei metodi che avevo creato per la mia ConsoleUtilities
        #region costanti ConsoleColor
        const ConsoleColor WRITING_COLOR = ConsoleColor.Gray;
        const ConsoleColor READING_COLOR = ConsoleColor.Cyan;
        const ConsoleColor LOW_COLOR = ConsoleColor.DarkGray;
        const ConsoleColor EVIDENCE_COLOR = ConsoleColor.White;
        #endregion

        // Create a class `ConsoleNotifier` that implements `INotifier` and prints on the Console the messages.
        public void Notify(string message)
        {
            Console.WriteLine(message);
        }
        
        // stampa in bianco
        public void HighNotify(string message)
        {
            var actualColor = Console.ForegroundColor;
            Console.ForegroundColor = EVIDENCE_COLOR;
            Console.WriteLine(message);
            Console.ForegroundColor = actualColor;
        }

        // stampa in grigio scuro
        public void LowNotify(string message)
        {
            var actualColor = Console.ForegroundColor;
            Console.ForegroundColor = LOW_COLOR;
            Console.WriteLine(message);
            Console.ForegroundColor = actualColor;
        }

        // stampa una riga vuota
        public void LittleBreak() => Console.WriteLine();

        // stampa una riga di '-'
        public void MediumBreak()
        {
            var s = new string('-', (Console.BufferWidth - 1));
            Console.WriteLine();
            Console.WriteLine(s);
            Console.WriteLine();
        }

        // stampa una riga di '=' evidenziati
        public void LargeBreak()
        {
            var s = new string('=', (Console.BufferWidth - 1));
            Console.WriteLine();
            HighNotify(s);
            Console.WriteLine();
        }

        // stampo un messaggio e aspetto un read
        public void Wait() => Wait("");   // se lo chiamo senza parametri, è come se lo chiamassi con stringa vuota
        public void Wait(string message)
        {
            if (!string.IsNullOrWhiteSpace(message)) Console.WriteLine(message);
            LowNotify("Premi un tasto qualsiasi per continuare.");
            Console.ReadLine();
        }

        // assegno il nome al titolo della console
        public void SetName(string name) => Console.Title = name;

        // svuoto la console
        public void Refresh() => Console.Clear();
    }
    #endregion

    #region eroi
    abstract class Hero
    {
        // Every character has an epic name, an amount of health-points, and a speed. 
        // When he is attacked, he loses a certain amount of health-point.

        #region costruttori
        protected Hero(INotifier notifier, string epicName, int health, int speed)
        {
            this.Notifier = notifier;
            this.EpicName = epicName;
            this.Health = health;
            this.Speed = speed;
        }
        #endregion

        #region proprietà
        public INotifier Notifier { get; set; }       // Non se se potrebbe mai esserci bisogno di cambiare Notifier, ma mi lascio aperta la possibilità
        public string EpicName { get; private set; }  // il nome non può essere cambiato dall'esterno
        public int Health { get; internal set; }      // la salute può essere modificata dalle classi derivate
        public int Speed { get; internal set; }       // la velocità nel nostro esempio non verrà mai modificata, ma in teoria potrebbe esserlo.
                                                      // Ad esempio, potremmo immaginare che accusare un colpo abbastanza potente la faccia diminuire
        public bool IsAlive => (this.Health > 0);     // Mi è sembrato comodo avere una proprietà che mi dica se l'eroe è vivo o morto
        #endregion

        public override string ToString() => $"{this.GetType().Name}: {this.EpicName} ({this.Health:#,0} pf / iniziativa {this.Speed:#,0})";

        // Ho scelto di dichiarare il metodo per gestire i danni subiti "virtual", anche se nel nostro esempio non sarà mai sovrascritto,
        // perchè potrebbe esserlo.
        // Ad esempio potremmo immaginare che gli orchi hanno la pellaccia dura e sottraggono un certo valore da ogni colpo subito.
        public virtual void RecuseDamage(int damage)
        {
            this.Health -= damage;
            if (! this.IsAlive) this.Notifier.HighNotify($"Il {this.GetType().Name} {this.EpicName} è morto!");
        }

        // Ho dichiarato 'abstract' questo metodo perché non ha un'implementazione di base, ognuno la declina a modo suo
        public abstract void Attack(Hero target);
    }

    class Warrior : Hero
    {
        // `Warrior`: has a property `DamagePercentage`, that tells which percentage of health-points is removed from the adversary: 
        // if the adversary has 80 hp and the percentage is 10%, the attack will subtract 8 hp. 
        // If the hp to remove are less than 5, the attack removes exactly 5 hp.

        #region costruttori
        public Warrior(INotifier notifier, string epicName, int health, int speed, int damagePercentage) : base(notifier, epicName, health, speed)
        {
            this.DamagePercentage = damagePercentage;
        }
        #endregion

        #region proprietà
        public int DamagePercentage { get; private set; }
        #endregion

        public override string ToString() => $"{base.ToString()} - Precisione: {DamagePercentage}";

        public override void Attack(Hero target)
        {
            var damage = target.Health / 100.00 * this.DamagePercentage;  // lo calcolo double, altrimenti qualunque numero inferiore a 100 diviso per 100 mi dà 0
            var intDamage = (int)damage;       // trasformo il danno calcolato come double in integer
            if (intDamage < 5) intDamage = 5;  // setto il danno minimo

            this.Notifier.Notify($"Il {this.GetType().Name} {this.EpicName} attacca il {target.GetType().Name} {target.EpicName} e gli infligge {intDamage} danni");
            target.RecuseDamage(intDamage);
        }
    }

    class Wizard : Hero
    {
        // `Wizard`: has a property `MaximumPower`, that tells the max magical power available, ma magic is a mistery, 
        // so the hp to remove are calculated every time multiplying the max power with a random number between 0 and 1.

        #region costruttori
        public Wizard(INotifier notifier, string epicName, int health, int speed, int maximumPower) : base(notifier, epicName, health, speed)
        {
            this.MaximumPower = maximumPower;
            this.powerMultiplier = new Random();  // ha più senso creare un suo random di istanza e poi chiamarci il next ad ogni attacco,  
                                                  // oppure creare un nuovo random all'interno del metodo Attack?
        }
        #endregion

        #region proprietà
        public int MaximumPower { get; private set; }
        #endregion

        #region campi
        private Random powerMultiplier;
        #endregion

        public override string ToString() => $"{base.ToString()} - Potere: {MaximumPower}";

        public override void Attack(Hero target)
        {
            var damage = MaximumPower * powerMultiplier.NextDouble();  // come per il guerriero, faccio i calcoli su campi double e poi li casto in integer
            var intDamage = (int)damage;

            this.Notifier.Notify($"Il {this.GetType().Name} {this.EpicName} attacca il {target.GetType().Name} {target.EpicName} e gli infligge {intDamage} danni");
            target.RecuseDamage(intDamage);
        }
    }

    class Ork : Hero
    {
        // `Ork`: has a property `Strength`, that tells how many fixed hp are removed from the adversary. 

        #region costruttori
        public Ork(INotifier notifier, string epicName, int health, int speed, int strength) : base(notifier, epicName, health, speed)
        {
            this.Strength = strength;
        }
        #endregion

        #region proprietà
        public int Strength { get; private set; }
        #endregion

        public override string ToString() => $"{base.ToString()} - Forza: {this.Strength}";

        public override void Attack(Hero target)
        {
            var intDamage = this.Strength;
            this.Notifier.Notify($"Il {this.GetType().Name} {this.EpicName} attacca il {target.GetType().Name} {target.EpicName} e gli infligge {intDamage} danni");
            target.RecuseDamage(intDamage);
        }
    }
    #endregion

    #region arena
    class Arena
    {
        #region costruttori
        public Arena(INotifier notifier)
        {
            // Non metterei la lista di eroi nel costruttore. L'arena esiste a prescindere da chi ci combatte dentro.
            Notifier = notifier;
        }
        #endregion

        #region proprietà
        public INotifier Notifier { get; set; }    // Non se se può mai succedere che il Notifier cambi dopo l'inizializzazione, ma mi lascio aperta la possibilità
        public List<Hero> Horde { get; set; }      // Un'arena può ospitare diversi combattimenti, la lista deve poter essere modificata
        private IOrderedEnumerable<Hero> HeroesBySpeed => this.Horde.OrderByDescending(Hero => Hero.Speed);  // Mi è sembrato comodo avere una proprità per l'ordinamento
        #endregion


        public void StartToFight()
        {
            this.Notifier.LargeBreak();
            this.Notifier.HighNotify("   ***   Inizia un nuovo combattimento nell'arena!   ***   ");
            this.Notifier.LargeBreak();

            while (Horde.Count(x => x.IsAlive) > 1) this.StartNewRound();

            var winner = Horde.Find(x => x.IsAlive);  // quando esco dal ciclo, l'unico rimasto in vita è il vincitore
            NotifyState();
            Notifier.Wait("Fine del combattimento.");
            Notifier.LargeBreak();
            Notifier.HighNotify($"   ***   {winner.EpicName} è il campione dell'arena!   ***   ");
            Notifier.LargeBreak();
        }

        private void StartNewRound()
        {
            // At every turn, every character has the possibility to fight.
            // The characters attack in order of speed: the faster obviously attacks first.
            // The game select a target randomly between the other fighters(a character cannot attack himself or a dead character).
            var myRandom = new Random();
                        
            this.NotifyState();

            // Se ho ben capito come funziona l'ordinamento sulle liste, con la proprietà 
            // private IOrderedEnumerable<Hero> OrderedHeroes => this.Horde.OrderByDescending(Hero => Hero.Speed);
            // creo una vista sulla lista Horde, quindi se le velocità dovessero cambiare tra un turno e l'altro 
            // dovrebbe riorganizzare la lettura automaticamente, giusto?
            // Oppure devo mettere qui l'istruzione di OrderBy, per ricalcolarla ad ogni turno?
            foreach (var hero in HeroesBySpeed)
            {
                if (hero.IsAlive)
                {
                    // creo una nuova lista dei bersagli validi, escludendo sè stesso e tutti gli eroi morti
                    var validTarget = Horde.ToList();
                    validTarget.Remove(hero);
                    validTarget.RemoveAll(x => ! x.IsAlive);

                    if (validTarget.Count > 0)
                    {
                        var target = validTarget.ElementAt(myRandom.Next(0, validTarget.Count));
                        hero.Attack(target);
                    }
                }
            }

            this.Notifier.LittleBreak();
            this.Notifier.Wait("Turno finito.");
            this.Notifier.MediumBreak();
        }

        private void NotifyState()
        {
            this.Notifier.Notify("   ***   Combattenti   ***   ");
            var counter = 0;
            foreach (var hero in this.HeroesBySpeed)
            {
                counter++;
                if (hero.IsAlive) this.Notifier.Notify($"{counter,3}. {hero.ToString()}");
                else  this.Notifier.LowNotify($"{counter,3}. {hero.ToString()}");

            }
            this.Notifier.LittleBreak();
        }

    }
    #endregion
}
