using RPG.Movement;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour 
    {
        Transform target;

        private void Update() 
        {
            if(target != null)
            {
                GetComponent<Mover>().MoveTo(target.position);
            }
        }

        public void Attack(CombatTarget combatTarget)
        {
            target = combatTarget.transform;
        }
    }
}