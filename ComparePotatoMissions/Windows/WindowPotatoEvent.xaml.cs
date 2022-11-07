using ComparePotatoMissions.Classes;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;



namespace ComparePotatoMissions.Forms
{
    /// <summary>
    /// Логика взаимодействия для WindowPotatoEvent.xaml
    /// </summary>
    public partial class WindowPotatoEvent : Window
    {
        private List<TextBox> textBoxesPlayers;
        private List<PlayerInfo> CurrentPlayersInfo { get; set; }

        public WindowPotatoEvent()
        {
            InitializeComponent();
        }

        private void WindowPotatoEvent_Loaded(object sender, RoutedEventArgs e)
        {
            textBoxesPlayers = new List<TextBox>();
            textBoxesPlayers.Add(TxtBxFirst);
            textBoxesPlayers.Add(TxtBxSecond);
            textBoxesPlayers.Add(TxtBxThird);
            textBoxesPlayers.Add(TxtBxFourth);
            textBoxesPlayers.Add(TxtBxFifth);
            textBoxesPlayers.Add(TxtBxSixth);

            CurrentPlayersInfo = new List<PlayerInfo>();            
        }

        private bool IsValidInput()
        {
            return true;
            /*return UInt32.TryParse(TxtBxFirst.Text, out _) && UInt32.TryParse(TxtBxSecond.Text, out _) && UInt32.TryParse(TxtBxThird.Text, out _) &&
                UInt32.TryParse(TxtBxFourth.Text, out _) && UInt32.TryParse(TxtBxFifth.Text, out _) && UInt32.TryParse(TxtBxSixth.Text, out _);*/
        }

        private async Task<PlayerInfo> GetPlayerInfo(long currentPlayerID)
        {
            PlayerInfo CurrentPlayerInfo;
            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri($"https://potato.tf/api/waveprogress?steamid={currentPlayerID}"),
            };

            Console.WriteLine($"Sending request for {currentPlayerID}");

            HttpResponseMessage response = await client.GetAsync("");

            var jsonResponse = await response.Content.ReadAsStringAsync();

            CurrentPlayerInfo = JsonSerializer.Deserialize<PlayerInfo>(jsonResponse);
            CurrentPlayerInfo.playerId = currentPlayerID;

            return CurrentPlayerInfo;
        }

        private async void BtnCompare_Click(object sender, RoutedEventArgs e)
        {            
            DataTable newDataTable = new DataTable();
            newDataTable.Columns.Add(new DataColumn("Image", typeof(BitmapSource)));
            newDataTable.Columns.Add(new DataColumn("Map", typeof(string)));
            newDataTable.Columns.Add(new DataColumn("Mission", typeof(string)));
            newDataTable.Columns.Add(new DataColumn("Summary", typeof(string)));

            /*Console.WriteLine($"Column count = {DataGridPlayersInfo.Columns.Count}");
            foreach (var column in DataGridPlayersInfo.Columns)
            {
                Console.WriteLine($"Header: {column.Header}, index: {column.DisplayIndex}");
            }*/
           
            for (int i = DataGridPlayersInfo.Columns.Count - 1; i >= 4; i--)
            {
                DataGridPlayersInfo.Columns.RemoveAt(i);
            }


            if (IsValidInput())
            {
                foreach (var textBox in textBoxesPlayers)
                {
                    if (long.TryParse(textBox.Text, out long currentPlayerID) && currentPlayerID > 0)
                    {
                        DataGridTextColumn newColumn = new DataGridTextColumn() 
                        { 
                            Header = currentPlayerID.ToString(),
                            Binding = new Binding(currentPlayerID.ToString()),                        
                        };
                        DataGridPlayersInfo.Columns.Add(newColumn);

                        newDataTable.Columns.Add(new DataColumn(currentPlayerID.ToString(), typeof(string)));

                        CurrentPlayersInfo.Add(await GetPlayerInfo(currentPlayerID));
                    }
                }

                for (int i = 0; i < CurrentPlayersInfo[0].waveProgress.Length; i++)
                {
                    DataRow newRow = newDataTable.NewRow();

                    Uri uriOfMapImage = new Uri($@"https://potato.tf{CurrentPlayersInfo[0].waveProgress[i].imageUrl}");
                    BitmapImage mapImage = new System.Windows.Media.Imaging.BitmapImage(uriOfMapImage);
                   
                    // 100 pxls
                    var scaleWeightOfMapImage = 100 / mapImage.PixelWidth;
                    var scaleHeightOfMapImage = 100 / mapImage.PixelHeight;

                    /*var testX = mapImage.PixelWidth * 0.2;
                    var testY = mapImage.PixelHeight * 0.2;*/
                    Console.WriteLine($"Map = {CurrentPlayersInfo[0].waveProgress[i].mapNiceName}");
                    Console.WriteLine($"PixelWidth = {mapImage.PixelWidth}, PixelHeight = {mapImage.PixelHeight}");
                    Console.WriteLine($"Width = {mapImage.Width}, Height = {mapImage.Height}");
                    Console.WriteLine($"DpiX = {mapImage.DpiX}, DpiY = {mapImage.DpiY}\n\n");

                    var targetBitmap = new TransformedBitmap(mapImage, new ScaleTransform(0.5, 0.5));
                    newRow["Image"] = targetBitmap;

                    newRow["Map"] = CurrentPlayersInfo[0].waveProgress[i].mapNiceName;                    
                    newRow["Mission"] = CurrentPlayersInfo[0].waveProgress[i].missionNiceName;

                    // Compute value of "Summary" column
                    int conjunctionOfPlayersMission = int.MaxValue;
                    int disjunctionOfPlayersMission = 0;
                    foreach (var playerInfo in CurrentPlayersInfo)
                    {
                        conjunctionOfPlayersMission &= playerInfo.waveProgress[i].wavesCompletedBitmask;
                        disjunctionOfPlayersMission |= playerInfo.waveProgress[i].wavesCompletedBitmask;
                    }

                    if (conjunctionOfPlayersMission == (int)Math.Pow(2, CurrentPlayersInfo[0].waveProgress[i].waveCount) - 1)
                    {
                        newRow["Summary"] = "✓";
                    }
                    else if (disjunctionOfPlayersMission == 0)
                    {
                        newRow["Summary"] = "✖";
                    }
                    else
                    {
                        newRow["Summary"] = "*";
                    }

                    // Compute each player mission progress
                    foreach (var playerInfo in CurrentPlayersInfo)
                    {
                        if (playerInfo.waveProgress[i].wavesCompletedBitmask == (int)Math.Pow(2, playerInfo.waveProgress[i].waveCount) - 1)
                        {
                            newRow[playerInfo.playerId.ToString()] = "✓";
                        }
                        else if (playerInfo.waveProgress[i].wavesCompletedBitmask == 0)
                        {
                            newRow[playerInfo.playerId.ToString()] = "✖";
                        }
                        else
                        {
                            newRow[playerInfo.playerId.ToString()] = "*";
                        }
                    }

                    newDataTable.Rows.Add(newRow);
                }
            }

            //DataGridPlayersInfo.Columns.Clear();
            DataGridPlayersInfo.ItemsSource = newDataTable.DefaultView;
        }
    }
}
