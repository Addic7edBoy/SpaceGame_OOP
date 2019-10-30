using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;


namespace game
{
    class Galaxy {
        //List<string> planets;
        //public ISet<string> planets = new HashSet<string>();
        public IDictionary<string, Planet> planetsObj = new Dictionary<string, Planet>();
        public string text;
    
        public void addPlanet(string name) {
            planetsObj.Add(name, new Planet(name));
            showPlanets();

        }
        public void removePlanet(string name) {
            planetsObj.Remove(name);
            showPlanets();
        }
        public void showPlanets() {
            text = string.Join(", ", planetsObj.Values.Select(p => p.name));

        }
    }
    class Planet {
        public IDictionary<string, Colony> coloniesObj = new Dictionary<string, Colony>();
        public string text;
        public string name;

        public Planet(string name){
            this.name = name;
        }

        public void addColony(string name){
            coloniesObj.Add(name, new Colony(name));
            showColonies();
        }
        public void removeColony(string name){
            coloniesObj.Remove(name);
            showColonies();
        }
        public void showColonies(){
            text = string.Join(", ", coloniesObj.Values.Select(p => p.name));
        }
    }

    class Colony {
        public int coalMine = 0;
        public int energyFactory = 0;
        public int stoneMine = 0;
        private int energy = 10;
        private int coal = 10;
        private int stone = 10;
        private int factory = 0;
        public int house = 0;
        private bool _resSwitch = false;
        public string name;
        public Colony(string name) {
            this.name = name;
        }
        public bool resSwitch{
            get {return _resSwitch;}
            set {_resSwitch = value;}
        }

         
        public void addBuilding(Colony colony){
            Console.WriteLine("Which building you want to add");
            string choice = Console.ReadLine();
            switch(choice){
                case "house":
                buildings.addHouse(this);
                break;
                case "coalmine":
                buildings.addCoalMine(this);
                break;
                case "energyfactory":
                buildings.addEnergyFactory(colony);
                break;
                case "stonemine":
                buildings.addStoneMine(colony);
                break;
            }
        }
        public void removeBuilding(Colony colony){
            Console.WriteLine("Which building you want to remove");
            string choice = Console.ReadLine();
            switch(choice){
                case "house":
                Console.Clear();
                buildings.removeHouse(this);
                break;
                case "coalmine":
                Console.Clear();
                buildings.removeCoalMine(this);
                break;
                case "energyfactory":
                Console.Clear();
                buildings.removeEnergyFactory(this);
                break;
                case "stonemine":
                Console.Clear();
                buildings.removeStoneMine(this);
                break;
            }
        }
        public void showBuilding(){
            Console.WriteLine("Current Colony has: \n {0} houses \n {1} Energy factories \n {2} Stone Mines \n {3} Coal Mines", this.house, this.energyFactory, this.stoneMine, this.coalMine);
        }
        public void showResource(){
            //Console.WriteLine("Current Colony has: \n {0} energy \n {1} stone \n {2} coal", this.energy, this.stone, this.coal);
            Console.WriteLine("Energy: ");
            Console.WriteLine("Coal: ");
            Console.WriteLine("Stone: ");
            Console.WriteLine("\n Press any key to continue..");
            while (!Console.KeyAvailable) 
            {
                Program.WriteAt(this.energy, 8, 0);
                Program.WriteAt(this.coal, 6, 1);
                Program.WriteAt(this.stone, 7, 2);
                Thread.Sleep(5000);
            }
            Console.Clear();
        }
        public void resourceCountThread(){
            while (resSwitch){
                Thread.Sleep(5000);
                this.energy += 1 + 1 * this.energyFactory;
                this.stone += 1 + 1 * this.stoneMine;
                this.coal += 1 + 1 * this.coalMine;
            }
        }
    }
    class buildings {
        static public void addCoalMine(Colony colony){
            colony.coalMine += 1;
        }
        static public void addEnergyFactory(Colony colony){
            colony.energyFactory += 1;
        }
        static public void addStoneMine(Colony colony){
            colony.stoneMine += 1;
        }
        static public void addHouse(Colony colony){
            colony.house += 1;
        }
        static public void removeCoalMine(Colony colony){
            colony.coalMine -= 1;
        }
        static public void removeEnergyFactory(Colony colony){
            colony.energyFactory -= 1;
        }
        static public void removeStoneMine(Colony colony){
            colony.stoneMine -= 1;
        }
        static public void removeHouse(Colony colony){
            colony.house -= 1;
        }
    }
    class Program
    {
        static public Galaxy newGalaxy = new Galaxy();
        static public string help = "Use these commands to navigate: \n 'add' - Add an object \n 'remove' - remove an object \n 'back' - upper level \n 'choose' - choose an object \n 'show' - display data";
        static public string location = " Your current location: NewGalaxy";
        static public string objects = "Current {0} has {1}: {2}";
        static public string aoof = "=============================================================";
        protected static int origRow;
        protected static int origCol;

