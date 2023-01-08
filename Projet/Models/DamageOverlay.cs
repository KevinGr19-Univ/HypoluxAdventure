using HypoluxAdventure.Core;
using HypoluxAdventure.Managers;
using HypoluxAdventure.Models.UI;
using HypoluxAdventure.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
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

        private Sprite _damageScreen;
        private Vector2 _scale;

        private float _timer;

        private int _nextNumberId;
        private Dictionary<int, DamageNumber> _damageNumbers;
        public SpriteFont Font { get; private set; }

        public DamageOverlay(Game1 game, GameManager gameManager) : base(game, gameManager) 
        {
            _nextNumberId = 0;
            _damageNumbers = new Dictionary<int, DamageNumber>();
            Font = game.Content.Load<SpriteFont>("Font/DamageFont");

            _damageScreen = new Sprite(game.Content.Load<Texture2D>("img/damageScreen"));
            _damageScreen.OriginNormalized = Vector2.Zero;
            _damageScreen.Depth = 0.1f;
            GraphicsUtils.SetPixelSize(_damageScreen, Application.SCREEN_WIDTH, Application.SCREEN_HEIGHT, ref _scale);

            _timer = PULSE_TIME;
        }

        public override void Update()
        {
            _timer += Time.DeltaTime;
            _damageScreen.Alpha = _timer<PULSE_TIME ? MathUtils.LerpOutToPower(0, 1, PULSE_TIME, _timer, 3) : 0;

            foreach (DamageNumber number in _damageNumbers.Values) number.Update();
        }

        public override void Draw()
        {
            _damageScreen.Draw(game.UICanvas, Vector2.Zero, 0, _scale);
            foreach (DamageNumber number in _damageNumbers.Values) number.Draw();
        }

        public void SpawnNumber(Vector2 gamePos, int damage, bool heal)
        {
            DamageNumber number = new DamageNumber(game,gameManager, _nextNumberId++,damage, gamePos, heal);
            _damageNumbers.Add(number.Id, number);
        }

        public void DespawnNumber(int id)
        {
            if (_damageNumbers.ContainsKey(id)) _damageNumbers.Remove(id);
            else Logger.Warn("Tried to remove unregistered damage number.");
        }

        public void Pulse()
        {
            _timer = 0;
        }
    }
}
