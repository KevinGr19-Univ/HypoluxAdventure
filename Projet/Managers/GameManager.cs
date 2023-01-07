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

namespace HypoluxAdventure.Managers
{
    internal enum GameState { Loading, Play, Pause, Transition, GameOver }

    internal class GameManager
    {
        private Game1 _game;
        public GameState State { get; private set; } = GameState.Play;

        public GameManager(Game1 game)
        {
            _game = game;
        }

        public HealthOverlay HealthOverlay;
        public DamageOverlay DamageOverlay { get; private set; }

        public RoomManager RoomManager { get; private set; }
        public InventoryManager InventoryManager { get; private set; }
        public ItemManager ItemManager { get; private set; }

        private PauseManager _pauseManager;
        private CameraManager _cameraManager;

        public Player Player { get; private set; }
        private Cursor _cursor;

        public void LoadContent()
        {
            State = GameState.Play;

            HealthOverlay = new HealthOverlay(_game, this);
            DamageOverlay = new DamageOverlay(_game, this);

            RoomManager = new RoomManager(_game, this);
            RoomManager.GenerateRooms();

            InventoryManager = new InventoryManager(_game, this);
            ItemManager = new ItemManager(_game, this);

            _pauseManager = new PauseManager(_game, this);
            _cameraManager = new CameraManager(_game, this);

            Player = new Player(_game, this);
            _cursor = new Cursor(_game, this);

            _game.Camera.Zoom = _cameraManager.TargetZoom = 1.5f;
        }

        public void UnloadContent()
        {
            _game.Camera.Position = Vector2.Zero;
            _game.Camera.Zoom = 1;
            _game.IsMouseVisible = true;
        }

        public void Update()
        {
            if(_gameOverTimer > 0)
            {
                _gameOverTimer -= Time.DeltaTime;
                if (_gameOverTimer <= 0) _game.LoadGameOver();
            }

            FrameInputs = GatherInputs();
            if (FrameInputs.Pause) SwitchPause();

            if (Inputs.IsKeyPressed(Keys.A)) Player.Damage(5); // DEBUG

            if(State != GameState.Pause)
            {
                Player.Update();
                RoomManager.Update();

                if (State == GameState.Play)
                {
                    ItemManager.Update();
                    InventoryManager.Update();
                }

                _cursor.Update();

                _cameraManager.TargetPosition = Player.Position;
                _cameraManager.Update();

                DamageOverlay.Update();
            }
            else
            {
                _pauseManager.Update();
            }
            HealthOverlay.Update();
        }

        public void Draw()
        {
            if(State != GameState.Pause)
            {
                _game.IsMouseVisible = false;
                
                if(State != GameState.GameOver)
                {
                    _cursor.Draw();
                    InventoryManager.Draw();
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
            HealthOverlay.Draw();
            DamageOverlay.Draw();
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

        }

        private const float GAME_OVER_TIME = 4;
        private float _gameOverTimer;

        public void GameOver()
        {
            State = GameState.GameOver;
            _gameOverTimer = GAME_OVER_TIME;

            _cameraManager.TargetZoom = 5.5f;
            HealthOverlay.PlayDeathAnimation();
        }

        public static float GetYDepth(float yPos) => MathUtils.InverseLerp(0, RoomManager.MAP_HEIGHT, yPos) * 0.2f + 0.4f; // from 0.4f to 0.6f
    }
}
