﻿using UnityEngine;
using System;
using System.Collections.Generic;
using game.actor;
using game.actor.commands;
using game.math;
using game.world;
using game.world.buildings;
using game.world.units;

namespace game.ui {
	class UIManager : MonoBehaviour {
		private static Texture2D UI_Move = Resources.Load<Texture2D>("Textures/T_UI_Move");
		private static Texture2D UI_Scan = Resources.Load<Texture2D>("Textures/T_UI_Scan");
		private static Texture2D UI_Build = Resources.Load<Texture2D>("Textures/T_UI_Build");
		private static Texture2D UI_Purify = Resources.Load<Texture2D>("Textures/T_UI_Purify");
		private static Texture2D UI_MoveH = Resources.Load<Texture2D>("Textures/T_UI_MoveH");
		private static Texture2D UI_ScanH = Resources.Load<Texture2D>("Textures/T_UI_ScanH");
		private static Texture2D UI_BuildH = Resources.Load<Texture2D>("Textures/T_UI_BuildH");
		private static Texture2D UI_PurifyH = Resources.Load<Texture2D>("Textures/T_UI_PurifyH");
		private static Texture2D UI_MoveC = Resources.Load<Texture2D>("Textures/T_UI_MoveC");
		private static Texture2D UI_ScanC = Resources.Load<Texture2D>("Textures/T_UI_ScanC");
		private static Texture2D UI_BuildC = Resources.Load<Texture2D>("Textures/T_UI_BuildC");
		private static Texture2D UI_PurifyC = Resources.Load<Texture2D>("Textures/T_UI_PurifyC");
		private static Texture2D UI_End = Resources.Load<Texture2D>("Textures/T_UI_End");
		private static Texture2D UI_EndH = Resources.Load<Texture2D>("Textures/T_UI_EndH");
		private static Texture2D UI_EndC = Resources.Load<Texture2D>("Textures/T_UI_EndC");

		private static Texture2D UI_Cond = Resources.Load<Texture2D>("Textures/T_UI_Cond");
		private static Texture2D UI_CondH = Resources.Load<Texture2D>("Textures/T_UI_CondH");
		private static Texture2D UI_CondC = Resources.Load<Texture2D>("Textures/T_UI_CondC");
		private static Texture2D UI_Gate = Resources.Load<Texture2D>("Textures/T_UI_Gate");
		private static Texture2D UI_GateH = Resources.Load<Texture2D>("Textures/T_UI_GateH");
		private static Texture2D UI_GateC = Resources.Load<Texture2D>("Textures/T_UI_GateC");
		private static Texture2D UI_Harv = Resources.Load<Texture2D>("Textures/T_UI_Harv");
		private static Texture2D UI_HarvH = Resources.Load<Texture2D>("Textures/T_UI_HarvH");
		private static Texture2D UI_HarvC = Resources.Load<Texture2D>("Textures/T_UI_HarvC");
		private static Texture2D UI_Tow = Resources.Load<Texture2D>("Textures/T_UI_Tow");
		private static Texture2D UI_TowH = Resources.Load<Texture2D>("Textures/T_UI_TowH");
		private static Texture2D UI_TowC = Resources.Load<Texture2D>("Textures/T_UI_TowC");

		private static Texture2D UI_Unit = Resources.Load<Texture2D>("Textures/T_UI_Unit");
		private static Texture2D UI_UnitH = Resources.Load<Texture2D>("Textures/T_UI_UnitH");
		private static Texture2D UI_UnitC = Resources.Load<Texture2D>("Textures/T_UI_UnitC");
		private static Texture2D UI_Delete = Resources.Load<Texture2D>("Textures/T_UI_Delete");
		private static Texture2D UI_DeleteH = Resources.Load<Texture2D>("Textures/T_UI_DeleteH");
		private static Texture2D UI_DeleteC = Resources.Load<Texture2D>("Textures/T_UI_DeleteC");

		private static Texture2D iconUnit = Resources.Load<Texture2D>("Textures/T_Icon_Unit");
		private static Texture2D iconBiome = Resources.Load<Texture2D>("Textures/T_Icon_Biome");
		private static Texture2D iconBuilding = Resources.Load<Texture2D>("Textures/T_Icon_Building");
		private static Texture2D iconMiasma = Resources.Load<Texture2D>("Textures/T_Icon_Miasma");

