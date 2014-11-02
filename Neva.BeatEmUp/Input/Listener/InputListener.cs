using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Neva.BeatEmUp.Input.Buffer;
using Neva.BeatEmUp.Input.Trigger;

namespace Neva.BeatEmUp.Input.Listener
{
    public delegate void InputEvent(InputEventArgs args);
    public abstract class InputListener
    {

        #region Properties

        #region Protected

        /// <summary>
        /// Itse bindit, string kuvaa bindin nimeä ja mapping nappeja jotka siihen on bindattu
        /// </summary>
        protected Dictionary<string, Mapping> Mappings
        {
            get;
            private set;
        }

        /// <summary>
        /// lista nimistä mihin mäppeihin nappi kuuluu
        /// </summary>
        protected Dictionary<int, List<string>> MappingNames
        {
            get;
            private set;
        }

        #endregion

        #region Public

        /// <summary>
        /// Pitää yllä framejen mäppejä
        /// </summary>
        public InputBuffer InputBuffer
        {
            get;
            protected set;
        }

        /// <summary>
        /// Palauttaa mäppäysten määrän
        /// </summary>
        public int Count
        {
            get { return Mappings.Count;  }
        }

        /// <summary>
        /// Palauttaa raakadatana napit jotka on bindattu
        /// </summary>
        public IEnumerable<int> RawBinds
        {
            get { return MappingNames.Keys;  }
        }

        #endregion

        #endregion

        #region Ctor

        protected InputListener()
        {
            MappingNames = new Dictionary<int, List<string>>();
            Mappings = new Dictionary<string, Mapping>();
        }

        #endregion

        #region Abstract

        /// <summary>
        /// Kuuntelee nappeja
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Update(GameTime gameTime);



        #endregion

        #region Methods

        #region Mapping

        /// <summary>
        /// Mäppää napin rekisteröimään input eventin
        /// </summary>
        /// <param name="mappingName">Mäppäyksen nimi (identifier)</param>
        /// <param name="func">Inputevent joka laukaistaan heti kun nappi on painettu/löysätty</param>
        /// <param name="triggers">Mitkä napit triggeröivät eventin</param>
        public void Map(string mappingName, InputEvent func, params ITrigger[] triggers)
        {
            if (func == null) throw new ArgumentNullException("func", "Callback cannot be null");
            if (triggers.Length == 0) throw new ArgumentException("Triggers[] can't be empty", "triggers");

            Mapping mapping = GetMapping(mappingName);
            if (mapping == null)
            {
                mapping = new Mapping(mappingName, func);
                Mappings[mappingName] = mapping;
            }
            else
            {
                throw new ArgumentException(String.Format("There is already mapping called {0}", mappingName), "mappingName");
            }

            foreach (var trigger in triggers)
            {
                MapTrigger(mappingName, trigger);
                mapping.Triggers.Add(trigger);
            }
        }

        /// <summary>
        /// Mäppää samalle bindille alternate keyn
        /// </summary>
        /// <param name="mappingName">Mihin bindiin</param>
        /// <param name="triggers">Mitkä napit</param>
        public void MapAlternate(string mappingName, params ITrigger[] triggers)
        {
            Mapping mapping = GetMapping(mappingName);

            if (mapping == null)
            {
                throw new ArgumentException("There is no mapping called " + mappingName, "mappingName");
            }

            foreach (var trigger in triggers)
            {
                MapTrigger(mappingName, trigger);
                mapping.Triggers.Add(trigger);
            }
        }

        /// <summary>
        /// Mäppää triggerin
        /// </summary>
        /// <param name="mappingName"></param>
        /// <param name="trigger"></param>
        private void MapTrigger(string mappingName, ITrigger trigger)
        {
            List<string> names = GetMappingNames(trigger);
            if (names == null)
            {
                names = new List<string>();
                MappingNames[trigger.TriggerHash()] = names;
            }

            if (!names.Contains(mappingName))
            {
                names.Add(mappingName);
            }
            else
            {
                throw new ArgumentException(
                    String.Format("Trying map {0} twice", mappingName)
                    );
            }
        }

