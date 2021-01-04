# YoutubeDL-PowerGUI
With this program, you can download any video, music, playlist, channel, out of any site!
All accessible to the most casual of users (ol' grandma), while at the same time, power users who want to have the job done within 3 button presses

## Downloads
  ### Windows
  Just unzip it, annoying installation bloat doesn't exist here
	I heavily suggest looking into #optimal-setup.
	So you can press F9, to start it from anywhere!
  
  ### Linux
  Linux Chads build pipes, they don't use GUI (kappa?)
  
## Why not use an online converter?
  - There is not a single online converter which can download entire playlists or channels.
  - They also are slower, as they download the video on their servers, then you download it from their servers. What happens here is you download it straight to your computer, the middle man is cut, and you won't have to wait **even a second** anymore!  
  No waiting for the download button -_-
  - You have to open your browser and trade advertisements or cookies, to download any video...
  - Accept terms of uses you never read
  - And hope the site has no viruses
  
## What is a PowerGUI
Graphical User Interfaces (GUI) were made for casual users. This means making a program accessible, but losing functionality (which very few users need so it's widely acceptable)
A PowerGUI is a GUI which has not lost any feature, any depth. Why? Because the advanced features are for advanced features, and these features exist to make their usage more efficient. In short, an advanced user **spends less time** with the program, to do the same task a casual user would.

You, as a user, want to get your job done, the program is merely your tool to getting it done. Pressing keys, or clicking buttons, the goal is finishing a task. No one interacts with a program for interaction's sake, and so, the less time you spend with it, the better. Power users spend the less possible time to finish their task, however small.

This program, is accessible, but being a PowerGUI, over time of usage will make you a power user, organically.
Here is an example of an advanced usage, coming up organically:
- Let's say on the 12th playlist you download, one video is removed from youtube. All other "competitors" will crash there, maybe one will skip the video.
If you search other software how to ignore removed videos, **there is literally no solution** - what are you going to do, touch the code? lmao
Here? You search https://github.com/ytdl-org/youtube-dl#options and just write
`-i` or `--ignore-errors` and then put your playlist.
- Let's say you want to download a channel, from the date 31/12/20 and before. How are you going to do that? **There is literally no solution** on other programs, meant for casual usage. No advanced user is born overnight, such needs come up organically. So, you search https://github.com/ytdl-org/youtube-dl#options and just write  `--datebefore 31/12/20`

Look at the CHOICES you are given! https://github.com/ytdl-org/youtube-dl#options
You may not use them, but you CAN use them when you need them.

### Customization
And let's say you want to download the thumbnail of every video you download, maybe it has a cool meme?
`--write-all-thumbnails [YourVideoURL]` and press that mysterious Custom Button, and name it "Thumbnails"

Now, every video you input, once you press that button, automatically downloads thumbnails atop the video!
  
### Keyboard Usage
Hotkeys - direct keyboard usage - makes your usage faster, and hence, finishing faster your task.
Download Video/Music/Custom1/Custom2/Custom3/Custom4 are mapped on Ctrl+1/2/3/4/5
You can also use tab, but honestly who uses that anymore? Mouses exist, but anyway, for accessibility, tab usage is complete as well, to ensure full usage of the software by exclusively using a keyboard, just like its fully usable by exclusively using a mouse.

Ultimately, the goal of this program, is to get users to finish their tasks, on the less possible time. To become power users, without **ever** reading documentation or experimenting around. Because in the end of the day, a tool usable by all is good, but improving based on the user's powers and requirements - **that** is the ultimate tool.
  
## Why not use other youtube video downloaders? They look prettier!
  - Other video downloaders purely download onto .mp4 and .mp3, and are made exclusively for casual use.
  - A casual user, if he wants **anything other than download a .mp4/.mp3** he will have no power to finish his task. E.g. download only videos above 1000 views. Good luck doing that on other youtube video downloaders!!
  - Clipboard is not automatically copied, meaning for each video, you have to:
  1. click the textbox
  2. right-click
  3. paste
  4. press Enter/click Download
  Compared to here, where it's automatically done for you. All you have to do is press Enter/click Download.  
  Saving time from a few buttons may seem negligible to you, but it ramps up the more you use it. Accessibility always wins in the end - the less tasks you have to do, the better for everyone. No more frustration.
  
**Ultimately, This is made for both casual users and power users.**

### Actual Competitors
  
  - YoutubeDLGUI https://github.com/MrS0m30n3/youtube-dl-gui
  	- UI looks horrible
	- There is no console log! If there is an error or warning, good luck finding it
	- You cannot input any options, like downloading from a channel, only videos after X(1000) views, or subtitles.
	- The download formats are predefined
	- No clipboard copy-paste saving you time
- https://vidd.ly/
	This is the peak example of what the PowerGUI was born for.
	Given that program's functionality, a user will NEVER use any advanced features, because he CANNOT use any advanced features.
	If he wants **any** other option than the default:
	- e.g. Download with youtube's subtitles in user's language and thumbnails
	- e.g. Download only videos of a playlist/channel over/under X views
	- e.g. Download only videos of a playlist/channel after/before X date, and then convert them to .mp3 or .wav
	- e.g. Download a channel from earliest to latest
	- e.g. Download only videos of a playlist/channel of size X
	- e.g. blablablablaaa, you can imagine how many different requirements each user has
	**HE CANNOT DO THE ABOVE**
	He will never achieve the most optimal and effortless usage of the simplest task he wants! Either give up, or go beg the developers lmao  
	
	Because surely, in the future that program may get the accessible clipboard feature, so you don't have to paste once in your life again, but it will never give you SO MANY CHOICES (when you need them) https://github.com/ytdl-org/youtube-dl#options

  
### On an entirely different level!
Honestly, the fact you can press 3 buttons
`ctrl+C (video URL) -> F9 (shortcut boot) -> Enter`
to download any video - finishing your task instantly by the time you press them - is something other "competitors" will **never** touch, and their users will be troubled by clicking buttons all day and exploring their directory instead lmao  
And even if they really reach this level of efficiency, they will **never** have as many choices https://github.com/ytdl-org/youtube-dl#options as this program.     They will be pretty but they will **never** become a PowerGUI. The opposite, however, is possible.
  
## Credits
I have to thank https://github.com/ytdl-org/youtube-dl and the developers which made youtube-dl, because it is the "backbone" of this software, hence it is packaged together.
That program as the command line (kinda the black text line you type on), and this program as the interface **everyone** can use!

This program is under GPL v3 license, made by me, Dimitrios Malandris, out of frustration on how time-consuming it was to download a single video (either install bloat software, or browse the directory and manually put arguments onto youtube-dl.exe)
I hope it is as useful to you, as it is to me :)

## Personal Disclaimer
This program is what you should expect from an 1.0 release. Functional, without a single crashing bug, while also covering all its users' needs (powerusers, mouse users, keyboard users, casual users). No more core functionality will ever be added, as there is nothing to add. No updates will be released, breaking stuff, as all the core functionality is **COMPLETE** (Kinda sad I have to stress this out, the state of software quality nowadays is below garbage)

I could be hit by a train, and it should function perfectly even 5 years from now (as long youtube-dl.exe works)
			
However, I plan to maintain it, and add misc features, like Localisation!
And perhaps make it a bit prettier.
  
## Why Download Videos?
Because videos often get taken down.
Youtube's servers cost money, and videos which youtube or other entities do not like, get taken down.
There is no better backup, than your own USBs/Hard Drives, instead of foreign corporate servers.
This is not an application for journalists and the like. I started downloading videos myself, when pretty much all video game music over more than 20 game titles was taken down en masse, by a certain company. (yes, even freaking video game music is not safe from censorship)
Suddenly, my background music playlist was empty. More than 500 songs I was daily listening to, were gone, and I couldn't do anything.
Given censorship is only rising, you never know when your favourite channel gets sniped, all of the good videos gone, because of a single video posted and instabanned. Even private videos get sniped.
tl;dr: download them so you can see them whenever you see fit, not when other corporations judge if you are approved to see them.
Also you can see them offline or later, without lagspikes, when travelling or in other boring situations.

## Download Sites Supported
https://ytdl-org.github.io/youtube-dl/supportedsites.html
