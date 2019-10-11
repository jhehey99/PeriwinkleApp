namespace PeriwinkleApp.Android.Source.AdapterModels
{
    public class QuestionAdapterModel
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public int Scale { get; set; }

        public bool Equals (QuestionAdapterModel other)
        {
            return Id == other.Id && Question.Equals (other.Question) && Scale == other.Scale;
        }
    }
}
