using System.Collections;
using System.Collections.Generic;
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
        public List<string> addedGlyphsInConvo = new List<string>();
        public List<GameObject> addedGlyphsInGrid = new List<GameObject>();

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
