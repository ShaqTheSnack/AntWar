using System;
using System.Diagnostics;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using AntEngine;
using WPFApp;
using static System.Formats.Asn1.AsnWriter;

namespace wpf_app;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    //Lav score til myrene, hvor mange der er i alt af den race --------------
    private const int ImageWidth = 200;
    private const int ImageHeight = 200;
    private const int TimerIntervalInMs = 100;
    private const int FinishRounds = 200; //Choose rounds before the game ends
    private int RoundCount;
    private WriteableBitmap _bitmap;
    readonly DispatcherTimer dispatcherTimer;
    private List<Type> players;
    private Map my_map;
    private AntHome antHome;
    private List<TeamList> TeamsList = new();
    enum GameMode
    {
        Training, //This is for testing
        DuoMatch, //This is for a 1 v 1
        Game      //This is for a PVP for many ants
    }
    GameMode gameMode = GameMode.DuoMatch; //<---- Write GameMode Here
    public MainWindow()
    {
        switch (gameMode)
        {
            case GameMode.Training:
                players = new List<Type> { typeof(RonnieColemant) }; //<---- Add your ant here
                my_map = new Map(ImageWidth, ImageHeight, players, startAnts: 1, PlayMode.SingleTraining);
                break;

            case GameMode.DuoMatch:
                players = new List<Type> { typeof(RonnieColemant), typeof(PMANDT) }; //<---- Add your ant here
                my_map = new Map(ImageWidth, ImageHeight, players, startAnts: 1, PlayMode.DuoMatch);
                break;

            case GameMode.Game:
                players = new List<Type> { typeof(RonnieColemant), typeof(GoBackAnt), typeof(GoBackAnt), typeof(GoBackAnt), typeof(GoBackAnt), typeof(GoBackAnt) }; //<---- Add your ant here
                my_map = new Map(ImageWidth, ImageHeight, players, startAnts: 1, PlayMode.Game);
                break;

        }

        InitializeComponent();
        CreateBitmap();

        //This is for the auto timer
        dispatcherTimer = new DispatcherTimer();
        dispatcherTimer.Interval = TimeSpan.FromMilliseconds(TimerIntervalInMs);
        dispatcherTimer.Tick += dispatcherTick;
    }



    private void CreateBitmap()
    {
        // Initialize the WriteableBitmap
        _bitmap = new WriteableBitmap(
            ImageWidth,
            ImageHeight,
            96,  // DPI X
            96,  // DPI Y
            PixelFormats.Bgra32,
            null);

        // Assign the bitmap to the Image control
        AntMap.Source = _bitmap;

    }

    private int Rounds {  get; set; }
    private void UpdateBitmap()
    {
        CreateBitmap();
        Rounds++;
        List<AntBase>? field;
        bool homes;
        int foodAmountPerRound;

        switch (gameMode)
        {
            case GameMode.Training:
                foodAmountPerRound = 1; // FOOD PER ROUND
                my_map.PlaceFoodTest(foodAmountPerRound, 90, 90);
                break;

            case GameMode.DuoMatch:
                if (Rounds % 2 == 0)
                {
                    foodAmountPerRound = 50; // FOOD PER ROUND                        
                    my_map.PlaceFood(foodAmountPerRound);
                }
                break;

            case GameMode.Game:
                if (Rounds % 2 == 0)
                {
                    foodAmountPerRound = 2; // FOOD PER ROUND
                    my_map.PlaceFood(foodAmountPerRound);
                }
                break;
        }


        var stat = my_map.GetStatistics();
        TeamListView.Items.Clear();
        for (int player = 0; player < players.Count; player++)
        {
            TeamListView.Items.Add(new TeamList { Species = players[player].Name, TeamAmount = stat[player], Color = "" });
        }


        for (int y = 0; y < ImageHeight; y++)
        {
            for (int x = 0; x < ImageWidth; x++)
            {
                field = my_map.GetAnts(x, y);

                int foodAmount = my_map.FoodMap[x, y];


                if (foodAmount > 0)
                {
                    byte red = 0;
                    byte green = 0;
                    byte blue = 0;

                    byte alpha = 255;

                    SetPixel(x, y, red, green, blue, alpha);
                }


                if ((field != null) && (field.Count > 0))
                {
                    byte red = 255;
                    byte green = 255;
                    byte blue = 0;

                    // Simplified color assignment using a switch statement
                    switch (field[0].Index)
                    {
                        case 0:
                            red = 255;
                            green = 0;
                            blue = 0;
                            break;

                        case 1:
                            red = 0;
                            green = 255;
                            blue = 0;
                            break;

                        case 2:
                            red = 128;
                            green = 0;
                            blue = 128;
                            break;

                        case 3:
                            red = 255;
                            green = 255;
                            blue = 0;
                            break;

                        case 4: 
                            red = 255;
                            green = 165;
                            blue = 0;
                            break;

                        case 5: 
                            red = 255;
                            green = 192;
                            blue = 203;
                            break;

                        case 6: 
                            red = 0;
                            green = 255;
                            blue = 255;
                            break;

                        case 7:
                            red = 255;
                            green = 0;
                            blue = 255;
                            break;

                        case 8:
                            red = 0;
                            green = 255;
                            blue = 0;
                            break;

                        case 9:
                            red = 139;
                            green = 69;
                            blue = 19;
                            break;

                        case 10:
                            red = 169;
                            green = 169;
                            blue = 169;
                            break;

                        case 11:
                            red = 255;
                            green = 127;
                            blue = 80;
                            break;

                        case 12:
                            red = 0;
                            green = 128;
                            blue = 128;
                            break;

                        case 13:
                            red = 255;
                            green = 215;
                            blue = 0;
                            break;

                        case 14:
                            red = 75;
                            green = 0;
                            blue = 130;
                            break;

                        // Default case (if the Index doesn't match any of the cases)
                        default:
                            red = 255;
                            green = 255;
                            blue = 255;  // White or any default color
                            break;
                    }

                    byte alpha = 255;
                    SetPixel(x, y, red, green, blue, alpha);
                }




                homes = my_map.AntHome(x, y);
                if (homes == true)
                {
                    byte black = 0;
                    byte green = 0;
                    byte blue = 255;

                    byte alpha = 255;

                    SetPixel(x, y, black, green, blue, alpha);
                }
            }
        }
    }


    private void SetPixel(int x, int y, byte r, byte g, byte b, byte a)
    {
        byte[] colorData = { b, g, r, a };

        Int32Rect rect = new Int32Rect(x, y, 1, 1);

        _bitmap.WritePixels(rect, colorData, 4, 0);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (!dispatcherTimer.IsEnabled)
        {
            dispatcherTimer.Start();

        }
    }

    public void Start()
    {
        my_map.PlayRound();
        UpdateBitmap();
        my_map.GameRounds = FinishRounds;
        RoundCount++;

        if (FinishRounds == RoundCount)
        {
            GameEnds();
        }
    }
    private void dispatcherTick(object sender, EventArgs e)
    {
        Start();
    }

    private void StopButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }



    private void GameEnds()
    {
        WinnerPage winnerWindow = new WinnerPage(my_map.WinnerList);

        winnerWindow.Show();

        this.Close();
    }
}

public class TeamList
{
    public object Species { get; set; }
    public int TeamAmount { get; set; }
    public object Color { get; set; }

}


