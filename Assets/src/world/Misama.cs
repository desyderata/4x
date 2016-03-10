﻿using UnityEngine;
using System.Collections;
using System;
using game.ui;

namespace game.world {
	class Miasma : MonoBehaviour {
		MiasmaModel model;
		WorldMap w;
		public int level { get; set; }

		Hex _h;
		public Hex h {
			get {
				return _h;
			}
			set {
				if (_h != null) {
					_h.unit = null;
				}
				this._h = value;
				if (_h != null) {
					transform.parent = _h.gameObject.transform;
					_h.miasma = this;
				}
			}
		}

		public void init(WorldMap w, Hex h) {
			this.w = w;
			this.h = h;

			var obj = new GameObject ("Misama Model");
			obj.transform.parent = transform;

			model = obj.AddComponent<MiasmaModel> ();
			model.init (this);
		}

		public void setVisible() {
			this.model.sr.enabled = true;
		}

		public bool canSpread() {
			switch (level) {
			case 0:
				return false;
			case 1:
				if (w.time == GameTime.Night) {
					return true;
				}
				return false;
			default:
				return true;
			}
		}

		void Update() {
			transform.localPosition = new Vector3 (0, 0, 0);
		}

		void Start() {
		
		}

		public override string ToString () {
			return String.Format ("Miasma");
		}
			
		public void Die() {
			Destroy(model);
			h = null;
			Destroy (gameObject);
		}

		private class MiasmaModel : MonoBehaviour {
			public SpriteRenderer sr;
			Miasma c;
			Sprite[] texs;

			public void init(Miasma c) {
				this.c = c;

				sr = gameObject.AddComponent<SpriteRenderer> ();
				sr.transform.localScale = new Vector3 (1.9f, 1.9f, 1.9f);
				transform.localPosition = new Vector3(0, 0, Layer.Miasma);

				texs = new Sprite[4] {
					Resources.Load<Sprite> ("Textures/T_Miasma0"),
					Resources.Load<Sprite> ("Textures/T_Miasma1"),
					Resources.Load<Sprite> ("Textures/T_Miasma2"),
					Resources.Load<Sprite> ("Textures/T_Miasma3")
				};

				sr.sprite = texs [0];
			}

			void Update() {
				sr.sprite = texs [c.level];
			}
		}
	}
}