using System.Text;
using VotingSystem.Model;

namespace VotingSystem.Controller;

public class GenerateSerialNo
{

    // generate 1 letter with 8 random digits
    // code from :
    // https://stackoverflow.com/questions/45106385/how-to-generate-random-string-of-numbers-and-letters-in-form-2-letters-4-num
    public String generateSerialNo()
    {
        var random = new Random();
        var letter = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var numbers = "0123456789";
        var stringChars = new char[9];

        for (int i = 0; i < 1; i++)
        {
            stringChars[i] = letter[random.Next(letter.Length)];
        }
        for (int i = 1; i < 9; i++)
        {
            stringChars[i] = numbers[random.Next(numbers.Length)];
        }

        var finalString = new String(stringChars);
        return finalString;
    }

}