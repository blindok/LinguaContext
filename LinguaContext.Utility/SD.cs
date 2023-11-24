namespace LinguaContext.Utility;

public static class SD
{
    public const string PublicFolderSatus = "Public";
    public const string PrivateFolderStatus = "Private";
    public const string ForFrinedsFolderStatus = "ForFriends";

    public const string Role_Admin = "admin";
    public const string Role_User = "user";
}

public static class DefaultSettings
{
    public const int    NewDailyCardsNumber   = 20;
    public const bool   DisplayTranslation    = true;
    public const bool   HighlightAnswer       = true;

    public const double IntervalModifier      = 1.0;
    public const double FailIntervalModifier  = 0.0;
    public const double HardIntervalModifier  = 1.2;
    public const double EasyIntervalModifier  = 1.3;

    public const double EaseFactor            = 2.5;
}
