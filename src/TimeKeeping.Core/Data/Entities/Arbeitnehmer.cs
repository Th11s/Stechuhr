namespace Th11s.TimeKeeping.Data.Entities
{
    //TODO AP: Der Arbeitnehmer könnte auch als "Arbeitnehmer in Abteilung" verstanden werden - dann ist hier die Standarddienstzeit und das Arbeitszeitmodell (später) gut aufgehoben.
    public class Arbeitnehmer : User
    {
        /// <summary>
        /// Aktuelles Stundensaldo über alle Arbeitszeiten
        /// </summary>
        public TimeSpan Stundensaldo { get; set; }

        //TODO: Änderungen am Stundensaldo erfassen

        public TimeSpan Standarddienstzeit { get; set; }


        public ICollection<Abteilung>? Abteilungen { get; set; }
    }
}
