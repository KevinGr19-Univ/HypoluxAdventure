using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HypoluxAdventure.Core;
using HypoluxAdventure.Models;
using HypoluxAdventure.Models.UI;
using HypoluxAdventure.Utils;
using Microsoft.Xna.Framework;
using HypoluxAdventure.Models.Items;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace HypoluxAdventure.Managers
{
    internal enum GameState { Play, Pause, Transition, GameOver }

    internal class GameManager
    {
        private Game1 _game;
        public GameState State { get; private set; } = GameState.Play;

        public GameManager(Game1 game)
        {
            _game = game;
        }

        public const int FINAL_FLOOR = -5;
        public int Floor { get; private set; } = 1;

        public float Difficulty => Floor / (FINAL_FLOOR + 1);

        public HealthOverlay HealthOverlay;
        public DamageOverlay DamageOverlay { get; private set; }
        public MinimapOverlay MinimapOverlay { get; private set; }

        public RoomManager RoomManager { get; private set; }
        public InventoryManager InventoryManager { get; private set; }
        public ItemManager ItemManager { get; private set; }

        public CameraManager CameraManager { get; private set; }
        private PauseManager _pauseManager;

        public Player Player { get; private set; }
        private Cursor _cursor;

        public void LoadContent()
        {
            State = GameState.Play;

            Player = new Player(_game, this);

            DamageOverlay = new DamageOverlay(_game, this);
            MinimapOverlay = new MinimapOverlay(_game, this);
            HealthOverlay = new HealthOverlay(_game, this);

            RoomManager = new RoomManager(_game, this);

            InventoryManager = new InventoryManager(_game, this);
            ItemManager = new ItemManager(_game, this);

            CameraManager = new CameraManager(_game, this);
            _pauseManager = new PauseManager(_game, this);
            _cursor = new Cursor(_game, this);

            LoadNextFloor();
            InventoryManager.AddItem(new Sword(_game, this));
        }

        public void UnloadContent()
        {
            _game.Camera.Position = Vector2.Zero;
            _game.Camera.Zoom = 1;
            _game.IsMouseVisible = true;
        }

        #region Next floor transition
        private const float FADE_TIME = 1.5f;
        private float _fadeTimer;

        public void StartNextFloorTransition()
        {
            State = GameState.Transition;
            CameraManager.TargetZoom = 4;
            _fadeTimer = 0;
        }

        public void LoadNextFloor()
        {
            Floor -= 1;
            if(Floor < FINAL_FLOOR)
            {
                LoadVictoryScreen();
                return;
            }

            ItemManager.Clear();
            MinimapOverlay.Clear();

            RoomManager.GenerateRooms();
            RoomManager.SpawnPlayer();
            MinimapOverlay.Visit(RoomManager.CurrentRoom.PointPos);

            _game.Camera.Position = RoomManager.CurrentRoom.Rectangle.Center;
            _game.Camera.Zoom = CameraManager.TargetZoom = 1.5f;

            State = GameState.Play;
        }
        #endregion

        public void LoadVictoryScreen()
        {
            _game.LoadVictory();
        }

        public void Update()
        {
            if(_gameOverTimer > 0)
            {
                _gameOverTimer -= Time.DeltaTime;
                if (_gameOverTimer <= 0) _game.LoadGameOver(Floor);
            }

            FrameInputs = GatherInputs();
            if (FrameInputs.Pause) SwitchPause();

            if (Inputs.IsKeyPressed(Keys.A)) Player.Damage(5); // DEBUG

            if(State != GameState.Pause)
            {
                Player.Update();

                if (State == GameState.Play)
                {
                    MinimapOverlay.Update();
                    RoomManager.Update();
                    ItemManager.Update();
                    InventoryManager.Update();
                }

                _cursor.Update();

                CameraManager.TargetPosition = Player.Position;
                CameraManager.Update();

                DamageOverlay.Update();
                HealthOverlay.Update();
            }
            else
            {
                _pauseManager.Update();
            }

            if (State == GameState.Transition && _fadeTimer < FADE_TIME)
            {
                _fadeTimer += Time.DeltaTime;
                if (_fadeTimer >= FADE_TIME)
                {
                    _fadeTimer = FADE_TIME;
                    LoadNextFloor();
                }
            }
            else if (_fadeTimer > 0) _fadeTimer -= Time.DeltaTime;
        }

        public void Draw()
        {
            if(State != GameState.Pause)
            {
                _game.IsMouseVisible = false;
                
                if(State != GameState.Transition)
                {
                    HealthOverlay.Draw();

                    if(State != GameState.GameOver)
                    {
                        _cursor.Draw();
                        InventoryManager.Draw();
                        MinimapOverlay.Draw();
                    }
                }
            }
            else
            {
                _game.IsMouseVisible = true;
                _pauseManager.Draw();
            }

            Player.Draw();
            RoomManager.Draw();
            ItemManager.Draw();
            DamageOverlay.Draw();

            if(_fadeTimer > 0)
            {
                Color color = new Color(Color.Black, _fadeTimer / FADE_TIME);
                _game.UICanvas.FillRectangle(0, 0, Application.SCREEN_WIDTH, Application.SCREEN_HEIGHT, color, 1);
            }
        }

        public FrameInputs FrameInputs { get; private set; }

        public FrameInputs GatherInputs()
        {
            InputLayout inputLayout = Inputs.InputLayout;

            int x = 0, y = 0;
            if (Inputs.IsKeyDown(inputLayout.NegX)) x -= 1;
            if (Inputs.IsKeyDown(inputLayout.PosY)) y -= 1;
            if (Inputs.IsKeyDown(inputLayout.PosX)) x += 1;
            if (Inputs.IsKeyDown(inputLayout.NegY)) y += 1;

            return new FrameInputs()
            {
                X = x,
                Y = y,
                Shoot = Inputs.IsClickPressed(Inputs.MouseButton.Left),
                Use = Inputs.IsClickPressed(Inputs.MouseButton.Right),
                DropItem = Inputs.IsKeyPressed(Keys.Tab),

                SlotScroll = Inputs.ScrollChange,
                Slot1 = Inputs.IsKeyPressed(Keys.D1),
                Slot2 = Inputs.IsKeyPressed(Keys.D2),
                Slot3 = Inputs.IsKeyPressed(Keys.D3),

                Pause = Inputs.IsKeyPressed(Keys.Space)
            };
        }

        public void SwitchPause()
        {
            if (State == GameState.Play) State = GameState.Pause;
            else if (State == GameState.Pause) State = GameState.Play;
        }

        public void ReturnToMenu()
        {
            _game.LoadMenu();
        }

        private const float GAME_OVER_TIME = 4;
        private float _gameOverTimer;

        public void GameOver()
        {
            State = GameState.GameOver;
            _gameOverTimer = GAME_OVER_TIME;

            CameraManager.TargetZoom = 5.5f;
            HealthOverlay.PlayDeathAnimation();
        }

        public static float GetYDepth(float yPos) => MathUtils.InverseLerp(0, RoomManager.MAP_HEIGHT, yPos) * 0.2f + 0.4f; // from 0.4f to 0.6f
    }
}
