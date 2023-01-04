using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HypoluxAdventure.Managers;

namespace HypoluxAdventure.Models
{
    internal abstract class LivingEntity : Entity
    {
        public LivingEntity(Game1 game, GameManager gameManager) : base(game, gameManager)
        {
            Health = MaxHealth;
        }

        #region Health
        abstract public int MaxHealth { get; }

        public bool IsDead => Health <= 0;

        private int _health;
        public int Health
        {
            get => _health;
            protected set
            {
                _health = Math.Clamp(value, 0, MaxHealth);
                if (IsDead) OnDeath();
            }
        }

        public virtual void OnDeath() { }
        #endregion

        #region Collisions
        public RectangleF BoundingBox { get; private set; }


        #endregion
    }
}
