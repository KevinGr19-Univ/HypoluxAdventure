using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Models.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models
{
    internal class DamageOverlay : GameObject
    {
        public const float PULSE_TIME = 1; 

        private Texture2D _damageScreen;
        // float _timer + const float PULSE_TIME => à faire quand MathUtils.LerpCubic existe

        private float _timer;

        private int _nextNumberId;
        private Dictionary<int, DamageNumber> _damageNumbers;
        public SpriteFont Font { get; private set; }

        public DamageOverlay(Game1 game, GameManager gameManager) : base(game, gameManager) 
        {
            _nextNumberId = 0;
            _damageNumbers = new Dictionary<int, DamageNumber>();
            Font = game.Content.Load<SpriteFont>("Font/DamageFont");
            _damageScreen = game.Content.Load<Texture2D>("img/damageScreen");
        }

        int damage = 1;
        public override void Update()
        {
            if (Inputs.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.R))
            {
                SpawnNumber(gameManager.Player.Position, damage);
                damage  = damage%19 + 1;
            }
            if (Inputs.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.T)) Pulse();
            foreach (DamageNumber number in _damageNumbers.Values) number.Update();
        }

        public override void Draw()
        {
            foreach (DamageNumber number in _damageNumbers.Values) number.Draw();
        }

        public void SpawnNumber(Vector2 gamePos, int damage)
        {
            DamageNumber number = new DamageNumber(game,gameManager, _nextNumberId++,damage, gamePos);
            _damageNumbers.Add(number.Id, number);
        }

        public void DespawnNumber(int id)
        {
            if (_damageNumbers.ContainsKey(id)) _damageNumbers.Remove(id);
            else Logger.Warn("Tried to remove unregistered damage number.");
        }

        public void Pulse()
        {
            
        }
    }
}
