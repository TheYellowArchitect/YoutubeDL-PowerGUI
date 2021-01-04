# YoutubeDL-PowerGUI
With this program, you can download any video, music, playlist, channel, out of any site!  
All accessible to the most casual of users (ol' grandma), while at the same time, power users who want to have the job done with 3 button presses!

## Downloads
  ### Windows
  Just unzip it!  
  No annoying installation bloat!
  Look into #optimal-setup, so you can press F9, and start it from anywhere!
  
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
Graphical User Interfaces (GUI) were made for casual users. This means making a program accessible, but losing some functionality (which very few users need so it's widely acceptable)
A PowerGUI is a GUI which has not lost any feature, any depth, which also does not annoy or frustrate the casual user with its advanced features. Why? Because the advanced features are for advanced users, and these features exist to make their usage more efficient. In short, an advanced user **spends less time** with the program, to do the same task a casual user would.

You, as a user, want to get your job done, the program is merely your tool to getting it done. Pressing keys, or clicking buttons, the goal is finishing a task. No one interacts with a program for interaction's sake, and so, the faster you are done with it, the better. Power users spend the less possible time to finish their task, however small.

This program, is accessible to everyone, accessible enough to be used exclusively by mouse without frustration!  
But being a PowerGUI, over time of usage will make you a power user, organically.  
#### Here are but a few examples of advanced requirements, coming up organically:  
- Let's say on the playlist you download, one video in the playlist is taken down by youtube and is inaccessible/deleted. All other video downloaders will crash there, maybe one will skip the video.  
If you search how to ignore removed videos, on other video downloaders, **there is literally no solution** - what are you going to do, touch the code? lmao!    
Here? You [choose what you want](https://github.com/ytdl-org/youtube-dl#options) and just write
`-i` or `--ignore-errors` and then copy-paste your playlist.
- Let's say you want to download a channel, from the date 31/12/20 and before.  
How are you going to do that? **There is literally no solution** on other video downloaders  
Here? You [choose what you want](https://github.com/ytdl-org/youtube-dl#options) and just write  `--datebefore 31/12/20` and then copy-paste your playlist
- Let's say you want to download a channel, with your language's subtitles, and only videos under 500 MB (saving your hard drive space)  
Obviously as you guessed, on other video downloaders, **There is literally no solution**  
Here, you [choose what you want](https://github.com/ytdl-org/youtube-dl#options) and just write
`--write-auto-sub --max-filesize 500m` and then copy-paste your playlist/channel

Look at the [CHOICES](https://github.com/ytdl-org/youtube-dl#options) you are given! 
You may not use them, you are never bothered with them, **but** you CAN use them when you need them.

### Customization
And let's say you want to download the thumbnail of every video you download, maybe it has a cool meme?
`--write-all-thumbnails` with your URL after, and press that mysterious Custom Button, and name it "Thumbnails"

Now, every video you input, once you press that button, automatically downloads thumbnails atop the video!  
Any option you input (see examples above) do not have to be written time and time again e.g. `--write-all-thumbnails` for every video, the goal of this program is to save time!
For example, you could have a button to always download the videos you put, with embedded subtitles, instead of writing the subtitles options every time!

### Hotkeys
(For advanced users)  
Download Video/Music/Custom1/Custom2/Custom3/Custom4 are mapped on Ctrl+1/2/3/4/5  
You can also use tab, but honestly who uses that anymore? Mouses exist, but anyway, for accessibility, tab usage is complete as well, to ensure full usage of the software by exclusively using a keyboard, just like its fully usable by exclusively using a mouse.

Ultimately, the goal of this program, is to get users to finish their tasks, on the less possible time. To become power users, without **ever** reading documentation or experimenting around. No advanced user is born overnight, such needs come up organically. And to have power users, casual users must be satisfied, to evolve organically. Because in the end of the day, a tool usable by all is good, but improving based on the user's powers and requirements - **that** is the ultimate tool.
  
## Why not use other youtube video downloaders? They look prettier!
  - Other video downloaders purely download onto .mp4 and .mp3, and are made exclusively for casual use.
  - A casual user, if he wants **anything other than download a .mp4/.mp3** he will have no power to finish his task.  
  For example, download only videos above 1000 views and/or with embedded youtube-subtitles. Good luck doing that on other youtube video downloaders
  - **"I don't care about that, I just want to download videos"** -> The clipboard is automatically copy-pasted unlike in any other video downloader!
  Take a look at the process to copy-paste a video onto any other downloader:
  1. left-click the textbox
  2. right-click
  3. paste
  4. press Enter/click Download
  
  **Compared to here, where clicking&pasting is automatically done for you**. All you have to do is press Enter/click Download.  
  Saving time from a few buttons may seem negligible to you, but it ramps up the more you use it. Accessibility always wins in the end - the less tasks you have to do, the better for everyone. Ultra casual users (grandma) won't struggle to copy something and wonder if she really copied, and everyone else who can do this reliably, don't have to ever bother doing this task. Copy-Paste-Enter is the entire process to download a video in all downloaders/converters, and this is the only program which utterly removes the -paste- part!  
  
**Ultimately, this program is made for both casual users and power users.**
This program has all the features a casual user would need, alongside all features a power user would need, and atop of that, makes sure the transition of a casual user to a power user can happen without any manual reading or other boring tasks!

### Video Downloader Competitors
  
  - [YoutubeDLGUI](https://github.com/MrS0m30n3/youtube-dl-gui)
  	- UI looks horrible
	- There is no console log! If there is an error or warning, good luck knowing what's wrong
	- You cannot input any options, like downloading from a channel only videos after X(1000) views, or subtitles.
	- The download formats are predefined
	- No clipboard copy-paste saving you time!
- [Viddly](https://vidd.ly/)
	- It is "free". It has **ADVERTISEMENTS** and blackmails you to a paid subscription to remove them? lmao wtf, escaping youtube advertisements to get viddly's!
	- Its code is **not open-source**, which means **it could easily hide a spyware/malware** and you have no way to prevent that or even know if it really has that!
	- You have to go through an entire installer!
	- Its download formats are predefined
	- No clipboard copy-paste saving you time
	
	Let's magically ignore the above, and assume, it was fully free, like this program, was open-source, and didn't have an installation process to work.
	This is the peak example of what the PowerGUI was born to rival - a polished casual program.
	Given that program's functionality, a user will **NEVER** use any advanced features, because he **CANNOT** use any advanced features.
	If he wants **any** other option than the default:
	- Download with youtube's subtitles in user's language and thumbnails
	- Download only videos of a playlist/channel over/under X views
	- Download only videos of a playlist/channel after/before X date, and then convert them to .mp3 or .wav
	- Download a channel from earliest to latest
	- Download only videos of a playlist/channel of size X
	- blablablablaaa, you can imagine how many different requirements each user has
	**HE CANNOT DO ANY OF THE ABOVE**
	He will never achieve the most optimal and effortless usage of the simplest task he wants! Either give up, or go beg the developers lmao  
	
	Because surely, in the future, a free clone of that program may get the accessible clipboard feature, so you don't have to paste once in your life again, but no program will never give you [SO MANY CHOICES](https://github.com/ytdl-org/youtube-dl#options) (when you need them)
	

  
### On an entirely different level!
Honestly, the fact you can press 3 buttons
`ctrl+C (video URL) -> F9 (shortcut boot) -> Enter`
to download any video - finishing your task instantly by the time you press them - is something other "competitors" will **never** touch, and their users will be troubled by clicking buttons all day and exploring their directory instead lmao 
And even if they really reach this level of efficiency, they will **never** have [as many choices](https://github.com/ytdl-org/youtube-dl#options) as this program.     They will be pretty but they will **never** become a PowerGUI. The opposite, however, is possible.
  
## Credits
I have to thank the developers who made [youtube-dl](https://github.com/ytdl-org/youtube-dl), because it is the "backbone" of this software, hence it is packaged together.
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
