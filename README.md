# t2tWinFormAppBarLib

Helper library for creating AppBars (forms which are docked to the side of the screen) in .NET WinForms.  AppBars reserve the required space, similar to the Taskbar in Windows.

Available via Nuget : https://www.nuget.org/packages/tip2tail.WinFormAppBarLib/

Based on:
* https://github.com/PhilipRieck/WpfAppBar which carries out a similar function for WPF applications.
* Various other sources found online.

What is it?
----------
A helper for turning a WinForms window into an "AppBar" like the Windows taskbar.

How do I use it?
----------------
To use, just call this code from anywhere within a normal WinForms window (say a button click or the initialize). Note that you can not call this until AFTER the window is initialized, if the HWND hasn't been created yet (like in the constructor), an error will occur.

```C#
// Setup
AppBarHelper.AppBarMessage = "UniqueAPIMesasgeForYourApplication";

// Make the window an AppBar and dock to the right of the screen:
AppBarHelper.SetAppBar(this, AppBarEdge.Right);

// Restore the window to a normal window:
AppBarHelper.SetAppBar(this, AppBarEdge.None);
```

I found a bug!
--------------
Please add an issue, or better yet send a pull request.  I can't say that I'll revisit this project in the future so any contributions are welcome.
Thanks!

That sounds okay... but licensing?
----------------------------------
No warranty of any kind implied.  MIT license.
