using System;
using System.Collections.Generic;
using System.Text;
using con_rogue.Interfaces;
using System.Linq;

namespace con_rogue
{
    public class AttackWithWeapon : IAction
    {
        private readonly Item _weapon;
        private readonly int _minDmg;
        private readonly int _maxDmg;

        public AttackWithWeapon(Item weapon, int minDmg, int maxDmg)
        {
            if(weapon.Type != Item.ItemType.Weapon)
            {
                throw new ArgumentException($"{weapon.Name} is not a weapon");
            }

            if(minDmg < 0)
            {
                throw new ArgumentException($"Weapon damage cannot be lower than 0");
            }

            if (minDmg > maxDmg)
            {
                throw new ArgumentException($"Minimum weapon damage must not be greater than maximum damage");
            }

            _weapon = weapon;
            _minDmg = minDmg;
            _maxDmg = maxDmg;
        }

        public void Execute(Entity actor, Entity target)
        {
            int damage = RNG.Generator(_minDmg, _maxDmg);

            string outcome = (actor is Player) ? $"You hit {target.Name.ToLower()} for {damage} damage" : $"{actor.Name} hits you for {damage} damage";
            string missOutcome = (actor is Player) ? $"You miss {target.Name}" : $"{target.Name} misses you";

            if (damage == 0)
            {
                AddToMessageLog(missOutcome);
            }
            else
            {
                AddToMessageLog(outcome);
                target.TakeDamage(damage);           
            }
        }

        private void AddToMessageLog(string msg)
        {
            GameSession.messageLog.Add(msg);
        }

    }
}