		// x.split("\n").map { |l| l.split(" ")[3] }.join(", ")
		private static Texture2D[] texes = new Texture2D[] {
			UI_Move, UI_Scan, UI_Build, UI_Purify, UI_MoveH,
			UI_ScanH, UI_BuildH, UI_PurifyH, UI_MoveC, UI_ScanC, UI_BuildC, UI_PurifyC, UI_End, UI_EndH, UI_EndC,
			UI_Cond, UI_CondH, UI_CondC, UI_Gate, UI_GateH, UI_GateC, UI_Harv, UI_HarvH, UI_HarvC, UI_Tow, UI_TowH, UI_TowC
		};

		public enum State {
			Default,
			Selected,
			Moving,
			Building
		};

		// Textures
		//Dictionary<string, Texture2D> texes;


		// GUI Stuff
		private GUIStyle ButtonStyle;
		Rect helpWindow = new Rect(20, 20, 200, 200);
		bool showHelp;
		bool building;

		private UINotification note;
		private UIHighlight hl;
		private UIMovement movement;
		public State state;

		WorldMap w;
		GameManager gm;
		public TutorialManager tm;
		MinimapManager mmm;
		public bool ev_view = true;
		TooltipUI t;

		Player p;
		public Hex h_target;
		protected Unit u_target {
			get {
				if (h_target != null && 
					h_target.units.Count > 0) {
					return h_target.units[0];
				} else {
					return null;
				}
			}
		}

		public void init(GameManager gm, Player p, WorldMap w) {
			this.gm = gm;
			this.p = p;
			this.w = w;

			state = State.Default;

			note = new GameObject ("UI Notification").AddComponent<UINotification> ();
			note.init ();
			note.transform.parent = transform;

			hl = new GameObject ("UI Highlight").AddComponent<UIHighlight> ();
			hl.init (this);
			hl.transform.parent = transform;

			tm = new GameObject("Tutorial Manager").AddComponent<TutorialManager>();
			tm.init ();
			tm.transform.parent = transform;

			mmm = gameObject.AddComponent<MinimapManager>();
			mmm.transform.parent = transform;

			//t = gameObject.AddComponent<TooltipUI> ();
			//t.init (this);

			//helper = gameObject.AddComponent<HelperUI>();
			//helper.init(this);

			showHelp = false;
			building = false;
		}

		void Start() { 
		}

		private void buildBuilding(BuildingType type) {
			try {
				p.AddCommand(new BuildCommand(p, u_target, h_target, type));
			} catch (Exception e) {
				Notify (e.Message, Color.red);
			}
			EventManager.PostBuildEvent(new BuildEventArgs { name = type.ToString(), turns = type.BuildTotal() / type.BuildPerTurn() });
			state = State.Selected;
		}

		// Tutorial
		public void postEvent(int code) {
			if (!tm.milestones [code] && tm.milestones[code-1]) {
				EventManager.PostTutorialEvent (new TutorialEventArgs { tut_id = code });
				tm.milestones [code] = true;
			}
		}

		public void postEvents(params int[] codes) {
			if (!tm.milestones [codes [0]] && tm.milestones [codes [0] - 1]) {
				foreach (int i in codes) {
					tm.milestones [i] = true;
				}
				tm.playQueue (codes);
			}
		}

		// Notification
		public void Notify(string msg, Color c) {
			this.note.post (msg, c);
		}

		// GUI
		private bool inToolbarBoundary(Vector3 v) {
			return v.y < Screen.height * .8f;
		}

		void MakeHelpWindow(int id) {
			if (GUI.Button (new Rect (10, 20, 100, 20), "Hello World")) {
				Debug.Log ("Clicked Window");
			}
			GUI.DragWindow();
		}

		public void LookAt(Hex h) {
			gm.pc.setLocation(w.l.HexPixel(h.loc));
		}

		public Hex GetHexAtMouse() {
			Vector3 worldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			HexLoc l = w.l.PixelHex (worldPos);
			if (w.map.ContainsKey (l)) {
				Hex h = w.map [l];
				return h;
			}
			return null;
		}

