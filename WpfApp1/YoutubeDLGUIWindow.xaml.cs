using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for YoutubeDLGUIWindow.xaml
    /// </summary>
    public partial class YoutubeDLGUIWindow : Window
    {
        ObservableCollection<GridDownloadItem> gridItemCollection = new ObservableCollection<GridDownloadItem>();

        bool showWindow = false;

        //PathStrings
            string defaultFilename = "\\%(title)s.%(ext)s";

            string defaultPathToYoutubeDL = "C:\\YoutubeDL\\youtube-dl.exe";

            string defaultPathToSavedVideos = "C:\\YoutubeDLVideos";
            //string homeOutput = "-o C:\\%HOMEPATH%\\Videos\\%(title)s.%(ext)s"
            //Got to have the %, along with the 2 parenthesis, and the s at the end, as that means a VARIABLE is inside to be auto-inserted.

            string currentPathToYoutubeDL;
            string currentPathToSavedVideos;//TODO: Perhaps make it exclusively a path, and make default format a seperate variable? (title/ext)

        StringBuilder commandPromptStringBuilder;

        public Dictionary<int, string> queryCommandDictionary = new Dictionary<int, string>();

        //Both the struct and the instance, are meant to be used only on stackpanel buttonPanel!
        CustomButtonStruct customButtonTemplate;

        public List<Button> customButtonsInStackPanel = new List<Button>();

        public int maxCustomButtons = 4;
        public bool DoMaxCustomButtonsExist = false;
        public int textBoxCustomButtonAssignment = 0;

        //No other data structure is better for this. 
        //As you cannot shuffle processes and all of them end on "random" times instead of determined
        //e.g. Queue/Stack/List(removing from the end)
        //  Could use a List since it auto-resizes properly and its indexes as well but whatever lmao
        //  Tis just a list, without auto-resizing!
        public Dictionary<int, Process> youtubeDLProcesses = new Dictionary<int, Process>();
        public int youtubeDLProcessIndex = 0;
        public Dictionary<int, bool> youtubeDLProcessesFinished = new Dictionary<int, bool>();
        public Dictionary<int, bool> youtubeDLProcessesIsSingleVideo = new Dictionary<int, bool>();//Playlists or entire channels

        public GridDownloadItem tempGridItem;

        public static RoutedCommand Ctrl1Command = new RoutedCommand();
        public static RoutedCommand Ctrl2Command = new RoutedCommand();
        public static RoutedCommand Ctrl3Command = new RoutedCommand();
        public static RoutedCommand Ctrl4Command = new RoutedCommand();
        public static RoutedCommand Ctrl5Command = new RoutedCommand();

        public Color[] colorProcessLogs = new Color[5];

        //See this https://stackoverflow.com/questions/665299/are-2-dimensional-lists-possible-in-c
        //UML ends up on this. A list of process lists, whereas each nested list inside, has OutputLogLineBreakRunHolder.
        public List<List<OutputLogLineBreakRunHolder>> consoleLogProcessOutputSuperList = new List<List<OutputLogLineBreakRunHolder>>();

        public Dictionary<object, int> processToColorDictionary = new Dictionary<object, int>();

        //Another superlist, to hold the dataDownloadGridItems, because a process can have more than one downloadItems
        //This has nothing to do with UI btw. Data and Visuals, are obviously seperated.
        public List<List<GridDownloadItem>> downloadGridProcessItems = new List<List<GridDownloadItem>>();

        //This is used for downloading multiple videos
        //Saving the latest number of current process here, caching it
        //and if the new number is different, create new downloadGridItem
        //      (Finishing/Clearing the last)
        //Parsing "Downloading videos X of Y" ftw!
        public Dictionary<int, int> youtubeDLProcessesCurrentVideoIndex = new Dictionary<int, int>();

        //Stores which query command each youtubeDLProcess has!
        public Dictionary<int, string> youtubeDLProcessesCustomQueryCommand = new Dictionary<int, string>();

        public string userConfigString;
        public string pathToSave;
        public int totalOpenTimes;



        public YoutubeDLGUIWindow()
        {
            InitializeComponent();

            InitializeDownloadGrid();

            InitializeQueryCommandFilters();

            InitializeCustomButtonTemplate();

            customButtonsInStackPanel.Add(DownloadCustom1);

            CommandPromptTextBox.Focus();

            DetermineClipboard();

            InitializeRenameTextBox();

            InitializeCustomShortcuts();

            InitializeFilePaths();

            InitializeColorLogs();

            ReadUserConfig();

            WriteUserConfig();

            //this.StateChanged += new EventHandler(WindowStateChanged);//This doesn't work because it only detects minimizing by clicking the button
            this.Activated += new EventHandler(WindowStateChanged);//Any time the window is once again on top, it happens
        }

        public void InitializeQueryCommandFilters()
        {
            queryCommandDictionary.Add(0, "--format mp4");//Download Video, Playlist, Channel, Misc
            //queryCommandDictionary.Add(1, "--default-search \"ytsearch\" -f bestaudio");//Download Audio
            //queryCommandDictionary.Add(1, "--best-audio");//Download Audio, has issues with downloading multiple videos
            queryCommandDictionary.Add(1, "--extract-audio --audio-format mp3");//Download Audio
        }

        public void InitializeCustomButtonTemplate()
        {
            customButtonTemplate = new CustomButtonStruct();

            customButtonTemplate.buttonHorizontalAlignment = HorizontalAlignment.Center;
            customButtonTemplate.buttonVerticalAlignment = VerticalAlignment.Top;
            customButtonTemplate.buttonMargin = new Thickness(20, 0, 0, 0);
            customButtonTemplate.buttonWidth = 75;
            customButtonTemplate.buttonStringContent = "Custom";
            customButtonTemplate.buttonParent = ButtonPanel;
        }

        public void InitializeDownloadGrid()
        {
            //TestFillDownloadItemList();

            //Hide it at boot, no1 wants the ugly bars on start
            DownloadGrid.Visibility = Visibility.Collapsed;

            //Link it with the grid here in the back-end of the code :)
            DownloadGrid.ItemsSource = gridItemCollection;

            //When downloadGrid is selected (by tab), insta-focus on the command prompt again!
            DownloadGrid.GotFocus += ChangeFocusTowardsCommandPrompt;
        }

        public void TestFillDownloadItemList()
        {
            gridItemCollection.Add(new GridDownloadItem("https://www.youtube.com/watch?v=JpOaQxrsaqI", "too much for nintendo - SSBM movement highlight video", "-1", "153", "-1"));
            gridItemCollection.Add(new GridDownloadItem("https://www.youtube.com/watch?v=2xl56IJGKwY", "Are You Just TOO SMART to Learn Anything?", "-1", "153", "-1"));
            gridItemCollection.Add(new GridDownloadItem("https://www.youtube.com/watch?v=acqpulP1hLo", "Starsector Review | Explore the Cosmos™ | Ruin Everything™", "-1", "153", "-1"));
        }

        public void ChangeFocusTowardsCommandPrompt(object sender, RoutedEventArgs e)
        {
            CommandPromptTextBox.Focus();
        }

        public void DetermineClipboard()
        {
            bool isTextDataOnClipboard = Clipboard.ContainsData(DataFormats.Text);
            bool isYoutubeVideoOnClipboard = false;

            if (isTextDataOnClipboard && GetIndexOfYoutubeVideoURL(Clipboard.GetText()) != -1)
                    isYoutubeVideoOnClipboard = true;

            //If clipboard contains youtube video, paste it on textbox
            if (isYoutubeVideoOnClipboard)
            {
                //Puts the clipboard onto the command prompt UI
                if (CommandPromptTextBox.Text.Contains("--") == false)
                    CommandPromptTextBox.Text = Clipboard.GetText();//Shows at UI too feelsgoodman
                else//user has put --options ;)
                    CommandPromptTextBox.Text = CommandPromptTextBox.Text + " " + Clipboard.GetText();
                CommandPromptTextBox.CaretIndex = CommandPromptTextBox.Text.Length;

                CommandPromptTextBox.Focus();

                //Focuses/Selects the CommandPromptTextBox, so you can just press Enter
                //and it downloads the clipboard :)
                //CommandPromptTextBox.Focus();//commented out because focus always happens here when you boot it
            }
        }

        public void InitializeRenameTextBox()
        {
            //Hide the box! It should appear only when a new button is made!
            RenameTextBoxBorder.Visibility = Visibility.Hidden;

            //When focus is lost by whatever reason, hide the text box and push its content onto the button
            RenameTextBox.LostFocus += ConvertTextBoxToPureContent;

            //Make it show OVER the button
            Canvas.SetZIndex(RenameTextBoxBorder, 8);//8 is hard-coded "always in front" yolo
        }

        public void InitializeCustomShortcuts()
        {
            Ctrl1Command.InputGestures.Add(new KeyGesture(Key.D1, ModifierKeys.Control));
            Ctrl2Command.InputGestures.Add(new KeyGesture(Key.D2, ModifierKeys.Control));
            Ctrl3Command.InputGestures.Add(new KeyGesture(Key.D3, ModifierKeys.Control));
            Ctrl4Command.InputGestures.Add(new KeyGesture(Key.D4, ModifierKeys.Control));
            Ctrl5Command.InputGestures.Add(new KeyGesture(Key.D5, ModifierKeys.Control));

            CommandBindings.Add(new CommandBinding(Ctrl1Command, Ctrl1Command_Executed));
            CommandBindings.Add(new CommandBinding(Ctrl2Command, Ctrl2Command_Executed));
            CommandBindings.Add(new CommandBinding(Ctrl3Command, Ctrl3Command_Executed));
            CommandBindings.Add(new CommandBinding(Ctrl4Command, Ctrl4Command_Executed));
            CommandBindings.Add(new CommandBinding(Ctrl5Command, Ctrl5Command_Executed));
        }

        public void InitializeFilePaths()
        {
            //Used to be defaultPathToYoutubeDL
            currentPathToYoutubeDL = System.AppDomain.CurrentDomain.BaseDirectory + "\\youtube-dl.exe";

            currentPathToSavedVideos = defaultPathToSavedVideos;
        }

        /// <summary>
        /// To be used by different processes
        /// Error Log, is not included.
        /// </summary>
        public void InitializeColorLogs()
        {
            //0 is the default gray
            //The rest were made from this https://www.sessions.edu/color-calculator/ (put color[1] there)
            //[3] sucks so I changed it, legitimately haven't used [4], I doubt anyone will reach there.
            colorProcessLogs[0] = Color.FromRgb(170, 170, 170);
            colorProcessLogs[1] = Color.FromRgb(151, 201, 255);
            colorProcessLogs[2] = Color.FromRgb(255, 205, 151);
            colorProcessLogs[3] = Color.FromRgb(154, 255, 151);
            colorProcessLogs[4] = Color.FromRgb(205, 151, 255);
        }

        /*public void InitializeHelpButton()
        {
            Binding helpBinding = new Binding("Width");
            helpBinding.Source = OutputLogBorder;
            HelpButton.SetBinding(HelpButton.Margin.Left, helpBinding);
        }*/




        //===========================
        //=== Button Clicks Below ===
        //===========================

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                CommandPromptTextBoxEnter();
                
        }

        private void ButtonEnterTextBox_Click(object sender, RoutedEventArgs e)
        {
            CommandPromptTextBoxEnter();
        }

        private void VideoButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            CommandPromptTextBoxEnter();
        }

        private void MusicButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            //Look at queryCommandDictionary;
            CommandPromptTextBoxEnter(1);
        }

        public void CommandPromptTextBoxEnter(int queryCommand = -1)
        {
            commandPromptStringBuilder = new StringBuilder(CommandPromptTextBox.Text);

            //Confirm no logic error (paths are ok)
            if (IsErrorDetected())
                return;

            //Polish the string (remove the youtube-dl.exe and remove white space at start)
            StringPolishedFormat(true, true);

            //For any non-default/expected button
            //e.g. Video is 0, and adds "--format mp4"
            //e.g. Music is 1, and adds "--extract-audio -f mp3"
            AddCustomQueryCommand(queryCommand);

            //Add -o (and check if there isn't already an '-o')
            DetermineOutputLocation();

            SaveInputQueryOnProcess();

            //Debugging
            Debug.WriteLine("cmdOutput:\n" + commandPromptStringBuilder.ToString());

            //Having finished everything, sets up processStartInfo and boots the process
            StartProcessProperly();

            CreateDownloadGridItem(youtubeDLProcessIndex - 1);

            //Reset the commandPromptBox
            CommandPromptTextBox.Text = "";
            CommandPromptTextBox.CaretIndex = 0;//Max is text.length;

            ResetConsoleLog();

        }

        public bool IsErrorDetected()
        {
            if (File.Exists(currentPathToYoutubeDL) == false)
            {
                MessageBox.Show("Path To YoutubeDL is **Unknown**\nIt should be in the same folder with this program you run!");
                return true;
            }
            else if (commandPromptStringBuilder.ToString() == "")
                return true;

            return false;
        }

        public void StringPolishedFormat(bool removeYoutubedlexe = true, bool removeStartingGap = true)
        {
            if (removeYoutubedlexe)
                commandPromptStringBuilder.Replace("youtube-dl.exe", "");

            if (removeStartingGap)
            {
                bool stopIteration = false;

                //Iterate from start for white space
                int i = 0;
                while(stopIteration && i < commandPromptStringBuilder.Length)
                {
                    if (commandPromptStringBuilder[i] == ' ')
                    {
                        stopIteration = true;
                    }

                    i++;
                }

                if (stopIteration)
                    commandPromptStringBuilder.Remove(0, i - 1);
            }
        }

        public void AddCustomQueryCommand(int givenQueryCommand)
        {
            int finalQueryCommandToApply = givenQueryCommand;

            //99% of the times ;)
            if (finalQueryCommandToApply == -1)
            {
                //If has no youtubeURL OR has youtubeURL with commands behind it
                if (IsStringPureYoutubeURL(commandPromptStringBuilder.ToString()) != 0)
                    return;

                //Make it "custom", aka video format, to mp4!
                finalQueryCommandToApply = 0;

                //tbh, I failed the parsing. The default should be --format mp4, not nothing, to not check against the 99% case all the time -_-
            }
            //else, add query command instead of being typed as is

            //URL goes in here
            string tempString = commandPromptStringBuilder.ToString();
            commandPromptStringBuilder.Clear();

            //I am using the Insert function as you cannot give a string value to a stringbuilder -_-
            //As for the string, it is: [queryCommand] + " " + "[URL]"
            commandPromptStringBuilder.Append(queryCommandDictionary[finalQueryCommandToApply] + " " + tempString);
        }

        /// <summary>
        /// Takes the commandPromptStringBuilder, and adds -o in the very front of it!
        /// </summary>
        public void DetermineOutputLocation()
        {
            //First, check if user has put output choices.
                //Final flag which will confirm if yes or not
                bool hasUserWrittenOutput = false;

                //Copy commandpromptstringbuilder temporarily onto tempStringBuilder so I can edit it without losing the OG
                StringBuilder tempStringBuilder = new StringBuilder();
                tempStringBuilder.Append(commandPromptStringBuilder.ToString());

                //Replace the output and compare lengths, to check if it has user output or not!
                tempStringBuilder.Replace("-o ", "");
                tempStringBuilder.Replace("--output ", "");
                if (tempStringBuilder.Length != commandPromptStringBuilder.Length)
                    hasUserWrittenOutput = true;

            //If no user output is detected, input your own output
            //((User's Output is on [query command], so no need to do anything there :)
            if (hasUserWrittenOutput == false)
            {
                //Save the current string, which is [queryCommand] + " " + [URL]
                string tempString = commandPromptStringBuilder.ToString();

                //Clear the final string, so we do this:
                //[Output] + " " + [queryCommand] + " " + [URL]
                //[Output] + " " + tempString
                commandPromptStringBuilder.Clear();
                commandPromptStringBuilder.Append("--output " + currentPathToSavedVideos + defaultFilename + " " + tempString);
            }
            
            
        }

        /// <summary>
        /// Saves the query input on each youtubeDL Process!
        /// </summary>
        public void SaveInputQueryOnProcess()
        {
            StringBuilder fullCommandStringBuilder = new StringBuilder(commandPromptStringBuilder.ToString());

            //Remove the URL if any
            int indexOfVideoURL = GetIndexOfYoutubeVideoURL(commandPromptStringBuilder.ToString());

            //If has URL, remove it
            if (indexOfVideoURL != -1)
                //Remove the URL
                fullCommandStringBuilder.Remove(indexOfVideoURL, fullCommandStringBuilder.Length - indexOfVideoURL);

            youtubeDLProcessesCustomQueryCommand.Add(youtubeDLProcessIndex, fullCommandStringBuilder.ToString());
        }

        public void StartProcessProperly()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(currentPathToYoutubeDL);

            if (showWindow == false)
            {
                //ShellExecute must be false to not create a window.
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;

                startInfo.WindowStyle = ProcessWindowStyle.Minimized;//Consider hidden
            }
            else
                startInfo.WindowStyle = ProcessWindowStyle.Normal;

            //Very important to get the output from youtube-dl!
            //https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.processstartinfo.redirectstandardoutput?view=net-5.0
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;

            startInfo.Arguments = commandPromptStringBuilder.ToString();

            //Debugging
            //Debug.WriteLine("cmdOutput:\n" + commandPromptStringBuilder.ToString());

            //Cache the process!
            youtubeDLProcesses[youtubeDLProcessIndex] = Process.Start(startInfo);
            

            //Event-Register the Process
            youtubeDLProcesses[youtubeDLProcessIndex].EnableRaisingEvents = true;
            youtubeDLProcesses[youtubeDLProcessIndex].OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
            youtubeDLProcesses[youtubeDLProcessIndex].ErrorDataReceived += new DataReceivedEventHandler(ErrorDataReceived);
            youtubeDLProcesses[youtubeDLProcessIndex].Disposed += new EventHandler(DisposedEvent);
            youtubeDLProcesses[youtubeDLProcessIndex].Exited += new EventHandler(ExitedEvent);

            youtubeDLProcesses[youtubeDLProcessIndex].BeginErrorReadLine();
            youtubeDLProcesses[youtubeDLProcessIndex].BeginOutputReadLine();

            youtubeDLProcessesFinished[youtubeDLProcessIndex] = false;

            //========================================
            //===Covering the multiple videos cases===
            //Is just a single video
            if (IsStringPlaylistOrChannel(commandPromptStringBuilder.ToString()) == false)
                    youtubeDLProcessesIsSingleVideo[youtubeDLProcessIndex] = true;
                else//is a playlist or a channel
                    youtubeDLProcessesIsSingleVideo[youtubeDLProcessIndex] = false;


                youtubeDLProcessesCurrentVideoIndex.Add(youtubeDLProcessIndex, 1);
            //========================================

            youtubeDLProcessIndex++;
        }

        /*Needs:
        /   1. URL (gotten from input)
        /   2. Title (gotten from [download])
        /       This is what puts it onto the UI as well, as it should be the last information.
        /   3. Time Remaining/ETA (gotten from [download])
        /   4. Progress (gotten from [download])
        /   5. Size (gotten from [download])
        */
        public void CreateDownloadGridItem(int processIndex)
        {
            GridDownloadItem createdDownloadGridItem = new GridDownloadItem();

            int indexOfVideoURL = GetIndexOfYoutubeVideoURL(commandPromptStringBuilder.ToString());

            if (indexOfVideoURL != -1)
                createdDownloadGridItem.URL = commandPromptStringBuilder.ToString().Remove(0, indexOfVideoURL);

            //Make sure you don't try to add on a null/non-created list!
            //Got to check if a nested list is even created!
            //Otherwise argument/index overflow!
            while (downloadGridProcessItems.Count <= processIndex + 1)
                downloadGridProcessItems.Add(new List<GridDownloadItem>());

            downloadGridProcessItems[processIndex].Add(createdDownloadGridItem);
        }


        public void DetermineFinalisationOfDownloadGridItem(GridDownloadItem checkItem)
        {
            //If it is finished, visualize it!
            //If it is not visualized already ofc!
            if (checkItem.IsFilled() && gridItemCollection.Contains(checkItem) == false)
            {
                //If data grid has not been used before, aka is empty from launch
                //Show it!
                if (gridItemCollection.Count == 0)
                {
                    DownloadGrid.Visibility = Visibility.Visible;

                    DetermineWindowResizing();
                }

                gridItemCollection.Add(checkItem);
            }
                
        }

        public void ResetConsoleLog()
        {
            //Reset the youtubeDLProcess Logs, of finished processes!
            DeleteFinishedYoutubeDLConsoleLogs();

            //Update the color dictionary!
            UpdateColorDictionary();
        }

        public void DeleteFinishedYoutubeDLConsoleLogs()
        {
            int processIndex;
            int i;
            //Iterate all youtubeDL processes
            for (processIndex = 0; processIndex < youtubeDLProcessIndex; processIndex++)
            {
                //If Finished
                if (youtubeDLProcessesFinished[processIndex] == true)
                {
                    //Iterate the log inside
                    for (i = 0; i < consoleLogProcessOutputSuperList[processIndex].Count; i++)
                    {
                        OutputLog.Inlines.Remove(consoleLogProcessOutputSuperList[processIndex][i].lineBreakElement);
                        OutputLog.Inlines.Remove(consoleLogProcessOutputSuperList[processIndex][i].runElement);
                    }
                }

            }
        }

        public void UpdateColorDictionary()
        {
            //Iterate all youtubeDL processes
            for (int processIndex = 0; processIndex < youtubeDLProcessIndex; processIndex++)
            {
                //If the process exists in the color dictionary AND is finished
                if (processToColorDictionary.ContainsKey(youtubeDLProcesses[processIndex]) && youtubeDLProcessesFinished[processIndex] == true)
                {
                    //Got to delete it from the dictionary **properly**

                    //Get the value we will remove (so we give it to another key process ;)
                    int valueRemoved = processToColorDictionary[youtubeDLProcesses[processIndex]];

                    //Which means not just deleting it from the dictionary
                    processToColorDictionary.Remove(youtubeDLProcesses[processIndex]);

                    //But also filling that gap, by replacing it with the new one!
                    processToColorDictionary.Add(youtubeDLProcesses[youtubeDLProcessIndex - 1], valueRemoved);

                    return;//Otherwise it will bug by trying to put the same key more than once!

                    //If you read the above lines carefully, you should realise, it really is but a key swap.
                }
            }
        }

        /// <summary>
        /// Puts the process into processToColorDictionary, if it doesn't exist!
        /// </summary>
        /// <param name="newProcess"></param>
        public void DetermineNewDictionaryKey(Process newProcess)
        {
            //If Process has not been recorded before, link the process to the proper color!
            if (processToColorDictionary.ContainsKey(newProcess) == true)
                return;

            //Iterate all youtubeDL processes
            for (int processIndex = 0; processIndex < youtubeDLProcessIndex; processIndex++)
            {
                //If the process exists in the color dictionary AND is finished
                if (processToColorDictionary.ContainsKey(youtubeDLProcesses[processIndex]) && youtubeDLProcessesFinished[processIndex] == true)
                {
                    //Useless key detected, time to replace it!

                    //Get the value we will remove (so we give it to another key process ;)
                    int valueRemoved = processToColorDictionary[youtubeDLProcesses[processIndex]];

                    //Which means not just deleting it from the dictionary
                    processToColorDictionary.Remove(youtubeDLProcesses[processIndex]);

                    //But also filling that gap, by replacing it with the new one!
                    processToColorDictionary.Add(newProcess, valueRemoved);

                    return;//Otherwise it will bug by trying to put the same key more than once!

                    //If you read the above lines carefully, you should realise, it really is but a key swap.
                }
            }

            //If it reached here, it means there are no empty slots, and a new one should be added!
            //Link it to the latest color!
            processToColorDictionary.Add(newProcess, processToColorDictionary.Count);
        }

        public void CustomClickEvent(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button) e.Source;

            if (CommandPromptTextBox.Text == "")
                return;

            //If no new custom button is to be created (aka there is another custom button aside of original)
            //Just run the command prompt. Else... create a new query to be linked for this button, and make a new button
            //To do this, first, we iterate the list of custom buttons to get which one this is
            int buttonIndexInsideCustomButtons = -1;
            for (int i = 0; i < customButtonsInStackPanel.Count; i++)
                if (customButtonsInStackPanel[i] == e.Source)
                    buttonIndexInsideCustomButtons = i;

            //If this isn't the last custom button
            if (clickedButton != customButtonsInStackPanel.Last())
                CommandPromptTextBoxEnter(buttonIndexInsideCustomButtons + 2);//Custom button index + the offset of button queries (2)
            
            //If no more custom buttons are allowed AND the final one is clicked AND is already defined!
            else if (DoMaxCustomButtonsExist && clickedButton.Content.ToString() != customButtonTemplate.buttonStringContent)
                CommandPromptTextBoxEnter(buttonIndexInsideCustomButtons + 2);//Custom button index + the offset of button queries (2)
            
            //else, this is the last custom button, which is never clicked before!
            else
            {
                //Cache the query for this button
                int queryContainsLink = DetermineNewCustomQueryCommand();
                if (queryContainsLink != -1)//If link, do the search!
                {
                    //Got to remove the text, otherwise it will be duplicated
                    //CommandPromptTextBox + CustomQueryCommand
                    CommandPromptTextBox.Text = CommandPromptTextBox.Text.Remove(0, GetIndexOfYoutubeVideoURL());

                    //MessageBox.Show("CommandPrompt is: " + CommandPromptTextBox.Text);//Pure URL

                    //Now run the cmd
                    CommandPromptTextBoxEnter(customButtonsInStackPanel.Count + 1);
                }
                    

                //Rename this button!
                DetermineRenamingContent(customButtonsInStackPanel[customButtonsInStackPanel.Count - 1]);

                //Tooltip this button with the appropriate Query you typed
                DetermineTooltip(buttonIndexInsideCustomButtons);

                //Create new Button
                if (DoMaxCustomButtonsExist == false)
                    customButtonsInStackPanel.Add(CreateButtonInCustomButtonPanel());
            }

            //Write current configs (new buttons) on userConfig text file!
            WriteUserConfig();

        }

        /// <summary>
        /// Returns -1 if no video link to search and it was exclusively for making a custom button
        /// </summary>
        /// <returns></returns>
        public int DetermineNewCustomQueryCommand()
        {
            //I use this as a flag. As long it is -1, I haven't detected a video link
            int endFormatOptionsIndex = GetIndexOfYoutubeVideoURL();

            String finalQueryString;

            //If it is STILL -1, it means there is NO video link!
            if (endFormatOptionsIndex == -1)
                finalQueryString = CommandPromptTextBox.Text;
            else//remove the video link part (to the right of the string, up to the end) ((and also trims it))
            {
                StringBuilder tempStringBuilder = new StringBuilder(CommandPromptTextBox.Text);

                finalQueryString = tempStringBuilder.Remove(endFormatOptionsIndex, tempStringBuilder.Length - endFormatOptionsIndex).ToString().Trim();
            }
                

            //Finally, put/save the query!
            queryCommandDictionary[customButtonsInStackPanel.Count + 1] = finalQueryString;

            return endFormatOptionsIndex;
        }

        /// <summary>
        /// Gets the end of format options on the command line
        /// More accurately, the very start index of the video URL
        /// If none -> Returns -1
        /// Checks for "https","youtube.com","youtu.be"
        /// </summary>
        /// <returns></returns>
        public int GetIndexOfYoutubeVideoURL(string commandPromptOverride = "")
        {
            StringBuilder tempStringBuilder;

            //Default, aka take the commandPromptTextBox
            if (commandPromptOverride == "")
                tempStringBuilder = new StringBuilder(CommandPromptTextBox.Text);
            else
                tempStringBuilder = new StringBuilder(commandPromptOverride);

            //We use this as a flag. As long it is -1, we haven't detected a video link
            int endFormatOptionsIndex = -1;
            endFormatOptionsIndex = tempStringBuilder.ToString().IndexOf("https");
            if (endFormatOptionsIndex == -1)
                endFormatOptionsIndex = tempStringBuilder.ToString().IndexOf("youtube.com");
            if (endFormatOptionsIndex == -1)
                endFormatOptionsIndex = tempStringBuilder.ToString().IndexOf("youtu.be");

            return endFormatOptionsIndex;
        }

        public int IsStringPureYoutubeURL(string givenCommand)
        {
            int indexOfVideoURL = GetIndexOfYoutubeVideoURL(givenCommand);

            //If there is a video URL
            if (indexOfVideoURL != -1)
            {
                //Find if there is anything behind it!
                //All commands have '-' and are behind the URL!
                if (commandPromptStringBuilder.ToString().Contains("-") && commandPromptStringBuilder.ToString().IndexOf("-") < indexOfVideoURL)
                    return 1;//String is not pure YoutubeURL, it contains commands!
                else
                    return 0;//String is pure YoutubeURL
            }

            return -1;//String is not even a youtubeURL lmao
        }

        public bool IsStringPlaylistOrChannel(string givenCommand)
        {
            //First, check if there is a videoURL
            int indexOfVideoURL = GetIndexOfYoutubeVideoURL(givenCommand);

            //If there is a video URL
            if (indexOfVideoURL != -1)
                if (givenCommand.Contains("/user/") || givenCommand.Contains("/channel/") || givenCommand.Contains("/playlist?"))
                    return true;

            return false;
        }

        //for (int i = 0; i < queryCommandDictionary.Count; i++)
        //MessageBox.Show(queryCommandDictionary[i]);

        Button CreateButtonInCustomButtonPanel()
        {
            Button createdButton = new Button();

            createdButton.Content = customButtonTemplate.buttonStringContent; //+ (customButtonsInStackPanel.Count + 1).ToString();
            createdButton.HorizontalAlignment = customButtonTemplate.buttonHorizontalAlignment;
            createdButton.VerticalAlignment = customButtonTemplate.buttonVerticalAlignment;
            createdButton.Margin = customButtonTemplate.buttonMargin;
            createdButton.Width = customButtonTemplate.buttonWidth;
            createdButton.Foreground = (SolidColorBrush) new BrushConverter().ConvertFrom("#FF440000");


            customButtonTemplate.buttonParent.Children.Add(createdButton);

            createdButton.Click += CustomClickEvent;

            //Check if this is the last button
            if (customButtonsInStackPanel.Count + 1 == maxCustomButtons)
                DoMaxCustomButtonsExist = true;

            //Hacky to put it here, but gotta move that downloadlabel!
            DownloadLabel.Margin = new Thickness(DownloadLabel.Margin.Left - 40,0,0,0);

            //Put proper tab index here, but also change the youtubeDL Exe/Save location tab index!
            UpdateTabIndexes(createdButton);

            return createdButton;
        }

        public void UpdateTabIndexes(Button createdButton)
        {
            createdButton.TabIndex = customButtonsInStackPanel.Count + 3;

            SaveDialogueButton.TabIndex = customButtonsInStackPanel.Count + 4;
            OutputLogMinimizeButton.TabIndex = customButtonsInStackPanel.Count + 5;
            //OpenYoutubeDLButton.TabIndex = customButtonsInStackPanel.Count + 5;
        }

        public void DetermineRenamingContent(Button renamingButton)
        {

            //Misc shit I tried to get textBox on the ButtonLocation PepeHands
                //renamingTextBox.RenderTransform.TryTransform(renamingTextBox.RenderTransform.Value);
                //renamingTextBox.TranslatePoint(renamingTextBox.RenderTransformOrigin, DownloadLabel);
                //RenameTextBoxBorder.RenderTransformOrigin = renamingButton.RenderTransformOrigin;
                //RenameTextBoxBorder.RenderTransform = renamingButton.RenderTransform;
                //RenameTextBoxBorder.TranslatePoint (renamingButton.RenderTransform.Value);
                //RenameTextBoxBorder.SetValue(LeftProperty, (double)RenameTextBoxBorder.GetValue(Canvas.LeftProperty) + (double)900);

            //Hide the button's content for now ((Can't delete it))
            renamingButton.Content = "";

            //Make the textbox appear again!
            RenameTextBoxBorder.Visibility = Visibility.Visible;

            //Move offset
                //51 is distance from center to Custom1 button's center
                //75 is button width, and obviously it's halved as I want the center
                //18 is margin (normally 20, but it works exactly on center no idea why, 20 should be the best)
                double offsetXFromCenter = 51 + (customButtonsInStackPanel.Count - 1) * (18 + 75/2);
            RenameTextBoxBorder.RenderTransform = new TranslateTransform(offsetXFromCenter, 23);

            //Make its width, proper
            RenameTextBox.Width = 50;
            //Make its height, proper
            RenameTextBox.Height = 18;

            //Focus so as to insta-write here
            RenameTextBox.Focus();
            RenameTextBox.Dispatcher.BeginInvoke(new Action(() => RenameTextBox.SelectAll()));
        }

        public void DetermineTooltip(int buttonIndexInsideCustomButtons)
        {
            //Create a tooltip
            ToolTip createdTooltip = new ToolTip();

            //Write the query onto the tooltip
            createdTooltip.Content = queryCommandDictionary[customButtonsInStackPanel.Count + 1];

            //Make the above tooltip its child
            customButtonsInStackPanel[buttonIndexInsideCustomButtons].ToolTip = createdTooltip;
        }

        private void OnRenameTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                CommandPromptTextBox.Focus();//This automatically calls the LostFocus event ;)
        }

        public void ConvertTextBoxToPureContent(object sender, RoutedEventArgs e)//e is TextBox
        {
            //Reset RenameTextBox
                RenameTextBox.Width = 0;
                RenameTextBox.Height = 0;

                RenameTextBoxBorder.Visibility = Visibility.Hidden;

            //Convert TextBox to Button.Content, and increase the index
            if (RenameTextBox.Text == "")
                customButtonsInStackPanel[textBoxCustomButtonAssignment].Content = "???";
            else
                customButtonsInStackPanel[textBoxCustomButtonAssignment].Content = RenameTextBox.Text;

            textBoxCustomButtonAssignment++;
        }



        //========================
        //== Keyboard Shortcuts ==
        //========================
            //Tis kinda spagghetti
            //The CtrlCommands on custom buttons should literally run the same code as custom click imo

        /*private void Command_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }*/

        private void Ctrl1Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CommandPromptTextBoxEnter();
        }

        private void Ctrl2Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CommandPromptTextBoxEnter(1);
        }

        private void Ctrl3Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (queryCommandDictionary.Count > 2)
                CommandPromptTextBoxEnter(2);
        }

        private void Ctrl4Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (queryCommandDictionary.Count > 3)
                CommandPromptTextBoxEnter(3);
        }

        private void Ctrl5Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (queryCommandDictionary.Count > 4)
                CommandPromptTextBoxEnter(4);
        }

        //============================
        //== User Setting File Paths==
        //============================

        /*public void OpenDialogueYoutubeDLEXE_Click(object sender, RoutedEventArgs e)
        {
            //Create OpenFileDialog
            OpenFileDialog openFileDialogue = new OpenFileDialog();

            openFileDialogue.Title = "Open the youtube-dl.exe";

            //Set the filter exclusively to .exe
            openFileDialogue.DefaultExt = ".exe";
            openFileDialogue.Filter = "Executables (*.exe)|*.exe";

            //TODO:
            //openFileDialogue.InitialDirectory = //default youtube-dl.exe place, aka this!

            //Launch OpenFileDialog by calling ShowDialog method
            if (openFileDialogue.ShowDialog() == true)
            {
                currentPathToYoutubeDL = openFileDialogue.FileName;

                //Update the UI Path, so user knows where youtube-dl.exe is located!
                YoutubeDLExePathUI.Text = currentPathToYoutubeDL;
            }
                

        }*/

        // 2020 AND MICROSOFT STILL HAS NOT PUT FUCKING SELECTDIRECTORYDIALOGUE
        // FUCKING HELL, BLOWING BILLIONS LEFT AND RIGHT, AND MINOR SHIT LIKE DIALOGUES ARE STILL UNFINISHED!
        // 
        // Thanks to this guy, I got it working, as a "select directory" dialogue LMAO
        // https://stackoverflow.com/questions/1922204/open-directory-dialog
        public void SaveDialogueVideoSaveLocation_Click(object sender, RoutedEventArgs e)
        {
            //Create OpenFileDialog
            SaveFileDialog saveInDirectoryDialogue = new SaveFileDialog();
            StringBuilder finalPath = new StringBuilder();

            saveInDirectoryDialogue.Title = "Select the folder location, where your downloads will be saved";

            saveInDirectoryDialogue.Filter = "Directory|*.this.directory";

            saveInDirectoryDialogue.FileName = "Select";

            //Launch OpenFileDialog by calling ShowDialog method
            if (saveInDirectoryDialogue.ShowDialog() == true)
            {
                finalPath = finalPath.Append(saveInDirectoryDialogue.FileName);

                //Remove fake filename from resulting path
                finalPath = finalPath.Replace("\\Select.this.directory", "");
                finalPath = finalPath.Replace(".this.directory", "");
                //Optimally, you would find the last \\'s index, and remove everything to the right but whatever who cares

                Debug.WriteLine("Current Path To Saved Videos:" + currentPathToSavedVideos);

                currentPathToSavedVideos = finalPath.ToString();

                Debug.WriteLine("Current Path To Saved Videos:" + currentPathToSavedVideos);

                //If user has changed the filename, create the new directory
                if (Directory.Exists(currentPathToSavedVideos) == false)
                    Directory.CreateDirectory(currentPathToSavedVideos);

                //Update the UI Path, so user knows where videos are saved!
                YoutubeDLSavePathUI.Text = currentPathToSavedVideos;

                //Remember where videos are saved
                WriteUserConfig();
            }
                
        }

        public void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo youtubeDLGithubRedirectInfo = new ProcessStartInfo();
            youtubeDLGithubRedirectInfo.UseShellExecute = true;
            youtubeDLGithubRedirectInfo.FileName = "https://github.com/ytdl-org/youtube-dl#options";

            Process.Start(youtubeDLGithubRedirectInfo);
        }

        public void MinimizeConsoleLog_Click(object sender, RoutedEventArgs e)
        {
            if (OutputLogBorder.Height == 100)
            {
                //Minimize!
                OutputLogBorder.BorderBrush = Brushes.Transparent;
                OutputLogBorder.Height = 2;

                OutputLogMinimizeButton.Content = "+";
                ToolTip buttonTooltip = ToolTipService.GetToolTip(OutputLogMinimizeButton) as ToolTip;
                buttonTooltip.Content = "Show the Console Log!\nIt has the output of your actions, in great detail!"; 
            }
            else
            {
                //Maximize!
                OutputLogBorder.BorderBrush = Brushes.LightSlateGray;
                OutputLogBorder.Height = 100;

                OutputLogMinimizeButton.Content = "-";
                ToolTip buttonTooltip = ToolTipService.GetToolTip(OutputLogMinimizeButton) as ToolTip;
                buttonTooltip.Content = "Minimize the Console Log!\nYou are unlikely to need its output anyway!";
            }

            //Should remove the hard-coding of color and height in the future.

            //Remember if user had it minimized or not!
            WriteUserConfig();
        }



        //==============================
        //== YoutubeDL Process Output ==
        //==============================

        public void ErrorDataReceived(object sendingProcess, DataReceivedEventArgs error)
        {
            if (String.IsNullOrEmpty(error.Data) == false)
            {
                //WPF is dogshit, so I have to lightly manage threads as well.
                //All the proper logic must happen below.
                this.Dispatcher.Invoke(() =>
                {
                    //Get what Process this is
                    int currentProcessIndex = GetIndexOfYoutubeDLProcess(sendingProcess);

                    //So it won't spam warning with red
                    if (error.Data.Contains("WARNING"))
                        InsertConsoleLogLine(error.Data, currentProcessIndex);
                    else//red text POG
                        InsertConsoleLogLine(error.Data, currentProcessIndex, true);

                    
                });
            }
        }

        public void DisposedEvent(object sendingprocess, EventArgs error)
        {
            MessageBox.Show("This Process got Disposed, Report this to the developer" + error.ToString());
        }

        public void ExitedEvent(object sendingprocess, EventArgs error)
        {
            //WPF is dogshit, so I have to lightly manage threads as well.
            //All the proper logic must happen below.
            this.Dispatcher.Invoke(() =>
            {
                //Get what Process this is
                int currentProcessIndex = GetIndexOfYoutubeDLProcess(sendingprocess);

                InsertConsoleLogLine("[exit] The Download Process Fully Exited", currentProcessIndex);

                //Clear all processes in here
                //As in, clear the string!
                for (int i = 0; i < downloadGridProcessItems[currentProcessIndex].Count; i++)
                    downloadGridProcessItems[currentProcessIndex][i].FinishClear();

                //Record that this process is **COMPLETE**
                youtubeDLProcessesFinished[currentProcessIndex] = true;
            });

        }

        //This function was shamelessly ripped off here, though it was heavily bugged
        //As in, I didnt have error output'd and stuff, and I got plenty of crashes on normal output
        //I still have to credit it, as I wouldn't know about "OutputHandler" and redirecting stdout, without it.
        //Microsoft's (shitty) documentation helped for the rest, and as always, trial and error -_-
        //https://github.com/detaybey/WrapYoutubeDl/blob/43dbe59d0f8fe968e4c35bacba1d92d5c3f2515e/WrapYoutubeDl/AudioDownloader.cs#L136
        public void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            //This function is called by youtube-dl.exe
            //The process is sendingProcess, so you can identify which one it is, out of the many processes you have
            //the second parameter (outLine) is the output
            //The formating, is cleverly done, so it can be parsed by other programs, like this one <3
            //
            //[download] is download data, which is the most often output
            //^It has variations.
            //[error] is the error, duh


            if (String.IsNullOrEmpty(outLine.Data)) //|| Finished)
                return;

            //Get what Process this is
            int currentProcessIndex = GetIndexOfYoutubeDLProcess(sendingProcess);

            //If it is from this process
            if (youtubeDLProcesses[currentProcessIndex] == sendingProcess)
            {
                //WPF is dogshit, so I have to lightly manage threads as well.
                //All the proper logic must happen below.
                this.Dispatcher.Invoke(() =>
                {
                    //If it is a percent output
                    //Could use Regex, but that is unreadable and I am not experienced enough with it
                    if (outLine.Data.Contains("%") && outLine.Data.Contains("ETA"))
                    {
                        //Replace the previous [download]!

                        //=====================================
                        //==Get percentage for the data grid!==
                        //=====================================

                            //Get the percentage value
                                StringBuilder percentageStringBuilder = new StringBuilder(outLine.Data);

                                int indexOfDownloadClose = percentageStringBuilder.ToString().LastIndexOf(']');

                                percentageStringBuilder.Remove(0, indexOfDownloadClose + 2);//+2 to include ']' and the whitespace!

                                int indexOfPercentSymbol = percentageStringBuilder.ToString().IndexOf('%');

                                percentageStringBuilder.Remove(indexOfPercentSymbol, percentageStringBuilder.Length - indexOfPercentSymbol);//+1 for whitespace
                            
                            //Update progress on the downloadGridItem, so as to show it on the UI!
                                //Find the current download grid item matching this process - its index!
                                int lastDownloadGridInCurrentProcess = downloadGridProcessItems[currentProcessIndex].Count - 1;
                                
                                //Update it to match current progress!
                                downloadGridProcessItems[currentProcessIndex][lastDownloadGridInCurrentProcess].Progress = percentageStringBuilder.ToString();
                                
                                //Determine if it should be finalized&visualized!
                                DetermineFinalisationOfDownloadGridItem(downloadGridProcessItems[currentProcessIndex][lastDownloadGridInCurrentProcess]);

                                //If "100%", visually make it "" aka no value in the UI, so as to not bloat!
                                if (downloadGridProcessItems[currentProcessIndex][lastDownloadGridInCurrentProcess].Progress.Contains("100"))
                                    downloadGridProcessItems[currentProcessIndex][lastDownloadGridInCurrentProcess].Progress = "";

                        //==================================
                        //==Get the ETA for the data grid!==
                        //==================================

                            //Get the ETA value
                                StringBuilder ETAStringBuilder = new StringBuilder(outLine.Data);

                                int indexOfEndETA = ETAStringBuilder.ToString().LastIndexOf("ETA");

                                //indexOfEndETA + 4, often goes out of length/count, so I got to find what is the max!
                                //Iterate and --;
                                int ValuesToAdd = 4;
                                while (indexOfEndETA + ValuesToAdd > ETAStringBuilder.Length)
                                    ValuesToAdd--;

                                ETAStringBuilder.Remove(0, indexOfEndETA + ValuesToAdd);//3 for "ETA" and 1 for whitespace, leading to 4!

                            //Update time remaining on the downloadGridItem, so as to show it on the UI!
                                //Find the current download grid item matching this process - its index!
                                //int lastDownloadGridInCurrentProcess = downloadGridProcessItems[currentProcessIndex].Count;
                                //Already found above!
                                
                                //Update it to match current estimated time!
                                downloadGridProcessItems[currentProcessIndex][lastDownloadGridInCurrentProcess].TimeRemaining = ETAStringBuilder.ToString();
                                
                                //Determine if it should be finalized&visualized!
                                DetermineFinalisationOfDownloadGridItem(downloadGridProcessItems[currentProcessIndex][lastDownloadGridInCurrentProcess]);

                                //If "100%", visually make it "" aka no value in the UI, so as to not bloat!
                                if (downloadGridProcessItems[currentProcessIndex][lastDownloadGridInCurrentProcess].TimeRemaining.Contains("00:00"))
                                    downloadGridProcessItems[currentProcessIndex][lastDownloadGridInCurrentProcess].TimeRemaining = "";

                        //===================================
                        //==Get the Size for the data grid!==
                        //===================================
                            //Get the Size value
                                StringBuilder SizeStringBuilder = new StringBuilder(outLine.Data);

                                int indexOfEndSize = SizeStringBuilder.ToString().LastIndexOf("iB ");

                                //MessageBox.Show("indexOfEndSize is: " + indexOfEndSize);

                                //Remove from the iB to the very end
                                SizeStringBuilder.Remove(indexOfEndSize - 1, SizeStringBuilder.Length - indexOfEndSize - 1);//-1 to remove the K/B (out of Kib/Mib)

                                //MessageBox.Show("SizeStringBuilder is: " + SizeStringBuilder.ToString());

                                int indexOfEndOf = SizeStringBuilder.ToString().LastIndexOf(" of ");

                                //MessageBox.Show("indexOfEndOf is: " + indexOfEndOf);

                                //Remove from the start to the end of " of "
                                SizeStringBuilder.Remove(0, indexOfEndOf + 3);//3 is for " of", could be 4 now that I think about it

                                //MessageBox.Show("SizeStringBuilder is: " + SizeStringBuilder.ToString());

                                //If Kilobytes, convert it to Megabytes
                                if (outLine.Data.Contains("Kib at"))
                                {
                                    float sizeAtKilobytes = float.Parse(SizeStringBuilder.ToString());
                                    sizeAtKilobytes = sizeAtKilobytes / 1000;    

                                    SizeStringBuilder.Clear();
                                    SizeStringBuilder.Append(sizeAtKilobytes.ToString());
                                }

                                //Debug.WriteLine("SizeStringBuilder is: " + SizeStringBuilder.ToString());

                                //Delete the decimal point
                                if (SizeStringBuilder.ToString().Contains("."))
                                {
                                    int indexOfDecimalPoint = SizeStringBuilder.ToString().IndexOf('.');

                                    SizeStringBuilder.Remove(indexOfDecimalPoint, SizeStringBuilder.Length - indexOfDecimalPoint);
                                }

                                //Debug.WriteLine("SizeStringBuilder is: " + SizeStringBuilder.ToString());

                            //Update Size on the downloadGridItem, so as to show it on the UI!
                                //Find the current download grid item matching this process - its index!
                                //int lastDownloadGridInCurrentProcess = downloadGridProcessItems[currentProcessIndex].Count;
                                //Already found above!
                                
                                //Update it to match current assumed size!
                                downloadGridProcessItems[currentProcessIndex][lastDownloadGridInCurrentProcess].Size = SizeStringBuilder.ToString();
                                
                                //Determine if it should be finalized&visualized!
                                DetermineFinalisationOfDownloadGridItem(downloadGridProcessItems[currentProcessIndex][lastDownloadGridInCurrentProcess]);

                                //Debug.WriteLine("Size " + downloadGridProcessItems[currentProcessIndex][lastDownloadGridInCurrentProcess].Size);


                        //=============================================
                        //==Get the Download Speed for the data grid!==
                        //=============================================
                            //Confirm there is a speed (can be "Unknown Speed")
                            if (outLine.Data.Contains("iB/s "))
                            {
                                //Get the Download Speed value
                                    StringBuilder DownloadSpeedStringBuilder = new StringBuilder(outLine.Data);

                                    int indexOfEndSpeed = DownloadSpeedStringBuilder.ToString().LastIndexOf("iB/s ");

                                    //MessageBox.Show("indexOfEndSize is: " + indexOfEndSpeed);

                                    //Remove from the iB/s to the very end
                                    DownloadSpeedStringBuilder.Remove(indexOfEndSpeed - 1, DownloadSpeedStringBuilder.Length - indexOfEndSpeed - 1);//-1 to remove the K/B (out of Kib/Mib)

                                    //MessageBox.Show("DownloadSpeedStringBuilder is: " + DownloadSpeedStringBuilder.ToString());

                                    int indexOfEndAt = DownloadSpeedStringBuilder.ToString().LastIndexOf(" at ");

                                    //MessageBox.Show("indexOfEndAt is: " + indexOfEndAt);

                                    //Remove from the start to the end of " of "
                                    DownloadSpeedStringBuilder.Remove(0, indexOfEndAt + 4);//4 is for " at "

                                    //MessageBox.Show("DownloadSpeedStringBuilder is: " + DownloadSpeedStringBuilder.ToString());

                                    //If Kilobytes, convert it to Megabytes
                                    if (outLine.Data.Contains("Kib/s "))
                                    {
                                        float sizeAtKilobytes = float.Parse(DownloadSpeedStringBuilder.ToString());
                                        sizeAtKilobytes = sizeAtKilobytes / 1000;    

                                        DownloadSpeedStringBuilder.Clear();
                                        DownloadSpeedStringBuilder.Append(sizeAtKilobytes.ToString());
                                    }

                                    //Debug.WriteLine("DownloadSpeedStringBuilder is: " + DownloadSpeedStringBuilder.ToString());

                                //Update Size on the downloadGridItem, so as to show it on the UI!
                                    //Find the current download grid item matching this process - its index!
                                    //int lastDownloadGridInCurrentProcess = downloadGridProcessItems[currentProcessIndex].Count;
                                    //Already found above!
                                
                                    //Update it to match current assumed size!
                                    downloadGridProcessItems[currentProcessIndex][lastDownloadGridInCurrentProcess].DownloadSpeed = DownloadSpeedStringBuilder.ToString();

                                    //TODO: If finished (by process exit), clear this on UI (unlike how its done on progress), and put command log text: [Complete] These videos are succesfully downloaded!
                            }

                            
                    }

                    //If it is the first download, showing the title!
                    if (outLine.Data.Contains("[download] Destination:"))
                    {
                        //Example of formatting:
                            //[download] Destination: C:\YoutubeDL\STAR.f303.webm

                        //Get the path, it is literally the thing to the next of the path!
                        StringBuilder titleString = new StringBuilder();

                        //Find where is the last path location, aka final folder, since title is right after ;)
                        int pathIndexEnd = outLine.Data.LastIndexOf("\\");

                        //titleString should be the outline.Data, starting from pathIndexEnd
                        titleString.Append(outLine.Data);
                        titleString.Remove(0, pathIndexEnd + 1);//+1 so it won't have one \ or sth in the title!

                        //And now we have the title, and the .fwhatever.format
                        //So, I remove the last 2 '.' characters, from the end, if no format is set (so merge will happen)
                        //Otherwise just remove one '.' character
                        if (titleString.ToString().Contains('.'))
                        {
                            pathIndexEnd = titleString.ToString().LastIndexOf('.');
                            titleString.Remove(pathIndexEnd, titleString.Length - pathIndexEnd);
                        }


                        //If contains set format, e.g. mp3 or mp4, it should have only one '.', instead of 2!
                        if (youtubeDLProcessesCustomQueryCommand[currentProcessIndex].Contains("--format ") == false && youtubeDLProcessesCustomQueryCommand[currentProcessIndex].Contains("-f ") == false && youtubeDLProcessesCustomQueryCommand[currentProcessIndex].Contains("--audio-format ") == false)
                        {
                            //For example: [title].f324.mkv -> With 2 remove '.', it becomes [title]
                            if (titleString.ToString().Contains('.'))
                            {
                                pathIndexEnd = titleString.ToString().LastIndexOf('.');
                                titleString.Remove(pathIndexEnd, titleString.Length - pathIndexEnd);
                            }
                        }

                        

                        Debug.WriteLine(titleString.ToString());

                        //Final Title, should now be titleString!
                        //Give it to the to-add-grid item!
                            //Find the current download grid item matching this process - its index!
                            int lastDownloadGridInCurrentProcess = downloadGridProcessItems[currentProcessIndex].Count - 1;

                            //MessageBox.Show("Download grid last " + lastDownloadGridInCurrentProcess);

                            //Update it to match title!
                            downloadGridProcessItems[currentProcessIndex][lastDownloadGridInCurrentProcess].Title = titleString.ToString();

                            //Determine if it should be finalized&visualized!
                            DetermineFinalisationOfDownloadGridItem(downloadGridProcessItems[currentProcessIndex][lastDownloadGridInCurrentProcess]);

                            //Debug
                            //Debug.WriteLine("Progress % " + downloadGridProcessItems[currentProcessIndex][lastDownloadGridInCurrentProcess].Title);

                    }

                    //If playlist/channel download check
                    if (youtubeDLProcessesIsSingleVideo[currentProcessIndex] == false && outLine.Data.Contains("Downloading video ") && outLine.Data.Contains(" of "))
                    {
                        //If it gets here, this runs before anything else (title/whatever), so, it would be ideal to create the downloadGridItem here...
                        //Ah well.

                        //===========================
                        //==Get current video index==
                        //===========================
                        StringBuilder currentVideoIndexStringBuilder = new StringBuilder(outLine.Data);
                        int ofEndIndex = currentVideoIndexStringBuilder.ToString().IndexOf(" of ");

                        currentVideoIndexStringBuilder.Remove(ofEndIndex, currentVideoIndexStringBuilder.Length - ofEndIndex);

                        int videoWordIndex = currentVideoIndexStringBuilder.ToString().LastIndexOf("video ");

                        currentVideoIndexStringBuilder.Remove(0, videoWordIndex + 6);

                        int newVideoIndex = int.Parse(currentVideoIndexStringBuilder.ToString());

                        //===========
                        //==Compare==
                        //===========

                        //If downloading the same video as latest (e.g. first)
                        if (newVideoIndex != youtubeDLProcessesCurrentVideoIndex[currentProcessIndex])//else if different video
                            CreateDownloadGridItem(currentProcessIndex);

                        youtubeDLProcessesCurrentVideoIndex[currentProcessIndex] = newVideoIndex;
                    }

                    InsertConsoleLogLine(outLine.Data, currentProcessIndex);

                    /*
                    // fire the process event
                    var perc = Convert.ToDecimal(Regex.Match(outLine.Data, @"\b\d+([\.,]\d+)?").Value, System.Globalization.CultureInfo.InvariantCulture);
                    if (perc > 100 || perc < 0)
                    {
                        Console.WriteLine("weird perc {0}", perc);
                        return;
                    }
                    this.Percentage = perc;
                    this.OnProgress(new ProgressEventArgs() { ProcessObject = this.ProcessObject, Percentage = perc });

                    // is it finished?
                    if (perc < 100)
                    {
                        return;
                    }

                    if (perc == 100 && !Finished)
                    {
                        OnDownloadFinished(new DownloadEventArgs() { ProcessObject = this.ProcessObject });
                    }
                    */
                });

            }


            
        }

        /// <summary>
        /// Gets the index of current process, among the youtubeDLProcesses[]
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public int GetIndexOfYoutubeDLProcess(object process)
        {
            for (int i = 0; i < youtubeDLProcesses.Count; i++)
            {
                if (youtubeDLProcesses[i] == process)
                    return i;
            }

            return -1;
        }

        public void InsertConsoleLogLine(string lineContent, int processLogIndex, bool overrideWithRedColor = false)
        {
            //Put the proper content on the consoleLog (linebreak + run), and cache it, so we add it on our lists!
            OutputLogLineBreakRunHolder createdLineLogContainer;

            //=======================================================
            //For error, we are not coloring by process coloring.
            if (overrideWithRedColor)
                createdLineLogContainer = InsertConsoleLogLine(lineContent, Color.FromRgb(255, 0, 0));

            //No color available for 5+ processes (because the console log would look cancerous, so we just gray it)
            else if (GetAmountOfRunningYoutubeDLProcesses() >= colorProcessLogs.Length)
                createdLineLogContainer = InsertConsoleLogLine(lineContent, Color.FromRgb(170, 170, 170));

            //Else, the normal coloring by matching the process index with color index
            else
            {
                DetermineNewDictionaryKey(youtubeDLProcesses[processLogIndex]);

                //Get the latest color ;)
                Color colorMatchingThisProcess = colorProcessLogs[processToColorDictionary[youtubeDLProcesses[processLogIndex] ] ];

                createdLineLogContainer = InsertConsoleLogLine(lineContent, colorMatchingThisProcess);
            }

            //=======================================================

            //Now, we've got to add it on the list
            //But first check if a nested list is even created!
            //Otherwise argument/index overflow!
            while (consoleLogProcessOutputSuperList.Count <= processLogIndex)
                consoleLogProcessOutputSuperList.Add(new List<OutputLogLineBreakRunHolder>());

            consoleLogProcessOutputSuperList[processLogIndex].Add(createdLineLogContainer);

            //If it is % ETA, remove the previous one!
            if (createdLineLogContainer.isPercentETA)
            {
                int lastIndex = consoleLogProcessOutputSuperList[processLogIndex].Count;

                //If previous is indeed % ETA as well, REMOVE IT!
                if (consoleLogProcessOutputSuperList[processLogIndex][lastIndex - 2].isPercentETA)
                {
                    OutputLog.Inlines.Remove(consoleLogProcessOutputSuperList[processLogIndex][lastIndex - 2].runElement);
                    OutputLog.Inlines.Remove(consoleLogProcessOutputSuperList[processLogIndex][lastIndex - 2].lineBreakElement);
                }
                    
            }
        }

        /// <summary>
        /// Add \n, then put text right below, and put it on the color of the process!
        /// </summary>
        /// <param name="lineContent"></param>
        /// <param name="colorToAdd"></param>
        public OutputLogLineBreakRunHolder InsertConsoleLogLine(string lineContent, Color colorToAdd)
        {
            //Output it onto the console log, with the following format:
            //  Different color for each different process
            //  Newline/LineBreak (\n) added
            
            //Create the linebreak and the run (and its content, aka string and color)
                LineBreak createdLineBreak = new LineBreak();
                Run createdRun = new Run(lineContent);
                if (colorToAdd != null)//Too many processes -_-
                    createdRun.Foreground = new SolidColorBrush(colorToAdd);

            //Add them on UI properly
                OutputLog.Inlines.Add(createdLineBreak);
                OutputLog.Inlines.Add(createdRun);

            //Cache them so we can use them later ;)
                OutputLogLineBreakRunHolder createdLineLogContainer = new OutputLogLineBreakRunHolder();
                createdLineLogContainer.lineBreakElement = createdLineBreak;
                createdLineLogContainer.runElement = createdRun;

                //Check if it is [download] % ETA blablabla spam
                if (lineContent.Contains("%") && lineContent.Contains("ETA"))
                    createdLineLogContainer.isPercentETA = true;
                else
                    createdLineLogContainer.isPercentETA = false;

                return createdLineLogContainer;
        }

        public int GetAmountOfRunningYoutubeDLProcesses()
        {
            int amountOfRunningYoutubeDLProcesses = 0;

            //Iterate all processes which have ran
            for (int i = 0; i < youtubeDLProcessIndex; i++)
            {
                //if a process has not finished (and exists because youtubeDLProcessIndex wouldn't reach there)
                //add it to the return value
                if (youtubeDLProcessesFinished[i] == false)
                    amountOfRunningYoutubeDLProcesses++;
            }

            return amountOfRunningYoutubeDLProcesses;

        }

        /// <summary>
        /// The difference with non-active index, is it counts its index among the ACTIVE processes
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public int GetIndexOfActiveYoutubeDLProcess(object process)
        {
            //First, we make a list containing EXCLUSIVELY unfinished youtubeDLProcesses 
            List<object> activeProcessList = new List<object>();

            for (int i = 0; i < youtubeDLProcesses.Count; i++)
            {
                if (youtubeDLProcessesFinished[i] == false)
                    activeProcessList.Add(youtubeDLProcesses[i]);
            }

            //Now, we iterate among this list to find our process!
            for (int i = 0; i < activeProcessList.Count; i++)
            {
                    if (activeProcessList[i] == process)
                        return i;
            }

            return -1;
        }

        /// <summary>
        /// When the window comes on the foreground again
        /// From minimize, alt-tab, or whatever
        /// it should check the clipboard!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void WindowStateChanged(object sender, EventArgs e)
        {
            DetermineClipboard();
        }

        //Invoked when the Data Grid appears
        //Because the Data Grid may be bigger than the window itself!
        public void DetermineWindowResizing()
        {
            //It is legit sad I do not know how to get dataGrid width (it is 0, same for border)
            if (this.Width < 1000)
                this.Width = 1000;

            if (this.Height < 500)
                this.Height = 500;
        }

        /// <summary>
        /// Used for buttons, 
        /// </summary>
        public void ReadUserConfig()
        {

            //If config exists
            if (File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + "\\userConfig.txt"))
            {
                userConfigString = File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\userConfig.txt");

                ParseUserConfigs();
            }

            totalOpenTimes++;
                
            
        }

        public void ParseUserConfigs()
        {
            //Format:
            //  [pathToSave]
            //  [timesOpened]
            //  [hadMinimizedOutputLogLastTime]
            //  {
            //  [customQuery1]
            //  [customQuery2]
            //  }
            //  
            //

            //Get first line (which is path to save
                int lineEnd = userConfigString.IndexOf("\n");
                //Get the path
                currentPathToSavedVideos = userConfigString.Substring(0, lineEnd);
                //Update the UI Path, so user knows where videos are saved!
                YoutubeDLSavePathUI.Text = currentPathToSavedVideos;

            //Cut it away from userConfigString
            userConfigString = userConfigString.Substring(lineEnd + 1, userConfigString.Length - lineEnd - 1);

            Debug.WriteLine("CurrentPath:" + currentPathToSavedVideos);

            lineEnd = userConfigString.IndexOf("\n");
            totalOpenTimes = int.Parse(userConfigString.Substring(0, lineEnd));

            //Do not show again the shitty label!
            if (totalOpenTimes > 10)
            {
                IntroDownloadLabel.Content = "";
                DownloadLabel.Content = "";
            }
                
            //Remember if user had minimized output log last time!
            if (userConfigString.Contains("True"))
                MinimizeConsoleLog_Click(null, null);

            int indexOfQueryStart = userConfigString.IndexOf('{');

            //If custom query commands exist!
            if (indexOfQueryStart != -1)
            {
                //Remove the download label
                DownloadLabel.Content = "";

                //Get the remaining query string, from { to }
                string totalQueryString = userConfigString.Substring(indexOfQueryStart + 2, userConfigString.Length - indexOfQueryStart - 2);

                while (true)
                {
                    //Find if \n is later than } which signifies the end
                    int indexOfCurrentQueryEnd = totalQueryString.IndexOf("\n");
                    int indexOfFileEnd = totalQueryString.IndexOf("}");

                    Debug.WriteLine(indexOfCurrentQueryEnd + " " + indexOfFileEnd);
                    Debug.WriteLine(totalQueryString);

                    //Break/gtfo
                    if (indexOfCurrentQueryEnd > indexOfFileEnd)
                        return;

                    //Got the query
                    string customQueryCommand = totalQueryString.Substring(0, indexOfCurrentQueryEnd);

                    //If the query is valid, put it in the button!
                    if (customQueryCommand.Contains('-'))
                        LoadButtonInCustomButtonPanel(customQueryCommand);
                        

                    //Reduce the total string which we read
                    totalQueryString = totalQueryString.Substring(indexOfCurrentQueryEnd + 1, totalQueryString.Length - indexOfCurrentQueryEnd - 1);

                    Debug.WriteLine(customQueryCommand);
                }
                
            }
            
        }

        public void LoadButtonInCustomButtonPanel(string queryCommandString)
        {
            queryCommandDictionary.Add(queryCommandDictionary.Count, queryCommandString);

            //If latest button is not contained, add it!
            if (customButtonsInStackPanel.Contains(customButtonsInStackPanel[customButtonsInStackPanel.Count - 1]) == false)
                customButtonsInStackPanel.Add(customButtonsInStackPanel[customButtonsInStackPanel.Count - 1]);

            //Change its name, to the argument
            customButtonsInStackPanel[customButtonsInStackPanel.Count - 1].Content = queryCommandString;

            //Tooltip
                //Create a tooltip
                ToolTip createdTooltip = new ToolTip();

                //Write the query onto the tooltip
                createdTooltip.Content = queryCommandString;

                //Make the above tooltip its child
                customButtonsInStackPanel[customButtonsInStackPanel.Count - 1].ToolTip = createdTooltip;

            //Create new Button and add it on custom buttons
            customButtonsInStackPanel.Add(CreateButtonInCustomButtonPanel());

            //So renaming won't bug -_-
            textBoxCustomButtonAssignment++;
        }

        public void WriteUserConfig()
        {
            //Format:
            //  [pathToSave]
            //  [timesOpened]
            //  [hadMinimizedOutputLogLastTime]
            //  {
            //  [customQuery1]
            //  [customQuery2]
            //  }
            //  
            //

            string contents = currentPathToSavedVideos + "\n" + totalOpenTimes + "\n" + IsOutputLogMinimized().ToString() + "\n";

            //If there are query commands!
            if (queryCommandDictionary.Count != 2)
            {
                //Start it
                contents = contents + "{" + "\n";

                //Put the query commands
                for (int i = 2; i < queryCommandDictionary.Count; i++)
                    contents = contents + queryCommandDictionary[i] + "\n";

                //Finish it
                contents = contents + "}" + "\n";
            }

            File.WriteAllText(System.AppDomain.CurrentDomain.BaseDirectory + "\\userConfig.txt", contents);
        }

        public bool IsOutputLogMinimized()
        {
            if (OutputLogBorder.Height == 2)
                return true;
            else
                return false;
        }


    }
}
