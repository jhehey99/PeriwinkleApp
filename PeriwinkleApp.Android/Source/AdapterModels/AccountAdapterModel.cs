namespace PeriwinkleApp.Android.Source.AdapterModels
{
    public class AccountAdapterModel
    {
        // Profile Picture
        
        public string Name { get; set; }
        public string Email { get; set; }

        public bool Equals (AccountAdapterModel other)
        {
            return Name.Equals (other.Name) && Email.Equals (other.Email);
        }
    }
}