        public static void WriteAt(int s, int x, int y)
        {
            try
                {
                Console.SetCursorPosition(origCol+x, origRow+y);
                Console.Write(s);
                }
            catch (ArgumentOutOfRangeException e)
                {
                Console.Clear();
                Console.WriteLine(e.Message);
                }
        }
        static void Main(string[] args)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            GalaxyMenu(newGalaxy);
        }
        static void GalaxyMenu(Galaxy newGalaxy){
            while (true) {
                Console.WriteLine(help + "\n" + aoof + "\n" + location + '\n' + objects, "Galaxy", "Planets", newGalaxy.text + '\n' + aoof);
                string a = Console.ReadLine();
                Console.Clear();
                if (a == "add") {
                    Console.WriteLine("type a name for the planet");
                    string name = Console.ReadLine();
                    newGalaxy.addPlanet(name);
                    Console.Clear();
                }
                else if (a == "remove") {
                    Console.WriteLine("type a name for the planet");
                    string name = Console.ReadLine();
                    newGalaxy.removePlanet(name);
                    Console.Clear();
                }
                else if (a == "show") {
                    Console.Clear();
                    newGalaxy.showPlanets();
                }
                else if (a == "choose"){
                    Console.WriteLine("Choose planet from: [{0}]", newGalaxy.text);
                    string choice = Console.ReadLine();
                    Console.Clear();
                    if (Program.newGalaxy.planetsObj.Keys.Any(p => p == choice)){
                        foreach (var item in newGalaxy.planetsObj)
                        {
                            if (item.Key == choice){
                                PlanetMenu(newGalaxy.planetsObj[choice]);
                            }
                        }
                    }
                    break;
                }
            }
        
        }
        static void PlanetMenu(Planet planet){
            while (true) {
                Console.WriteLine(help + "\n" + aoof + "\n" + location + " --> Planet " + planet.name + '\n' + objects, "Planet", "Colonies", planet.text + '\n' + aoof);

                string b = Console.ReadLine();
                Console.Clear();
                if (b == "add") {
                    Console.WriteLine("Type a name for the colony");
                    string name = Console.ReadLine();
                    planet.addColony(name);
                    Console.Clear();
                }
                else if (b == "remove") {
                    Console.WriteLine("Type colony name");
                    string name = Console.ReadLine();
                    planet.removeColony(name);
                    Console.Clear();
                }
                else if (b == "show") {
                    Console.Clear();
                    planet.showColonies();
                }
                else if (b == "choose"){
                    Console.WriteLine("Choose colony from: [{0}]", planet.text);
                    string choice = Console.ReadLine();
                    Console.Clear();
                    if (planet.coloniesObj.Keys.Any(p => p == choice)){
                        foreach (var item in planet.coloniesObj)
                        {
                            if (item.Key == choice){
                                ColonyMenu(planet.coloniesObj[choice], planet);
                            }
                        }
                    }
                    break;
                }
                else if (b == "back"){
                    Console.Clear();
                    GalaxyMenu(newGalaxy);
                }
            }
        }
        static void ColonyMenu(Colony colony, Planet planet){
            colony.resSwitch = true;
            Thread count = new Thread(new ThreadStart(colony.resourceCountThread));
            count.Start();
            while (true){
                 Console.WriteLine(help + "\n" + aoof + "\n" + location + "--> {0} --> {1}", planet.name, colony.name + '\n' + aoof);
                string switchCase = Console.ReadLine();
                switch (switchCase)
                {
                    case "add":
                    colony.addBuilding(colony);
                    break;
/*                     case "add factory":
                        colony.addFactory();
                        Console.Clear();
                        break;
                    case "add house":
                        colony.addHouse();
                        Console.Clear();
                        break;
                    case "remove factory":
                        colony.removeFactory();
                        Console.Clear();
                        break;
                    case "remove house":
                        colony.removeHouse();
                        Console.Clear();
                        break; */
                    case "show buildings":
                        //Console.Clear();
                        colony.showBuilding();
                        break;
                    case "show res":
                        Console.Clear();
                        colony.showResource();
                        break;
                    case "back":
                        colony.resSwitch = false;
                        count.Join();
                        Console.Clear();
                        PlanetMenu(planet);
                        break;
                }
            }
        }        
    }
}