		void Update() {
			if (Input.GetMouseButtonUp (0)) {
				if (!inToolbarBoundary (Input.mousePosition)) {
					if ((state == State.Default) || (state == State.Selected)) {
						Hex h = GetHexAtMouse ();
						if (h != null) {
							this.h_target = h;
							if (this.tm.milestones [1] == false) {
								postEvent (1);
							} else if (!tm.milestones [6] && tm.milestones [5]) {
								postEvent (6);
							} else if (!tm.milestones [11] && tm.milestones [10]) {
								postEvent (11);
							}
						}
						state = State.Selected;
					} else if (state == State.Moving) {
						if (Input.GetMouseButtonUp (0)) {
							Hex h = GetHexAtMouse ();
							var unit = u_target;
							try {
								p.AddAllCommands (MoveCommand.pathfind (w, p, unit, h));
								this.h_target = h;
							} catch (Exception e) {
								Notify (e.Message, Color.red);
							}
							state = State.Selected;
							postEvent (3);
							//EventManager.TriggerMoveEventAfter(new MoveEventArgs { stamina = unit.actions });
							Debug.Log ("Added Move Command");
						}
					} else if (state == State.Building) {

					}
				}
			}

			if (Input.GetMouseButtonUp(1)) {
				if (!inToolbarBoundary(Input.mousePosition)) {
					if (state == State.Selected) {
						Hex h = GetHexAtMouse();
						Unit u = u_target;

						if (u != null) {
							try {
								p.AddAllCommands(MoveCommand.pathfind(w, p, u, h));
								this.h_target = h;
								postEvent(3);
							} catch (Exception e) {
								EventManager.PostInvalidAction(new InvalidActionArgs { msg = e.Message });
							}
						}

					}
				}
			}

			if (state == State.Selected &&
				u_target != null &&
				Input.GetKeyUp(KeyCode.B)) {
				state = State.Building;
			}

			if (state == State.Selected &&
				u_target != null &&
				Input.GetKeyUp(KeyCode.M)) {
				state = State.Moving;
			}

			if (state == State.Moving &&
				h_target != null) {

			}

			if ((state == State.Default || state == State.Selected)
				&& Input.GetKeyUp(KeyCode.Tab)) {
				var u = u_target;

				foreach (KeyValuePair<HexLoc, Hex> kv in w.map) {
					var h = kv.Value;
					if (h.units.Count > 0 && h.units[0] != u_target && h.units[0].actions > 0) {
						h_target = h;
						LookAt(h);
						break;
					}
				}
			}

			if (state == State.Building) {
				if (Input.GetKeyUp(KeyCode.Alpha1)) {
					buildBuilding(BuildingType.Conduit);
				}

				if (Input.GetKeyUp(KeyCode.Alpha2)) {
					buildBuilding(BuildingType.Harvester);
				}

				if (Input.GetKeyUp(KeyCode.Alpha3)) {
					buildBuilding(BuildingType.Purifier);
				}

				if (Input.GetKeyUp(KeyCode.Alpha4)) {
					buildBuilding(BuildingType.WarpGate);
				}
			}

			/*
			if (Input.GetKeyDown (KeyCode.Escape)) {
				Application.Quit();
			}
			*/
		}






