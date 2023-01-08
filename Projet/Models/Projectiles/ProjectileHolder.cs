using HypoluxAdventure.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HypoluxAdventure.Models.Projectiles
{
    internal class ProjectileHolder : GameObject
    {
        private List<Projectile> _projectiles = new List<Projectile>();
        private List<Projectile> _projectilesToRemove = new List<Projectile>();

        public ProjectileHolder(Game1 game, GameManager gameManager) : base(game, gameManager) { }

        public override void Update()
        {
            _projectiles.ForEach((projectile) => projectile.Update());
            _projectilesToRemove.ForEach((projectile) =>
            {
                _projectiles.Remove(projectile);
                projectile.OnDespawn();
            });
            _projectilesToRemove.Clear();
        }

        public override void Draw()
        {
            _projectiles.ForEach((projectile) => projectile.Draw());
        }

        public void SpawnProjectile(Projectile projectile)
        {
            _projectiles.Add(projectile);
        }

        public void DespawnProjectile(Projectile projectile)
        {
            if (_projectiles.Contains(projectile) && !_projectilesToRemove.Contains(projectile))
                _projectilesToRemove.Add(projectile);
        }

        public void Clear()
        {
            _projectiles.ForEach((projectile) => projectile.OnDespawn());
            _projectiles.Clear();
        }
    }
}
