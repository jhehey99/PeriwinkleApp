namespace PeriwinkleApp.Core.Sources.Services.Interfaces
{
    public interface IInputValidationService
    {
        // chars min 5, max 32, is a Word
        void ValidateUsername (string username, int min = 1, int max = 32);

        // chars min 5, max 32, Contains at least 1 digit, 1 Uppercase, is a Word
        void ValidatePassword (string password, int min = 1, int max = 32);

        // Word, min and max
        void ValidateFirstName (string fname, int min = 1, int max = 64);
        
        // Word, min and max
        void ValidateLastName (string lname, int min = 1, int max = 48);

        // Digits, start with 9 + 9 digits
        void ValidateContact (string contact);

        // 128 max, may @ then .
        void ValidateEmail (string email);
    }
}
