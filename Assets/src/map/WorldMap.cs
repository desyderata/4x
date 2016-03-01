﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using game.actor;
using game.map.units;
using UnityEngine;

namespace game.map {
    class WorldMap : MonoBehaviour{
        public Dictionary<HexLoc, Hex> map;
        public Layout l;

        public void init(Layout l) {
            this.l = l;

            map = new Dictionary<HexLoc, Hex>();

            for (int x = 0; x <= 64; x++) {
                for (int y = 0; y <= 64; y++) {
					HexLoc hl = new HexLoc(x, y);
                    map.Add(hl, makeHex(hl));
                }
            }
            //map[new HexLoc(10, 10, -20)].corrupted = true;
        }

        private Hex makeHex(HexLoc l) {
            var o = new GameObject(l.ToString());
            o.transform.parent = transform;
            Hex h = o.AddComponent<Hex>();
            h.init(this, l);

            return h;
        }
        
        void Start() {

        }

        void Update() {

        }

        public void PreTurn(Actor old, Actor cur) {

            foreach (KeyValuePair<HexLoc, Hex> kv in map) {
                kv.Value.PreTurn(old, cur);
            }



        }


        public void NewTurn(Actor old, Actor cur) {
            List<Building> poweredBuildings = new List<Building>();
            foreach (KeyValuePair<HexLoc, Hex> kv in map) {
                kv.Value.pn = null;
                var b = kv.Value.building;
                if (b != null) {
                    b.grided = false;
                    b.pn = null;
                    if (b.Powered()) {
                        poweredBuildings.Add(b);
                    }
                }
            }

            foreach (Building b in poweredBuildings) {
                if (!b.grided) {
                    b.SpreadPower(new PowerNetwork());
                }
            }


            foreach (KeyValuePair<HexLoc, Hex> kv in map) {
                kv.Value.NewTurn(old, cur);
            }
        }

        public void PostTurn(Actor old, Actor cur) {
            foreach (KeyValuePair<HexLoc, Hex> kv in map) {
                kv.Value.PostTurn(old, cur);
            }
        }

    }
}
