Ghost Writer
============

*It writes your code so you don't have to.*

This is a Windows Forms application that allows you to live-code during a presentation without actually having to type. It does so by scripting the keystrokes of your live-coding and sending them to your editor.

## Tutorial

This tutorial creates a demo for a presentation about the origin of "Hello world" in computing. Live coding is done in Visual Studio. A demo consists of some number of "steps", which are roughly analogous to "slides" in PowerPoint. Each step consists of several parts:

- Notes
    - Analogous to "speaker notes" in PowerPoint.
- Starting Code / Finished Code
    - The code as it should appear at the start / end of the step.
    - This is the contingency plan if the auto-typing gets messed up - you can push either of these directly to the app at any time.
- Keyboard Data
    - Defines the actual keystrokes that are sent to the target app.

---

Create a new console app in Visual Studio named "GhostWriterDemo". Leave Visual Studio open - Ghost Writer will send keystrokes to it.

---

Click `File > New` in the menu bar.

---

Go to `Edit > Set target application` and enter "Microsoft Visual Studio". This is how we set the application that Ghost Write will send keystrokes to. Note that if you have multiple instances of Visual Studio running, it will prompt you to select which one to send keystrokes to.

---

### Step 1

In this step, we'll begin talking about the origins of the phrase "Hello world".

---

In the `Presentation` tab, enter the following in the `Notes` box (these are analogous to Powerpoint's "Speaker Notes"):

```
History of "Hello world"
 - Catchphrase of 1950s New York radio disc jockey William B. Williams
 - 1974 Bell Laboratories internal memo by Brian Kernighan
 - Popularized by 1978 book The C Programming Language
```

We'll be talking about William B. Williams during the first step, so highlight "Catchphrase of 1950s New York radio disc jockey William B. Williams", then right-click and select `Highlight` (or type `Ctrl + H`).

---

Go to the `Auto-Typing` tab and enter the following script into the `Keyboard Data` box:

```
// History of 'Hello world'
// - Catchphrase of 1950s New York radio disc jockey William B. Williams

```

Note that this script contains no tags and does nothing special - all characters (including newlines) are typed as they appear.

---

Click the `Execute` button. Visual Studio should gain focus and type the two lines of comments automatically.

The typing is kind of slow for comments, so let's make speed it up. To do so, we'll add `[Fast]` and `[/Fast]` tags around the part of the script that we want typed fast. Now when we click `Execute` it's much faster.

```
[Fast]// History of "Hello world"
// - Catchphrase of 1950s New York radio disc jockey William B. Williams
[/Fast]
```

Note that a list of all tags is available by right-clicking on the `Keyboard Data` textbox. Selecting a tag inserts it at the current keyboard position.

---

At the bottom of the `Presentation` tab, there is a box for `Starting Code` and `Finished Code`. 

Since there should be no code on the screen at the beginning of this step, leave `Starting Code` blank.

For `Finished Code`, copy the code that was typed by the script into Visual Studio, and paste it into the textbox.

If you click `Push to Application`, then the contents of the Starting Code / Finished Code textbox are copied to Visual studio.

---

### Step 2

In this step, we'll be talking about the first computer usage of "Hello world".

---

In the menu, select `Edit > Add Step > After current`. Note that the `Starting code` is the `Finished code` of the first step.

---

Click the `Previous` button and copy the contents of the `Notes` box from the first step. Click `Next`, then paste into the `Notes` box of the second step. Remove the highlighting on the first line and highlight the second line, since we're reminding ourselves of what we're talking about on this step.

---

Switch to the `Auto-Typing` tab and enter the following script in the `Keyboard Data` box:

```
[GotoLine 3][Fast]// - 1974 Bell Laboratories internal memo by Brian Kernighan
// - Popularized by 1978 book The C Programming Language
[/Fast]
```

Note that since we're not starting from a blank file, we need to specify which line to start on. NEVER assume that the keyboard cursor is in the right position at the start of the step.

---

Click the `Execute` button. The new comment line should be typed into Visual Studio. Copy the code from Visual Studio and paste it into the `Finished Code` textbox in the `Presentation` tab.

---

### Step 3

In this step we're going to actually do the live-coding of "Hello world".

---

In the menu, select `Edit > Add Step > After current`.

---

For the `Notes` of the new step enter the following:

```
Without further ado, I present "Hello world" in C#!
```

---

In the `Keyboard Data` textbox, enter the following:

```

Console.WriteLine("Hello, world!");

```

---

Execute the step. Copy the code from Visual Studio and paste it into the `Finished Code` textbox.

---

Save the presentation. This concludes the tutorial for creating a demo.

## Presentation Mode

Ghost Writer is designed to work with Logitech presentation remotes. To enter presentation mode, ensure the menu item `Options > Presentation Mode` is checked. When in presentation mode, the right arrow goes the next step, left goes to the previous step, and the play button executes the current step.