		void OnGUI() {
			float b_height = (Screen.height * .2f) / 3;
			float b1_start = (Screen.height * .8f);

			float p1_min = 0; float p1_max = Screen.height * .25f;

			// Panel 1

			// event text 1
			if (GUI.Button (new Rect(p1_min, Screen.height * .8f, Screen.width*.25f - b_height, b_height), "Event")) {

			}

			// event text 2
			if (GUI.Button (new Rect(p1_min, Screen.height * .8f + b_height , Screen.width*.25f - b_height, b_height), "Event")) {

			}

			// event text 3
			if (GUI.Button (new Rect(p1_min, Screen.height * .8f + b_height + b_height, Screen.width*.25f - b_height, b_height), "Event")) {

			}

			// event button 1
			if (GUI.Button (new Rect(Screen.width * .25f - b_height, b1_start, b_height, b_height), iconUnit)) {

			}

			// event button 2
			if (GUI.Button (new Rect(Screen.width * .25f - b_height, b1_start + b_height, b_height, b_height), iconUnit)) {

			}

			// event button 3
			if (GUI.Button (new Rect(Screen.width * .25f - b_height, b1_start + b_height + b_height, b_height, b_height), iconUnit)) {

			}

			// Panel 2
			GUILayout.BeginArea (new Rect (Screen.width *.25f, Screen.height * .8f, Screen.width * .50f, Screen.height * .2f));
			GUILayout.BeginHorizontal ();

			var width = GUILayout.Width(Screen.width * .1f);
			var height = GUILayout.Height(Screen.height * .1f);

			ButtonStyle = new GUIStyle(GUI.skin.label);
			ButtonStyle.normal.background = UI_Move; ButtonStyle.hover.background = UI_MoveH; ButtonStyle.active.background = UI_MoveC;

			// Move Button
			if (GUILayout.Button("", ButtonStyle, width, height)) {
				if (state == State.Selected) {
					if (u_target != null) {
						//EventManager.TriggerMoveEventBefore(new MoveEventArgs {stamina = u_target.actions});
						state = State.Moving;
						postEvent (2);
					}
				} else {
					state = State.Default;
				}
				//Debug.Log ("Added Move Command");
			}

			// Build Button
			ButtonStyle.normal.background = UI_Build; ButtonStyle.hover.background = UI_BuildH; ButtonStyle.active.background = UI_BuildC;
			if (GUILayout.Button("", ButtonStyle, width, height)){
				if (state == State.Selected) {
					if (u_target != null) {
						state = State.Building;
						postEvents (7, 9);
					}
				} else if (state == State.Building) {
					state = State.Selected;
				}
				//Debug.Log ("Added Build Command");
			}

			// Scan Button
			ButtonStyle.normal.background = UI_Scan; ButtonStyle.hover.background = UI_ScanH; ButtonStyle.active.background = UI_ScanC;
			if (GUILayout.Button("", ButtonStyle, width, height)) {
				if (state == State.Selected) {
					if (u_target != null) {
						try {
							p.AddCommand (new ScanCommand (p, u_target, h_target));
							postEvent(4);
						} catch (Exception e) {
							EventManager.PostInvalidAction (new InvalidActionArgs{ msg = e.Message });
						}

					}
				}
				//Debug.Log ("Added Scan Command");
			}

			ButtonStyle.normal.background = UI_Purify; ButtonStyle.hover.background = UI_PurifyH; ButtonStyle.active.background = UI_PurifyC;
			if (GUILayout.Button("", ButtonStyle, width, height)) {
				if (state == State.Selected) {
					if (u_target != null) {
						try {
							p.AddCommand (new CleanseCommand (p, u_target));
						} catch (Exception e) {
							EventManager.PostInvalidAction (new InvalidActionArgs{ msg = e.Message });
						}

					}
				}
				//Debug.Log ("Added Purify Command");
			}

			ButtonStyle.normal.background = UI_End; ButtonStyle.hover.background = UI_EndH; ButtonStyle.active.background = UI_EndC;
			if (GUILayout.Button ("", ButtonStyle, GUILayout.Width (Screen.width * .12f), height)) {
				p.AddCommand(new EndTurnCommand(p));
				//Debug.Log ("Added End Turn Command");
			}

			GUILayout.EndHorizontal ();
			GUILayout.EndArea ();

			if (state == State.Building) {
				GUILayout.BeginArea (new Rect (Screen.width * .3f, Screen.height * .7f, Screen.width / 2, Screen.height * .9f));
				GUILayout.BeginHorizontal ();

				ButtonStyle.normal.background = UI_Cond; ButtonStyle.hover.background = UI_CondH; ButtonStyle.active.background = UI_CondC;
				if (GUILayout.Button ("", ButtonStyle, GUILayout.Width (Screen.height * 0.08f), GUILayout.Height (Screen.height * 0.08f))) {
					buildBuilding(BuildingType.Conduit);
					postEvents (10);
				}

				ButtonStyle.normal.background = UI_Harv; ButtonStyle.hover.background = UI_HarvH; ButtonStyle.active.background = UI_HarvC;
				if (GUILayout.Button ("", ButtonStyle, GUILayout.Width (Screen.height * 0.08f), GUILayout.Height (Screen.height * 0.08f))) {
					buildBuilding(BuildingType.Harvester);
					postEvents (8, 9);
				}

				ButtonStyle.normal.background = UI_Tow; ButtonStyle.hover.background = UI_TowH; ButtonStyle.active.background = UI_TowC;
				if (GUILayout.Button ("", ButtonStyle, GUILayout.Width (Screen.height * 0.08f), GUILayout.Height (Screen.height * 0.08f))) {
					buildBuilding(BuildingType.Purifier);
				}

				ButtonStyle.normal.background = UI_Gate; ButtonStyle.hover.background = UI_GateH; ButtonStyle.active.background = UI_GateC;
				if (GUILayout.Button ("", ButtonStyle, GUILayout.Width (Screen.height * 0.08f), GUILayout.Height (Screen.height * 0.08f))) {
					buildBuilding(BuildingType.WarpGate);
				}
				GUILayout.EndHorizontal ();
				GUILayout.EndArea ();
			}

			// Right side mini-toolbar.
			GUILayout.BeginArea(new Rect(Screen.width * .5f, Screen.height * .7f, Screen.width / 2, Screen.height * .9f));
			GUILayout.BeginHorizontal();

			if (h_target != null &&
				h_target.building != null &&
				h_target.building.GetType() == typeof(WarpGate)) {
				var wg = (WarpGate)h_target.building;

				ButtonStyle.normal.background = UI_Unit; ButtonStyle.hover.background = UI_UnitH; ButtonStyle.active.background = UI_UnitC;
				if (GUILayout.Button("", ButtonStyle, GUILayout.Width(Screen.height * 0.08f), GUILayout.Height(Screen.height * 0.08f))) {
					try {
						p.AddCommand(new WarpUnitCommand(p, wg));
					} catch (Exception e) {
						Notify (e.Message, Color.red);
					}
				}

			}

			if (h_target != null && h_target.building != null) {
				ButtonStyle.normal.background = UI_Delete; ButtonStyle.hover.background = UI_DeleteH; ButtonStyle.active.background = UI_DeleteC;
				if (GUILayout.Button("", ButtonStyle, GUILayout.Width(Screen.height * 0.08f), GUILayout.Height(Screen.height * 0.08f))) {
					try {
						p.AddCommand(new DeleteBuildingCommand(p, h_target));
					} catch (Exception e) {
						Notify (e.Message, Color.red);
					}
				}

			}
			GUILayout.EndHorizontal();
			GUILayout.EndArea();

			// Panel 3
			// Help Button
			if (GUI.Button (new Rect(Screen.width * .75f, b1_start, b_height, b_height), iconUnit)) {
				showHelp = !showHelp;
			}

			// Objectives Button
			if (GUI.Button (new Rect(Screen.width * .75f, b1_start + b_height, b_height, b_height), iconUnit)) {

			}

			// Extra Button
			if (GUI.Button (new Rect(Screen.width * .75f, b1_start + b_height + b_height, b_height, b_height), iconUnit)) {

			}

			// TODO
			// Rest of the panel

			// TODO
			// Build Menu

			// help window
			if (showHelp) {
				helpWindow = GUI.Window(0, helpWindow, MakeHelpWindow, "Help Window");
			}
		}

