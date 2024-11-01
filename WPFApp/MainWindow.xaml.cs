using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AntEngine;

namespace wpf_app;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private const int ImageWidth = 200;
    private const int ImageHeight = 200;
    private WriteableBitmap _bitmap;

    private Map my_map;

    public MainWindow()
    {
        var players = new List<Type> { typeof(TestAntNorth), typeof(TestAntSouth) };
        my_map = new Map(ImageWidth, ImageHeight, players, startAnts: 1, PlayMode.DuoMatch);

        InitializeComponent();
        CreateBitmap();
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


    private void UpdateBitmap()
    {
        CreateBitmap();
        List<AntBase>? field;
        int foodAmountPerRound = 1;
        my_map.PlaceFood(foodAmountPerRound);

        for (int y = 0; y < ImageHeight; y++)
        {
            for (int x = 0; x < ImageWidth; x++)
            {
                field = my_map.GetAnts(x, y);

                int foodAmount = my_map.FoodMap[x, y];


                if ((field != null) && (field.Count > 0))
                {
                    byte red = 255;
                    byte green = 255;
                    byte blue = 0;

                    if (field[0].Index == 0)
                    {
                        red = (byte)field.Count;
                    }
                    if (field[0].Index == 1)
                    {
                        green = (byte)field.Count;
                    }

                    byte alpha = 255; // Full opacity

                    SetPixel(x, y, red, green, blue, alpha);
                }

                if (foodAmount > 0)
                {
                    byte black = 0;
                    byte green = 0;
                    byte blue = 0;

                    byte alpha = 255;

                    SetPixel(x, y, black, green, blue, alpha);
                }
            }
        }
    }

    //TODO: Draw Home in

    private void SetPixel(int x, int y, byte r, byte g, byte b, byte a)
    {
        // Create an array to represent the pixel color in BGRA format
        byte[] colorData = { b, g, r, a };

        // Define the area to update (1x1 pixel)
        Int32Rect rect = new Int32Rect(x, y, 1, 1);

        // Update the pixel color using WritePixels
        _bitmap.WritePixels(rect, colorData, 4, 0);
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        my_map.PlayRound();
        UpdateBitmap();
    }
}