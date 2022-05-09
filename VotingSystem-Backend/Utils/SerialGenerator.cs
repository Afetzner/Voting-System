namespace VotingSystem.Utils {
    public abstract class SerialGenerator 
    {

        // generate 1 letter with 8 random digits
        // code from :
        // https://stackoverflow.com/questions/45106385/how-to-generate-random-string-of-numbers-and-letters-in-form-2-letters-4-num
        public static string Generate(char first)
        {
            var random = new Random();
            var numbers = "0123456789";
            var stringChars = new char[9];

            stringChars[0] = first;
            for (int i = 1; i < 9; i++)
            {
                stringChars[i] = numbers[random.Next(numbers.Length)];
            }

            var finalString = new string(stringChars);
            return finalString;
        }
    }
}