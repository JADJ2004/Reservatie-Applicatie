using System.Runtime.InteropServices;

public class UserInterface
{
    private int SelectedIndex;
    private string[] Options;
    private string Prompt;


    public UserInterface(string prompt, string[] options)
    {
        {
            Prompt = prompt;
            Options = options;
            SelectedIndex = 0;
        }
    }

    private void DisplayOptions()
    {
        Console.WriteLine(Prompt);

        for (int i = 0; i < Options.Length; i++)
        {
            string currentOption = Options[i];
            string prefix;

            if (i == SelectedIndex)
            {
                prefix = "-";
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else
            {
                prefix = " ";
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine($" {prefix} {currentOption}");
        }
        Console.ResetColor();
    }

    public int Run()
    {
        ConsoleKey keyPressed;
        do
        {
            DisplayOptions();

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            keyPressed = keyInfo.Key;

            if (keyPressed == ConsoleKey.UpArrow)
            {
                SelectedIndex--;
                if (SelectedIndex == -1)
                {
                    SelectedIndex = Options.Length - 1;
                }
            }
            else if (keyPressed == ConsoleKey.DownArrow)
            {
                SelectedIndex++;
                if (SelectedIndex == Options.Length)
                {
                    SelectedIndex = 0;
                }
            }
            else if (keyPressed == ConsoleKey.Enter)
            {
                break; // Exit the loop when Enter key is pressed
            }
            // Clear the console to avoid reprinting the menu
            Console.Clear();
        } while (true);

        return SelectedIndex;
    }


}