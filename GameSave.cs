using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Starship
{
    public  class GameSave 
    {

        
        public List<Saucer> saucers;
        public List<Tir> tirs;
        public Vector2 vaisseauPosition;
        public string playerName;
        public int score;

        public GameSave(List<Saucer> saucers, List<Tir> tirs, Vector2 vaisseauPosition, string playerName, int score)
        {
            this.saucers = saucers;
            this.tirs = tirs;
            this.vaisseauPosition = vaisseauPosition;
            this.playerName = playerName;
            this.score = score;
        }

        public GameSave()
        {
            
                this.saucers = new List<Saucer>();
                this.tirs = new List<Tir>();
                this.vaisseauPosition = new Vector2(0, 0);
                this.playerName = "";
                this.score = 0;
            
            
        }


        public void saveGame()
        {
            string filename = "save.json";
            //check if the file exists
            if (!System.IO.File.Exists(filename))
            {
                //create the file
                System.IO.File.Create(filename);
            }
            //write the data
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            System.IO.File.WriteAllText(filename, json);

        }

        public GameSave loadGame()
        {
            string filename = "save.json";
            //check if the file exists
            if (!System.IO.File.Exists(filename))
            {
                //create the file
                System.IO.File.Create(filename);
            }
            //write the data
           


                string jsonFromFile = System.IO.File.ReadAllText(filename);
                GameSave gameState = (GameSave)JsonConvert.DeserializeObject<GameSave>(jsonFromFile);

                Console.WriteLine("game loaded successfully.");
                return gameState;
                    
            

        }

        //check if there is a save file
        public bool isSaveExists()
        {
            string filename = "save.json";
            //check if the file exists
            if (!System.IO.File.Exists(filename))
            {
                return false;
            }
            else
            {
               //check if the file is empty
               if (new FileInfo(filename).Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }


        }

        //clear save file
        public void clearSave()
        {
            string filename = "save.json";
            //check if the file exists
            if (!System.IO.File.Exists(filename))
            {
                //create the file
                System.IO.File.Create(filename);
            }
            //write the data
            try
            {
                //delete any data from the file
                File.WriteAllText(filename, string.Empty);
                Console.WriteLine("game cleared successfully.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error occurred while clearing game: " + e.Message);
            }

        }










    }
}
