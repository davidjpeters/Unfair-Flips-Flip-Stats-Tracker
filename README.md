#### Flip Stats Tracker
A simple mod that keeps track of how many heads you've gotten vs. how many you should have gotten, for better or for worse

Keeps track of:
- Total Flips
- Actual Heads - How many heads you've actually flipped
- Expected Heads - How many heads you should have flipped
- Luck Factor - How many heads ahead or behind what probability says you should have
- Streak Odds - What the odds of your current heads streak is, given your current probability

**Note:** This was just for fun and is far from robust if you
- upgrade your probability of getting heads midstreak, the streak counter won't be accurate until your next streak
- close your game, stats won't persist between sessions

##### Install:
- Download [BepInEx](https://github.com/BepInEx/BepInEx/releases) x86 release (Built on 5.4)
  - For Windows and Linux, use the windows release
- Unzip it in your games folder (Go to game in steam library -> manage -> manage -> browse local files)
- Move the .dll file into BepInEx/plugins
- For Linux, add `WINEDLLOVERRIDES="winhttp=n,b" %command%` to your launch options, not sure if this or similar is required on Windows
- Launch the game
