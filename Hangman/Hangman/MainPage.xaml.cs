using System.ComponentModel;
using System.Data;
using System.Diagnostics;

namespace Hangman;

public partial class MainPage : ContentPage, INotifyPropertyChanged
{

    #region UI Props
    public string Spotlight
    {
        get => spotlight; set
        {
            spotlight = value;
            OnPropertyChanged();
        }
    }

    public List<char> Letters
    {
        get => letters; set
        {
            letters = value;
            OnPropertyChanged();
        }
    }

    public string Msg
    {
        get => msg; set
        {
            msg = value;
            OnPropertyChanged();
        }
    }

    public string GameStatus
    {
        get => gameStatus; set
        {
            gameStatus = value;
            OnPropertyChanged();
        }
    }

    public string CurrImg
    {
        get => currImg; set
        {
            currImg = value;
            OnPropertyChanged();
        }
    }
    #endregion

    #region Fields
    List<string> words = new List<string>()
    {
        "python",
        "javascript",
        "maui",
        "csharp",
        "mongodb",
        "sql",
        "xaml",
        "word",
        "excel",
        "powerpoint",
        "code",
        "hotreload",
        "snippets"
    };

    List<char> guessed = new List<char>();
    string answ = "";
    private string spotlight;
    private List<char> letters = new List<char>();
    private string msg;
    int errW = 0;
    int maxErrW = 6;
    private string gameStatus;
    private string currImg = "img0.jpg";

    #endregion

    public MainPage()
	{
		InitializeComponent();
        Letters.AddRange("abcdefghijklmnopqrstuvwxyz");
        BindingContext = this;
        PickWord();
        CalculateWord(answ, guessed);
    }

    #region Game Engine
    private void PickWord()
    {
        answ = words[new Random().Next(0, words.Count)];
    }

    private void CalculateWord(string answer, List<char> guessed) {
        var tmp = answer.Select(x => (guessed.IndexOf(x) >= 0 ? x : '_')).ToArray();

        Spotlight = string.Join(' ', tmp);
    }

    private void UpdateStatus()
    {
        GameStatus = $"Errors: {errW} of {maxErrW}";
    }
    #endregion

    private void Button_Clicked(object sender, EventArgs e)
    {
        var btn = sender as Button;
        if (btn != null)
        {
            var letter = btn.Text;
            btn.IsEnabled = false;

            HandleGuess(letter[0]);
        }
    }

    private void HandleGuess(char l)
    {
        if (guessed.IndexOf(l) == -1)
        {
            guessed.Add(l);
        }

        if(answ.IndexOf(l) >= 0)
        {
            CalculateWord(answ, guessed);
            CheckIfWon();
        }
        else if( answ.IndexOf(l) == -1)
        {
            errW++;
            UpdateStatus();
            CheckIfLost();
            CurrImg = $"img{errW}.jpg";
        }
    }

    private void CheckIfLost()
    {
        if(errW == maxErrW)
        {
            Msg = "You Lost!";
            DisableLetters();
        }
    }
    private void EnableLetters()
    {
        foreach (var ch in LettersCont.Children)
        {
            var btn = ch as Button;
            if (btn != null)
            {
                btn.IsEnabled = true;
            }
        }
    }
    private void DisableLetters()
    {
        foreach(var ch in LettersCont.Children)
        {
            var btn = ch as Button;
            if(btn != null)
            {
                btn.IsEnabled = false;
            }
        }
    }

    private void CheckIfWon()
    {
        if(Spotlight.Replace(" ", "") == answ)
        {
            Msg = "You Win!";
            DisableLetters();
        }
    }

    private void Reset_Btn(object sender, EventArgs e)
    {
        errW = 0;
        guessed = new List<char>();
        CurrImg = "img0.jpg";
        PickWord();
        CalculateWord(answ, guessed);
        Msg = "";
        UpdateStatus();
        EnableLetters();
    }
}

