using System;
using System.Collections.Generic;

namespace Test_console_project
{
    internal class Program
    {
        static void Main(string[] args)
        {
        }

        class Weapon
        {
            private List<Bullet> _bullets;

            public void Fire()
            {
                if (_bullets.Count == 0)
                    return;


                _bullets[0].Move();
                _bullets.RemoveAt(0);
            }
        }

        class Bullet
        {
            private int _damage;

            public void Move() { }

            public void Collision(Player other)
            {
                other.TakeDamage(_damage);
            }
        }

        class Player
        {
            private int _health;

            public void TakeDamage(int damage) 
            { 
                if (damage < 0)
                    damage = 0;

                _health -= damage;

                if (_health < 0)
                    _health = 0;
            }
        }

        class Bot
        {
            private Weapon _weapon;

            public void OnSeePlayer()
            {
                _weapon.Fire();
            }
        }
    }
}
