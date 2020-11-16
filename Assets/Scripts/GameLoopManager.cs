using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace AdVd.GlyphRecognition
{
    public class GameLoopManager : MonoBehaviour
    {
        [Header("Time of day")]
        //In hours
        public int currentTime;
        public int maxTime;
        public int minTime;

        [Header("Characters In Loop")]
        public bool inConversation;
        public int currentDialogueCounter;
        public List<Alien> aliens;

        private int alienCounter;

        [Header("Sounds")]
        public AudioClip submitAnswer;
        public AudioClip correctSymbolSound;
        public AudioClip incorrectSymbolSound;
        public AudioClip tellMeMoreSound;
        public AudioClip nextAlienSound;

        [Header("Animation")]
        public Animator alienSlideAnimator;
        private float timeBeforeNextConversation;
        [Header("glyphs")]
        public GlyphSet alienSymbols;
        //Compile le glyphe et son index dans le glyphset en fonction de son nom, comme ça on peut ressortir les informations facilement (Item1 et Item2)
        public Dictionary<string, Tuple<int, Glyph>> glyphDictionary = new Dictionary<string, Tuple<int, Glyph>>();

        //In game
        [Header("Added symbols in convo")]
        public List<string> addedGlyphsInConvo = new List<string>();
        public List<GameObject> addedGlyphsInGrid = new List<GameObject>();

        public GameObject gridObject;
        public GameObject symbolUIElement;

        //Singleton
        public static GameLoopManager gameManagerInstance;

        // Start is called before the first frame update
        void Start()
        {
            InitializeInfo();
            if (!gameManagerInstance)
            {
                gameManagerInstance = this.GetComponent<GameLoopManager>();
            }
        }

        private void InitializeInfo()
        {
            currentTime = maxTime;
            alienCounter = 0;
            inConversation = false;
            currentDialogueCounter = 0;
            int i = 0;
            foreach (Glyph g in alienSymbols)
            {
                glyphDictionary.Add(g.ToString(), new Tuple<int, Glyph>(i, g));
                i += 1;
            }
        }

        private void CheckIfGoToNextAlien()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AddNewSymbolToLayout(Glyph g)
        {
            string glyphName = g.ToString();
            int count = addedGlyphsInGrid.Count;
            if (!addedGlyphsInConvo.Contains(glyphName))
            {
                GameObject tile = (GameObject)Instantiate(Resources.Load("UI/AddedSymbolPrefab"), gridObject.transform);
                addedGlyphsInGrid.Add(tile);
                addedGlyphsInConvo.Add(glyphName);
                AddListenersToGridObject(tile, g);
            }
        }

        /// <summary>
        /// The value of c passed into the lambda is the value of c at the time the lambda is called (delegating at that point is like operating in a coroutine). 
        /// Since count iterates from 0-maxCalue over the course of the foreach loop, by the time the click listener is triggered, it is already maxValue, and will always be maxValue. 
        /// If we want to pass a different value into each lambda, we need to create a local value that captures the proper value of count for each iteration of the loop, hence changing
        /// the scope by calling it in a separate method.
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="index"></param>
        public void AddListenersToGridObject(GameObject tile, Glyph g)
        {
            //Glyph
            GlyphDisplay gDisplay = tile.transform.GetChild(0).GetComponent<GlyphDisplay>();
            gDisplay.glyph = g; 
            //Button
            Button b = tile.transform.GetChild(1).GetComponent<Button>();
            b.onClick.AddListener(delegate { DeleteGridSymbol(tile); });
        }

        public void DeleteGridSymbol(GameObject tile)
        {
            int index = GetDictionaryIndex(tile);
            int count = addedGlyphsInGrid.Count;
            if (index < count)
            {
                Destroy(addedGlyphsInGrid[index]);
                addedGlyphsInGrid.RemoveAt(index);
                addedGlyphsInConvo.RemoveAt(index);
            }
        }

        /// <summary>
        /// We cant directly attach an index to the delegate functions, because the index of our dictionaries is subject to change if we 
        /// delete a dictionary. (dictionaries that come after in the list will have their index re-updated). As such, we iterate through our
        /// object list until we find the corresponding object, at which point we know what index theyre in.
        /// </summary>
        public int GetDictionaryIndex(GameObject tileObject)
        {
            for (int i = 0; i < addedGlyphsInGrid.Count; i++)
            {
                if (GameObject.ReferenceEquals(tileObject, addedGlyphsInGrid[i]))
                {
                    Debug.Log("Found object index is: " + i.ToString());
                    return i;
                }
            }
            //If we dont find, but this should never happen
            Debug.LogError("Couldnt find such tile object in our dictionary list");
            return 0;
        }


        private IEnumerator ConversationCooldown()
        {
            yield return new WaitForSeconds(timeBeforeNextConversation);
            inConversation = false;
            yield return null;
        }

        public class Tuple<T, U>
        {
            public T Item1 { get; private set; }
            public U Item2 { get; private set; }

            public Tuple(T item1, U item2)
            {
                Item1 = item1;
                Item2 = item2;
            }
        }

        public static class Tuple
        {
            public static Tuple<T, U> Create<T, U>(T item1, U item2)
            {
                return new Tuple<T, U>(item1, item2);
            }
        }

    }
}
