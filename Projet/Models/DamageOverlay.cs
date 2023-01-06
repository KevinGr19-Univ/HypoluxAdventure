using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models
{
    internal class DamageOverlay : GameObject
    {
        private Texture2D _damageScreen;
        // float _timer + const float PULSE_TIME => à faire quand MathUtils.LerpCubic existe

        private int _nextNumberId;
        private Dictionary<int, DamageNumber> _damageNumbers;
        public SpriteFont Font { get; private set; }

        public DamageOverlay(Game1 game, GameManager gameManager) : base(game, gameManager) 
        {
            _nextNumberId = 0;
            _damageNumbers = new Dictionary<int, DamageNumber>();
            Font = game.Content.Load<SpriteFont>("Font/DamageFont");
        }

        public override void Update()
        {
            foreach (DamageNumber number in _damageNumbers.Values) number.Update();
        }

        public override void Draw()
        {
            foreach (DamageNumber number in _damageNumbers.Values) number.Draw();
        }

        public void SpawnNumber(Vector2 gamePos, int damage)
        {
            DamageNumber number = new DamageNumber(game,gameManager, _nextNumberId++,damage);
            _damageNumbers.Add(number.Id, number);
        }

        public void DespawnNumber(int id)
        {
            if (_damageNumbers.ContainsKey(id)) _damageNumbers.Remove(id);
            else Logger.Warn("Tried to remove unregistered damage number.");
        }
    }
}
