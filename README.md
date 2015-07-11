# Spritefont Editor
Spritefont Editor is an editing tool for Xna ".spritefont" files.

Getting Started
---------------

Open the SFEditor.sln with MonoDevelop (or VisualStudio/Xamarin Studio), expand SFEditor project folder and build the project for your platform.

Using SFWidget in your app
--------------------------

While making this I made sure to seperate the main view component so that it can easily be embeded inside an editor.

If you are using Xwt, simply call the widget like so:
```c#
SFEditor.SFWidget sf_widget = new SFEditor.SFWidget();
```

If you are using Gtk or Wpf you can add Xwt as references, and than convert the widget to native toolkit like so(replace Gtk with your toolkit of choice):
```c#
var sf_widget = new SFEditor.SFWidget();
Xwt.GtkBackend.GtkEngine ge = new Xwt.GtkBackend.GtkEngine();
var gtk_widget = (Gtk.Widget)ge.GetNativeWidget(sf_widget);
```

What about Mac
--------------
I don't have a Mac so I can't compile for it, but you could build a Mac version yourself if you wanted. What you would need to do is:

1. Build Xwt for Mac from: https://github.com/mono/xwt/tree/a9014f461e23d27e393b9d89180b2ade92e0b168
2. Copy Xwt.Gtk.Mac and Xwt.Mac dll files to ThirdParty/Xwt/Gtk and ThirdParty/Xwt/Mac folders
3. Create a project SFEditor.Mac that has the above dll referenced, plus as well has ThirdParty/Xwt/Gtk/Gtk.dll referenced, and ThirdParty/Xwt/Gtk/Gtk.dll.config added as link and set to "copy if newer"
4. For "Program.cs" use a copy of https://github.com/cra0zy/SFEditor/blob/master/SFEditor/SFEditor.Windows/Program.cs#L11 with the selected line replaced by:
```c#
Settings.Init(Platform.Mac, ToolkitType.Cocoa, new[] { ToolkitType.Gtk, ToolkitType.Cocoa });
```
That should be it, Gtk version should work without problems, but Cocoa version will probably have a bug or two.

My TODO List for this project
-----------------------------
* Add code for ProjectDirty
* Implement Undo / Redo to XwtPlus.TextEditor
* Setup some extension API for MonoGame Pipeline tool and connect this to it
* Add a setup for easy installation

