﻿using UnityEngine;
using System.Collections.Generic;
using System;
using game.actor;
using game.actor.commands;
using game.math;
using game.world;
using game.world.units;
using game.world.buildings;

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

        // x.split("\n").map { |l| l.split(" ")[3] }.join(", ")
        private static Texture2D[] texes = new Texture2D[] {
            UI_Move, UI_Scan, UI_Build, UI_Purify, UI_MoveH,
            UI_ScanH, UI_BuildH, UI_PurifyH, UI_MoveC, UI_ScanC, UI_BuildC, UI_PurifyC, UI_End, UI_EndH, UI_EndC,
            UI_Cond, UI_CondH, UI_CondC, UI_Gate, UI_GateH, UI_GateC, UI_Harv, UI_HarvH, UI_HarvC, UI_Tow, UI_TowH, UI_TowC
        };

		private GUIStyle ButtonStyle;

		WorldMap w;
		Player p;

		TooltipUI t;
		public HelperUI helper;
        MinimapManager mmm;
		public bool ev_view = true;

		HighlightModel model;
        MovementModel movement;

		Hex h_target;
		Unit u_target {
            get {
                if (h_target != null && 
                    h_target.units.Count > 0) {
                    return h_target.units[0];
                } else {
                    return null;
                }
            }
        }
		State state;
		//bool building;

		public enum State {
			Default,
			Selected,
			Moving,
			Building,
		};

        public void init(Player player, WorldMap w) {
            foreach (Texture2D tex in texes) {
                tex.filterMode = FilterMode.Point;
            }
			this.p = player;
			this.w = w;

			t = gameObject.AddComponent<TooltipUI> ();
			t.init (this);

            mmm = gameObject.AddComponent<MinimapManager>();

			model = new GameObject ("Highlight Model").AddComponent<HighlightModel> ();
			model.init (this);

            movement = new GameObject("Movement Model").AddComponent<MovementModel>();
            movement.init(this);

            state = State.Default;
			//building = false;
		}

		void Start () {
			helper = gameObject.AddComponent<HelperUI>();
			helper.init();
		}

        private bool inToolbarBoundary(Vector3 v) {
            // TODO: this seems a tiny bit too high. The bottoms of
            // buttons pass through clicks, and there's a section on top
            // with no button which blocks clicks.
            return v.x > Screen.width * .3f && v.x < Screen.width * .76f
                    && v.y > Screen.height * .1f && v.y < Screen.height * .28f;
        }

        void Update() {
            if (Input.GetMouseButtonUp(0)) {
                if (!inToolbarBoundary(Input.mousePosition)) {
                    if (((state == State.Default) || (state == State.Selected))) {
                        Hex h = GetHexAtMouse();
                        if (h != null) {
                            this.h_target = h;
                        }
                        state = State.Selected;

                    } else if (state == State.Moving) {
                        if (Input.GetMouseButtonUp(0)) {
                            Hex h = GetHexAtMouse();
                            var unit = u_target;
                            try {
                                p.AddAllCommands(MoveCommand.pathfind(w, p, unit, h));
                                this.h_target = h;
                            } catch (Exception e) {
                                EventManager.PostInvalidAction(new InvalidActionArgs { msg = e.Message });
                            }
                            state = State.Selected;
                            EventManager.TriggerMoveEventAfter(new MoveEventArgs { stamina = unit.actions });
                            Debug.Log("Added Move Command");
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

		private Hex getSelected() {
			return h_target;
		}

        private void buildBuilding(BuildingType type) {
            try {
                p.AddCommand(new BuildCommand(p, u_target, h_target, type));
            } catch (Exception e) {
                print(e);
            }
            EventManager.TriggerBuildEvent(new BuildEventArgs { name = type.ToString(), turns = type.BuildTotal() / type.BuildPerTurn() });
            state = State.Selected;
        }

        
		void OnGUI() {

            GUILayout.BeginArea(new Rect (Screen.width*.3f, Screen.height*.8f, Screen.width/2, Screen.height*.9f));
			GUILayout.BeginHorizontal ();

            var width = GUILayout.Width(Screen.width * .08f);
            var height = GUILayout.Height(Screen.height * .13f);

            ButtonStyle = new GUIStyle(GUI.skin.label);

			ButtonStyle.normal.background = UI_Move; ButtonStyle.hover.background = UI_MoveH; ButtonStyle.active.background = UI_MoveC;

			if (GUILayout.Button("", ButtonStyle, width, height)) {
				if (state == State.Selected) {
					if (u_target != null) {
						EventManager.TriggerMoveEventBefore(new MoveEventArgs {stamina = u_target.actions});
						state = State.Moving;
					}
				} else {
					state = State.Default;
				}
			}

			ButtonStyle.normal.background = UI_Build; ButtonStyle.hover.background = UI_BuildH; ButtonStyle.active.background = UI_BuildC;
			if (GUILayout.Button("", ButtonStyle, width, height)){
				if (state == State.Selected) {
					if (u_target != null) {
						EventManager.TriggerBuildMenuEvent(new GameEventArgs {});
						state = State.Building;
					}
				} else if (state == State.Building) {
					state = State.Selected;
				}
			}

			ButtonStyle.normal.background = UI_Scan; ButtonStyle.hover.background = UI_ScanH; ButtonStyle.active.background = UI_ScanC;
			if (GUILayout.Button("", ButtonStyle, width, height)) {
				if (state == State.Selected) {
					if (u_target != null) {
						try {
							p.AddCommand (new ScanCommand (p, u_target, h_target));
						} catch (Exception e) {
							EventManager.PostInvalidAction (new InvalidActionArgs{ msg = e.Message });
						}
					}
				}
				Debug.Log ("Added Scan Command");
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
				Debug.Log ("Added Cleanse Command");
			}

			ButtonStyle.normal.background = UI_End; ButtonStyle.hover.background = UI_EndH; ButtonStyle.active.background = UI_EndC;
			if (GUILayout.Button ("", ButtonStyle, GUILayout.Width (Screen.width * .12f), height)) {
				p.AddCommand(new EndTurnCommand(p));
				Debug.Log ("Added End Turn Command");

			}
			GUILayout.EndHorizontal ();
			GUILayout.EndArea ();

			if (state == State.Building) {
				GUILayout.BeginArea (new Rect (Screen.width * .3f, Screen.height * .7f, Screen.width / 2, Screen.height * .9f));
				GUILayout.BeginHorizontal ();

				ButtonStyle.normal.background = UI_Cond; ButtonStyle.hover.background = UI_CondH; ButtonStyle.active.background = UI_CondC;
				if (GUILayout.Button ("", ButtonStyle, GUILayout.Width (Screen.height * 0.08f), GUILayout.Height (Screen.height * 0.08f))) {
                    buildBuilding(BuildingType.Conduit);
                }

				ButtonStyle.normal.background = UI_Harv; ButtonStyle.hover.background = UI_HarvH; ButtonStyle.active.background = UI_HarvC;
				if (GUILayout.Button ("", ButtonStyle, GUILayout.Width (Screen.height * 0.08f), GUILayout.Height (Screen.height * 0.08f))) {
                    buildBuilding(BuildingType.Harvester);
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

                ButtonStyle.normal.background = UI_Unit;
                ButtonStyle.hover.background = UI_UnitH;
                ButtonStyle.active.background = UI_UnitC;
                if (GUILayout.Button("", ButtonStyle, GUILayout.Width(Screen.height * 0.08f), GUILayout.Height(Screen.height * 0.08f))) {
                    try {
                        p.AddCommand(new WarpUnitCommand(p, wg));
                    } catch (Exception e) {
                        print(e);
                    }
                }

            }

            if (h_target != null &&
                h_target.building != null) {

                ButtonStyle.normal.background = UI_Delete;
                ButtonStyle.hover.background = UI_DeleteH;
                ButtonStyle.active.background = UI_DeleteC;
                if (GUILayout.Button("", ButtonStyle, GUILayout.Width(Screen.height * 0.08f), GUILayout.Height(Screen.height * 0.08f))) {
                    try {
                        p.AddCommand(new DeleteBuildingCommand(p, h_target));
                    } catch (Exception e) {
                        print(e);
                    }
                }

            }

            GUILayout.EndHorizontal();
            GUILayout.EndArea();

        }

        class HighlightModel : MonoBehaviour {
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

        class MovementModel : MonoBehaviour {
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
