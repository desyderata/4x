﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using game.map.units;
using game.actor;

namespace game.map {
    public enum Biome {
        Highlands, Plains, Forrest, Ocean, Desert, Jungle
    }

    public static class BiomeExtensions {
        private static Sprite sprite = Resources.Load<Sprite>("Textures/Hexagon");

        public static Sprite GetSprite(this Biome b) {
            return sprite;
        }

        public static float Dropoff(this Biome b) {
            switch (b) {
                case Biome.Highlands:
                    return 1f;
                case Biome.Plains:
                    return 1f;
                case Biome.Forrest:
                    return 1f;
                case Biome.Ocean:
                    return 1f;
                case Biome.Desert:
                    return -1f;
                case Biome.Jungle:
                    return 1f;
                default:
                    return -1f;
            }

        }

        public static bool Passable(this Biome b) {
            return b.PassCost() != -1;
        }

        public static int PassCost(this Biome b) {
            switch(b) {
                case Biome.Highlands:
                    return 1;
                case Biome.Plains:
                    return 1;
                case Biome.Forrest:
                    return 1;
                case Biome.Ocean:
                    return 1;
                case Biome.Desert:
                    return -1;
                case Biome.Jungle:
                    return 1;
                default:
                    return -1;
            }
        }
     }

    class Hex : MonoBehaviour {
        WorldMap w;
        public HexLoc loc;
        public Biome b;

        // TODO: use enum instead?
        public bool corrupted;

        public HashSet<Unit> units;

        HexModel model;
        public Building building;
        public bool powered;

        public void init(WorldMap w, HexLoc loc) {
            this.w = w;
            this.loc = loc;
            this.b = Biome.Forrest;
            this.corrupted = false;

            units = new HashSet<Unit>();

            transform.localPosition = w.l.HexPixel(loc);

            var o = new GameObject("Hex Model");
            o.transform.parent = transform;
            o.transform.localPosition = new Vector2(0, 0);
            model = o.AddComponent<HexModel>();
            model.init(this);
        }

        public void NewTurn(Actor old, Actor cur) {
            if (building != null) {
                building.NewTurn(old, cur);
            }
            foreach(Unit u in units) {
                u.NewTurn(old, cur);
            }
        }

        public List<Hex> Neighbors() {
            List<Hex> n = new List<Hex>();
            
            for(int i = 0; i < 6; i++) {
                HexLoc l = loc.Neighbor(i);
                if (w.map.ContainsKey(l)) {
                    n.Add(w.map[l]);
                }
            }

            return n;
        }

        void Start() {

        }

        void Update() {
        }

        private class HexModel : MonoBehaviour {
            public Hex h;
            SpriteRenderer sp;

            public void init(Hex h) {
                this.h = h;

                sp = gameObject.AddComponent<SpriteRenderer>();

                sp.sprite = Resources.Load<Sprite>("Textures/Hexagon");
                sp.transform.localScale = new Vector3(1.9f, 1.9f);
            }

            void Update() {
                Color c;
                switch(h.b) {
                case Biome.Highlands:
                    c = Color.gray;
                    break;
                case Biome.Plains:
                    c = Color.yellow;
                    break;
                case Biome.Forrest:
                    c = Color.green;
                    break;
                case Biome.Ocean:
                    c = Color.blue;
                    break;
                case Biome.Desert:
                    c = Color.white;
                    break;
                case Biome.Jungle:
                    c = Color.green;
                    break;
                default:
                    c = Color.black;
                    break;
                }

                if (h.corrupted) {
                    c = new Color(0.5f, 0, 0.5f);
                }

                sp.color = c;
            }
        }
    }
}
