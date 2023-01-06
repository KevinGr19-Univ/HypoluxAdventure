﻿using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HypoluxAdventure.Core;
using HypoluxAdventure.Models;
using HypoluxAdventure.Models.UI;

namespace HypoluxAdventure.Managers
{
    internal enum GameState { Loading, Play, Pause, Transition }

    internal class GameManager
    {
        private Game1 _game;
        public GameState State { get; private set; } = GameState.Play;

        public GameManager(Game1 game)
        {
            _game = game;
        }

        public GameOverlay GameOverlay;
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

            GameOverlay = new GameOverlay(_game, this);
            DamageOverlay = new DamageOverlay(_game, this);

            RoomManager = new RoomManager(_game, this);
            RoomManager.GenerateRooms();

            InventoryManager = new InventoryManager(_game, this);
            ItemManager = new ItemManager(_game, this);

            _pauseManager = new PauseManager(_game, this);
            _cameraManager = new CameraManager(_game, this);

            Player = new Player(_game, this);
            _cursor = new Cursor(_game, this);

            _game.Camera.Zoom = 1.5f;
        }

        public void UnloadContent()
        {
            _game.Camera.Zoom = 1;
        }

        public void Update()
        {
            FrameInputs = GatherInputs();

            // Pause button
            if (Inputs.IsKeyPressed(Keys.Space)) SwitchPause();

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
                _cameraManager.Update();

                GameOverlay.Update();
                DamageOverlay.Update();
            }
            else
            {
                _pauseManager.Update();
            }
        }

        public void Draw()
        {
            if(State == GameState.Play)
            {
                _game.IsMouseVisible = false;
                _cursor.Draw();
                GameOverlay.Draw();
                InventoryManager.Draw();
            }
            else
            {
                _game.IsMouseVisible = true;
                if (State == GameState.Pause) _pauseManager.Draw();
            }

            Player.Draw();
            RoomManager.Draw();
            ItemManager.Draw();

            DamageOverlay.Draw();
        }

        public FrameInputs FrameInputs { get; private set; }

        public FrameInputs GatherInputs()
        {   
            int x = 0, y = 0;
            if (Inputs.IsKeyDown(Keys.Z) || Inputs.IsKeyDown(Keys.Up)) y -= 1;
            if (Inputs.IsKeyDown(Keys.S) || Inputs.IsKeyDown(Keys.Down)) y += 1;
            if (Inputs.IsKeyDown(Keys.Q) || Inputs.IsKeyDown(Keys.Left)) x -= 1;
            if (Inputs.IsKeyDown(Keys.D) || Inputs.IsKeyDown(Keys.Right)) x += 1;

            return new FrameInputs()
            {
                X = x,
                Y = y,
                Shoot = Inputs.IsClickPressed(Inputs.MouseButton.Left),
                Use = Inputs.IsClickPressed(Inputs.MouseButton.Right),
                SlotScroll = Inputs.ScrollChange,
                DropItem = Inputs.IsKeyPressed(Keys.Tab)
            };
        }

        public void SwitchPause()
        {
            if (State == GameState.Play) State = GameState.Pause;
            else if (State == GameState.Pause) State = GameState.Play;
        }

    }
}
