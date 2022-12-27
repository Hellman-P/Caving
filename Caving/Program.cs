using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;

namespace Caving
{
    internal class Program
    {
        public static int currentRoom = 0;
        class CaveRoom
        {
            public SortedList<string, string> GenericKeywords = new SortedList<string, string>();
            public SortedList<string, string> UniqueKeywords = new SortedList<string, string>();
            public SortedList<string, bool> ExplorationUnlocks = new SortedList<string, bool>();
            public SortedList<int, List<string>> PossiblePathsTriggers = new SortedList<int, List<string>>();
            public SortedList<int, bool> PossiblePathsBooleans = new SortedList<int, bool>();
            public SortedList<int, string> PathExitText = new SortedList<int, string>();
            public SortedList<int, int> PathLeadsToRoom = new SortedList<int, int>();
        }


        static void uniqueKeywordUnlocks(string input, SortedList<string, string> uniqueKeywords, SortedList<string, bool> unlocks)
        {
            // Writing unique keyword text and also unlocking exploration keys
            Console.WriteLine(uniqueKeywords[input]);
            unlocks[input] = true;
        }


        static void roomExitManager(SortedList<int, List<string>> pathTriggers, SortedList<int, bool> pathBooleans, SortedList<string, bool> unlocks, SortedList<int, string> exitPathText, SortedList<int, int> pathLeadsTo, List<string> roomDescription)
        {
            Console.WriteLine("You could consider moving forwards, maybe you should? (y/n)\n");

            // Separating player input into list of strings
            string playerInput = Console.ReadLine();
            string playerInputLower = playerInput.ToLower();
            string[] separatedPlayerInput = playerInputLower.Split(' ', ',', '.');
            Console.WriteLine();

            // Checking of the player wants to move to another room
            bool d = true;
            foreach (string input in separatedPlayerInput)
            {
                switch (input)
                {
                    case "yes":
                    case "y":
                    case "forward":
                    case "forwards":
                        // Checking what options the player has unlocked and showing what they can do

                        // Putting true keywords in list
                        var foundKeywords = new List<string>();
                        foreach (KeyValuePair<string, bool> keyValue in unlocks)
                        {
                            if (keyValue.Value == true)
                                foundKeywords.Add(keyValue.Key);
                        }

                        // Comparing each possible paths keywords with found keywords
                        bool a = false;
                        foreach (KeyValuePair<int, List<string>> listOfTriggers in pathTriggers)
                        {
                            bool b = true;
                            foreach (string pathTrígger in listOfTriggers.Value)
                            {
                                if (!foundKeywords.Contains(pathTrígger))
                                {
                                    b = false;
                                    break;
                                }
                            }
                            if (b == true)
                            {
                                pathBooleans[listOfTriggers.Key] = true;
                                a = true;
                            }
                        }
                        if (a == true)
                        {
                            a = false;

                            // Listing open path options and asking the player what they want to do
                            Console.WriteLine("The plausible options are probably...\n");
                            var pathOptions = new List<int>();
                            foreach (KeyValuePair<int, bool> keyValue1 in pathBooleans)
                            {
                                if (keyValue1.Value == true)
                                {
                                    Console.WriteLine(exitPathText[keyValue1.Key]);
                                    Console.WriteLine();
                                    pathOptions.Add(keyValue1.Key);
                                }
                            }
                            {
                                Console.WriteLine("non-existent... You should explore more");
                            }
                            Console.WriteLine("You could choose one of these or continue exploring more\n");

                            // Reading and separating players choice
                            string playerOption = Console.ReadLine();
                            string playerOptionLower = playerOption.ToLower();
                            string[] separatedplayerOption = playerOptionLower.Split(' ', ',', '.');
                            Console.WriteLine();
                            //AlternativeWords(separatedPlayerInput); // Make this do something and use it before passing on

                            // opening choosen path both ways
                            bool c = true;
                            c = true;
                            foreach (KeyValuePair<int, int> keyValue2 in pathLeadsTo)
                            {
                                foreach (string optionInput in separatedplayerOption)
                                {
                                    if (c == true)
                                    {
                                        if (foundKeywords.Contains(optionInput))
                                        {
                                            c = false;
                                            currentRoom = keyValue2.Value;
                                            Console.WriteLine("You brave yourself and move forward. It's not an easy path but you find yourself in a new room after some struggling.\n");
                                            Console.WriteLine(roomDescription[currentRoom]);
                                            Console.WriteLine("What will you do now?");
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        Console.WriteLine("You haven't found any good paths yet, explore more\n");
                        break;

                    case "no":
                    case "n":
                    case "back":
                        Console.WriteLine("Not now, you need to explore more");
                        break;

                    default:
                        if (d == true)
                        {
                            d = false;
                            Console.WriteLine("You'll consider it later...");
                        }
                        break;
                }
            }

        }


        static void keywordParser(string playerInput, SortedList<string, string> genericKeywords, SortedList<string, string> uniqueKeywords, SortedList<string, bool> unlocks, SortedList<int, List<string>> pathTriggers, SortedList<int, bool> pathBooleans, SortedList<int, string> exitPathText, SortedList<int, int> pathLeadsToo, List<string> roomDescription)
        {
            // Separating player input into list of strings
            string[] separatedPlayerInput = playerInput.Split(' ', ',', '.');

            // Comparing player inputs to all keywords
            foreach (string input in separatedPlayerInput)
            {
                // Comparing generic keywords
                foreach (KeyValuePair<string, string> keyValue in genericKeywords)
                {
                    if (keyValue.Key == input)
                        Console.WriteLine(keyValue.Value);
                }
                // Comparing unique keywords
                foreach (KeyValuePair<string, string> keyValue in uniqueKeywords)
                {
                    if (keyValue.Key == input)
                        uniqueKeywordUnlocks(input, uniqueKeywords, unlocks);
                }

                switch (input)
                {
                    // Checking if player wants to leave
                    case "exit":
                        roomExitManager(pathTriggers, pathBooleans, unlocks, exitPathText, pathLeadsToo, roomDescription);
                        break;
                    case "cannot":
                        Console.WriteLine("You can't do that");
                        break;
                    default:
                        break;
                }
            }
        }

        static string AlternativeWords(string input)
        {
            var foundTriggerWords = new List<string>();
            // Separating player input into list of strings
            string[] separatedPlayerInput = input.Split(' ', ',', '.');

            // Comparing player inputs to all keywords
            foreach (string separatedInput in separatedPlayerInput)
            {
                // Changing word synonyms into trigger words
                switch (separatedInput)
                {

                    // All room keywords
                    case "feel":
                    case "touch":
                    case "touches":
                    case "feels":
                        foundTriggerWords.Add("feel");
                        break;
                    case "look":
                    case "looks":
                    case "squint":
                    case "squints":
                        foundTriggerWords.Add("look");
                        break;
                    case "listen":
                    case "sound":
                        foundTriggerWords.Add("listen");
                        break;
                    case "lamp":
                    case "flashlight":
                    case "light":
                        foundTriggerWords.Add("lamp");
                        break;
                    case "drink":
                        foundTriggerWords.Add("drink");
                        break;
                    case "eat":
                    case "consume":
                        foundTriggerWords.Add("eat");
                        break;
                    case "smell":
                    case "smells":
                    case "sniff":
                    case "sniffs":
                        foundTriggerWords.Add("smell");
                        break;
                    case "floor":
                    case "ground":
                        foundTriggerWords.Add("floor");
                        break;
                    case "walk": 
                    case "move":
                    case "stroll":
                    case "explore":
                        foundTriggerWords.Add("walk");
                        break;
                    case "wall":
                    case "walls":
                        foundTriggerWords.Add("wall");
                        break;


                    // Room specific keywords

                    //room 0
                    case "stand":
                        foundTriggerWords.Add("stand");
                        break;
                    case "angle":
                    case "lean":
                    case "leaning":
                        foundTriggerWords.Add("angle");
                        break;
                    case "air":
                    case "breath":
                    case "breathing":
                    case "Oxygen":
                    case "inhale":
                    case "inhales":
                        foundTriggerWords.Add("air");
                        break;
                    case "dripping":
                    case "dripp":
                    case "water":
                        foundTriggerWords.Add("dripping");
                        break;
                    case "echo":
                    case "reverb":
                    case "resounding":
                        foundTriggerWords.Add("echo");
                        break;

                    // Unique Keywords
                    case "crack":
                    case "fissure":
                    case "crevice":
                    case "gap":
                    case "slit":
                        foundTriggerWords.Add("crack");
                        break;
                    case "backpack":
                    case "bag":
                    case "equipment":
                        foundTriggerWords.Add("backpack");
                        break;
                    case "tunnel":
                    case "path":
                        foundTriggerWords.Add("tunnel");
                        break;
                    case "pickaxe":
                    case "tool":
                    case "tools":
                        foundTriggerWords.Add("pickaxe");
                        break;



                    // Room 1
                    // Room specific generic keywords
                    case "pillar":
                    case "column":
                    case "support":
                    case "pole":
                        foundTriggerWords.Add("pillar");
                        break;
                    case "rapids":
                    case "rapid":
                    case "rushing":
                    case "rush":
                        foundTriggerWords.Add("rapids");
                        break;

                    // Unique keywords
                    case "rope":
                    case "hook":
                        foundTriggerWords.Add("rope");
                        break;
                    case "swim":
                    case "bathe":
                    case "dip":
                    case "dive":
                    case "plunge":
                        foundTriggerWords.Add("swim");
                        break;
                    case "river":
                    case "stream":
                    case "canal":
                    case "channel":
                        foundTriggerWords.Add("river");
                        break;

                    // Exit
                    case "exit":
                    case "leave":
                    case "door":
                    case "forward":
                    case "out":
                    case "enter":
                        foundTriggerWords.Add("exit");
                        break;
                    default:
                        break;
                }
            }
            if (foundTriggerWords.Count == 0)
            {
                foundTriggerWords.Add("cannot");
            }
            if (foundTriggerWords.Contains("exit"))
            {
                foundTriggerWords.Clear();
                foundTriggerWords.Add("exit");
            }
            string playerOutput = String.Join(", ", foundTriggerWords);
            return playerOutput;
        }
        static void Main(string[] args)
        {
            // Generating Rooms
            var caveRoomsList = new List<CaveRoom>();
            var roomDescriptions = new List<string>();

            caveRoomsList.Add(new CaveRoom // 0
            {
                GenericKeywords = new SortedList<string, string>(),
                UniqueKeywords = new SortedList<string, string>(),
                ExplorationUnlocks = new SortedList<string, bool>(),
                PossiblePathsTriggers = new SortedList<int, List<string>>(),
                PossiblePathsBooleans = new SortedList<int, bool>(),
                PathExitText = new SortedList<int, string>(),
                PathLeadsToRoom = new SortedList<int, int>(),
            }); ;
            caveRoomsList.Add(new CaveRoom // 1
            {
                GenericKeywords = new SortedList<string, string>(),
                UniqueKeywords = new SortedList<string, string>(),
                ExplorationUnlocks = new SortedList<string, bool>(),
                PossiblePathsTriggers = new SortedList<int, List<string>>(),
                PossiblePathsBooleans = new SortedList<int, bool>(),
                PathExitText = new SortedList<int, string>(),
                PathLeadsToRoom = new SortedList<int, int>(),
            });
            caveRoomsList.Add(new CaveRoom // 2
            {
                GenericKeywords = new SortedList<string, string>(),
                UniqueKeywords = new SortedList<string, string>(),
                ExplorationUnlocks = new SortedList<string, bool>(),
                PossiblePathsTriggers = new SortedList<int, List<string>>(),
                PossiblePathsBooleans = new SortedList<int, bool>(),
                PathExitText = new SortedList<int, string>(),
                PathLeadsToRoom = new SortedList<int, int>(),
            });

            // Building room #0

            //Room description
            roomDescriptions.Add("It's quiet, you hear your movements echo and the quiet dripping of water. You also notice water is running across the floor\n");

            // Generic keywords
            // Keywords used by all rooms
            caveRoomsList[0].GenericKeywords.Add("feel", "You feel the nearest rock surfaces, they feel smooth and cold, you also feel the flow of air across the room");
            caveRoomsList[0].GenericKeywords.Add("look", "You try to squint your eyes to see but it's to no avail");
            caveRoomsList[0].GenericKeywords.Add("listen", "You hear the dripping of water and its echo");
            caveRoomsList[0].GenericKeywords.Add("lamp", "Your lamp is broken from the fall");
            caveRoomsList[0].GenericKeywords.Add("drink", "You have nothing to drink");
            caveRoomsList[0].GenericKeywords.Add("eat", "You have nothing to eat");
            caveRoomsList[0].GenericKeywords.Add("smell", "There's no smell of note");
            caveRoomsList[0].GenericKeywords.Add("floor", "The floor of the room is uneaven and feels like it's tilting");
            caveRoomsList[0].GenericKeywords.Add("walk", "You walk around the room, folling the wall and counting your steps." +
                "\nThe room is probably quite big based on how many steps you took but you're not sure exactly where you started so it's just a guess." +
                "\n You also found a think crack in the wall, a slippery path upwards with dripping water and an echoing tunnel quite high on the wall");
            // Room specific generic keywords
            caveRoomsList[0].GenericKeywords.Add("stand", "You stand, feeling a little disoriented\n");
            caveRoomsList[0].GenericKeywords.Add("angle", "The flooer is tilted, making it a bit hard to move around\n");
            caveRoomsList[0].GenericKeywords.Add("air", "You look for the source of flowing air, it comes from a crack in the wall\n");
            caveRoomsList[0].GenericKeywords.Add("dripping", "The dripping noise is coming from water sliding down what feels like a steep tunnel" +
                "\nIt's probably where the water on the floor is coming from" +
                "\nIt might go further up but I think it's too slippery to climb\n");
            caveRoomsList[0].GenericKeywords.Add("echo", "You locate the echo, it's coming from a tunnel quite high up. when I jump I can feel it\n");

            // Unique keywords
            caveRoomsList[0].UniqueKeywords.Add("crack", "Along the wall you can feel a thin crack. But it's a bit to thin for you to squeeze through without taking of some of your equipment\n");
            caveRoomsList[0].UniqueKeywords.Add("backpack", "Taking off your backpack will make it less laboursome easier to move through small passages\n");

            caveRoomsList[0].UniqueKeywords.Add("tunnel", "On the side of the room quite high on the wall it feels like there could be a tunnel" +
                "\nYou could probably make your way up there if you could use something to climb with\n");
            caveRoomsList[0].UniqueKeywords.Add("pickaxe", "You could use your pickaxe\n");

            // Exploration unlocks
            caveRoomsList[0].ExplorationUnlocks.Add("crack", false);
            caveRoomsList[0].ExplorationUnlocks.Add("backpack", false);

            caveRoomsList[0].ExplorationUnlocks.Add("tunnel", false);
            caveRoomsList[0].ExplorationUnlocks.Add("pickaxe", false);

            // Possible path triggers
            caveRoomsList[0].PossiblePathsTriggers.Add(0, new List<string>() { "crack", "backpack" });
            caveRoomsList[0].PossiblePathsTriggers.Add(1, new List<string>() { "tunnel", "pickaxe" });

            // Possible path booleans
            caveRoomsList[0].PossiblePathsBooleans.Add(0, false);
            caveRoomsList[0].PossiblePathsBooleans.Add(1, false);

            // What room different exits lead too
            caveRoomsList[0].PathLeadsToRoom.Add(0, 1);
            caveRoomsList[0].PathLeadsToRoom.Add(1, 2);

            // Exit Path texts
            caveRoomsList[0].PathExitText.Add(0, "You can take of your backpack and suck in your chest to be able to fit through the tight crack.\n" +
                "\nBut theres no guarantee you'll make it.");

            caveRoomsList[0].PathExitText.Add(1, "You can use your pickaxe as a combing tool to get up into the tunnel high on the wall, hoping it leads somewhere usefull\n");



            // Building room #1

            // Room Description
            roomDescriptions.Add("As you enter the room you feel yourself having been cut by jagged edges on the walls" +
                "\nIt feels damp and you can hear running water\n");

            // Generic keywords
            // Keywords used by all rooms
            caveRoomsList[1].GenericKeywords.Add("feel", "You feel hard edges and damp rocks\n");
            caveRoomsList[1].GenericKeywords.Add("look", "You try your best but it's still too dark\n");
            caveRoomsList[1].GenericKeywords.Add("listen", "The running water is drowning out any other possible sounds," +
                "\nit's close\n");
            caveRoomsList[1].GenericKeywords.Add("lamp", "Your light broke in the fall\n");
            caveRoomsList[1].GenericKeywords.Add("drink", "You have nothing to drink\n");
            caveRoomsList[1].GenericKeywords.Add("eat", "You have nothing to eat\n");
            caveRoomsList[1].GenericKeywords.Add("smell", "The air feel more cold and smells fresh\n");
            caveRoomsList[1].GenericKeywords.Add("floor", "The floor is level and moist\n");
            caveRoomsList[1].GenericKeywords.Add("wall", "Walking along the wall doesn't give you mutch information except the room being small and the source of the noise." +
                "\nThere's what feels like an underground river on one side of the room\n");
            caveRoomsList[1].GenericKeywords.Add("walk", "You find a pillar in the middle of the room and what feels like an underground rover on one side of the room\n");
            // Room specific generic keywords
            caveRoomsList[1].GenericKeywords.Add("pillar", "feeling around the pillar doesn't yeild much but while doing so you feel a slight draft from above");
            caveRoomsList[1].GenericKeywords.Add("rapids", "theres a chance the river might be to dangerous to cross or swim along." +
                "\nIt's impossible to tell without getting in\n");
            caveRoomsList[1].GenericKeywords.Add("water", " There's a rapid river on one side of the room");

            //Unique keywords
            caveRoomsList[1].UniqueKeywords.Add("rope", "You could throw my hook rope towards the draft from above the pillar and hope there's something up there\n");
            caveRoomsList[1].UniqueKeywords.Add("swim", "You could try swimming across or along the river, but it's dangerous\n");
            caveRoomsList[1].UniqueKeywords.Add("river", "The river could be dangerous but it might be your only chance\n");

            // Exploration unlocks
            caveRoomsList[1].ExplorationUnlocks.Add("rope", false);

            caveRoomsList[1].ExplorationUnlocks.Add("swim", false);
            caveRoomsList[1].ExplorationUnlocks.Add("river", false);

            // Possible path triggers
            caveRoomsList[1].PossiblePathsTriggers.Add(2, new List<string>() { "rope"});
            caveRoomsList[1].PossiblePathsTriggers.Add(3, new List<string>() { "swim", "river" });

            // Possible paths booleans
            caveRoomsList[1].PossiblePathsBooleans.Add(2, false);
            caveRoomsList[1].PossiblePathsBooleans.Add(3, false);

            // What room different exits lead too
            caveRoomsList[1].PathLeadsToRoom.Add(2, 2);
            caveRoomsList[1].PathLeadsToRoom.Add(3, 2); // Change this so it's lead somewhere else

            // Exit path texts
            caveRoomsList[1].PathExitText.Add(2, "You can throw the rope towards the suspected opening in the roof next to the pillar and try to climb it");
            caveRoomsList[1].PathExitText.Add(3, "You can try crossing or swimming along the river and hope you find something before it exhausts you");



            // Building room #2
            roomDescriptions.Add("You enter a smelly cave...\n");

            // Generic keywords
            // Keywords used by all rooms

            // Room specific generic keywords

            //Unique keywords


            // Exploration unlocks


            // Possible path triggers


            // Possible paths booleans


            // What room different exits lead too


            // Exit path texts




            // Intro to game and describing current room
            Console.WriteLine("You make your way down into the cave, a journey you have made many times before." +
                "\nThe winding paths, tunnels with sharp edges, the cracks so thin you can barely breathe." +
                "\nThey have all become everyday obsticles for you, not worse than your old daily commute." +
                "\nYou've become complacent and confident doing this. When the unthinkable happens..." +
                "\nYour old rope you put there on your first expedition snaps." +
                "\nYou fall, it feels like you fall further than you should." +
                "\nYour equipment bashing against the wall during the fall, your light breaking. " +
                "\nYou hit your head and pass out..." +
                "\n" +
                "\n...You wake up some time later, it's hard to tell how much time has passed." +
                "\nAll you know is you fell, you are in the dark and no one knows you're down here..." +
                "\nHelp won't be coming...");
            Console.WriteLine();
            Console.WriteLine(roomDescriptions[currentRoom]);
            Console.WriteLine("What will you do?");

            //Looping systems
            while (true)
            {
                var currentRoomGenericSortedList = caveRoomsList[currentRoom].GenericKeywords;
                var currentRoomUniqueSortedList = caveRoomsList[currentRoom].UniqueKeywords;
                var currentRoomUnlocks = caveRoomsList[currentRoom].ExplorationUnlocks;
                var currentRoomPathTriggers = caveRoomsList[currentRoom].PossiblePathsTriggers;
                var currentRoomPathBooleans = caveRoomsList[currentRoom].PossiblePathsBooleans;
                var currentRoomPathExitTexts = caveRoomsList[currentRoom].PathExitText;
                var currentRoomPathLeadsTo = caveRoomsList[currentRoom].PathLeadsToRoom;

                // Take player input, put it into lowercase and pass it to the AlternativeWords
                string playerInput = Console.ReadLine();
                string input = playerInput.ToLower();
                keywordParser(AlternativeWords(input), currentRoomGenericSortedList, currentRoomUniqueSortedList, currentRoomUnlocks, currentRoomPathTriggers, currentRoomPathBooleans, currentRoomPathExitTexts, currentRoomPathLeadsTo, roomDescriptions);
                //exit game trigger when done
            }
        }
    }
}