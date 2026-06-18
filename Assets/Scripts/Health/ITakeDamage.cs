namespace Health
{
    public interface ITakeDamage
    {
        /// <summary>
        /// Makes the object/entity take damage. Implemented internally
        /// by each entity.
        /// </summary>
        public bool TryTakeDamage(int quantity, DeathCauses cause);

        public void TakeUnavoidableDamage(int quantity, DeathCauses cause);
    }
}