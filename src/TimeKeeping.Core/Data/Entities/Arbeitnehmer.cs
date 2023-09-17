namespace Th11s.TimeKeeping.Data.Entities
{
    public class Arbeitnehmer : User
    {
        /// <summary>
        /// Aktuelles Stundensaldo über alle Arbeitszeiten
        /// </summary>
        public TimeSpan Stundensaldo { get; set; }

        //TODO: Änderungen am Stundensaldo erfassen

        public TimeSpan Standarddienstzeit { get; set; }
    }
}