        #endregion

        /// <summary>
        /// Tarkistaa onko triggeriä mäpätty mihinkään
        /// </summary>
        /// <param name="trigger"></param>
        /// <returns></returns>
        public bool HasMappings(ITrigger trigger)
        {
            return MappingNames.ContainsKey(trigger.TriggerHash());
        }

        /// <summary>
        /// Hakee kaikki triggerin mäppäykset
        /// </summary>
        /// <param name="trigger">Minkä napin mäppäykset</param>
        /// <returns>Listan kaikista mäppäyksistä, lista on tyhjä jos ei ole yhtään</returns>
        public List<Mapping> GetMappings(ITrigger trigger)
        {
            List<Mapping> mappings = new List<Mapping>();
            List<string> names = GetMappingNames(trigger);
            // ei ole yhtään mäppäystä
            if (names == null) return mappings;

            names.ForEach(name => mappings.Add(Mappings[name]));
            return mappings;
        }

        /// <summary>
        /// Hakee mäppäyksen, palauttaa null jos ei löydy
        /// </summary>
        /// <param name="mappingName">Mitä mäppiä haetaan</param>
        /// <returns>Mäppäyksen joka vastaa nimeä, null jos ei löydy</returns>
        public Mapping GetMapping(string mappingName)
        {
            Mapping mapping;
            Mappings.TryGetValue(mappingName, out mapping);
            return mapping;
        }

        /// <summary>
        /// Hakee kaikki mäppäykset joihin tämä näppäin on mäpätty
        /// </summary>
        /// <param name="trigger">Minkä triggerin mäppäyksiä etitään</param>
        /// <returns>Listan nimistä joihin tämä näppäin on mäpätty, null jos ei ole mihinkään</returns>
        public List<String> GetMappingNames(ITrigger trigger)
        {
            List<string> names;
            MappingNames.TryGetValue(trigger.TriggerHash(), out names);
            return names;
        }

        /// <summary>
        /// Poistaa kaikki mäppäykset
        /// </summary>
        /// <returns>true</returns>
        public bool ClearMappings()
        {
            Mappings.Clear();
            MappingNames.Clear();
            return true;
        }

        /// <summary>
        /// Poistaa triggerin kaikki mäppäykset
        /// </summary>
        /// <param name="trigger">Minkä napin mäppäykset poistetaan</param>
        /// <returns>True jos poistettiin, false jos ei (ei ole mäppäyksiä)</returns>
        public bool ClearMappings(ITrigger trigger)
        {
            // jos ei ole mitään niin turha yrittää poistaakkaan
            if (!HasMappings(trigger)) return false;

            var names = GetMappingNames(trigger);
            foreach (var name in names)
            {
                Mappings[name].Triggers.Remove(trigger);
                // jos ei ole enää yhtään bindiä niin poistetaan koko mäppäys
                if (Mappings[name].Triggers.Count == 0)
                {
                    Mappings.Remove(name);
                }
            }
            // lopuksi poistetaan viittaukset
            MappingNames.Remove(trigger.TriggerHash());
            return true;
        }

        /// <summary>
        /// Poistaa mäppäyksen ja kaikki sen viitteet
        /// </summary>
        /// <param name="mappingName">Mikä mäppäys poistetaan</param>
        /// <returns>true jos poistettiin, false muuten</returns>
        public bool RemoveMapping(string mappingName)
        {
            if (!Mappings.ContainsKey(mappingName)) return false;

            var mapping = Mappings[mappingName];
            foreach (var trigger in mapping.Triggers)
            {
                MappingNames[trigger.TriggerHash()].Remove(mappingName);
            }
            Mappings.Remove(mappingName);
            return true;
        }

        #endregion
    }
}
