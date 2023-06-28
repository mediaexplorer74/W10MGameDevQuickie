namespace GameManager
{

    public class MachineGun : Weapon
    {
        public MachineGun()
        {
            cooldown = 0.1f;
            maxAmmo = 30;
            Ammo = maxAmmo;
            reloadTime = 2f;
        }

        protected override void CreateProjectiles(Player player)
        {
            ProjectileData pd = new()
            {
                Position = player.Position,
                Rotation = player.Rotation,
                Lifespan = 2f,
                Speed = 800/*600*/, //RnD
                Damage = 1
            };

            ProjectileManager.AddProjectile(pd);
        }
    }
}