		private class UINotification : MonoBehaviour {
			public TextMesh msg;
			private float clock;
			//Font font = Resources.Load<Font>("misc/LCDfont");	

			public void init() {
				msg = gameObject.AddComponent<TextMesh> ();
				msg.text = "Debug";
				msg.characterSize = 0.3f;
				msg.color = new Color (1, 1, 1, 1);
				msg.alignment = TextAlignment.Center;
				msg.anchor = TextAnchor.MiddleCenter;
				transform.localPosition = new Vector2 (0, -1);
				clock = 0.0f;
			}

			public void post(string text, Color c) {
				msg.text = text;
				msg.color = c;
				clock = 0.0f;
			}

			void Start() {
			}

			void Update() {
				if (this.clock < 150) {
					clock += 1f;
					if (this.clock > 75) {
						Color c = msg.color;
						c.a -= 0.01f;
						msg.color = c;
					}
				} else {
					msg.color = new Color(0, 0, 0, 0);
				}
			}
		}

		private class UIHighlight : MonoBehaviour {
			SpriteRenderer sp;
			UIManager m;

			public void init(UIManager um) {
				this.m = um;
			}

			void Start() {
				sp = gameObject.AddComponent<SpriteRenderer>();
				sp.sprite = Resources.Load<Sprite>("Textures/T_Selection");
				sp.color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
				sp.enabled = false;
			}

			void Update() {
				if (m.h_target != null) {
					transform.parent = m.h_target.gameObject.transform;
					// Have to set local position each time because changing
					// transform parent doesn't move the game object.
					transform.localPosition = new Vector3(0, 0, Layer.PseudoUI);
					transform.localScale = new Vector3(1.9f, 1.9f, 1.9f);
					sp.enabled = true;
				} else {
					sp.enabled = false;
				}
			}
		}

		private class UIMovement : MonoBehaviour {
			SpriteRenderer sp;
			UIManager m;

			public void init(UIManager um) {
				this.m = um;
			}

			void Start() {
				sp = gameObject.AddComponent<SpriteRenderer>();
				sp.sprite = Resources.Load<Sprite>("Textures/T_Selection");
				sp.color = new Color(0.38f, 1f, 1f, 0.6f);
				sp.enabled = false;
			}

			void Update() {
				if (m.h_target != null && m.state == State.Moving) {
					transform.parent = m.h_target.gameObject.transform;
					// Have to set local position each time because changing
					// transform parent doesn't move the game object.
					transform.localPosition = new Vector3(0, 0, Layer.PseudoUI);
					transform.localScale = new Vector3(1.9f, 1.9f, 1.9f) * 2.5f * m.u_target.actions;
					sp.enabled = true;
				} else {
					sp.enabled = false;
				}
			}
		}
	}
}