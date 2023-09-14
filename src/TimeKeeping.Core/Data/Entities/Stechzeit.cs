namespace Th11s.TimeKeeping.Data.Entities
{
    public class Stechzeit
    {
        public DateOnly Datum { get; set; }
        public TimeOnly Uhrzeit { get; set; }
        
        public StechTyp Typ { get; set; }
    }

    public enum StechTyp
    {
        Undefiniert,
        Arbeitsbeginn,
        Arbeitsende,
        Pausenbeginn,
        Pausenende
    }
}
