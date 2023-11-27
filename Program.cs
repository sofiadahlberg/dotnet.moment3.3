using System;
using Newtonsoft.Json;
using System.Collections;
using System.Text.Json.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using System.Runtime.CompilerServices;

namespace moment_3
{
    class GuestBookObject
    {
        public int Id
        {
            get;
            set;
        }
        public string Author
        {
            get;
            set;
        }
        public string Textmessage
        {
            get;
            set;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //Lista för att spara objekt i gästboken
            List<GuestBookObject> posts = new List<GuestBookObject>();
            //sökväg för att spara gästboken
            string guestbookPath = "guestbook.json";

            //Kolla om JSON-filen existerar
            if (File.Exists(guestbookPath))
            {
                // Läser JSON file och läser in i "post"listan
                string json = File.ReadAllText(guestbookPath);
                posts = JsonConvert.DeserializeObject<List<GuestBookObject>>(json);
            }

            bool guestbook = true;

            while (guestbook)
            {

                char userInput = MainMenu(posts); // Huvudmeny

                switch (userInput)
                {
                    case '1':

                        Option1(posts);
                        break;

                    case '2':
                        Console.Clear();
                        Option2(posts);
                        break;

                    case 'x':
                        Console.Clear(); // Rensar konsolen
                        Console.WriteLine("Programmet avslutas");
                        guestbook = false;
                        break;

                    default:
                        Console.WriteLine("Inkorrekt, försök igen...");
                        break;

                }

                string updatedJson = JsonConvert.SerializeObject(posts);
                File.WriteAllText(guestbookPath, updatedJson);
                Console.WriteLine("Meddelandet har sparats i JSONfilen.");

            }
        }

        static void Option1(List<GuestBookObject> posts)//Option 1
        {

            Console.WriteLine("S K R I V  I N L Ä G G  I  G Ä S T B O K E N");

            //No empty strings
            string author = string.Empty;
            string textmessage = string.Empty;

            while (string.IsNullOrWhiteSpace(author))
            {
                //Namn
                Console.Write("Ange ditt namn: ");
                author = Console.ReadLine().Trim();

                if (string.IsNullOrWhiteSpace(author))
                {
                    Console.WriteLine("Namn får inte vara tom");
                }
            }
            while (string.IsNullOrWhiteSpace(textmessage))
            {
                //Skriv inlägg
                Console.Write("Skriv ditt inlägg: ");
                textmessage = Console.ReadLine().Trim();
                //Om meddelandet är tomt
                if (string.IsNullOrWhiteSpace(textmessage))
                {
                    Console.WriteLine("Inlägget får inte vara tomt");
                }
            }


            int setId = 1;
            if (posts.Count > 0)
            {
                setId = posts.Max(post => post.Id) + 1;
            }

            GuestBookObject post = new GuestBookObject
            {
                Id = setId,
                Author = author,
                Textmessage = textmessage
            };

            //Spara till JSONFil
            posts.Add(post);
            Console.WriteLine("Ditt meddelande har sparats i gästboken");

            Console.WriteLine("Tryck på 'H' för att gå tillbaka till huvudmenyn.");

            char MainmenuOrNewPost = Console.ReadKey().KeyChar; // Wait for user input

            if (MainmenuOrNewPost == 'H' || MainmenuOrNewPost == 'h')
            {
                Console.Clear();


            }
            else
            {
                Console.WriteLine("Felaktig inmatning. Tryck på Enter för att fortsätta.");
                Console.ReadKey(); // Vänta på enter key
                Console.Clear();
            }
        }

        static void Option2(List<GuestBookObject> posts)
        {
            Console.WriteLine("V Ä L J  E T T  I N L Ä G G   A T T  T A  B O R T");
            Console.WriteLine(" ");
            Console.WriteLine("Ange Id och tryck på enter för det inlägg som ska raderas:");
            // Läser in input och omvandlar från string till int 
            //Läser in meddelanden från json
            if (posts.Count > 0)
            {

                foreach (var post in posts)
                {
                    Console.WriteLine("[" + post.Id + "]  " + post.Author + " - " + post.Textmessage);
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Gästboken är tom.");
            }

            if (int.TryParse(Console.ReadLine(), out int deletePostId))
            {
                //Söker efter inlägg som matchar id
                var deletePost = posts.FirstOrDefault(post => post.Id == deletePostId);
                //Delete post
                if (deletePost != null)
                {
                    //Vill du verkligen radera?
                    Console.WriteLine($"Är du säker på att du vill radera Id {deletePost.Id} - {deletePost.Author} - {deletePost.Textmessage}? Tryck på Enter för att fortsätta (tryck på annan tangent för att avbryta).");

                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                    {
                        posts.Remove(deletePost);
                        Console.WriteLine("Inlägget har tagits bort");
                        
                    }
                    else
                    {
                        Console.WriteLine("Meddelandet har inte raderats");
                        
                    }
                }
                else
                {
                    Console.WriteLine("Inget inlägg med givet Id hittades");
                }
            }
            else
            {
                Console.WriteLine("Ogiltigt Id, Ange ett heltal");

            }

            Console.WriteLine("Tryck på 'H' för att gå tillbaka till huvudmenyn.");
            char MainmenuOrDeletePost = Console.ReadKey().KeyChar;

            if (MainmenuOrDeletePost == 'H' || MainmenuOrDeletePost == 'h')
            {
                Console.Clear();
                MainMenu(posts);
            }
            else
            {
                Console.Clear();
            }
        }
        static char MainMenu(List<GuestBookObject> posts)
        {
            //Huvudmenytext
            Console.WriteLine("S O F I A 'S  G U E S T B O O K");
            Console.WriteLine(" ");
            Console.WriteLine("1. Skriv ett inlägg i gästlistan");
            Console.WriteLine("2. Ta bort inlägg från gästlistan");
            Console.WriteLine(" ");
            Console.WriteLine("X. Avsluta");
            Console.WriteLine();

            GetPosts(posts);

            char userInput = Console.ReadKey().KeyChar;
            if (userInput != '1' && userInput != '2' && userInput != 'x' && userInput != 'X')
            {
                Console.Clear();
                Console.WriteLine("Felaktig inmatning. Tryck på Enter för att fortsätta.");
                Console.ReadKey(); // Wait for Enter key
                Console.Clear();
            }
            return userInput;
        }

        static void GetPosts(List<GuestBookObject> posts)
        {
            //Meddelanden från json
            if (posts.Count > 0)
            {
                Console.WriteLine("Inlägg från gästboken:");
                foreach (var post in posts)
                {
                    Console.WriteLine("[" + post.Id + "]  " + post.Author + " - " + post.Textmessage);
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Gästboken är tom.");
            }
        }
    }
}



