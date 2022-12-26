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
            Console.WriteLine("I could consider moving forwards, maybe I should?");

            // Separating player input into list of strings
            string playerInput = Console.ReadLine();
            string playerInputLower = playerInput.ToLower();
            string[] separatedPlayerInput = playerInputLower.Split(' ', ',', '.');

            // Checking of the player wants to move to another room
            foreach (string input in separatedPlayerInput)
            {
                switch (input)
                {
                    case "yes":
                    case "y":
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
                            int listingOtions = 1;
                            var pathOptions = new List<int>();
                            foreach (KeyValuePair<int, bool> keyValue1 in pathBooleans)
                            {
                                if (keyValue1.Value == true)
                                {
                                    Console.Write($"{listingOtions}. ");
                                    Console.WriteLine(exitPathText[keyValue1.Key]);
                                    Console.WriteLine();
                                    pathOptions.Add(keyValue1.Key);
                                    listingOtions++;
                                }
                            }
                            Console.WriteLine("I could choose one of these or continue exploring more");

                            // Reading and separating players choice
                            string playerOption = Console.ReadLine();
                            string playerOptionLower = playerOption.ToLower();
                            string[] separatedplayerOption = playerOptionLower.Split(' ', ',', '.');
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
                                            Console.WriteLine(exitPathText[currentRoom]);
                                            Console.WriteLine(roomDescription[currentRoom]);
                                            // also make it so the path opens for the room you enter
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        break;

                    case "no":
                    case "n":
                        Console.WriteLine("Not now, I need to explore more");
                        break;

                    default:
                        Console.WriteLine("I'll consider it later...");
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
                        foundTriggerWords.Add("eat");
                        break;

                    // Room specific keywords

                    //room 1
                    case "angle":
                    case "floor":
                    case "lean":
                    case "leaning":
                        foundTriggerWords.Add("angle");
                        break;
                    case "air":
                    case "breath":
                    case "breathing":
                    case "Oxygen":
                        foundTriggerWords.Add("air");
                        break;
                    case "walls":
                    case "wall":
                        foundTriggerWords.Add("walls");
                        break;
                    case "dripping":
                    case "dripp":
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


                    // Exit
                    case "exit":
                    case "leave":
                    case "door":
                        foundTriggerWords.Add("exit");
                        break;
                    default:
                        foundTriggerWords.Add("cannot"); // triggers when it shouldn't
                        break;
                }
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

            // Building room #1

            //Room description
            roomDescriptions.Add("It's quiet, you hear your movements echo and the quiet dripping of water. You also notice water is running across the floor\n");

            // Generic keywords
            // Keywords used by all rooms
            caveRoomsList[0].GenericKeywords.Add("feel", "You feel the nearest rock surfaces, they feel smooth and cold");
            caveRoomsList[0].GenericKeywords.Add("look", "You try to squint you eyes to see but it's to no avail");
            caveRoomsList[0].GenericKeywords.Add("listen", "You hear the dripping of water and it's echo");
            caveRoomsList[0].GenericKeywords.Add("lamp", "Your lamp is broken from the fall");
            caveRoomsList[0].GenericKeywords.Add("drink", "You have nothing to drink");
            caveRoomsList[0].GenericKeywords.Add("eat", "You have nothing to eat");
            // Room specific generic keywords
            caveRoomsList[0].GenericKeywords.Add("angle", "The flooer is tilted, making it a bit hard to move around\n");
            caveRoomsList[0].GenericKeywords.Add("air", "The air is thin and it's hard to breethe. I'm probably deep down\n");
            caveRoomsList[0].GenericKeywords.Add("walls", "The walls are smooth\n");
            caveRoomsList[0].GenericKeywords.Add("dripping", "The dripping noice is coming from water sliding down what feels like a steep tunnel." +
                "\nIt's probably where the water on the floor is coming from." +
                "\nIt might go further up but I think it's too slippery to climb\n");
            caveRoomsList[0].GenericKeywords.Add("echo", "The echoing comes from above, the roof is probably very high\n");

            // Unique keywords
            caveRoomsList[0].UniqueKeywords.Add("crack", "On the higher side of the room I can feel a thin crack in the wall. But it's a bit to thin for me to squeeze through as is\n");
            caveRoomsList[0].UniqueKeywords.Add("backpack", "Taking of my backback will make it less laboursome and make it easier to move through small paths\n");

            caveRoomsList[0].UniqueKeywords.Add("tunnel", "On the lower side of the room quite high on a wall it feels like there could be a tunnel." +
                "\nI could probably make my way up there if I could use something to climb with\n");
            caveRoomsList[0].UniqueKeywords.Add("pickaxe", "I could use my pickaxe as a climbing pick to get up into that tunnel high up\n");

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
            caveRoomsList[0].PathExitText.Add(0, "I take of my backpack and if I breath out then hold my breath I can probably fit through the thin crack");

            caveRoomsList[0].PathExitText.Add(1, "I use the pickaxe and use it as a climbing pick to make my way up to the tunnel");



            // Building room #2

            // Room Description
            roomDescriptions.Add("You enter a wet cave...\n");

            // Generic keywords
            // Keywords used by all rooms
            caveRoomsList[1].GenericKeywords.Add("feel", "");
            caveRoomsList[1].GenericKeywords.Add("look", "");
            caveRoomsList[1].GenericKeywords.Add("listen", "");
            caveRoomsList[1].GenericKeywords.Add("lamp", "");
            caveRoomsList[1].GenericKeywords.Add("drink", "");
            caveRoomsList[1].GenericKeywords.Add("eat", "");

            //Unique keywords


            // Exploration unlocks


            // Possible path triggers


            // Possible paths booleans
            caveRoomsList[1].PossiblePathsBooleans.Add(1, false);

            // What room different exits lead too


            // Exit path texts




            // Building room #3
            roomDescriptions.Add("You enter a smelly cave...\n");

            // Generic keywords
            // Keywords used by all rooms
            caveRoomsList[2].GenericKeywords.Add("feel", "");
            caveRoomsList[2].GenericKeywords.Add("look", "");
            caveRoomsList[2].GenericKeywords.Add("listen", "");
            caveRoomsList[2].GenericKeywords.Add("lamp", "");
            caveRoomsList[2].GenericKeywords.Add("drink", "");
            caveRoomsList[2].GenericKeywords.Add("eat", "");

            //Unique keywords


            // Exploration unlocks


            // Possible path triggers


            // Possible paths booleans


            // What room different exits lead too


            // Exit path texts




            // Intro to game and describing current room
            Console.WriteLine("You make your way down into the cave, a journey you have made many times before." +
                "\nThe winding paths, tunnels with sharp edges, the cracks so thin you can barely breathe." +
                "\nThey have all become everyday obsicles for you, not worse than your old daily commute." +
                "\nYou've become complacent and confident doing this. When the unthinkable happens..." +
                "\nyour old rope you put there on your first expedition snaps." +
                "\nYou fall, it feels like you fall further than you should." +
                "\nYour light smashes against the wall and breaks and in the fall you hit your head and pass out..." +
                "\n" +
                "\n...You wake uo some time later, it's hard to tell how much time has passed." +
                "\nAll you know is you fell, you are in the dark and no one knows you're down here..." +
                "\nHelp wont me coming...");
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
            }
        }
    }
}