using BankingSystem.Services;

BankService bankService = new();
string? input;
Console.WriteLine("Welcome to AwesomeGIC Bank! What would you like to do?");
do
{
    Console.WriteLine("[T] Input transactions");
    Console.WriteLine("[I] Define interest rules");
    Console.WriteLine("[P] Print statement");
    Console.WriteLine("[Q] Quit");
    Console.Write("> ");
    input = Console.ReadLine()?.Trim().ToUpper();

    switch (input)
    {
        case "T":
            bankService.InputTransaction();
            break;
        case "I":
            bankService.DefineInterestRules();
            break;
        case "P":
            bankService.PrintStatement();
            break;
        case "Q":
            Console.WriteLine("Thank you for banking with AwesomeGIC Bank.\nHave a nice day!");
            break;
        default:
            Console.WriteLine("Invalid option, please try again.");
            break;
    }
} while (input != "Q");
