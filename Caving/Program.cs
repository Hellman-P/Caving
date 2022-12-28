using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using static System.Net.Mime.MediaTypeNames;

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
                                            Console.WriteLine("You brave yourself and move forward. It's not easy but you find yourself in a new room after some struggling.\n"); // aaaaaaa, I want custom ones for different paaaathsssss (I forgor) maybe a bonus I'll do during polish
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

                    // Words used in muliple rooms
                    case "feel":
                    case "touch":
                    case "touches":
                    case "feels":
                    case "examine":
                        foundTriggerWords.Add("feel");
                        break;
                    case "look":
                    case "looks":
                    case "squint":
                    case "squints":
                    case "search":
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
                    case "inhale":
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
                    case "stand":
                        foundTriggerWords.Add("stand");
                        break;
                    case "dripping":
                    case "dripp":
                    case "water":
                    case "ruuning":
                        foundTriggerWords.Add("dripping");
                        break;

                    // Room specific keywords

                    //room 0
                    case "angle":
                    case "lean":
                    case "leaning":
                        foundTriggerWords.Add("angle");
                        break;
                    case "air":
                    case "Oxygen":
                    case "flow":
                        foundTriggerWords.Add("air");
                        break;
                    case "echo":
                    case "reverb":
                    case "resounding":
                        foundTriggerWords.Add("echo");
                        break;
                    case "climb":
                    case "ascend":
                    case "scale":
                        foundTriggerWords.Add("climb");
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

                    // Unique keywords
                    case "ledge":
                    case "overhang":
                    case "ridge":
                    case "protrusion":
                    case "steep":
                        foundTriggerWords.Add("ledge");
                        break;
                    case "drop":
                    case "fall":
                    case "unhand":
                    case "release":
                    case "grip":
                        foundTriggerWords.Add("drop");
                        break;

                    case "slide":
                    case "glide":
                    case "slip":
                    case "skate":
                        foundTriggerWords.Add("slide");
                        break;

                    // Room 2
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
                    case "go":
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

            caveRoomsList.Add(new CaveRoom // Room 0
            {
                GenericKeywords = new SortedList<string, string>(),
                UniqueKeywords = new SortedList<string, string>(),
                ExplorationUnlocks = new SortedList<string, bool>(),
                PossiblePathsTriggers = new SortedList<int, List<string>>(),
                PossiblePathsBooleans = new SortedList<int, bool>(),
                PathExitText = new SortedList<int, string>(),
                PathLeadsToRoom = new SortedList<int, int>(),
            }); ;
            caveRoomsList.Add(new CaveRoom // Room 1
            {
                GenericKeywords = new SortedList<string, string>(),
                UniqueKeywords = new SortedList<string, string>(),
                ExplorationUnlocks = new SortedList<string, bool>(),
                PossiblePathsTriggers = new SortedList<int, List<string>>(),
                PossiblePathsBooleans = new SortedList<int, bool>(),
                PathExitText = new SortedList<int, string>(),
                PathLeadsToRoom = new SortedList<int, int>(),
            });
            caveRoomsList.Add(new CaveRoom // Room 2
            {
                GenericKeywords = new SortedList<string, string>(),
                UniqueKeywords = new SortedList<string, string>(),
                ExplorationUnlocks = new SortedList<string, bool>(),
                PossiblePathsTriggers = new SortedList<int, List<string>>(),
                PossiblePathsBooleans = new SortedList<int, bool>(),
                PathExitText = new SortedList<int, string>(),
                PathLeadsToRoom = new SortedList<int, int>(),
            });
            caveRoomsList.Add(new CaveRoom // Room 3
            {
                GenericKeywords = new SortedList<string, string>(),
                UniqueKeywords = new SortedList<string, string>(),
                ExplorationUnlocks = new SortedList<string, bool>(),
                PossiblePathsTriggers = new SortedList<int, List<string>>(),
                PossiblePathsBooleans = new SortedList<int, bool>(),
                PathExitText = new SortedList<int, string>(),
                PathLeadsToRoom = new SortedList<int, int>(),
            });
            caveRoomsList.Add(new CaveRoom // Room 4
            {
                GenericKeywords = new SortedList<string, string>(),
                UniqueKeywords = new SortedList<string, string>(),
                ExplorationUnlocks = new SortedList<string, bool>(),
                PossiblePathsTriggers = new SortedList<int, List<string>>(),
                PossiblePathsBooleans = new SortedList<int, bool>(),
                PathExitText = new SortedList<int, string>(),
                PathLeadsToRoom = new SortedList<int, int>(),
            });
            caveRoomsList.Add(new CaveRoom // Room 5
            {
                GenericKeywords = new SortedList<string, string>(),
                UniqueKeywords = new SortedList<string, string>(),
                ExplorationUnlocks = new SortedList<string, bool>(),
                PossiblePathsTriggers = new SortedList<int, List<string>>(),
                PossiblePathsBooleans = new SortedList<int, bool>(),
                PathExitText = new SortedList<int, string>(),
                PathLeadsToRoom = new SortedList<int, int>(),
            });
            caveRoomsList.Add(new CaveRoom // Room 6 (ending scene)
            {
                GenericKeywords = new SortedList<string, string>(),
                UniqueKeywords = new SortedList<string, string>(),
                ExplorationUnlocks = new SortedList<string, bool>(),
                PossiblePathsTriggers = new SortedList<int, List<string>>(),
                PossiblePathsBooleans = new SortedList<int, bool>(),
                PathExitText = new SortedList<int, string>(),
                PathLeadsToRoom = new SortedList<int, int>(),
            });
            caveRoomsList.Add(new CaveRoom // Room 7 (ending scene(tr(sl)u(g)e ending))
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
            caveRoomsList[0].GenericKeywords.Add("drink", "You cup your hands and drink the cold water from the ground." +
                "\nIt tastes like stale dirt...\n");
            caveRoomsList[0].GenericKeywords.Add("eat", "You have nothing to eat");
            caveRoomsList[0].GenericKeywords.Add("smell", "There's no smell of note");
            caveRoomsList[0].GenericKeywords.Add("floor", "The floor of the room is uneaven and feels like it's tilting");
            caveRoomsList[0].GenericKeywords.Add("wall", "The walls much like the ground feel smooth and and cold. You could explore around and see if you find some openings.\n");
            caveRoomsList[0].GenericKeywords.Add("walk", "You walk around the room, folling the wall and counting your steps." +
                "\nThe room is probably quite big based on how many steps you took but you're not sure exactly where you started so it's just a guess." +
                "\nYou also found a think crack in the wall, a slippery path upwards with dripping water and an echoing tunnel quite high on the wall");
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

            caveRoomsList[0].UniqueKeywords.Add("climb", "On the side of the room quite high on the wall it feels like there could be a tunnel you could climb into." +
                "\nYou could probably make your way up there if you could use something to climb with\n");
            caveRoomsList[0].UniqueKeywords.Add("pickaxe", "You could use your pickaxe\n");

            // Exploration unlocks
            caveRoomsList[0].ExplorationUnlocks.Add("crack", false);
            caveRoomsList[0].ExplorationUnlocks.Add("backpack", false);

            caveRoomsList[0].ExplorationUnlocks.Add("climb", false);
            caveRoomsList[0].ExplorationUnlocks.Add("pickaxe", false);

            // Possible path triggers
            caveRoomsList[0].PossiblePathsTriggers.Add(0, new List<string>() { "crack", "backpack" });
            caveRoomsList[0].PossiblePathsTriggers.Add(1, new List<string>() { "climb", "pickaxe" });

            // Possible path booleans
            caveRoomsList[0].PossiblePathsBooleans.Add(0, false);
            caveRoomsList[0].PossiblePathsBooleans.Add(1, false);

            // What room different exits lead too
            caveRoomsList[0].PathLeadsToRoom.Add(0, 2);
            caveRoomsList[0].PathLeadsToRoom.Add(1, 1);

            // Exit Path texts
            caveRoomsList[0].PathExitText.Add(0, "You can take of your backpack, leave your pickaxe and suck in your chest to be able to fit through the tight crack\n" +
                "\nBut theres no guarantee you'll make it.");

            caveRoomsList[0].PathExitText.Add(1, "You can use your pickaxe as a climbing tool to get up into the tunnel high on the wall, hoping it leads somewhere usefull." +
                "\nYou will have to leave your pickaxe behind");



            // Building room #1
            // Room Description
            roomDescriptions.Add("You lay in a pile on a wet floor. The water is running towards one end of the room." +
                "\nIt's cold and the sound of running water is drowning out all other noise");

            // Generic keywords
            // Keywords used by all rooms
            caveRoomsList[1].GenericKeywords.Add("feel", "You feel the nearest surface you can find. It's cold stone, water running over it\n");
            caveRoomsList[1].GenericKeywords.Add("look", "No matter how hard you try you can't see a thing in the dark\n");
            caveRoomsList[1].GenericKeywords.Add("listen", "The sound of dripping and falling water is too loud to hear much else\n");
            caveRoomsList[1].GenericKeywords.Add("lamp", "Your light broke in the fall, You can't use it\n");
            caveRoomsList[1].GenericKeywords.Add("drink", "You drink from the water running across the room. It tastes cold and more refreshing than expected\n");
            caveRoomsList[1].GenericKeywords.Add("eat", "You have nothing to eat\n");
            caveRoomsList[1].GenericKeywords.Add("smell", "The room smells slightly musty\n");
            caveRoomsList[1].GenericKeywords.Add("floor", "The floor is mostly level with water running across it\n");
            caveRoomsList[1].GenericKeywords.Add("wall", "feeling along the wall you feel water running down across it. You also find the room to not be very large. " +
                "\nAt one point the wall suddenly ends and you feel around to realise that you're near a ledge. It's hard to know how far the drop is unless you try dropping something first. " +
                "\nYou also find a much less steep part of the ledge but it's still much to steep to walking along with how slippery it is\n");
            caveRoomsList[1].GenericKeywords.Add("walk", "Walking around the room you suddenly feel the lack of ground under your step as you almost fall down." +
                "\nAll the water is rushing towards this ledge. You could drop something down it to see how far down the fall goes. " +
                "\nYou also find a much less steep portion, still to steep to walk down but you could take a risk and slide down it\n");
            // Room specific generic keywords
            caveRoomsList[1].GenericKeywords.Add("water", "The water is running towards one end of the room for even though the floor is level");
            caveRoomsList[1].GenericKeywords.Add("stand", "You stand, feeling a little disoriented\n");
            caveRoomsList[1].GenericKeywords.Add("ledge", "The ledge is steep and you can't feel the bottom no matter what you do. The water doesn't seem to be falling" +
            "\nthat far but it's hard to tell. You could drop heavy down it to see if you hear it land\n");

            //Unique keywords
            caveRoomsList[1].UniqueKeywords.Add("drop", "You'd need to drop something heavy for it to be heard over the running water. The only thing you have on hand is your pickaxe." +
                "\nIt's not something you'd want to lose before fully deciding to move forward");

            caveRoomsList[1].UniqueKeywords.Add("slide", "The slide is to steep to walk on, and slippery from the water. " +
                "\nBut nothing except self preservation instict is stopping you from going down it\n");

            // Exploration unlocks
            caveRoomsList[1].ExplorationUnlocks.Add("drop", false);

            caveRoomsList[1].ExplorationUnlocks.Add("slide", false);

            // Possible path triggers
            caveRoomsList[1].PossiblePathsTriggers.Add(2, new List<string>() { "drop" });
            caveRoomsList[1].PossiblePathsTriggers.Add(3, new List<string>() { "slide" });

            // Possible paths booleans
            caveRoomsList[1].PossiblePathsBooleans.Add(2, false);
            caveRoomsList[1].PossiblePathsBooleans.Add(3, false);

            // What room different exits lead too
            caveRoomsList[1].PathLeadsToRoom.Add(2, 3);
            caveRoomsList[1].PathLeadsToRoom.Add(3, 0);

            // Exit path texts
            caveRoomsList[1].PathExitText.Add(2, "You could try try jumping down from the ledge. But before you do you would need to drop your pickaxe and see if the fall is safe");
            caveRoomsList[1].PathExitText.Add(3, "You could sit down on the ledge and go down the slide. Just hoping for the best");



            // Building room #2
            // Room Description
            roomDescriptions.Add("As you enter the room you feel yourself having been cut by jagged edges on the walls" +
                "\nIt feels damp and you can hear running water");

            // Generic keywords
            // Keywords used by all rooms
            caveRoomsList[2].GenericKeywords.Add("feel", "You feel hard edges and damp rocks\n");
            caveRoomsList[2].GenericKeywords.Add("look", "You try your best but it's still too dark\n");
            caveRoomsList[2].GenericKeywords.Add("listen", "The running water is drowning out any other possible sounds," +
                "\nit's close\n");
            caveRoomsList[2].GenericKeywords.Add("lamp", "Your light broke in the fall\n");
            caveRoomsList[2].GenericKeywords.Add("drink", "You have nothing to drink\n");
            caveRoomsList[2].GenericKeywords.Add("eat", "You have nothing to eat\n");
            caveRoomsList[2].GenericKeywords.Add("smell", "The air feel more cold and smells fresh\n");
            caveRoomsList[2].GenericKeywords.Add("floor", "The floor is level and moist\n");
            caveRoomsList[2].GenericKeywords.Add("wall", "Walking along the wall doesn't give you mutch information except the room being small and the source of the noise." +
                "\nThere's what feels like an underground river on one side of the room\n");
            caveRoomsList[2].GenericKeywords.Add("walk", "You find a pillar in the middle of the room and what feels like an underground rover on one side of the room\n");
            // Room specific generic keywords
            caveRoomsList[2].GenericKeywords.Add("pillar", "feeling around the pillar doesn't yeild much but while doing so you feel a slight draft from above");
            caveRoomsList[2].GenericKeywords.Add("rapids", "theres a chance the river might be to dangerous to cross or swim along." +
                "\nIt's impossible to tell without getting in\n");
            caveRoomsList[2].GenericKeywords.Add("water", " There's a rapid river on one side of the room");

            //Unique keywords
            caveRoomsList[2].UniqueKeywords.Add("rope", "You could throw my hook rope towards the draft from above the pillar and hope there's something up there\n");
            caveRoomsList[2].UniqueKeywords.Add("swim", "You could try swimming across or along the river, but it's dangerous\n");
            caveRoomsList[2].UniqueKeywords.Add("river", "The river could be dangerous but it might be your only chance\n");

            // Exploration unlocks
            caveRoomsList[2].ExplorationUnlocks.Add("rope", false);

            caveRoomsList[2].ExplorationUnlocks.Add("swim", false);
            caveRoomsList[2].ExplorationUnlocks.Add("river", false);

            // Possible path triggers
            caveRoomsList[2].PossiblePathsTriggers.Add(4, new List<string>() { "rope"});
            caveRoomsList[2].PossiblePathsTriggers.Add(5, new List<string>() { "swim", "river" });

            // Possible paths booleans
            caveRoomsList[2].PossiblePathsBooleans.Add(4, false);
            caveRoomsList[2].PossiblePathsBooleans.Add(5, false);

            // What room different exits lead too
            caveRoomsList[2].PathLeadsToRoom.Add(4, 4);
            caveRoomsList[2].PathLeadsToRoom.Add(5, 3);

            // Exit path texts
            caveRoomsList[2].PathExitText.Add(4, "You could throw the rope towards the suspected opening in the roof next to the pillar and try to climb it");
            caveRoomsList[2].PathExitText.Add(5, "You could try swimming along the river. It will probably be more like being dragged along it's currents." +
                "\nBut you can hope you wash up somewhere safe");



            // Building room #3
            // Room Description
            roomDescriptions.Add("You end up in what seems to be a very large room. You hear the noise from a rushing underground river nearby." +
                "\nThe sound echoes far. It's somewhat damp but the air is fresh. You're probably getting closer to the surface");

            // Generic keywords
            // Keywords used by all rooms
            caveRoomsList[3].GenericKeywords.Add("feel", "");
            caveRoomsList[3].GenericKeywords.Add("look", "");
            caveRoomsList[3].GenericKeywords.Add("listen", "");
            caveRoomsList[3].GenericKeywords.Add("lamp", "");
            caveRoomsList[3].GenericKeywords.Add("drink", "");
            caveRoomsList[3].GenericKeywords.Add("eat", "");
            caveRoomsList[3].GenericKeywords.Add("smell", "");
            caveRoomsList[3].GenericKeywords.Add("floor", "");
            caveRoomsList[3].GenericKeywords.Add("wall", "");
            caveRoomsList[3].GenericKeywords.Add("walk", "");
            // Room specific generic keywords
            caveRoomsList[3].GenericKeywords.Add("rubble", "blocked path\n");
            caveRoomsList[3].GenericKeywords.Add("large", "big room void safe for explosives \n");

            //Unique keywords
            caveRoomsList[3].UniqueKeywords.Add("dynamite", "\n");

            // Exploration unlocks
            caveRoomsList[3].ExplorationUnlocks.Add("dynamite", false);

            // Possible path triggers
            caveRoomsList[3].PossiblePathsTriggers.Add(6, new List<string>() { "dynamite" });

            // Possible paths booleans
            caveRoomsList[3].PossiblePathsBooleans.Add(6, false);

            // What room different exits lead too
            caveRoomsList[3].PathLeadsToRoom.Add(6, 4);

            // Exit path texts
            caveRoomsList[3].PathExitText.Add(6, "");



            // Building room #4
            // Room Description
            roomDescriptions.Add("Something about you being near the surface\n");

            // Generic keywords
            // Keywords used by all rooms
            caveRoomsList[4].GenericKeywords.Add("feel", "");
            caveRoomsList[4].GenericKeywords.Add("look", "");
            caveRoomsList[4].GenericKeywords.Add("listen", "");
            caveRoomsList[4].GenericKeywords.Add("lamp", "");
            caveRoomsList[4].GenericKeywords.Add("drink", "");
            caveRoomsList[4].GenericKeywords.Add("eat", "");
            caveRoomsList[4].GenericKeywords.Add("smell", "T");
            caveRoomsList[4].GenericKeywords.Add("floor", "");
            caveRoomsList[4].GenericKeywords.Add("wall", "");
            caveRoomsList[4].GenericKeywords.Add("walk", "");
            // Room specific generic keywords
            caveRoomsList[4].GenericKeywords.Add("light", "near surface\n");
            caveRoomsList[4].GenericKeywords.Add("sound", "near surface\n");
            caveRoomsList[4].GenericKeywords.Add("slug", "slug hole\n");

            //Unique keywords
            caveRoomsList[4].UniqueKeywords.Add("climb", "\n");

            caveRoomsList[4].UniqueKeywords.Add("slug", "\n");

            // Exploration unlocks
            caveRoomsList[4].ExplorationUnlocks.Add("climb", false);
            caveRoomsList[4].ExplorationUnlocks.Add("slug", false);

            // Possible path triggers
            caveRoomsList[4].PossiblePathsTriggers.Add(6, new List<string>() { "climb" });
            caveRoomsList[4].PossiblePathsTriggers.Add(7, new List<string>() { "slug" });

            // Possible paths booleans
            caveRoomsList[4].PossiblePathsBooleans.Add(6, false);
            caveRoomsList[4].PossiblePathsBooleans.Add(7, false);

            // What room different exits lead too
            caveRoomsList[4].PathLeadsToRoom.Add(6, 6);
            caveRoomsList[4].PathLeadsToRoom.Add(7, 5);

            // Exit path texts
            caveRoomsList[4].PathExitText.Add(6, "");
            caveRoomsList[4].PathExitText.Add(7, "");



            // Building room #5
            // Room Description
            roomDescriptions.Add("slug room\n");

            // Generic keywords
            // Keywords used by all rooms
            caveRoomsList[5].GenericKeywords.Add("feel", "");
            caveRoomsList[5].GenericKeywords.Add("look", "");
            caveRoomsList[5].GenericKeywords.Add("listen", "");
            caveRoomsList[5].GenericKeywords.Add("lamp", "");
            caveRoomsList[5].GenericKeywords.Add("drink", "");
            caveRoomsList[5].GenericKeywords.Add("eat", "");
            caveRoomsList[5].GenericKeywords.Add("smell", "T");
            caveRoomsList[5].GenericKeywords.Add("floor", "");
            caveRoomsList[5].GenericKeywords.Add("wall", "");
            caveRoomsList[5].GenericKeywords.Add("walk", "");
            // Room specific generic keywords
            caveRoomsList[5].GenericKeywords.Add("bone", "\n");
            caveRoomsList[5].GenericKeywords.Add("equipment", "\n");
            caveRoomsList[5].GenericKeywords.Add("shell", "\n");
            caveRoomsList[5].GenericKeywords.Add("push", "\n");
            caveRoomsList[5].GenericKeywords.Add("slug", "\n");


            //Unique keywords
            caveRoomsList[5].UniqueKeywords.Add("air", "\n");

            // Exploration unlocks
            caveRoomsList[5].ExplorationUnlocks.Add("air", false);

            // Possible path triggers
            caveRoomsList[5].PossiblePathsTriggers.Add(8, new List<string>() { "air", "hole" });

            // Possible paths booleans
            caveRoomsList[5].PossiblePathsBooleans.Add(8, false);

            // What room different exits lead too
            caveRoomsList[5].PathLeadsToRoom.Add(8, 7);

            // Exit path texts
            caveRoomsList[5].PathExitText.Add(8, "");



            // Ending scenes
            // Building room #6 (ending scene)
            roomDescriptions.Add("regular ending\n");

            // Building room #7 (ending scene(tr(sl)u(g)e ending))
            roomDescriptions.Add("slug ending\n");


            // choosing a start room
            var random = new Random();
            currentRoom = random.Next(0, 2);

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
                if (currentRoom == 6 || currentRoom == 7)
                {
                    Console.WriteLine(roomDescriptions[currentRoom]);
                    Console.WriteLine();
                    Console.WriteLine("Press any key to exit the game...");
                    Console.ReadLine();
                    Environment.Exit(0);
                }

                // Take player input, put it into lowercase and pass it to the AlternativeWords
                string playerInput = Console.ReadLine();
                string input = playerInput.ToLower();
                keywordParser(AlternativeWords(input), currentRoomGenericSortedList, currentRoomUniqueSortedList, currentRoomUnlocks, currentRoomPathTriggers, currentRoomPathBooleans, currentRoomPathExitTexts, currentRoomPathLeadsTo, roomDescriptions);
            }
        }
    }
}