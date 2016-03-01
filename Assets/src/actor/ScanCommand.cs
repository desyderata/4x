﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using game.map.units;
using game.map;
using UnityEngine;

namespace game.actor {
	class ScanCommand : Command {

		Unit u;
		Hex target;

		public ScanCommand(Actor a, Unit u, Hex target) : base(a) {
			this.target = target;
			Console.WriteLine(target);
		}

		public override void Apply(WorldMap w) {
			UnityEngine.Debug.Log("Scanned " + target);
			target.scanned = true;
			foreach (Hex h in target.Neighbors()) {
				h.scanned = true;
			}
		}
	}
}

