# RTDeckLinkDotNet

### Overview 

This is a set of libraries I created years ago that I thought might be useful to someone else.  There is some overlap in the code and refactoring that was never done but if you need to write output to decklink using System.Drawing this will get you up and running quickly.

There is a sample application that should work assuming you have deckink cards installed with the drivers.  It's tested with the latest drivers as of November 2017 but it should work with anything in the recent past.

There are 3 access modes, 'Single', 'Dual' and 'KeyFill'.  Single connects to a single output, dual to two outputs that you write to separately and in key fill mode you write to one output and the library creates the key out for you.  This is done in software, there are some cards that do this in hardware but at the time the cards we used it needed to be done in software.

### Details

Depending on what output mode you want the classes to check out are in the 'RTDeckLinkDotNet' project.  Use either the 'DLAccessForSingle', 'DLAccessForDual' or 'DLAccessForKeyFill'.  Each of these allows you to open up a settings window and select the decklink you want to write to and then connect to it.  There is a callback that passes a Graphics object that can be written on and is copied to the output buffer.

There is also an option to connect to a 'Fake Decklink' in each of the classes.  This allows work to be done and testing on computers without decklink cards.  It simply shows a Form with what would be written to the decklink as its content.  This is not super efficient and can't keep up with high frame rates even on fast computers.

The clases have a couple other methods like showing a preview window but overall it's pretty simple.  The core of the code for copying the bitmap to the decklink is based on the code provided by BlackMagic in their sdk.

### Using the Sample

When you run the sample you'll first need to select the card and/or fake decklink settings.  Select 'setup' from the file menu and the setting UI should populate with what cards are installed.  If you want to use the fake decklink the settings will need to be changed from '0'.  Next click connect to decklink.  That's it.





